using PoshBoutique.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoshBoutique.Providers
{
    public class HollowMethodsTracker<T> : IModelTracker<T>
    {
        public void TrackItemCreated(T item)
        {
        }
    }
}