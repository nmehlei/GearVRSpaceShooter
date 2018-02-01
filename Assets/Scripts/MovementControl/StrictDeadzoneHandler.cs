using System;
using UnityEngine;

namespace Assets.Scripts.MovementControl
{
    /// <summary>
    /// Deadzone handler with a more strict approach, based on axial dead zone calculation
    /// </summary>
    public class StrictDeadzoneHandler : DeadzoneHandler
    {
        // Constructors

        public StrictDeadzoneHandler(float innerDeadzoneValue, float outerDeadzoneValue)
            : base(innerDeadzoneValue, outerDeadzoneValue)
        {

        }

        // Methods

        public override float AdjustValue(float sourceValue, Vector3 rotationVector)
        {
            if (sourceValue > -InnerDeadzoneValue && sourceValue < InnerDeadzoneValue)
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