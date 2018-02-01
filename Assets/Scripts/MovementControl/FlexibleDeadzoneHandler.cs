using System;
using UnityEngine;

namespace Assets.Scripts.MovementControl
{
    /// <summary>
    /// Deadzone handler with a more flexible approach, based on radial dead zone calculation
    /// </summary>
    public class FlexibleDeadzoneHandler : DeadzoneHandler
    {
        // Constructors

        public FlexibleDeadzoneHandler(float innerDeadzoneValue, float outerDeadzoneValue)
            : base(innerDeadzoneValue, outerDeadzoneValue)
        {
            
        }

        // Methods

        public override float AdjustValue(float sourceValue, Vector3 rotationVector)
        {
            if (rotationVector.magnitude < InnerDeadzoneValue)
                return 0;
            if (sourceValue < -OuterDeadzoneValue)
                return -1;
            if (sourceValue > OuterDeadzoneValue)
                return 1;

            // calculate scale factor to have a range from 0 to 1 even though input values are restricted due to dead zones
            var scaleFactor = 1 / (OuterDeadzoneValue - InnerDeadzoneValue);

            return sourceValue * scaleFactor;
        }
    }
}