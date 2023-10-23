using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HackerMinigame;

public class Hacker : MonoBehaviour
{

    private HackerMinigameWrapper _hackerMinigame;

    void Start()
    {
        _hackerMinigame = new HackerMinigameWrapper();
        Terminal.WriteLine(_hackerMinigame.ParseCommand("pwd"));       
    }

    void ShowFileSystem()
    {
        Terminal.ClearScreen();
        Terminal.WriteLine("Current directory: ");
        Terminal.WriteLine(_hackerMinigame.ParseCommand("ls"));
    }

    void OnUserInput(string input)
    {
        if(input == "clear")
        {
            Terminal.ClearScreen();
            return;
        }
        Terminal.WriteLine(_hackerMinigame.ParseCommand(input));
    }

}
