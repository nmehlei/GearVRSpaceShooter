using System;
using UnityEngine;
using UnityEngine.UI;

public class ShipRotateController : MonoBehaviour
{
    // Constants

    public static readonly float DefaultBaseMovementSpeed = 60;
    public static readonly float DEFAULT_ROTATION_SPEED = 1.5f;
    public static readonly float DefaultAcceleration = 0.015f;

    //private const float Acceleration = 0.015f;

    // Unity fields

    [Header("Speed")]
    public float BaseMovementSpeed = DefaultBaseMovementSpeed;
    public float Acceleration = DefaultAcceleration;
    public float yawRotationSpeed = DEFAULT_ROTATION_SPEED;
    public float pitchRotationSpeed = DEFAULT_ROTATION_SPEED;
    public float rollRotationSpeed = DEFAULT_ROTATION_SPEED;

    [Header("Speed Steps")]
    public int ForwardSpeedSteps = 8;
    public int BackwardSpeedSteps = 4;

    [Header("Deadzone")]
    [Range(0f, 0.99f)]
    public float innerDeadzoneValue = 0.01f;
    [Range(0.01f, 1f)]
    public float outerDeadzoneValue = 0.25f;

    //[Header("Text-Output elements")]

    //public Text moveTextLabel;
    //public Text moveTextLabel2;

    //public Text upperTextLabelRight;

    [Header("Slider properties")]
    
    public Transform XUpTransform;
    public Transform XDownTransform;

    public Transform YUpTransform;
    public Transform YDownTransform;

    public Transform ZUpTransform;
    public Transform ZDownTransform;

    public Transform CurrentSpeedTransform;
    public Transform TargetSpeedTransform;
    public float ScaleValuePerSpeedStep;

    // Fields
    
    //private bool movementEnabled = true;
    //private float lastMovementEnabledChange = 0;
    private float currentSpeedFactor = 0;
    private float targetSpeedFactor = 0;

    // Methods

    void FixedUpdate () {
        OVRInput.FixedUpdate();

        //var rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
        var rotation = GetRotationQuaternion();        

        // movement can be disabled to relax hand and give the user the option of letting the controller go w/o affecting flight
        if (true) //movementEnabled
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

            //if (OVRInput.Get(OVRInput.Button.Back) && Time.time - lastMovementEnabledChange > 0.5f)
            //{
                //movementEnabled = false;
                //lastMovementEnabledChange = Time.time;
            //}
        }
        //else
        //{
            //if (OVRInput.Get(OVRInput.Button.Back) && Time.time - lastMovementEnabledChange > 0.5f)
            //{
                //movementEnabled = true;
                //lastMovementEnabledChange = Time.time;
            //}
        //}

        //float targetSpeedFactor = 0;

        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
        {
            int stepSum = ForwardSpeedSteps + BackwardSpeedSteps;
            var speedTouchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
	        //targetSpeedFactor = speedTouchPosition.x < 0.4f ? speedTouchPosition.y : 0;
            // + 1 to only have positive values, divided by 2 to have range 0 to 1
	        targetSpeedFactor = (speedTouchPosition.y + 1) / 2;
	        targetSpeedFactor = targetSpeedFactor * stepSum;
	        targetSpeedFactor -= BackwardSpeedSteps;

	        //currentSpeedFactor = UpdateSpeedFactor(currentSpeedFactor, targetSpeedFactor);

	        //moveTextLabel.text = string.Format("moving-X: {0:N2}{1}moving-Y: {2:N2}{3}targetSF: {4:N2}{5}speedFactor: {6:N2}", 
            //    speedTouchPosition.x, Environment.NewLine, speedTouchPosition.y, Environment.NewLine, targetSpeedFactor,
            //    Environment.NewLine, currentSpeedFactor);
        }
	    else
        {
            //moveTextLabel.text = string.Format("moving-X: n/a{0}moving-Y: n/a{1}targetSF: {2:N2}{3}speedFactor: {4:N2}",
            //    Environment.NewLine, Environment.NewLine, targetSpeedFactor, Environment.NewLine, currentSpeedFactor);
        }

        currentSpeedFactor = UpdateSpeedFactor(currentSpeedFactor, targetSpeedFactor);

        if (CurrentSpeedTransform != null)
        {
            CurrentSpeedTransform.localScale = new Vector3(1, currentSpeedFactor * ScaleValuePerSpeedStep, 1);
        }
        if (TargetSpeedTransform != null)
        {
            TargetSpeedTransform.localScale = new Vector3(1, targetSpeedFactor * ScaleValuePerSpeedStep, 1);
        }

        ApplyMovementTranslation(transform, BaseMovementSpeed);
    }

    private Quaternion GetRotationQuaternion()
    {
        // get active controller, either left- or right-hand based
        //var activeController = OVRInput.GetActiveController();

        // get gyroscope instead of accelerometer values because more precise AND based on orientation, not movement.
        // This is important because values have origin in calibrated null value (holding position?)
        //return OVRInput.GetLocalControllerRotation(activeController);
        //var rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
        return OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTrackedRemote);
    }

    private float CalculateAdjustedRotationFactor(float factor)
    {
        // factor in inner and outer deadzones, inner to correct for "jitter" and outer to handle out-of-bounds input ranges

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
            return previousSpeed + Acceleration;
        }
        if (targetSpeed < previousSpeed)
        {
            return previousSpeed - Acceleration;
        }
        return previousSpeed;
    }

    private void ApplyMovementTranslation(Transform transform, float baseMovementSpeed)
    {
        transform.position += transform.forward * currentSpeedFactor * baseMovementSpeed * Time.deltaTime;
    }
}