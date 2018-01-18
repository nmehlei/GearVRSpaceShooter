using System;
using Assets;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private DateTime startTime;

    public Text upperTextLabelLeft;
    public int points = 0;
    //public int nextSubGoalNumber = 1;
    public int maxSubGoalNumber = 10;

    private bool _gameEnded;
    private TimeSpan _gameEndTimeSpan;
    
    void OnTriggerEnter(Collider other)
    {
        var subGoal = other.gameObject.GetComponent<SubGoal>();

        if (subGoal != null)
        {
            var subGoalManager = SubGoalManager.GetInstance();
            var nextSubGoalNumber = subGoalManager.NextSubGoalNumber;
            if (subGoal.subGoalNumber == nextSubGoalNumber)
            {
                points++;
                subGoalManager.IncrementSubGoalNumber();

                // if last one
                if (subGoal.subGoalNumber == maxSubGoalNumber)
                {
                    _gameEnded = true;
                    _gameEndTimeSpan = DateTime.UtcNow - startTime;
                }
            }
        }        
    }
    
    void Start ()
    {
        startTime = DateTime.UtcNow;
    }

    private bool restarted = false;
	void FixedUpdate ()
	{
	    if (_gameEnded)
	    {
	        OVRInput.FixedUpdate();

	        if (!restarted)
	        {
	            upperTextLabelLeft.text = string.Format("Press trigger to restart{0}Your time was: {1}:{2}", Environment.NewLine,
	                _gameEndTimeSpan.Minutes < 10 ? "0" + _gameEndTimeSpan.Minutes : _gameEndTimeSpan.Minutes.ToString(),
	                _gameEndTimeSpan.Seconds < 10 ? "0" + _gameEndTimeSpan.Seconds : _gameEndTimeSpan.Seconds.ToString());

	            // wait for user input so user has time to read his/her time/score
	            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
	            {
	                upperTextLabelLeft.text = "restarting";
                    //var levelManager = LevelManager.GetInstance();
                    //levelManager.ResetLevel();
	                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	                SubGoalManager.GetInstance().Reset();
                    restarted = true;
	            }
            }
	    }
	    else
	    {
	        var referenceTime = DateTime.UtcNow;
	        var timeSpan = referenceTime - startTime;

	        upperTextLabelLeft.text = string.Format("Points: {0}{1}Time: {2}:{3}", points, Environment.NewLine, timeSpan.Minutes < 10 ? "0" + timeSpan.Minutes : timeSpan.Minutes.ToString(),
                timeSpan.Seconds < 10 ? "0" + timeSpan.Seconds : timeSpan.Seconds.ToString());
        }	    
    }
}