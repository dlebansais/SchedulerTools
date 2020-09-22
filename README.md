# SchedulerTools
Manage tasks in the Windows scheduler.

[![Build status](https://ci.appveyor.com/api/projects/status/olu0fg1v4a329k5v?svg=true)](https://ci.appveyor.com/project/dlebansais/schedulertools)

## Requirements

This tool requires .NET Framework 4.8. 

## Creating a task.

Provide a task name and the path to the executable file. You must also choose if you want the task to be launched at normal level or as administrator.

````
Scheduler.AddTask("My task name", "C:\mytask.exe", TaskRunLevel.Highest);
````

## Checking if a task is scheduled to run

Call `IsTaskActive` with the path to the task executable file.

## Removing a task

To remove a task from the scheduler, making sure it won't be started again, call `RemoveTask` with the path to the task executable file.

Note: this won't stop the program if already launched.
