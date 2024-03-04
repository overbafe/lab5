using System.IO;

namespace ExplorerNotepad.Models
{
    public class Directories : Explorer
    {
        public Directories(string Name) : base(Name)
        {
            SourceName = Name;
            Image = "Assets/iconFolder.png";
        }

        public Directories(DirectoryInfo directoryName) : base(directoryName.Name)
        {
            SourceName = directoryName.FullName;
            Image = "Assets/iconFolder.png"; 
        }
    }
}
