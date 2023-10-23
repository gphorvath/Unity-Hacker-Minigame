using System;
using System.Linq;
using System.Collections.Generic;

namespace HackerMinigame.SystemProcesses
{
    public class ProcessManager
    {
        private List<Process> processes;
        private Random random;

        public ProcessManager()
        {
            processes = new List<Process>();
            random = new Random();

            // Start some default processes
            StartProcess("system");
            StartProcess("network-service");
            StartProcess("x-server");
            StartProcess("terminal");
        }

        public Process StartProcess(string name)
        {
            int randomPID;
            do
            {
                randomPID = random.Next(1000, 9999);
            }
            while (ProcessExists(randomPID)); // Ensure the PID is unique

            var newProcess = new Process(randomPID, name);
            processes.Add(newProcess);

            return newProcess;
        }

        private bool ProcessExists(int pid)
        {
            // Check if there's already a process with the given PID
            return processes.Exists(p => p.PID == pid);
        }

        public string ListProcesses()
        {
            string processList = "PID\t\tName\n";

            // Sort the processes by PID before listing them
            var sortedProcesses = processes.OrderBy(p => p.PID);
            foreach (var process in sortedProcesses)
            {
                processList += $"{process.PID}\t{process.Name}\n";
            }

            return processList;
        }

        public bool KillProcess(int pid)
        {
            var process = processes.Find(p => p.PID == pid);
            if (process != null)
            {
                processes.Remove(process);
                return true; // process killed
            }

            return false; // process not found
        }
    }
}