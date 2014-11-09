using BulkRename.Filters;
using BulkRename.ViewModels;
using Innouvous.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BulkRename
{
    /// <summary>
    /// Interaction logic for FilterEditWindow.xaml
    /// </summary>
    public partial class FilterEditWindow : Window
    {
        public readonly FilterEditWindowViewModel viewModel = new FilterEditWindowViewModel();

        public FilterEditWindow()
        {
            InitializeComponent();

            Cancelled = true;

            this.DataContext = viewModel;
        }

        //bool isEdit = false;

        private FilterDefinition? existingFilter = null;

        public FilterEditWindow(FilterDefinition existingFilter) : this()
        {
            //isEdit = true;
            this.existingFilter = existingFilter;

            viewModel.FilterChanged+= viewModel_FilterChanged;

            viewModel.SelectedFilter = existingFilter.FilterName;
        }
        public void SetPreviewInputText(string text)
        {
            viewModel.PreviewInput = text;
        }

        private void viewModel_FilterChanged(object sender, AbstractFilter filter)
        {
            Dictionary<string, string> kvs = new Dictionary<string, string>();
            foreach (var o in existingFilter.Value.Options)
            {
                kvs.Add(o.Name, o.Value);
            }

            foreach (OptionViewModel o in viewModel.CurrentOptions)
            {
                if (kvs.ContainsKey(o.Name))
                    o.Value = kvs[o.Name];
            }

            viewModel.FilterChanged -= viewModel_FilterChanged;
        }


        public bool Cancelled { get; private set; }
        
        public FilterDefinition GetFilterArgs()
        {
            return viewModel.GetFilterArgs();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Cancelled = true;
            this.Close();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Validates the filter
                GetFilterArgs();

                Cancelled = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBoxFactory.ShowError(ex);
                //MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
