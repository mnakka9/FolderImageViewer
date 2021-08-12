using System.Collections.Generic;
using System.IO;

namespace FolderImageViewer
{
    public class MainFolder
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public IList<Folder> ChildFolders { get; set; }
    }

    public class Folder
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public FileInfo[] FilesList { get; set; }
    }
}
