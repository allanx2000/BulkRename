using BulkRename.Filters;
using Innouvous.Utils;
using Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BulkRename.ViewModels
{
    public class FilterEditWindowViewModel : ObservableClass
    {
        private string selectedFilter;
        private int criteriaWidth;
        private string previewInput;
        private ObservableCollection<OptionViewModel> currentOptions = new ObservableCollection<OptionViewModel>();

        public FilterEditWindowViewModel()
        {
            Cancelled = true;
        }

        public void SetExistingFilter(FilterDefinition filter)
        {
            this.SelectedFilter = filter.FilterName;
            RaiseEvent("SelectedFilter");

            this.currentOptions = new ObservableCollection<OptionViewModel>();

            foreach (var o in filter.Options)
            {
                currentOptions.Add(new OptionViewModel(o));
            }

            RaiseEvent("CurrentOptions");

        }

        #region Properties

        public bool Cancelled { get; private set; }

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
                if (!String.IsNullOrEmpty(selectedFilter) && !String.IsNullOrEmpty(PreviewInput))
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

        #endregion

        #region Commands

        public ICommand OKCommand
        {
            get
            {
                return new CommandHelper(obj => DoOK(obj));
            }
        }

        private void DoOK(object window)
        {
            var win = window as FilterEditWindow;
            if (win == null)
                return;

            try
            {
                //Validates the filter
                GetFilterArgs();

                Cancelled = false;

                win.Close();
            }
            catch (Exception ex)
            {
                MessageBoxFactory.ShowError(ex);
                //MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                return new CommandHelper(obj => DoCancel(obj)); 
            }
        }

        private void DoCancel(object window)
        {
            (window as FilterEditWindow).Close();
        }
        
        #endregion

        /// <summary>
        /// Gets the FilterArg generated from the view. Throws exception on argument validation errors
        /// </summary>
        /// <returns></returns>
        /// 
        public FilterDefinition GetFilterArgs()
        {
            return new FilterDefinition(selectedFilter, GetOptions());
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
        }
    }

}
