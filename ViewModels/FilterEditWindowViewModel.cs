using BulkRename.Filters;
using Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRename.ViewModels
{
    public class FilterEditWindowViewModel : ObservableClass
    {
        private string selectedFilter;

        public delegate void FilterChangedEventHandler(object sender, AbstractFilter filter);

        public event FilterChangedEventHandler FilterChanged;

        private int criteriaWidth;

        private string previewInput;

        public string PreviewInput
        {
            get
            {
                return previewInput;
            }
            set
            {
                previewInput = value;

                RaiseEvent("PreviewInput");

                RaiseEvent("PreviewOutput");
            }
        }

        public string PreviewOutput
        {
            get
            {
                if (!String.IsNullOrEmpty(selectedFilter))
                {
                    try
                    {
                        return FilterFactory.GetFilter(selectedFilter).DoFilter(previewInput, GetOptions());
                    }
                    catch (Exception e)
                    {
                        return e.Message;
                    }
                }
                else return "";
            }
        }


        private AbstractFilter GetSelectedFilter()
        {
            return FilterFactory.GetFilter(selectedFilter);
        }

        private List<Option> GetOptions()
        {
            var options = CurrentOptions.Select(x => x.Data).ToList();

            GetSelectedFilter().Validate(options);
            
            return options;
        }

        /// <summary>
        /// Gets the FilterArg generated from the view. Throws exception on argument validation errors
        /// </summary>
        /// <returns></returns>
        /// 
        public FilterDefinition GetFilterArgs()
        {
            return new FilterDefinition(selectedFilter, GetOptions());
        }

        public int CriteriaWidth
        {
            get
            {
                return criteriaWidth;
            }
            set
            {
                criteriaWidth = value;
                RaiseEvent("CriteriaWidth");
            }
        }
        public string SelectedFilter
        {
            get
            {
                return selectedFilter;
            }
            set
            {
                selectedFilter = value;
                RaiseEvent("SelectedFilter");

                RefreshCurrentOptions();

            }
        }

        private void RefreshCurrentOptions()
        {
            var filter = FilterFactory.GetFilter(selectedFilter);

            currentOptions.Clear();

            int maxLength = 0;

            foreach (var o in filter.GetOptions())
            {
                maxLength = Math.Max(maxLength, o.Name.Length);

                var ovm = new OptionViewModel(o);
                ovm.PropertyChanged += (sender, args) =>
                {
                    if (args.PropertyName == "Value")
                        RaiseEvent("PreviewOutput");
                };

                currentOptions.Add(ovm);
            }

            maxLength = maxLength * 8;

            foreach (var o in currentOptions)
            {
                o.LabelWidth = maxLength;
            }

            if (FilterChanged != null)
            {
                FilterChanged.Invoke(this, filter);
            }

        }

        private ObservableCollection<OptionViewModel> currentOptions = new ObservableCollection<OptionViewModel>();
        public ObservableCollection<OptionViewModel> CurrentOptions
        {
            get
            {
                return currentOptions;
            }
        }

        public List<string> Filters
        {
            get
            {
                var filters = FilterFactory.GetFilterNames();
                filters.OrderBy(s => s).Reverse();

                return filters.ToList();
            }
        }
    }

}
