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
    public float yawRotationSpeed = DEFAULT_ROTATION_SPEED;
    public float pitchRotationSpeed = DEFAULT_ROTATION_SPEED;
    public float rollRotationSpeed = DEFAULT_ROTATION_SPEED;

    [Header("Deadzone")]
    [Range(0f, 0.99f)]
    public float innerDeadzoneValue = 0.01f;
    [Range(0.01f, 1f)]
    public float outerDeadzoneValue = 0.25f;

    [Header("Text-Output elements")]

    public Text moveTextLabel;
    //public Text moveTextLabel2;

    //public Text upperTextLabelRight;

    [Header("Slider-Output elements")]
    
    public Transform XUpTransform;
    public Transform XDownTransform;

    public Transform YUpTransform;
    public Transform YDownTransform;

    public Transform ZUpTransform;
    public Transform ZDownTransform;

    //[Header("Joystick-Output elements")]
    //public Transform joystockTransform;

    // Fields
    
    private float currentSpeedFactor = 0;

    //private float lastControllerFix = 0;

    private bool movementEnabled = false;
    private float lastMovementEnabledChange = 0;

    // Methods

    void FixedUpdate () {
        OVRInput.FixedUpdate();

        //var rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
        var rotation = GetRotationQuaternion();        

        // movement can be disabled to relax hand and give the user the option of letting the controller go w/o affecting flight
        if (movementEnabled)
        {
            var yawFactor = CalculateYawFactor(rotation);
            var pitchFactor = CalculatePitchFactor(rotation);
            var rollFactor = CalculateRollFactor(rotation);

            // do rotate
            transform.Rotate(yawFactor * yawRotationSpeed, pitchFactor * pitchRotationSpeed, rollFactor * rollRotationSpeed);

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
                    ZUpTransform.localScale = new Vector3(rollFactor * -2, 1, 1);
                    ZDownTransform.localScale = new Vector3(0, 1, 1);
                }
                else
                {
                    ZUpTransform.localScale = new Vector3(0, 1, 1);
                    ZDownTransform.localScale = new Vector3(rollFactor * -2, 1, 1);
                }
            }

            // TODO: remove debug
            //upperTextLabelRight.text = string.Format("X: {0:N2}{1}Y: {2:N2}{3}Z: {4:N2}", rotation.x, Environment.NewLine,
            //        rotation.y, Environment.NewLine, rotation.z);

            //if (joystockTransform != null)
            //{
            //    joystockTransform.eulerAngles = new Vector3(yawFactor, pitchFactor, rollFactor);
            //}
            
            if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) && Time.time - lastMovementEnabledChange > 0.5f)
            {
                movementEnabled = false;
                lastMovementEnabledChange = Time.time;
            }
        }
        else
        {
            //upperTextLabelRight.text = string.Format("X: -{0}Y: -{1}Z: -", Environment.NewLine, Environment.NewLine);

            if (OVRInput.Get(OVRInput.Button.PrimaryTouchpad) && Time.time - lastMovementEnabledChange > 0.5f)
            {
                //Debug.Log("enabled movement");
                movementEnabled = true;
                lastMovementEnabledChange = Time.time;
            }
        }

        float targetSpeedFactor = 0;

        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
	    {
	        var speedTouchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
	        //targetSpeedFactor = speedTouchPosition.x < 0.4f ? speedTouchPosition.y : 0;
	        targetSpeedFactor = speedTouchPosition.y + 0.5f;

	        currentSpeedFactor = UpdateSpeedFactor(currentSpeedFactor, targetSpeedFactor);

	        moveTextLabel.text = string.Format("moving-X: {0:N2}{1}moving-Y: {2:N2}{3}targetSF: {4:N2}{5}speedFactor: {6:N2}", 
                speedTouchPosition.x, Environment.NewLine, speedTouchPosition.y, Environment.NewLine, targetSpeedFactor,
                Environment.NewLine, currentSpeedFactor);
        }
	    else
        {
            currentSpeedFactor = UpdateSpeedFactor(currentSpeedFactor, targetSpeedFactor);
            
	        moveTextLabel.text = string.Format("moving-X: n/a{0}moving-Y: n/a{1}targetSF: {2:N2}{3}speedFactor: {4:N2}",
	            Environment.NewLine, Environment.NewLine, targetSpeedFactor, Environment.NewLine, currentSpeedFactor);
        }

        ApplyMovementTranslation(transform);
    }

    private Quaternion GetRotationQuaternion()
    {
        // get active controller, either left- or right-hand based
        var activeController = OVRInput.GetActiveController();

        // get gyroscope instead of accelerometer values because more precise AND based on orientation, not movement.
        // This is important because values have origin in calibrated null value (holding position?)
        return OVRInput.GetLocalControllerRotation(activeController);
        //var rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
    }

    private float CalculateAdjustedRotationFactor(float factor)
    {
        if (factor > -innerDeadzoneValue && factor < innerDeadzoneValue)
            return 0;
        if (factor < -outerDeadzoneValue)
            return -outerDeadzoneValue;
        if (factor > outerDeadzoneValue)
            return outerDeadzoneValue;
        return factor;
    }

    private float CalculateYawFactor(Quaternion rotation)
    {
        return CalculateAdjustedRotationFactor(rotation.x);
    }

    private float CalculatePitchFactor(Quaternion rotation)
    {
        return CalculateAdjustedRotationFactor(rotation.y);
    }

    private float CalculateRollFactor(Quaternion rotation)
    {
        return CalculateAdjustedRotationFactor(rotation.z);
    }

    private float UpdateSpeedFactor(float previousSpeed, float targetSpeed)
    {
        if (targetSpeed > previousSpeed)
        {
            return previousSpeed + SpeedFactorStep;
        }
        if (targetSpeed < previousSpeed)
        {
            return previousSpeed - SpeedFactorStep;
        }
        return previousSpeed;
    }

    private void ApplyMovementTranslation(Transform transform)
    {
        transform.position += transform.forward * currentSpeedFactor * 60 * Time.deltaTime;
    }
}