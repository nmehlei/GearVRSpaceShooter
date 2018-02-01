using System;
using Assets;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the current score of the game and resets it if requested
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // Unity Properties

    [Tooltip("Label for displaying current score aswell as tips.")]
    public Text TextLabel;

    [Tooltip("Current points (or start points at design time). Has to be lower than MaxSubGoalNumber.")]
    public int Points = 0;

    [Tooltip("Maximal amount of sub goals.")]
    public int MaxSubGoalNumber = 10;

    // Fields

    private DateTime _startTime;    
    private bool _gameEnded;
    private TimeSpan _gameEndTimeSpan;
    private bool _restarted = false;

    // Methods

    void OnTriggerEnter(Collider other)
    {
        var subGoal = other.gameObject.GetComponent<SubGoal>();

        // only trigger-collisions with subgoals are of interest here, so ignore the rest
        if (subGoal != null)
        {
            var subGoalManager = SubGoalManager.GetInstance();
            var nextSubGoalNumber = subGoalManager.NextSubGoalNumber;

            // if the player went through the current sub goal ..
            if (subGoal.SubGoalNumber == nextSubGoalNumber)
            {
                // then switch to the next one
                Points++;
                subGoalManager.IncrementSubGoalNumber();

                // if it was the last one, then set flag for game end
                if (subGoal.SubGoalNumber == MaxSubGoalNumber)
                {
                    _gameEnded = true;
                    _gameEndTimeSpan = DateTime.UtcNow - _startTime;
                }
            }
        }        
    }
    
    void Start ()
    {
        _startTime = DateTime.UtcNow;
    }

	void FixedUpdate ()
	{
	    OVRInput.FixedUpdate();

        if (_gameEnded)
	    {
	        if (!_restarted)
	        {
	            TextLabel.text = string.Format("Press trigger to restart{0}Your time was: {1}", Environment.NewLine,
	                _gameEndTimeSpan.ToCounterTimeString());

	            // wait for user input so user has time to read his/her time/score
	            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
	            {
	                TextLabel.text = "restarting";
	                ResetLevel();
                    _restarted = true;
	            }
            }
	    }
	    else
	    {
	        var referenceTime = DateTime.UtcNow;
	        var timeSpan = referenceTime - _startTime;

	        TextLabel.text = string.Format("Points: {0}{1}Time: {2}", Points, Environment.NewLine, timeSpan.ToCounterTimeString());

            // resets level as soon as back button was pressed and released
	        if (OVRInput.GetUp(OVRInput.Button.Back))
	        {
	            ResetLevel();
            }
        }	    
    }

    /// <summary>
    /// Basically reset the whole game, including object positions and states aswell as accumulated points and timers
    /// </summary>
    private void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SubGoalManager.GetInstance().Reset();
    }
}