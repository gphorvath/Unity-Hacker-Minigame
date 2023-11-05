using System;
using System.Collections.Generic;
using UnityEngine;
using HackerMinigame.FileSystem;
using HackerMinigame.SystemProcesses;

namespace HackerMinigame
{
    public class HackerMinigameManager : MonoBehaviour
    {
        [Header("File System Configuration")]
        [field: SerializeField] private string[] initialPaths = { "bin", "home/user", "logs" };
        [field: SerializeField] private File[] initialFiles;
        [field: SerializeField] private List<char> _dirtyCharacters = new List<char>() { ' ', '\n', '\r', '\t', '$', '>', '<', '/', '\\' };
        private FileSystemManager _fileSystemManager;

        [Header("System Processes Configuration")]
        [field: SerializeField] private string[] initialProcessNames = { "system", "network-service", "x-server", "terminal" };
        private ProcessManager _processManager;

        // Events
        public event Action<Process> OnProcessKilled;

        void Start()
        {
            _fileSystemManager = new FileSystemManager(initialPaths, initialFiles);
            _processManager = new ProcessManager(initialProcessNames);
            Terminal.WriteLine("Type 'help' for a list of commands");
        }

        void OnUserInput(string input)
        {
            if (input == "clear")
            {
                Terminal.ClearScreen();
                return;
            }
            Terminal.WriteLine(ParseCommand(input));
        }

        // Call this function with input from the user
        public string ParseCommand(string input)
        {
            string[] parts = input.Split(' ');
            string command = parts[0];
            string output = "";

            switch (command)
            {
                case "cat":
                    output = cat(parts);
                    break;
                case "cd":
                    output = cd(parts);
                    break;
                case "echo":
                    output = echo(input);
                    break;
                case "kill":
                    output = kill(parts);
                    break;
                case "ls":
                    output = ls();
                    break;
                case "mkdir":
                    output = mkdir(parts);
                    break;
                case "ps":
                    output = ps();
                    break;
                case "pwd":
                    output = pwd();
                    break;
                case "rm":
                    output = rm(parts);
                    break;
                case "touch":
                    output = touch(parts);
                    break;
                case "help":
                    output = help();
                    break;
                default:
                    output = "Unknown command";
                    break;
            }
            return output;
        }

        private string help()
        {
            return "Commands:\n" +
                "cat <file> - read file\n" +
                "cd <directory> - change directory\n" +
                "clear - clear the screen\n" +
                "echo <content> - print content\n" +
                "echo <content> > <file> - write content to file\n" +
                "help - show this help message\n" +
                "kill <pid> - kill process\n" +
                "ls - list contents of current directory\n" +
                "mkdir <directory> - create directory\n" +
                "ps - list processes\n" +
                "pwd - print current directory\n" +
                "rm <file/directory> - delete file or directory\n" +
                "touch <file> - create file\n";
        }

        private string cat(string[] parts)
        {
            string output = "";
            if (parts.Length > 1)
            {
                if(checkMissingOperand(parts[1]))
                {
                    output = "Specify a file name";
                    return output;
                }
                output = _fileSystemManager.ReadFile(parts[1]);
            }
            else
            {
                output = "Specify a file";
            }
            return output;
        }

        private string cd(string[] parts)
        {
            string output = "";
            if (parts.Length > 1)
            {
                bool success = _fileSystemManager.ChangeDirectory(parts[1]);
                if (!success)
                {
                    output = "No such directory";
                }
            }
            else
            {
                output = "Specify a directory";
            }
            return output;
        }

        private string echo(string input)
        {
            string output = " ";
            // Splitting the input command by '>'
            string[] redirectionParts = input.Split(new[] { '>' }, 2); // Limiting to splitting into 2 parts

            if (redirectionParts.Length > 1)
            {
                // There's a '>' in the command, so we expect redirection to a file
                string fileName = redirectionParts[1].Trim(); // Getting the filename and removing any leading/trailing spaces

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    output = "Specify a file name";
                }
                else
                {
                    // Get the content from the original command, ignoring the "echo" part and the file name
                    string[] commandParts = redirectionParts[0].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string content = "";

                    if (commandParts.Length > 1) // Checking if there's content to write
                    {
                        for (int i = 1; i < commandParts.Length; i++) // Starting from 1 to ignore the "echo" part
                        {
                            content += commandParts[i] + " ";
                        }
                        content = content.TrimEnd(); // Remove the trailing space
                    }

                    _fileSystemManager.CreateFile(fileName, content);
                }
            }
            else
            {
                // No '>' means normal echo to the console/terminal
                string[] parts = input.Split(' ');
                if (parts.Length > 1)
                {
                    for (int i = 1; i < parts.Length; i++)
                    {
                        output += parts[i] + " ";
                    }
                    output = output.TrimEnd(); // Remove the trailing space
                }
                else
                {
                    output = "Nothing to echo";
                }
            }
            return output;
        }

        private string kill(string[] parts)
        {
            string output = "";
            if (parts.Length > 1)
            {
                int pid;
                bool success = int.TryParse(parts[1], out pid);
                if (success)
                {
                    Process process = _processManager.GetProcess(pid);
                    if (process == null)
                    {
                        output = "No such process";
                    }
                    else if (process.Name == "system")
                    {
                        output = "Cannot kill system process";
                    }
                    else
                    {
                        success = _processManager.KillProcess(pid);
                        if (!success)
                        {
                            output = "No such process";
                        }
                        else if (success)
                        {
                            OnProcessKilled?.Invoke(process);
                        }
                    }
                }
                else
                {
                    output = "Invalid PID";
                }
            }
            else
            {
                output = "Specify a PID";
            }
            return output;
        }

        private string ls()
        {
            return _fileSystemManager.ListContents();
        }

        private string mkdir(string[] parts)
        {
            string output = "";

            if (parts.Length > 1)
            {
                if (checkMissingOperand(parts[1]))
                {
                    output = "Specify a directory name";
                    return output;
                }

                if (checkDirtyCharacters(parts[1]))
                {
                    output = "Invalid directory name";
                    return output;
                }

                if (_fileSystemManager.DirectoryExists(parts[1]))
                {
                    output = "Directory already exists";
                }
                else
                {
                    _fileSystemManager.CreateDirectory(parts[1]);
                }
            }
            else
            {
                output = "Specify a directory name";
            }
            return output;
        }

        private string ps()
        {
            return _processManager.ListProcesses();
        }

        private string pwd()
        {
            return _fileSystemManager.GetCurrentPath();
        }

        private string rm(string[] parts)
        {
            string output = "";
            if (parts.Length > 1)
            {
                if (checkMissingOperand(parts[1]))
                {
                    output = "Specify a file or directory name";
                    return output;
                }
                if (!_fileSystemManager.FileExists(parts[1]) && !_fileSystemManager.DirectoryExists(parts[1]))
                {
                    output = "No such file or directory";
                    return output;
                }

                _fileSystemManager.Delete(parts[1]);
            }
            else
            {
                output = "Specify a file or directory name";
            }
            return output;
        }

        private string touch(string[] parts)
        {
            string output = "";
            if (parts.Length > 1)
            {
                if (checkMissingOperand(parts[1]))
                {
                    output = "Specify a file name";
                    return output;
                }


                _fileSystemManager.CreateFile(parts[1], "");
            }
            else
            {
                output = "Specify a file name";
            }
            return output;
        }


        private bool checkDirtyCharacters(string input)
        {
            foreach (char c in _dirtyCharacters)
            {
                if (input.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }

        private bool checkMissingOperand(string input)
        {
            if (input == "")
            {
                return true;
            }
            return false;
        }   
    }
}