namespace SchedulerTools
{
    /// <summary>
    /// The program privilege when launched.
    /// </summary>
    public enum TaskRunLevel
    {
        /// <summary>
        /// Tasks will be run with the least privileges.
        /// </summary>
        LUA = 0,

        /// <summary>
        /// Tasks will be run with the highest privileges.
        /// </summary>
        Highest = 1,
    }
}
