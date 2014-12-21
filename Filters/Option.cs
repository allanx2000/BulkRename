using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkRename.Filters
{
    /// <summary>
    /// Holds information of a single filter option
    /// </summary>
    public struct Option
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Value { get; private set; }

        public Option(string name, string description)
            : this()
        {
            Name = name;
            Description = description;
        }

        private Option(string name, string description, string value)
            : this(name, description)
        {
            Value = value;
        }

        public Option SetValue(string value)
        {
            return new Option(Name, Description, value);
        }
    }
}
