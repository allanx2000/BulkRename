using BulkRename.Filters;
using Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRename.ViewModels
{
    public class OptionViewModel : ObservableClass
    {
        
        private Option data;
        private int labelWidth = 100;
        
        #region Properties
        public int LabelWidth { 
            get
            {
                return labelWidth;
            }
            set
            {
                labelWidth = value;

                RaiseEvent("LabelWidth");
            }
        }

        public Option Data
        {
            get
            {
                return data;
            }
            private set
            {
                
                data = value;

                RaiseEvent("Data");
                RaiseEvent("Value");
            }
        }

        public string Name
        {
            get
            { 
                return data.Name; 
            }
        }

        public string Description
        {
            get
            {
                return data.Description;
            }
        }

        public string Value
        {
            get
            {
                return data.Value;
            }
            set
            {
                SetValue(value);
            }
        }
        #endregion


        public void SetValue(string value)
        {
            Data = data.SetValue(value);
            RaiseEvent("Value");
        }


        public OptionViewModel(Option option)
        {
            this.Data = option;
        }

    }
}
