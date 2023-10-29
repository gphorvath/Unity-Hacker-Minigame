using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HackerMinigame.SystemProcesses;
using HackerMinigame.FileSystem;

namespace HackerMinigame
{
    public class HackingEventSubscriber : MonoBehaviour
    {
        private void OnEnable()
        {
            FindObjectOfType<HackerMinigameManager>().OnProcessKilled += OnProcessKilled;
        }

        private void OnDisable()
        {
            FindObjectOfType<HackerMinigameManager>().OnProcessKilled -= OnProcessKilled;
        }

        public void OnProcessKilled(Process process)
        {
            Terminal.WriteLine($"Process {process.Name} killed");
        }
    }
}