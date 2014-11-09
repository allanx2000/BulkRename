using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRename.Filters
{
    public struct Option
    {
        public string Name {get; private set; }
        public string Description {get; private set;}
        public string Value {get; private set;}

        public Option(string name, string description) :this()
        {
            Name = name;
            Description = description;
        }

        private Option(string name, string description, string value) : this(name,description)
        {
            Value = value;
        }

        public Option SetValue(string value)
        {
            return new Option(Name, Description, value);
        }
    }

 
    public abstract class AbstractFilter
    {
        protected AbstractFilter(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }
        
        public void Validate(FilterDefinition args)
        {
            Validate(args.Options);
        }

        public string DoFilter(string name, FilterDefinition args)
        {
            return DoFilter(name, args.Options);
        }
        
        public abstract List<Option> GetOptions();
        public abstract void Validate(List<Option> options);
        public abstract string DoFilter(string name, List<Option> options);


        protected Option GetOption(List<Option> options, string TrimChar)
        {
            return options.First(x => x.Name == TrimChar);
        }
    }
}
