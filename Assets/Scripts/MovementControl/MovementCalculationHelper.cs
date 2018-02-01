using System;
using UnityEngine;

namespace Assets.Scripts.MovementControl
{
    /// <summary>
    /// Outsourced out of MovementControlComponent for separation of converns and easier testing
    /// </summary>
    public static class MovementCalculationHelper
    {
        /// <summary>
        /// factor in inner and outer deadzones, inner to correct for "jitter" and outer to handle out-of-bounds input ranges
        /// </summary>
        /// <param name="factor">input value that is to be adjusted</param>
        /// <param name="rotation">rotation quaternion</param>
        /// <param name="deadzoneHandler"></param>
        /// <returns>adjusted rotationf actor</returns>
        public static float CalculateAdjustedRotationFactor(float factor, Quaternion rotation, DeadzoneHandler deadzoneHandler)
        {
            var rotationVector = new Vector3(rotation.x, rotation.y, rotation.z);

            return deadzoneHandler.AdjustValue(factor, rotationVector);
        }

        /// <summary>
        /// Helper method to calculate yaw factor, easier to handle than based directly on axis
        /// </summary>
        /// <param name="rotation">rotation quaternion</param>
        /// <param name="deadzoneHandler">deadzone handler to use</param>
        /// <returns>yaw factor</returns>
        public static float CalculateYawFactor(Quaternion rotation, DeadzoneHandler deadzoneHandler)
        {
            return CalculateAdjustedRotationFactor(rotation.x, rotation, deadzoneHandler);
        }

        /// <summary>
        /// Helper method to calculate pitch factor, easier to handle than based directly on axis
        /// </summary>
        /// <param name="rotation">rotation quaternion</param>
        /// <param name="deadzoneHandler">deadzone handler to use</param>
        /// <returns>pitch factor</returns>
        public static float CalculatePitchFactor(Quaternion rotation, DeadzoneHandler deadzoneHandler)
        {
            return CalculateAdjustedRotationFactor(rotation.y, rotation, deadzoneHandler);
        }

        /// <summary>
        /// Helper method to calculate roll factor, easier to handle than based directly on axis
        /// </summary>
        /// <param name="rotation">rotation quaternion</param>
        /// <param name="deadzoneHandler">deadzone handler to use</param>
        /// <returns>roll factor</returns>
        public static float CalculateRollFactor(Quaternion rotation, DeadzoneHandler deadzoneHandler)
        {
            return CalculateAdjustedRotationFactor(rotation.z, rotation, deadzoneHandler);
        }

        /// <summary>
        /// Update the given speed factor based on current speed values
        /// </summary>
        /// <param name="previousSpeed">previous speed to update</param>
        /// <param name="targetSpeed">targeted speed step</param>
        /// <param name="acceleration">acceleration value</param>
        /// <returns>updated speed factor</returns>
        public static float UpdateSpeedFactor(float previousSpeed, float targetSpeed, float acceleration)
        {
            if (targetSpeed > previousSpeed)
            {
                return previousSpeed + acceleration;
            }
            if (targetSpeed < previousSpeed)
            {
                return previousSpeed - acceleration;
            }
            return previousSpeed;
        }

        /// <summary>
        /// Applies movement to the given transform
        /// </summary>
        /// <param name="transform">Transform to act on</param>
        /// <param name="baseMovementSpeed">base movement speed for calculation</param>
        /// <param name="currentSpeedFactor">current speed factor</param>
        public static void ApplyMovementTranslation(Transform transform, float baseMovementSpeed, float currentSpeedFactor)
        {
            transform.position += transform.forward * currentSpeedFactor * baseMovementSpeed * Time.deltaTime;
        }
    }
}