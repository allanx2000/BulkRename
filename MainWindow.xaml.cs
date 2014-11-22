using BulkRename.Filters;
using BulkRename.ViewModels;
using Innouvous.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

namespace BulkRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = viewModel;
        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = DialogsUtility.CreateOpenFileDialog(
                title: "Add Files", multiselect: true, followLinks: false);

            ofd.ShowDialog();

            foreach (string file in ofd.FileNames)
            {
                viewModel.AddFile(new FileViewModel(file));
            }

        }

        private void RemoveFileButton_Click(object sender, RoutedEventArgs e)
        {
            List<FileViewModel> selected = new List<FileViewModel>();

            foreach (FileViewModel f in FilesListBox.SelectedItems)
            {
                selected.Add(f);
            }

            foreach (var f in selected)
            {
                viewModel.RemoveFile(f);
            }
        }

        private void AddFilterButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new FilterEditWindow();

            SetPreviewText(window);

            window.ShowDialog();

            if (!window.Cancelled)
            {
                var filter = window.GetFilterArgs();
                viewModel.AddFilter(new FilterDefinitionViewModel(filter));
            }
        }


        private void MoveFilterUpButton_Click(object sender, RoutedEventArgs e)
        {
            var fvm = GetFilterViewModel(sender);
            if (fvm != null)
            {
                bool moved = viewModel.MoveUp(fvm);
            }
        }

        private FilterDefinitionViewModel GetFilterViewModel(object obj)
        {
            var ctrl = obj as Control;

            if (ctrl != null)
                return ctrl.DataContext as FilterDefinitionViewModel;
            else
                return null;
        }
        private void MoveFilterDownButton_Click(object sender, RoutedEventArgs e)
        {
            var fvm = GetFilterViewModel(sender);
            if (fvm != null)
            {
                bool moved = viewModel.MoveDown(fvm);
            }
        }

        private void RemoveFilterButton_Click(object sender, RoutedEventArgs e)
        {
            var fvm = GetFilterViewModel(sender);
            if (fvm != null)
            {
                var result = MessageBoxFactory.ShowConfirmAsBool("Are you sure you want to delete the selected filter?", "Confirm Delete");

                if (result)
                {
                    viewModel.RemoveFilter(fvm);
                }
            }
        }

        private void RemoveAllFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBoxFactory.ShowConfirmAsBool("Are you sure you want to delete ALL filters?", "Confirm Delete");

            if (result)
            {
                viewModel.ClearFilters();
            }

        }


        private void RefreshNamesButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.UpdatePreviewNames();
        }

        private void RenameAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (viewModel.Files.Count == 0)
            {
                MessageBoxFactory.ShowError("No files to rename.", "No Files");
                return;
            }
            else if (viewModel.Filters.Count == 0)
            {
                MessageBoxFactory.ShowError("No filters to run.", "No Filters");
                return;
            }

            //Update Names
            viewModel.UpdatePreviewNames();

            //Copy and move
            if (MessageBoxFactory.ShowConfirmAsBool("Perform batch rename?", "Confirm Batch Rename"))
                DoRename();
        }

        private void DoRename()
        {


            viewModel.UpdatePreviewNames();

            int ctr = 0;

            List<FileViewModel> filesToRemove = new List<FileViewModel>();

            bool hasErrors = false;

            foreach (FileViewModel file in viewModel.Files)
            {
                try
                {
                    File.Move(file.FilePath, file.NewFilePath);
                    
                    file.RenameError = null;

                    filesToRemove.Add(file);

                    ctr++;
                }
                catch (Exception e)
                {
                    file.RenameError = e;
                    hasErrors = true;
                }
            }

            foreach (var f in filesToRemove)
            {
                viewModel.RemoveFile(f);
            }

            if (!hasErrors)
                MessageBoxFactory.ShowInfo("Batch rename completed successfully. Renamed " + ctr + " files.", "Batch Rename Completed");
            else
                MessageBoxFactory.ShowError("Batch rename completed with errors. Renamed " + filesToRemove.Count + " files", "Batch Rename Completed With Errors");
        }

        private void EditFilterButton_Click(object sender, RoutedEventArgs e)
        {
            var fvm = GetFilterViewModel(sender);
            if (fvm != null)
            {
                var window = new FilterEditWindow(fvm.FilterDef);

                SetPreviewText(window);

                window.ShowDialog();

                if (!window.Cancelled)
                {
                    fvm.FilterDef = window.GetFilterArgs();
                    viewModel.UpdatePreviewNames();
                }
            }
        }

        private void SetPreviewText(FilterEditWindow window)
        {
            if (viewModel.Files.Count > 0)
                window.SetPreviewInputText(viewModel.Files[0].NewName);
        }

        private void SaveFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sdf = DialogsUtility.CreateSaveFileDialog(title: "Save Filters");
            DialogsUtility.AddExtension(sdf, "XML", "*.xml");
            DialogsUtility.AddExtension(sdf, "All Files", "*.*");

            sdf.ShowDialog(this);

            if (!String.IsNullOrEmpty(sdf.FileName))
                SaveFilters(sdf.FileName);
        }

        private void LoadFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = DialogsUtility.CreateOpenFileDialog(title: "Load Filters From File");
            DialogsUtility.AddExtension(ofd, "XML", "*.xml");
            DialogsUtility.AddExtension(ofd, "All Files", "*.*");

            ofd.ShowDialog(this);

            if (!String.IsNullOrEmpty(ofd.FileName))
                LoadFilters(ofd.FileName);
        }


        private void SaveFilters(string file)
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(List<SerializableFilterDefinition>));

            List<SerializableFilterDefinition> filters = viewModel.Filters.Select(x => SerializableFilterDefinition.Create(x.FilterDef)).ToList();

            xmlser.Serialize(new StreamWriter(file), filters);
        }

        private void LoadFilters(string file)
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(List<SerializableFilterDefinition>));

            List<SerializableFilterDefinition> filters = (List<SerializableFilterDefinition>)xmlser.Deserialize(new StreamReader(file));

            viewModel.ClearFilters();

            foreach (var f in filters)
            {
                viewModel.AddFilter(new FilterDefinitionViewModel(f.GetFilterDefinition()));
            }

        }

    }
}
