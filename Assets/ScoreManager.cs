using System;
using System.Collections;
using System.Collections.Generic;
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

    void OnCollisionEnter(Collision collisionInfo)
    {

    }

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
	
	void FixedUpdate ()
	{
	    if (_gameEnded)
	    {
	        OVRInput.FixedUpdate();
            upperTextLabelLeft.text = string.Format("Press touchpad to restart{0}time was: {1}:{2}", Environment.NewLine,
                _gameEndTimeSpan.Minutes < 10 ? "0" + _gameEndTimeSpan.Minutes : _gameEndTimeSpan.Minutes.ToString(),
                _gameEndTimeSpan.Seconds < 10 ? "0" + _gameEndTimeSpan.Seconds : _gameEndTimeSpan.Seconds.ToString());
	        if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad))
	        {
	            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	            SubGoalManager.GetInstance().Reset();
	        }
	    }
	    else
	    {
	        var referenceTime = DateTime.UtcNow;
	        var timeSpan = referenceTime - startTime;
	        upperTextLabelLeft.text = string.Format("{0} points{1}time: {2}:{3}", points, Environment.NewLine, timeSpan.Minutes < 10 ? "0" + timeSpan.Minutes : timeSpan.Minutes.ToString(),
                timeSpan.Seconds < 10 ? "0" + timeSpan.Seconds : timeSpan.Seconds.ToString());
        }	    
    }
}
