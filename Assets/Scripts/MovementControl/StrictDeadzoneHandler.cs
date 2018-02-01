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
                return -OuterDeadzoneValue;
            if (sourceValue > OuterDeadzoneValue)
                return OuterDeadzoneValue;

            return sourceValue;
        }
    }
}