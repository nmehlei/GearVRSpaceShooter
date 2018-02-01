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
        /// <param name="rotation">complete rotation quaternion</param>
        /// <param name="deadzoneHandler"></param>
        /// <returns></returns>
        public static float CalculateAdjustedRotationFactor(float factor, Quaternion rotation, DeadzoneHandler deadzoneHandler)
        {
            var rotationVector = new Vector3(rotation.x, rotation.y, rotation.z);

            return deadzoneHandler.AdjustValue(factor, rotationVector);
        }

        /// <summary>
        /// Helper method to calculate yaw factor, easier to handle than based directly on axis
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="deadzoneHandler"></param>
        /// <returns></returns>
        public static float CalculateYawFactor(Quaternion rotation, DeadzoneHandler deadzoneHandler)
        {
            return CalculateAdjustedRotationFactor(rotation.x, rotation, deadzoneHandler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="deadzoneHandler"></param>
        /// <returns></returns>
        public static float CalculatePitchFactor(Quaternion rotation, DeadzoneHandler deadzoneHandler)
        {
            return CalculateAdjustedRotationFactor(rotation.y, rotation, deadzoneHandler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rotation"></param>
        /// <param name="deadzoneHandler"></param>
        /// <returns></returns>
        public static float CalculateRollFactor(Quaternion rotation, DeadzoneHandler deadzoneHandler)
        {
            return CalculateAdjustedRotationFactor(rotation.z, rotation, deadzoneHandler);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="previousSpeed"></param>
        /// <param name="targetSpeed"></param>
        /// <param name="acceleration"></param>
        /// <returns></returns>
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
        /// Applies 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="baseMovementSpeed"></param>
        /// <param name="currentSpeedFactor"></param>
        public static void ApplyMovementTranslation(Transform transform, float baseMovementSpeed, float currentSpeedFactor)
        {
            transform.position += transform.forward * currentSpeedFactor * baseMovementSpeed * Time.deltaTime;
        }
    }
}