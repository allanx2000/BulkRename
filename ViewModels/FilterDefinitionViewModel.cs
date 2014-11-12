using BulkRename.Filters;
using Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRename.ViewModels
{
    public class FilterDefinitionViewModel : ObservableClass
    {
        private FilterDefinition args;

        public FilterDefinitionViewModel(FilterDefinition args)
        {
            this.args = args;
        }

        public string FilterType
        {
            get
            {
                return args.FilterName;
            }
        }

        public string Description
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                foreach (var o in args.Options)
                {
                    sb.AppendLine(o.Name + ": " + o.Value);
                }

                return sb.ToString();
            }
        }

        public FilterDefinition FilterDef
        {
            get
            {
                return args;
            }
            set
            {
                args = value;
                RaiseEvent("FilterDef");
                RaiseEvent("Description");
                RaiseEvent("Name");
            }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
