using System.Collections.Generic;

namespace HackerMinigame.FileSystem
{
    public class Directory
    {
        public string Name { get; private set; }
        public Directory Parent { get; private set; }
        public List<Directory> Directories { get; private set; }
        public List<File> Files { get; private set; }

        public Directory(string name, Directory parent = null)
        {
            Name = name;
            Parent = parent;
            Directories = new List<Directory>();
            Files = new List<File>();
        }

        public void AddDirectory(Directory directory)
        {
            directory.Parent = this;
            Directories.Add(directory);
        }

        public void AddFile(File file)
        {
            Files.Add(file);
        }
    }
}