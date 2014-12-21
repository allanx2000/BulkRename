using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRename.Filters
{
    /// <summary>
    /// Holds information used to initialize/save a filter
    /// </summary>
    public struct FilterDefinition
    {
        public string FilterName { get; private set; }
        public List<Option> Options { get; private set; }

        public FilterDefinition(string filterName, List<Option> options)
            : this()
        {
            FilterName = filterName;
            Options = options;
        }

        public AbstractFilter GetFilter()
        {
            return FilterFactory.GetFilter(FilterName);
        }
    }
}
