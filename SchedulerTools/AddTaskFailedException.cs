namespace SchedulerTools;

using System;

/// <summary>
/// Represents an exception thrown when the API could not create the icon.
/// </summary>
[Serializable]
#pragma warning disable CA1032 // Implement standard exception constructors
public class AddTaskFailedException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
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
