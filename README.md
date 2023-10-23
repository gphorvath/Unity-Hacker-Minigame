# Hacking Minigame

## Description

Hacking minigame built on top of the terminal by [GameDevTV Terminal Hacker](https://github.com/CompleteUnityDeveloper2/2_Terminal_Hacker).

This is designed to be a mock terminal without any real functionality. The mock terminal gives the player the feeling of hacking and will ultimately fire an event when a goal is achieved that will trigger other aspects of the game.

## Example Goals

* Destroy the evidence - `rm` files on the system
* (TODO) Plant evidence - `cp` files to the system
* Disable security cameras, alarms, etc. - use `ps` to locate and `kill` running processes
* Find passwords / pins - locate and `cat` files on the system
* (TODO) Activate a virus or security device - `run\start` an executable file

## Commands

| Command | Implemented | Description |
| --- | --- | --- |
| cat \<file> | Yes | Read a file |
| cd \<directory> | Yes | Change directory |
| clear | Yes | Clear the screen |
| cp \<directory/file> \<directory> | No | Copy a file from one directory to another |
| echo \<content> | Yes | Print content |
| echo \<content> > \<file> | Yes | Write content to file |
| help | Yes | Show a list of commands |
| kill \<pid> | Yes | Kill the process with id \<pid> |
| ls | Yes | List contents of current directory |
| mkdir \<directory> | Yes | Create directory |
| ps | Yes | List running processes |
| pwd | Yes | Print current directory |
| rm \<file/directory> | Yes | Delete file or directory |
| run \<file> | No | Run a script once. |
| start \<file> | No | Create a process from an executable file. |
| touch \<file> | Yes | create file |

## TODO

* Pagination for commands that produce long results
* Implement `less` and `more` commands for pagination
* Implement events that fire when goals are achieved
* Implement `cp` to copy files
* Implement `run` and `start` for running scripts and processes
