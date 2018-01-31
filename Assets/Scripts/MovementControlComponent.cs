using System;
using UnityEngine;

public class MovementControlComponent : MonoBehaviour
{
    // Constants

    public static readonly float DefaultBaseMovementSpeed = 60;
    public static readonly float DefaultRotationSpeed = 1.5f;
    public static readonly float DefaultAcceleration = 0.015f;
    
    // Unity fields

    [Header("Speed")]
    [Tooltip("Base reference speed for forward and backward movement, not rotation.")]
    public float BaseMovementSpeed = DefaultBaseMovementSpeed;
    [Tooltip("Amount of speed that can be gained or lost during one game tick.")]
    public float Acceleration = DefaultAcceleration;
    [Tooltip("Rotation speed for the Yaw axis.")]
    public float YawRotationSpeed = DefaultRotationSpeed;
    [Tooltip("Rotation speed for the Pitch axis.")]
    public float PitchRotationSpeed = DefaultRotationSpeed;
    [Tooltip("Rotation speed for the Roll axis.")]
    public float RollRotationSpeed = DefaultRotationSpeed;

    [Header("Speed Steps")]
    [Tooltip("")]
    public int ForwardSpeedSteps = 8;
    [Tooltip("")]
    public int BackwardSpeedSteps = 4;

    [Header("Deadzone")]
    [Range(0f, 0.99f)]
    [Tooltip("")]
    public float InnerDeadzoneValue = 0.01f;
    [Range(0.01f, 1f)]
    [Tooltip("")]
    public float OuterDeadzoneValue = 0.25f;
    
    [Header("Slider properties")]

    [Tooltip("")]
    public Transform XUpTransform;
    [Tooltip("")]
    public Transform XDownTransform;

    [Tooltip("")]
    public Transform YUpTransform;
    [Tooltip("")]
    public Transform YDownTransform;

    [Tooltip("")]
    public Transform ZUpTransform;
    [Tooltip("")]
    public Transform ZDownTransform;

    [Tooltip("")]
    public Transform CurrentSpeedTransform;
    [Tooltip("")]
    public Transform TargetSpeedTransform;
    [Tooltip("")]
    public float ScaleValuePerSpeedStep;

    [Tooltip("")]
    public Transform MiniShipTransform;

    [Header("Misc")]
    [Tooltip("Allows to invert the Y axis if preferred.")]
    public bool InvertY;

    [Tooltip("Use right handed controller. Otherwise use left handed.")]
    public bool UseRightHandedController = true;

    // Fields
    
    private bool _movementEnabled = true;
    private float _currentSpeedFactor = 0;
    private float _targetSpeedFactor = 0;

    // Methods

    void FixedUpdate () {
        OVRInput.FixedUpdate();

        HandleRotation();
        HandleSpeed();   
    }

