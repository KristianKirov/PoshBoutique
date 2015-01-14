using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Data.Models
{
    public interface IModelTracker<T>
    {
        void TrackItemCreated(T item);
    }
}
