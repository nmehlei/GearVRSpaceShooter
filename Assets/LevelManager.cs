using System;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class LevelManager
    {
        // Constructors

        private LevelManager()
        {
            
        }

        // Fields

        private static LevelManager _instance;

        // Methods

        public static LevelManager GetInstance()
        {
            if(_instance != null)
                _instance = new LevelManager();

            return _instance;
        }

        /// <summary>
        /// Basically reset the whole game, including object positions and states aswell as accumulated points and timers
        /// </summary>
        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SubGoalManager.GetInstance().Reset();
        }
    }
}