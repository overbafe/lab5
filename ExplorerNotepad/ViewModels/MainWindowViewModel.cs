using ExplorerNotepad.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using System.Linq;

namespace ExplorerNotepad.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<Explorer> ExplorerCollection;

        public ObservableCollection<Explorer> ExplorerCollectionProperties { get => ExplorerCollection; set => this.RaiseAndSetIfChanged(ref ExplorerCollection, value); }

        private int currentIndex;
        public int currentIndexProperties
        {
            get => currentIndex; set
            {
                this.RaiseAndSetIfChanged(ref currentIndex, value);
            }
        }
        public void openExplorer()
        {
            outTextFolder = string.Empty;
            VisibilityExplorerProperties = true;
        }

        private bool VisibilityExplorer;
        public bool VisibilityExplorerProperties { get => VisibilityExplorer; set => this.RaiseAndSetIfChanged(ref VisibilityExplorer, value); }

        private string outTextFolder;
        public string outTextFolderProperties { get => outTextFolder; set => this.RaiseAndSetIfChanged(ref outTextFolder, value); }

        private string currentPath = Directory.GetCurrentDirectory(); // Renamed to avoid conflict

        public MainWindowViewModel()
        {

            VisibilityExplorer = true;

            outTextFolder = string.Empty;

            ExplorerCollection = new ObservableCollection<Explorer>();

            fillCollection(currentPath);
        }

        public void returnBack()
        {
            outTextFolder = string.Empty;
            VisibilityExplorerProperties = true;
        }

        public void openButton_openRegime()
        {
            if (ExplorerCollection[currentIndexProperties] is Directories)
            {
                if (ExplorerCollection[currentIndexProperties].Header == "..")
                {
                    var temp_path_first = Directory.GetParent(currentPath);
                    if (temp_path_first != null)
                    {
                        fillCollection(temp_path_first.FullName);
                        currentPath = temp_path_first.FullName;
                    }
                    else if (temp_path_first == null) fillCollection("");
                }
                else
                {
                    var temp_path_second = ExplorerCollection[currentIndex].SourceName;
                    fillCollection(ExplorerCollection[currentIndexProperties].SourceName);
                    currentPath = temp_path_second;
                }
            }
            else
            {
                // Logic for opening a file goes here
            }
        }

        public async void fillCollection(string varPath)
        {
            ExplorerCollection.Clear();
            if (varPath != "")
            {
                var DirectoryInformation = new DirectoryInfo(varPath);
                ExplorerCollection.Add(new Directories(".."));
                foreach (var directory in await Task.Run(() => DirectoryInformation.GetDirectories()))
                {
                    ExplorerCollection.Add(new Directories(directory));
                }
                foreach (var fileinfo in await Task.Run(() => DirectoryInformation.GetFiles()))
                {
                    if (fileinfo.Extension == ".jpg" || fileinfo.Extension == ".png" || fileinfo.Extension == ".ico" || fileinfo.Extension == ".bmp" || fileinfo.Extension == ".gif")
                    {
                        ExplorerCollection.Add(new Files(fileinfo));
                    }
                }
            }
            else if (varPath == "")
            {
                foreach (var disk in Directory.GetLogicalDrives())
                {
                    ExplorerCollection.Add(new Directories(disk));
                }
            }
            currentIndexProperties = 0;
        }
        private Bitmap? imagePreview;
        public Bitmap? ImagePreview
        {
            get => imagePreview;
            set => this.RaiseAndSetIfChanged(ref imagePreview, value);
        }

        public void DoubleTap()
        {
            var selectedExplorer = ExplorerCollectionProperties[currentIndexProperties];
            var imageExtensions = new[] { ".png", ".ico", "jpg", "dmp", "gif" };

            // Check if the selected item is a file
            if (selectedExplorer is Files file)
            {
                // Check if the file is a PNG
                if (imageExtensions.Contains(Path.GetExtension(file.SourceName), StringComparer.OrdinalIgnoreCase))
                {
                    // Load the image
                    var imagePath = Path.Combine(currentPath, file.SourceName);
                    var image = new Bitmap(imagePath);

                    // Set the image preview
                    ImagePreview = image;

                    // Clear text and set visibility
                    outTextFolderProperties = "";
                    VisibilityExplorerProperties = true;
                    currentIndexProperties = 0;
                }
                else
                {
                    openExplorer();
                }
            }
            else
            {
                openButton_openRegime();
            }
        }
    }
}
