using System.Collections.Generic;
using System.Linq;

namespace HackerMinigame.FileSystem
{
    public class FileSystemManager
    {
        public Directory root { get; private set; }
        public Directory CurrentDirectory { get; private set; }

        public FileSystemManager()
        {
            root = new Directory("/");
            CurrentDirectory = root;

            // Setup initial directories and files
            var bin = new Directory("bin");
            root.AddDirectory(bin);

            var home = new Directory("home");
            root.AddDirectory(home);

            var logs = new Directory("logs");
            root.AddDirectory(logs);

            var user = new Directory("user");
            home.AddDirectory(user);

            var file1 = new File("readme.txt", "This is a readme file.");
            user.AddFile(file1);
        }

        public FileSystemManager(string[] initialPaths, File[] initialFiles)
        {
            root = new Directory("/");
            CurrentDirectory = root;

            // Setup initial directories
            foreach (var path in initialPaths)
            {
                var parts = path.Split('/');
                var dir = root;
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == "")
                        continue;
                    var newDir = new Directory(parts[i]);
                    dir.AddDirectory(newDir);
                    dir = newDir;
                }
            }

            // Setup initial files
            foreach (File file in initialFiles)
            {
                var parts = file.Name.Split('/');
                var dir = root;
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    dir = dir.Directories.Find(d => d.Name == parts[i]);
                }
                var newFile = new File(parts[parts.Length - 1], file.Content);
                dir.AddFile(newFile);
            }
        }

        public bool ChangeDirectory(string name)
        {
            if (name == "..")
            {
                if (CurrentDirectory == root)
                {
                    return false; // Can't go above root
                }
                // Logic to move up in the directory structure
                // This is simplified and assumes each directory only has one parent
                CurrentDirectory = CurrentDirectory.Parent;
                return true;
            }

            foreach (var dir in CurrentDirectory.Directories)
            {
                if (dir.Name == name)
                {
                    CurrentDirectory = dir;
                    return true;
                }
            }
            return false;
        }

        public string GetCurrentPath()
        {
            var path = CurrentDirectory.Name;
            var parent = CurrentDirectory.Parent;

            while (parent != null)
            {
                if (parent.Name != "/")
                    path = parent.Name + "/" + path;
                else
                    path = parent.Name + path;
                parent = parent.Parent;
            }

            if (path == "/")
                return path;
            else
                return path + "/";
        }

        public string ListContents()
        {
            string contents = GetCurrentPath() + "\n";

            // Sort and list directories
            var sortedDirectories = CurrentDirectory.Directories.OrderBy(dir => dir.Name);
            foreach (var dir in sortedDirectories)
            {
                contents += "- " + dir.Name + "/\n";
            }

            //contents += "\n"; // Add a newline between directories and files, for readability

            // Sort and list files
            var sortedFiles = CurrentDirectory.Files.OrderBy(file => file.Name);
            foreach (var file in sortedFiles)
            {
                contents += "- " + file.Name + "\n";
            }

            return contents;
        }


        public string ReadFile(string name)
        {
            foreach (var file in CurrentDirectory.Files)
            {
                if (file.Name == name)
                {
                    return file.Content;
                }
            }
            return "No such file";
        }

        public void CreateDirectory(string name)
        {
            var newDir = new Directory(name);
            CurrentDirectory.AddDirectory(newDir);
        }

        public void CreateFile(string name)
        {
            var newFile = new File(name, "");
            CurrentDirectory.AddFile(newFile);
        }

        public void CreateFile(string name, string content)
        {
            var newFile = new File(name, content);
            CurrentDirectory.AddFile(newFile);
        }

        public void Delete(string name)
        {
            foreach (var dir in CurrentDirectory.Directories)
            {
                if (dir.Name == name)
                {
                    CurrentDirectory.Directories.Remove(dir);
                    return;
                }
            }

            foreach (var file in CurrentDirectory.Files)
            {
                if (file.Name == name)
                {
                    CurrentDirectory.Files.Remove(file);
                    return;
                }
            }
        }
    }
}