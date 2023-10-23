using System.Collections.Generic;

namespace HackerMinigame.FileSystem
{

    public class File
    {
        public string Name { get; private set; }
        public string Content { get; private set; }

        public File(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }
}