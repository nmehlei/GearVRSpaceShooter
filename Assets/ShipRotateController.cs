using System;
using UnityEngine;
using UnityEngine.UI;

public class ShipRotateController : MonoBehaviour
{
    // Constants

    private static readonly float DEFAULT_ROTATION_SPEED = 1.5f;

    private const float SpeedFactorStep = 0.005f;

    // Unity fields

    [Header("Speed")]
    public float rotationSpeed = DEFAULT_ROTATION_SPEED;

    [Header("Deadzone")]
    [Range(0f, 0.99f)]
    public float innerDeadzoneValue = 0.01f;
    [Range(0.01f, 1f)]
    public float outerDeadzoneValue = 0.25f;

    [Header("Text-Output elements")]

    public Text moveTextLabel;
    public Text moveTextLabel2;

    public Text upperTextLabelRight;

    [Header("Slider-Output elements")]
    
    public Transform XUpTransform;
    public Transform XDownTransform;

    public Transform YUpTransform;
    public Transform YDownTransform;

    public Transform ZUpTransform;
    public Transform ZDownTransform;

    [Header("Joystick-Output elements")]
    public Transform joystockTransform;

    // Fields
    
    private float currentSpeedFactor = 0;

    private float lastControllerFix = 0;

    private bool movementEnabled = false;
    private float lastMovementEnabledChange = 0;

    // Methods

    void FixedUpdate () {
        OVRInput.FixedUpdate();

        var rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
        // get gyroscope instead of accelerometer values because more precise AND based on orientation, not movement.
        // This is important because values have origin in calibrated null value (holding position?)

        // movement can be disabled to relax hand and give the user the option of letting the controller go w/o affecting flight
        if (movementEnabled)
        {
            // horizontal pitch-yaw or sth
            var yawFactor = rotation.x;
            if (yawFactor > -innerDeadzoneValue && yawFactor < innerDeadzoneValue)
                yawFactor = 0;
            if (yawFactor < -outerDeadzoneValue)
                yawFactor = -outerDeadzoneValue;
            if (yawFactor > outerDeadzoneValue)
                yawFactor = outerDeadzoneValue;

            // pitchFactor
            var pitchFactor = rotation.y;
            if (pitchFactor > -innerDeadzoneValue && pitchFactor < innerDeadzoneValue)
                pitchFactor = 0;
            if (pitchFactor < -outerDeadzoneValue)
                pitchFactor = -outerDeadzoneValue;
            if (pitchFactor > outerDeadzoneValue)
                pitchFactor = outerDeadzoneValue;

            // roll
            var rollFactor = rotation.z;
            if (rollFactor > -innerDeadzoneValue && rollFactor < innerDeadzoneValue)
                rollFactor = 0;
            if (rollFactor < -outerDeadzoneValue)
                rollFactor = -outerDeadzoneValue;
            if (rollFactor > outerDeadzoneValue)
                rollFactor = outerDeadzoneValue;

            // do rotate
            transform.Rotate(yawFactor * rotationSpeed, pitchFactor * rotationSpeed, rollFactor * rotationSpeed);

            // update rotation sliders
            if (YUpTransform != null && YDownTransform != null)
            {
                if (yawFactor > 0)
                {
                    YUpTransform.localScale = new Vector3(1, yawFactor * 2, 1); //TODO: weird values
                    YDownTransform.localScale = new Vector3(1, 0, 1);
                }
                else
                {
                    YUpTransform.localScale = new Vector3(1, 0, 1);
                    YDownTransform.localScale = new Vector3(1, yawFactor * 2, 1);
                }
            }

            if (XUpTransform != null && XDownTransform != null)
            {
                if (pitchFactor > 0)
                {
                    XUpTransform.localScale = new Vector3(pitchFactor * 2, 1, 1);
                    XDownTransform.localScale = new Vector3(0, 1, 1);
                }
                else
                {
                    XUpTransform.localScale = new Vector3(0, 1, 1);
                    XDownTransform.localScale = new Vector3(pitchFactor * 2, 1, 1);
                }
            }

            if (ZUpTransform != null && ZDownTransform != null)
            {
                if (rollFactor > 0)
                {
                    ZUpTransform.localScale = new Vector3(rollFactor * 2, 1, 1);
                    ZDownTransform.localScale = new Vector3(0, 1, 1);
                }
                else
                {
                    ZUpTransform.localScale = new Vector3(0, 1, 1);
                    ZDownTransform.localScale = new Vector3(rollFactor * 2, 1, 1);
                }
            }

            upperTextLabelRight.text = string.Format("X: {0:N2}{1}Y: {2:N2}{3}Z: {4:N2}", rotation.x, Environment.NewLine,
                    rotation.y, Environment.NewLine, rotation.z);

            if (joystockTransform != null)
            {
                joystockTransform.eulerAngles = new Vector3(yawFactor, pitchFactor, rollFactor);
            }
            
            if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) && Time.time - lastMovementEnabledChange > 0.5f)
            {
                movementEnabled = false;
                lastMovementEnabledChange = Time.time;
            }
        }
        else
        {
            upperTextLabelRight.text = string.Format("X: -{0}Y: -{1}Z: -", Environment.NewLine, Environment.NewLine);

            if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) && Time.time - lastMovementEnabledChange > 0.5f)
            {
                Debug.Log("enabled movement");
                movementEnabled = true;
                lastMovementEnabledChange = Time.time;
            }
        }

        float targetSpeedFactor = 0;

        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
	    {
	        var speedTouchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
	        targetSpeedFactor = speedTouchPosition.x < 0.4f ? speedTouchPosition.y : 0;

	        if (targetSpeedFactor > currentSpeedFactor)
	        {
	            currentSpeedFactor += SpeedFactorStep;
	        }
            else if (targetSpeedFactor < currentSpeedFactor)
	        {
	            currentSpeedFactor -= SpeedFactorStep;
            }

	        moveTextLabel.text = string.Format("moving-X: {0:N2}{1}moving-Y: {2:N2}{3}targetSF: {4:N2}{5}speedFactor: {6:N2}", 
                speedTouchPosition.x, Environment.NewLine, speedTouchPosition.y, Environment.NewLine, targetSpeedFactor,
                Environment.NewLine, currentSpeedFactor);
        }
	    else
	    {
	        if (targetSpeedFactor > currentSpeedFactor)
	        {
	            currentSpeedFactor += SpeedFactorStep;
	        }
	        else if (targetSpeedFactor < currentSpeedFactor)
	        {
	            currentSpeedFactor -= SpeedFactorStep;
	        }

	        moveTextLabel.text = string.Format("moving-X: n/a{0}moving-Y: n/a{1}targetSF: {2:N2}{3}speedFactor: {4:N2}",
	            Environment.NewLine, Environment.NewLine, targetSpeedFactor, Environment.NewLine, currentSpeedFactor);
        }

        transform.position += transform.forward * currentSpeedFactor;
    }
}