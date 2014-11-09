using Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkRename.ViewModels
{
    public class MainWindowViewModel : ObservableClass
    {
        #region Files
        private ObservableCollection<FileViewModel> files= new ObservableCollection<FileViewModel>();

        public ObservableCollection<FileViewModel> Files
        {
            get
            {
                return files;
            }
        }

        private FileViewModel selectedFile;
        public FileViewModel SelectedFile
        {
            get
            {
                return selectedFile;
            }
            set
            {
                selectedFile = value;

                RaiseEvent("SelectedFile");
                RaiseEvent("FileSelected");
            }
        }

        public bool FileSelected
        {
            get
            {
                return SelectedFile != null;
            }
        }

        public void ClearFiles()
        {
            files.Clear();
        }

        public void RemoveFile(FileViewModel file)
        {
            files.Remove(file);
        }

        public void AddFile(FileViewModel file)
        {
            if (!files.Contains(file))
            {
                files.Add(file);

                file.NewName = Filter(file.OriginalName);
            }
        }

        #endregion

        #region Filters
        private ObservableCollection<FilterDefinitionViewModel> filters = new ObservableCollection<FilterDefinitionViewModel>();

        public ObservableCollection<FilterDefinitionViewModel> Filters
        {
            get
            {
                return filters;
            }
        }

        private FilterDefinitionViewModel selectedFilter;
        public FilterDefinitionViewModel SelectedFilter
        {
            get
            {
                return selectedFilter;
            }
            set
            {
                selectedFilter = value;

                RaiseEvent("SelectedFilter");
                RaiseEvent("FilterSelected");
            }
        }

        public bool FilterSelected
        {
            get
            {
                return SelectedFilter != null;
            }
        }

       

        public void RemoveFilter(FilterDefinitionViewModel filter)
        {
            filters.Remove(filter);

            UpdatePreviewNames();
        }

        public void AddFilter(FilterDefinitionViewModel filter)
        {
            if (!filters.Contains(filter))
            {
                filters.Add(filter);

                UpdatePreviewNames();
            }
        }

        public bool MoveUp(FilterDefinitionViewModel filter)
        {
            int currentIndex = filters.IndexOf(filter);

            if (currentIndex != 0)
            {
                filters.Move(currentIndex, currentIndex - 1);

                UpdatePreviewNames();
                return true;
            }
            else
                return false;
        }

        public bool MoveDown(FilterDefinitionViewModel filter)
        {
            int currentIndex = filters.IndexOf(filter);

            if (currentIndex < filters.Count - 1)
            {
                filters.Move(currentIndex, currentIndex + 1);

                UpdatePreviewNames();

                return true;
            }
            else
                return false;
        }

        public void UpdatePreviewNames()
        {
            foreach (var file in Files)
            {
                string filename = file.OriginalName;

                file.NewName = Filter(filename);
            }
        }

        public string Filter(string input)
        {
            foreach (FilterDefinitionViewModel fvm in filters)
            {
                input = fvm.FilterDef.GetFilter().DoFilter(input, fvm.FilterDef);
            }

            return input;
        }

        public void ClearFilters()
        {
            Filters.Clear();
        }

        #endregion

    }
}
