using System;

namespace Assets
{
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
        /// 
        /// </summary>
        public void Reset()
        {
            NextSubGoalNumber = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        public void IncrementSubGoalNumber()
        {
            NextSubGoalNumber++;
        }
    }
}