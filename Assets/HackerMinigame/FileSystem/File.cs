using System.Collections.Generic;

namespace HackerMinigame.FileSystem
{
    [System.Serializable]
    public struct File
    {
        public string Name;
        public string Content;

        public File(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }
}