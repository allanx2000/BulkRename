using Innouvous.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BulkRename.ViewModels
{
    public class FileViewModel : ObservableClass //Put in Utils namespace
    {
        private FileInfo file;

        private string newName;


        public FileViewModel(string file)
            : this(new FileInfo(file))
        { }


        public FileViewModel(FileInfo file)
        {
            if (!file.Exists)
                throw new Exception(file.FullName + " does not exist");

            this.file = file;
            newName = OriginalName;
        }


        public string FilePath
        {
            get
            {
                return file.FullName;
            }
        }

        public string OriginalName
        {
            get
            {
                return file.Name;
            }
        }

        public string NewName
        {
            get
            {
                return newName;
            }
            set
            {
                newName = value;
                RaiseEvent("NewName");
                RaiseEvent("NewFilePath");
            }
        }

        public override bool Equals(object obj)
        {
            return obj is FileViewModel
                && obj.GetHashCode() == this.GetHashCode();

        }

        public override int GetHashCode()
        {
            return file.FullName.GetHashCode();
        }

        public string NewFilePath
        {
            get
            {
                return Path.Combine(file.Directory.FullName, NewName);
            }
        }

        private Exception renameException;

        public Exception RenameError { 
            get
            {
                return renameException;
            }
            set
            {
                renameException = value;

                RaiseEvent("RenameError");
                RaiseEvent("HasError");
                RaiseEvent("ErrorMessage");
                RaiseEvent("ErrorPanelVisibility");
            }}

        public bool HasError
        {
            get
            {
                return renameException != null;
            }
        }

        public Visibility ErrorPanelVisibility
        {
            get
            {
                return HasError ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public string ErrorMessage
        {
            get
            {
                return HasError ? renameException.Message : "";
            }
        }
    }
}
