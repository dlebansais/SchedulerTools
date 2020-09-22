namespace SchedulerTools
{
    using System;
    using System.IO;
    using Contracts;
    using Microsoft.Win32.TaskScheduler;

    /// <summary>
    /// Represents a manager for scheduled Windows tasks.
    /// </summary>
    public static class Scheduler
    {
        #region Client Interface
        /// <summary>
        /// Adds a task that will launch a program every time someone logs in. The program must have privilege 'AsInvoker' and NOT 'Highest'.
        /// </summary>
        /// <param name="taskName">Task name, whatever you want that can be a file name.</param>
        /// <param name="exeName">The full path to the program to launch.</param>
        /// <param name="runLevel">The program privilege when launched.</param>
        /// <returns>True if successful; otherwise, false.</returns>
        public static bool AddTask(string taskName, string exeName, TaskRunLevel runLevel)
        {
            Contract.RequireNotNull(taskName, out string TaskName);
            Contract.RequireNotNull(exeName, out string ExeName);

            try
            {
                // Remove forbidden characters since the name must not contain them.
                char[] InvalidChars = Path.GetInvalidFileNameChars();
                foreach (char InvalidChar in InvalidChars)
                    TaskName = TaskName.Replace(InvalidChar, ' ');

                // Create a task that launch a program when logging in.
                using TaskService Scheduler = new TaskService();
                using Trigger LogonTrigger = Trigger.CreateTrigger(TaskTriggerType.Logon);
                using ExecAction RunAction = (ExecAction)Microsoft.Win32.TaskScheduler.Action.CreateAction(TaskActionType.Execute);
                RunAction.Path = ExeName;

                // Try with a task name (mandatory on new versions of Windows)
                if (AddTaskToScheduler(Scheduler, TaskName, LogonTrigger, RunAction, runLevel))
                    return true;

                // Try without a task name (mandatory on old versions of Windows)
                if (AddTaskToScheduler(Scheduler, null, LogonTrigger, RunAction, runLevel))
                    return true;
            }
            catch (Exception e)
            {
                throw new AddTaskFailedException(e);
            }

            return false;
        }

        /// <summary>
        /// Check if a particular task is active, by its executable name.
        /// </summary>
        /// <param name="exeName">Task executable name.</param>
        /// <returns>True if active; otherwise, false.</returns>
        public static bool IsTaskActive(string exeName)
        {
            Contract.RequireNotNull(exeName, out string ExeName);

            bool IsFound = false;
            EnumTasks(ExeName, OnList, ref IsFound);
            return IsFound;
        }

        /// <summary>
        /// Remove an active task.
        /// </summary>
        /// <param name="exeName">Task executable name.</param>
        /// <param name="isFound">True if found (and removed), false if not found.</param>
        public static void RemoveTask(string exeName, out bool isFound)
        {
            Contract.RequireNotNull(exeName, out string ExeName);

            isFound = false;
            EnumTasks(ExeName, OnRemove, ref isFound);
        }
        #endregion

        #region Implementation
        private delegate void EnumTaskHandler(Task task, ref bool returnValue);

        private static void OnList(Task task, ref bool returnValue)
        {
            Trigger LogonTrigger = task.Definition.Triggers[0];
            if (LogonTrigger.Enabled)
                returnValue = true;
        }

        private static void OnRemove(Task task, ref bool returnValue)
        {
            TaskService Scheduler = task.TaskService;
            TaskFolder RootFolder = Scheduler.RootFolder;
            RootFolder.DeleteTask(task.Name, false);
        }

        private static void EnumTasks(string exeName, EnumTaskHandler handler, ref bool returnValue)
        {
            string ProgramName = Path.GetFileName(exeName);

            try
            {
                using TaskService Scheduler = new TaskService();

                foreach (Task t in Scheduler.AllTasks)
                {
                    try
                    {
                        TaskDefinition Definition = t.Definition;
                        if (Definition.Actions.Count != 1 || Definition.Triggers.Count != 1)
                            continue;

                        if (!(Definition.Actions[0] is ExecAction AsExecAction))
                            continue;

                        if (!AsExecAction.Path.EndsWith(ProgramName, StringComparison.InvariantCulture) || Path.GetFileName(AsExecAction.Path) != ProgramName)
                            continue;

                        handler(t, ref returnValue);
                        return;
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                    }
                }
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
            }
        }

        private static bool AddTaskToScheduler(TaskService scheduler, string? taskName, Trigger logonTrigger, ExecAction runAction, TaskRunLevel runLevel)
        {
            try
            {
                Task task = scheduler.AddTask(taskName, logonTrigger, runAction);
                task.Definition.Principal.RunLevel = runLevel;

                Task newTask = scheduler.RootFolder.RegisterTaskDefinition(taskName, task.Definition, TaskCreation.CreateOrUpdate, null, null, TaskLogonType.None, null);
                return true;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return false;
            }
        }
        #endregion
    }
}
