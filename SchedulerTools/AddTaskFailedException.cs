namespace SchedulerTools;

using System;

/// <summary>
/// Represents an exception thrown when the API could not create the icon.
/// </summary>
[Serializable]
public class AddTaskFailedException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddTaskFailedException"/> class.
    /// </summary>
    /// <param name="innerException">The exception that is the cause of the current exception.</param>
    internal AddTaskFailedException(Exception innerException)
        : base(innerException?.Message, innerException)
    {
    }
}