    /// <summary>
    /// calculates current rotational input and updates ship orientation aswell as cockpit UI elements accordingly
    /// </summary>
    private void HandleRotation()
    {
        // movement can be disabled to relax hand and give the user the option of letting the controller go w/o affecting flight
        if (_movementEnabled)
        {
            var rotation = GetRotationQuaternion();

            var yawFactor = CalculateYawFactor(rotation);
            var pitchFactor = CalculatePitchFactor(rotation);
            var rollFactor = CalculateRollFactor(rotation);

            // do rotate, based on specified rotation speeds
            transform.Rotate(yawFactor * YawRotationSpeed, pitchFactor * PitchRotationSpeed, rollFactor * RollRotationSpeed);

            // update UI elements inside cockpit
            UpdateRotationSliders(yawFactor, pitchFactor, rollFactor);
            UpdateMiniShipHologram(yawFactor, pitchFactor, rollFactor);

            // check if user requested movement disabling
            if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
            {
                DisableMovement();
            }
        }
        else
        {
            // check if user requested movement enabling
            if (OVRInput.GetUp(OVRInput.Button.PrimaryTouchpad))
            {
                EnableMovement();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="yawFactor">current relative yaw factor</param>
    /// <param name="pitchFactor">current relative pitch factor</param>
    /// <param name="rollFactor">current relative roll factor</param>
    private void UpdateRotationSliders(float yawFactor, float pitchFactor, float rollFactor)
    {
        // update rotation sliders
        if (YUpTransform != null && YDownTransform != null)
        {
            //if (yawFactor > 0)
            //{
                YUpTransform.localScale = new Vector3(1, yawFactor * 2, 1); //TODO: weird values
                YDownTransform.localScale = new Vector3(1, 0, 1);
            /*}
            else
            {
                YUpTransform.localScale = new Vector3(1, 0, 1);
                YDownTransform.localScale = new Vector3(1, yawFactor * 2, 1);
            }*/
        }

        if (XUpTransform != null && XDownTransform != null)
        {
            //if (pitchFactor > 0)
            //{
                XUpTransform.localScale = new Vector3(pitchFactor * 2, 1, 1);
                XDownTransform.localScale = new Vector3(0, 1, 1);
            /*}
            else
            {
                XUpTransform.localScale = new Vector3(0, 1, 1);
                XDownTransform.localScale = new Vector3(pitchFactor * 2, 1, 1);
            }*/
        }

        if (ZUpTransform != null && ZDownTransform != null)
        {
            //if (rollFactor > 0)
            //{
                ZUpTransform.localScale = new Vector3(rollFactor * -2, 1, 1);
                ZDownTransform.localScale = new Vector3(0, 1, 1);
            /*}
            else
            {
                ZUpTransform.localScale = new Vector3(0, 1, 1);
                ZDownTransform.localScale = new Vector3(rollFactor * -2, 1, 1);
            }*/
        }
    }

    /// <summary>
    /// updates the hologram inside the cockpit which shows the current relative orientation of the input
    /// </summary>
    /// <param name="yawFactor">current relative yaw factor</param>
    /// <param name="pitchFactor">current relative pitch factor</param>
    /// <param name="rollFactor">current relative roll factor</param>
    private void UpdateMiniShipHologram(float yawFactor, float pitchFactor, float rollFactor)
    {        
        if (MiniShipTransform == null) return;

        // update the mini ship orientation
        MiniShipTransform.localRotation = new Quaternion(yawFactor, pitchFactor, rollFactor, 1);
    }

    /// <summary>
    /// 
    /// </summary>
    private void HandleSpeed()
    {
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
        {
            int stepSum = ForwardSpeedSteps + BackwardSpeedSteps;
            var speedTouchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);
            // + 1 to only have positive values, divided by 2 to have range 0 to 1
            _targetSpeedFactor = (speedTouchPosition.y + 1) / 2;
            _targetSpeedFactor = _targetSpeedFactor * stepSum;
            _targetSpeedFactor -= BackwardSpeedSteps; //TODO wat
        }

        _currentSpeedFactor = UpdateSpeedFactor(_currentSpeedFactor, _targetSpeedFactor);

        if (CurrentSpeedTransform != null)
        {
            CurrentSpeedTransform.localScale = new Vector3(1, _currentSpeedFactor * ScaleValuePerSpeedStep, 1);
        }
        if (TargetSpeedTransform != null)
        {
            TargetSpeedTransform.localScale = new Vector3(1, _targetSpeedFactor * ScaleValuePerSpeedStep, 1);
        }

        ApplyMovementTranslation(transform, BaseMovementSpeed);
    }

    /// <summary>
    /// Enables movement, refactored out for easier changes
    /// </summary>
    public void EnableMovement()
    {
        _movementEnabled = true;
    }

    /// <summary>
    /// Disables movement, refactored out for easier changes
    /// </summary>
    public void DisableMovement()
    {
        _movementEnabled = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private Quaternion GetRotationQuaternion()
    {
        // get active controller
        var controller = UseRightHandedController ? OVRInput.Controller.RTrackedRemote : OVRInput.Controller.LTrackedRemote;

        // get gyroscope instead of accelerometer values because more precise AND based on orientation, not movement.
        // This is important because values have origin in calibrated null value (as in, the holding position)
        return OVRInput.GetLocalControllerRotation(controller);
    }

    /// <summary>
    /// factor in inner and outer deadzones, inner to correct for "jitter" and outer to handle out-of-bounds input ranges
    /// </summary>
    /// <param name="factor">input value that is to be adjusted</param>
    /// <returns></returns>
    private float CalculateAdjustedRotationFactor(float factor)
    {
        if (factor > -InnerDeadzoneValue && factor < InnerDeadzoneValue)
            return 0;
        if (factor < -OuterDeadzoneValue)
            return -OuterDeadzoneValue;
        if (factor > OuterDeadzoneValue)
            return OuterDeadzoneValue;

        return factor;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private float CalculateYawFactor(Quaternion rotation)
    {
        return CalculateAdjustedRotationFactor(rotation.x);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private float CalculatePitchFactor(Quaternion rotation)
    {
        return CalculateAdjustedRotationFactor(rotation.y);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rotation"></param>
    /// <returns></returns>
    private float CalculateRollFactor(Quaternion rotation)
    {
        return CalculateAdjustedRotationFactor(rotation.z);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="previousSpeed"></param>
    /// <param name="targetSpeed"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="baseMovementSpeed"></param>
    private void ApplyMovementTranslation(Transform transform, float baseMovementSpeed)
    {
        transform.position += transform.forward * _currentSpeedFactor * baseMovementSpeed * Time.deltaTime;
    }
}