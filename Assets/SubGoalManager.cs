using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class SubGoalManager
    {
        private SubGoalManager()
        {
            Reset();
        }

        private static SubGoalManager _instance;

        public static SubGoalManager GetInstance()
        {
            if(_instance == null)
                _instance = new SubGoalManager();

            return _instance;
        }

        public int NextSubGoalNumber { get; private set; }

        public void Reset()
        {
            NextSubGoalNumber = 1;
        }

        public void IncrementSubGoalNumber()
        {
            NextSubGoalNumber++;

            Debug.Log("IncrementSubGoalNumber !!!");
        }
    }
}
