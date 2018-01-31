using System;

namespace Assets
{
    /// <summary>
    /// class that manages the current sub goal counter as a singleton instance
    /// </summary>
    public class SubGoalManager
    {
        // Constructors

        private SubGoalManager()
        {
            Reset();
        }

        // Fields

        private static SubGoalManager _instance;

        // Properties

        public int NextSubGoalNumber { get; private set; }

        // Methods

        public static SubGoalManager GetInstance()
        {
            if(_instance == null)
                _instance = new SubGoalManager();

            return _instance;
        }        

        /// <summary>
        /// Resets the current sub goal number to start value
        /// </summary>
        public void Reset()
        {
            NextSubGoalNumber = 1;
        }

        /// <summary>
        /// Allows to increment the current sub goal number from outside
        /// </summary>
        public void IncrementSubGoalNumber()
        {
            NextSubGoalNumber++;
        }
    }
}