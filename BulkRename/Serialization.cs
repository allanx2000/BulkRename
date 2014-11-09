using BulkRename.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRename
{
    [Serializable]
    public struct SerializableOption
    {

        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public SerializableOption(Option o) : this()
        {
            this.Description = o.Description;
            this.Name = o.Name;
            this.Value = o.Value;
        }

        public static List<SerializableOption> ToSerializableOptions(List<Option> list)
        {
            List<SerializableOption> data = new List<SerializableOption>();

            foreach (var o in list)
            {
                data.Add(new SerializableOption(o));
            }

            return data;
        }

        public static List<Option> ToOptions(List<SerializableOption> options)
        {
            List<Option> data = new List<Option>();
            
            foreach (var o in options)
            {
                data.Add(new Option(o.Name, o.Description).SetValue(o.Value));
            }

            return data;
        }

    }

    [Serializable]
    public struct SerializableFilterDefinition
    {
        public static SerializableFilterDefinition Create(FilterDefinition filterDefinition)
        {
            return new SerializableFilterDefinition()
            {
                FilterName = filterDefinition.FilterName,
                SerializableOptions = SerializableOption.ToSerializableOptions(filterDefinition.Options)
            };
        }

        public string FilterName { get; set; }
        public List<SerializableOption> SerializableOptions { get; set; }

        public FilterDefinition GetFilterDefinition()
        {
            return new FilterDefinition(FilterName, SerializableOption.ToOptions(SerializableOptions));
        }
    }
}
