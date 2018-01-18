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

        public void ResetLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            SubGoalManager.GetInstance().Reset();
        }
    }
}