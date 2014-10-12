using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoshBoutique.Search
{
    public class IndexField
    {
        public string FieldName { get; private set; }

        public string FieldValue { get; private set; }

        public bool Store { get; private set; }

        public bool Analyze { get; private set; }

        public IndexField(string fieldName, string fieldValue) : this(fieldName, fieldValue, store:false, analyze:true)
        {
        }

        public IndexField(string fieldName, string fieldValue, bool store, bool analyze)
        {
            this.FieldName = fieldName;
            this.FieldValue = fieldValue;
            this.Store = store;
            this.Analyze = analyze;
        }
    }
}
