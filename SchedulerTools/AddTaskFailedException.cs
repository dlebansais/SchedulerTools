namespace SchedulerTools
{
    using System;

#pragma warning disable CA1032 // Implement standard exception constructors
#pragma warning disable CA2237 // Mark ISerializable types with SerializableAttribute
    /// <summary>
    /// Represents an exception thrown when the API could not create the icon.
    /// </summary>
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
#pragma warning restore CA2237 // Mark ISerializable types with SerializableAttribute
#pragma warning restore CA1032 // Implement standard exception constructors
}
