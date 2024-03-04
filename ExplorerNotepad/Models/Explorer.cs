namespace ExplorerNotepad.Models
{
    public abstract class Explorer
    {
        public Explorer(string Name)
        {
            Header = Name;
        }

        public string Header { get; set; }
        public string Image { get; set; }
        public string SourceName { get; set; }
    }
}
