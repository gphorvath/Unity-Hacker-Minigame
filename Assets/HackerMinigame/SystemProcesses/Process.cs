using System;
using System.Collections.Generic;

namespace HackerMinigame.SystemProcesses
{
    public class Process
    {
        public int PID { get; private set; }
        public string Name { get; private set; }

        public Process(int pid, string name)
        {
            PID = pid;
            Name = name;
        }
    }
}