using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace PoshBoutique.Polyfills
{
    public static class HostingEnvironmentPolyfills
    {
        private static BackgroundWorkSchedulerPf _backgroundWorkScheduler;

        [SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)] 
        public static void QueueBackgroundWorkItemPf(Action<CancellationToken> workItem)
        {
            HostingEnvironmentPolyfills.QueueBackgroundWorkItemPf(ct => { workItem(ct); return Task.FromResult<object>(null); });
        }

        [SecurityPermission(SecurityAction.LinkDemand, Unrestricted = true)] 
        public static void QueueBackgroundWorkItemPf(Func<CancellationToken, Task> workItem)
        {
            BackgroundWorkSchedulerPf scheduler = Volatile.Read(ref _backgroundWorkScheduler);

            // If the scheduler doesn't exist, lazily create it, but only allow one instance to ever be published to the backing field
            if (scheduler == null)
            {
                BackgroundWorkSchedulerPf newlyCreatedScheduler = new BackgroundWorkSchedulerPf(HostingEnvironment.UnregisterObject);
                scheduler = Interlocked.CompareExchange(ref _backgroundWorkScheduler, newlyCreatedScheduler, null) ?? newlyCreatedScheduler;
                if (scheduler == newlyCreatedScheduler)
                {
                    HostingEnvironment.RegisterObject(scheduler); // Only call RegisterObject if we just created the "winning" one
                }
            }

            scheduler.ScheduleWorkItem(workItem);
        }
    }

    internal sealed class BackgroundWorkSchedulerPf : IRegisteredObject
    {
        private readonly CancellationTokenHelperPf _cancellationTokenHelper = new CancellationTokenHelperPf(canceled: false);
        private int _numExecutingWorkItems; // number of executing work items, not scheduled work items; a work item might never be scheduled
        private readonly Action<BackgroundWorkSchedulerPf> _unregisterCallback;
        private readonly Action _workItemCompleteCallback;

        internal BackgroundWorkSchedulerPf(Action<BackgroundWorkSchedulerPf> unregisterCallback, Action workItemCompleteCallback = null)
        {
            _unregisterCallback = unregisterCallback;
            _workItemCompleteCallback = workItemCompleteCallback;
        }

        private void FinalShutdown()
        {
            _unregisterCallback(this);
        }

        // we can use 'async void' here since we're guaranteed to be off the AspNetSynchronizationContext
        private async void RunWorkItemImpl(Func<CancellationToken, Task> workItem)
        {
            Task returnedTask = null;
            try
            {
                returnedTask = workItem(_cancellationTokenHelper.Token);
                await returnedTask.ConfigureAwait(continueOnCapturedContext: false);
            }
            catch (Exception ex)
            {
                // ---- exceptions caused by the returned task being canceled
                if (returnedTask != null && returnedTask.IsCanceled)
                {
                    return;
                }

                // ---- exceptions caused by CancellationToken.ThrowIfCancellationRequested()
                OperationCanceledException operationCanceledException = ex as OperationCanceledException;
                if (operationCanceledException != null && operationCanceledException.CancellationToken == _cancellationTokenHelper.Token)
                {
                    return;
                }
            }
            finally
            {
                WorkItemComplete();
            }
        }

        public void ScheduleWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (_cancellationTokenHelper.IsCancellationRequested)
            {
                return; // we're not going to run this work item
            }

            // Unsafe* since we want to get rid of Principal and other constructs specific to the current ExecutionContext
            ThreadPool.UnsafeQueueUserWorkItem(state =>
            {
                lock (this)
                {
                    if (_cancellationTokenHelper.IsCancellationRequested)
                    {
                        return; // we're not going to run this work item
                    }
                    else
                    {
                        _numExecutingWorkItems++;
                    }
                }

                RunWorkItemImpl((Func<CancellationToken, Task>)state);
            }, workItem);
        }

        public void Stop(bool immediate)
        {
            // Hold the lock for as little time as possible
            int currentWorkItemCount;
            lock (this)
            {
                _cancellationTokenHelper.Cancel(); // dispatched onto a new thread
                currentWorkItemCount = _numExecutingWorkItems;
            }

            if (currentWorkItemCount == 0)
            {
                // There was no scheduled work, so we're done
                FinalShutdown();
            }
        }

        private void WorkItemComplete()
        {
            // Hold the lock for as little time as possible
            int newWorkItemCount;
            bool isCancellationRequested;
            lock (this)
            {
                newWorkItemCount = --_numExecutingWorkItems;
                isCancellationRequested = _cancellationTokenHelper.IsCancellationRequested;
            }

            // for debugging & unit tests
            if (_workItemCompleteCallback != null)
            {
                _workItemCompleteCallback();
            }

            if (newWorkItemCount == 0 && isCancellationRequested)
            {
                // Cancellation was requested, and we were the last work item to complete, so everybody is finished
                FinalShutdown();
            }
        }
    }

    internal sealed class CancellationTokenHelperPf : IDisposable
    {
        private const int STATE_CREATED = 0;
        private const int STATE_CANCELING = 1;
        private const int STATE_CANCELED = 2;
        private const int STATE_DISPOSING = 3;
        private const int STATE_DISPOSED = 4; // terminal state

        // A CancellationTokenHelper which is already marked as disposed; useful for avoiding
        // allocations of CancellationTokenHelper instances which are never observed.
        internal static readonly CancellationTokenHelperPf StaticDisposed = GetStaticDisposedHelper();

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private int _state;

        public CancellationTokenHelperPf(bool canceled)
        {
            if (canceled)
            {
                _cts.Cancel();
            }
            _state = (canceled) ? STATE_CANCELED : STATE_CREATED;
        }

        internal bool IsCancellationRequested
        {
            get { return _cts.IsCancellationRequested; }
        }

        internal CancellationToken Token
        {
            get { return _cts.Token; }
        }

        // Cancels the token.
        public void Cancel()
        {
            if (Interlocked.CompareExchange(ref _state, STATE_CANCELING, STATE_CREATED) == STATE_CREATED)
            {
                // Only allow cancellation if the token hasn't yet been canceled or disposed.
                // Cancel on a ThreadPool thread so that we can release the original thread back to IIS.
                // We can use UnsafeQUWI to avoid an extra ExecutionContext capture since CancellationToken already captures it.
                ThreadPool.UnsafeQueueUserWorkItem(_ =>
                {
                    try
                    {
                        _cts.Cancel();
                    }
                    catch
                    {
                        // ---- all exceptions to avoid killing the worker process.
                    }
                    finally
                    {
                        if (Interlocked.CompareExchange(ref _state, STATE_CANCELED, STATE_CANCELING) == STATE_DISPOSING)
                        {
                            // A call to Dispose() came in on another thread while we were in the middle of a cancel
                            // operation. That thread will no-op, so we'll dispose of it here.
                            _cts.Dispose();
                            Interlocked.Exchange(ref _state, STATE_DISPOSED);
                        }
                    }
                }, null);
            }
        }

        // Disposes of the token.
        public void Dispose()
        {
            // Only allow a single call to Dispose.
            int originalState = Interlocked.Exchange(ref _state, STATE_DISPOSING);
            switch (originalState)
            {
                case STATE_CREATED:
                case STATE_CANCELED:
                    // If Cancel() hasn't yet been called or has already run to completion,
                    // the underlying CTS guarantees that the Dispose method won't block
                    // or throw, so we can just call it directly.
                    _cts.Dispose();
                    Interlocked.Exchange(ref _state, STATE_DISPOSED);
                    break;

                case STATE_DISPOSED:
                    // If the object was already disposed, we need to reset the flag here
                    // since we accidentally blew it away with the original Exchange.
                    Interlocked.Exchange(ref _state, STATE_DISPOSED);
                    break;

                // Otherwise, the object is already canceling or disposing, so the
                // other thread will handle the call to Dispose().
            }
        }

        private static CancellationTokenHelperPf GetStaticDisposedHelper()
        {
            CancellationTokenHelperPf helper = new CancellationTokenHelperPf(false);
            helper.Dispose();
            return helper;
        }
    }
}