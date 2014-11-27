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

            //Cancelled = true;

            this.DataContext = viewModel;
        }
        
        public FilterEditWindow(FilterDefinition existingFilter) : this()
        {
            viewModel.SetExistingFilter(existingFilter);
        }

        public void SetPreviewInputText(string text)
        {
            viewModel.PreviewInput = text;
        }

        public bool Cancelled {
            get
            {
                return viewModel.Cancelled;
            }
        }
        
        public FilterDefinition GetFilterArgs()
        {
            return viewModel.GetFilterArgs();
        }


        /*private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Validates the filter
                GetFilterArgs();

                //Cancelled = false;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBoxFactory.ShowError(ex);
                //MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }*/
    }
}
