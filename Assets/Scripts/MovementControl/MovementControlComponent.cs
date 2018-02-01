using System;
using Assets.Scripts.MovementControl;
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
    [Tooltip("Amount of speed steps for forward movement.")]
    public int ForwardSpeedSteps = 8;
    [Tooltip("Amount of speed steps for backward movement.")]
    public int BackwardSpeedSteps = 4;

    [Header("Deadzone")]
    [Range(0f, 0.99f)]
    [Tooltip("Inner Deadzone that will be ignored from input.")]
    public float InnerDeadzoneValue = 0.01f;
    [Range(0.01f, 1f)]
    [Tooltip("Outer Deadzone that will be ignored from input.")]
    public float OuterDeadzoneValue = 0.25f;

    [Tooltip("Preferred Deadzone handling style to use.")]
    public DeadzoneHandlingStyle DeadzoneHandling;

    [Header("Slider properties")]

    [Tooltip("Transform for the X up slider.")]
    public Transform XUpTransform;
    [Tooltip("Transform for the X down slider.")]
    public Transform XDownTransform;

    [Tooltip("Transform for the Y up slider.")]
    public Transform YUpTransform;
    [Tooltip("Transform for the Y down slider.")]
    public Transform YDownTransform;

    [Tooltip("Transform for the Z up slider.")]
    public Transform ZUpTransform;
    [Tooltip("Transform for the Z down slider.")]
    public Transform ZDownTransform;

    [Tooltip("Maximal value for axis slider scaling.")]
    public float AxisTransformScaleMax;

    [Tooltip("Transform for the current speed slider.")]
    public Transform CurrentSpeedTransform;
    [Tooltip("Transform for the target speed slider.")]
    public Transform TargetSpeedTransform;
    [Tooltip("Scale value to use for each achieved speed step.")]
    public float ScaleValuePerSpeedStep;

    [Tooltip("Transform for the Mini-Ship hologram.")]
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
    private DeadzoneHandler _deadzoneHandler;

    // Methods

    void Start()
    {
        // Initialize dead zone handling
        switch (DeadzoneHandling)
        {
            case DeadzoneHandlingStyle.Flexible:
                _deadzoneHandler = new FlexibleDeadzoneHandler(InnerDeadzoneValue, OuterDeadzoneValue);
                break;
            case DeadzoneHandlingStyle.Strict:
                _deadzoneHandler = new StrictDeadzoneHandler(InnerDeadzoneValue, OuterDeadzoneValue);
                break;
                // could be expanded to support more handlers, for example one that uses non-linear value scaling
        }
    }

    void FixedUpdate ()
    {
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

            var yawFactor = MovementCalculationHelper.CalculateYawFactor(rotation, _deadzoneHandler);
            var pitchFactor = MovementCalculationHelper.CalculatePitchFactor(rotation, _deadzoneHandler);
            var rollFactor = MovementCalculationHelper.CalculateRollFactor(rotation, _deadzoneHandler);

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
    /// Updates the sliders inside the cockpit which shows the current usage of each axis
    /// </summary>
    /// <param name="yawFactor">current relative yaw factor</param>
    /// <param name="pitchFactor">current relative pitch factor</param>
    /// <param name="rollFactor">current relative roll factor</param>
    private void UpdateRotationSliders(float yawFactor, float pitchFactor, float rollFactor)
    {
        // update rotation sliders
        if (YUpTransform != null && YDownTransform != null)
        {
            if (yawFactor > 0)
            {
                YUpTransform.localScale = new Vector3(1, yawFactor * AxisTransformScaleMax, 1);
                YDownTransform.localScale = new Vector3(1, 0, 1);
            }
            else
            {
                YUpTransform.localScale = new Vector3(1, 0, 1);
                YDownTransform.localScale = new Vector3(1, yawFactor * AxisTransformScaleMax, 1);
            }
        }

        if (XUpTransform != null && XDownTransform != null)
        {
            if (pitchFactor > 0)
            {
                XUpTransform.localScale = new Vector3(pitchFactor * AxisTransformScaleMax, 1, 1);
                XDownTransform.localScale = new Vector3(0, 1, 1);
            }
            else
            {
                XUpTransform.localScale = new Vector3(0, 1, 1);
                XDownTransform.localScale = new Vector3(pitchFactor * AxisTransformScaleMax, 1, 1);
            }
        }

        if (ZUpTransform != null && ZDownTransform != null)
        {
            if (rollFactor > 0)
            {
                ZUpTransform.localScale = new Vector3(rollFactor * -AxisTransformScaleMax, 1, 1);
                ZDownTransform.localScale = new Vector3(0, 1, 1);
            }
            else
            {
                ZUpTransform.localScale = new Vector3(0, 1, 1);
                ZDownTransform.localScale = new Vector3(rollFactor * -AxisTransformScaleMax, 1, 1);
            }
        }
    }

    /// <summary>
    /// Updates the hologram inside the cockpit which shows the current relative orientation of the input
    /// </summary>
    /// <param name="yawFactor">current relative yaw factor</param>
    /// <param name="pitchFactor">current relative pitch factor</param>
    /// <param name="rollFactor">current relative roll factor</param>
    private void UpdateMiniShipHologram(float yawFactor, float pitchFactor, float rollFactor)
    {        
        if (MiniShipTransform == null) return;

        const float miniShipScaleFactor = 0.25f;

        // update the mini ship orientation
        MiniShipTransform.localRotation = new Quaternion(yawFactor * miniShipScaleFactor, 
            pitchFactor * miniShipScaleFactor, rollFactor * miniShipScaleFactor, 1);
    }

    /// <summary>
    /// Handle the speed progression for the player object
    /// </summary>
    private void HandleSpeed()
    {
        // only update target speed factor if touchpad is being touched
        if (OVRInput.Get(OVRInput.Touch.PrimaryTouchpad))
        {
            int totalStepsSum = ForwardSpeedSteps + BackwardSpeedSteps;
            var speedTouchPosition = OVRInput.Get(OVRInput.Axis2D.PrimaryTouchpad);

            // + 1 to only have positive values, divided by 2 to have range 0 to 1
            // results in normalized input
            var normalizedInput = (speedTouchPosition.y + 1) / 2;

            // calculate targeted speed step and then 
            _targetSpeedFactor = normalizedInput * totalStepsSum;

            // remove offset due to normalization
            _targetSpeedFactor -= BackwardSpeedSteps;
        }

        _currentSpeedFactor = MovementCalculationHelper.UpdateSpeedFactor(_currentSpeedFactor, _targetSpeedFactor, Acceleration);

        // update UI elements
        if (CurrentSpeedTransform != null)
        {
            CurrentSpeedTransform.localScale = new Vector3(1, _currentSpeedFactor * ScaleValuePerSpeedStep, 1);
        }
        if (TargetSpeedTransform != null)
        {
            TargetSpeedTransform.localScale = new Vector3(1, _targetSpeedFactor * ScaleValuePerSpeedStep, 1);
        }

        MovementCalculationHelper.ApplyMovementTranslation(transform, BaseMovementSpeed, _currentSpeedFactor);
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
    /// Gets the rotation quaternion for the selected controller
    /// </summary>
    /// <returns>rotation quaternion</returns>
    private Quaternion GetRotationQuaternion()
    {
        // get active controller
        var controller = UseRightHandedController ? OVRInput.Controller.RTrackedRemote : OVRInput.Controller.LTrackedRemote;

        // get gyroscope instead of accelerometer values because more precise AND based on orientation, not movement.
        // This is important because values have origin in calibrated null value (as in, the holding position)
        return OVRInput.GetLocalControllerRotation(controller);
    }
    
    /// <summary>
    /// Validate unity fields for easier usage
    /// </summary>
    public void OnValidate()
    {
        if(BaseMovementSpeed < 0)
            Debug.LogError("BaseMovementSpeed can't be negative!");

        if (Acceleration < 0)
            Debug.LogError("Acceleration can't be negative!");

        if(YawRotationSpeed < 0 || PitchRotationSpeed < 0 || RollRotationSpeed < 0)
            Debug.LogError("Rotation speeds can't be negative!");

        if (ForwardSpeedSteps < 0)
            Debug.LogError("ForwardSpeedSteps can't be negative!");

        if (BackwardSpeedSteps < 0)
            Debug.LogError("BackwardSpeedSteps can't be negative!");

        if (InnerDeadzoneValue < 0 || InnerDeadzoneValue > 1)
            Debug.LogError("InnerDeadzoneValue is out of bounds!");

        if (OuterDeadzoneValue < 0 || OuterDeadzoneValue > 1)
            Debug.LogError("OuterDeadzoneValue is out of bounds!");

        if (InnerDeadzoneValue > OuterDeadzoneValue)
            Debug.LogError("InnerDeadzoneValue is larger than OuterDeadzoneValue!");

        if(AxisTransformScaleMax == 0 && (XUpTransform != null || XDownTransform != null || YUpTransform != null || YDownTransform != null 
            || ZUpTransform != null || ZDownTransform != null))
            Debug.LogError("AxisTransformScaleMax can't be 0 when Axis transforms are set!");

        if(ScaleValuePerSpeedStep == 0 && (TargetSpeedTransform != null || CurrentSpeedTransform != null))
            Debug.LogError("ScaleValuePerSpeedStep can't be 0 when Speed transforms are set!");
    }
}