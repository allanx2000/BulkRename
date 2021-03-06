﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRename.Filters
{
    public class FilterFactory
    {
        /// <summary>
        /// Register the different filters here
        /// </summary>
        private static Dictionary<string, AbstractFilter> filters = new Dictionary<string, AbstractFilter>()
        {
            {RegexFilter.NAME, new RegexFilter()},
            {TrimFilter.NAME, new TrimFilter()}
        };

        public static IEnumerable<string> GetFilterNames()
        {
            return filters.Keys;
        }

        public static AbstractFilter GetFilter(FilterDefinition def)
        {
            return GetFilter(def.FilterName);
        }

        public static AbstractFilter GetFilter(string name)
        {
            return filters[name];
        }
    }
}
