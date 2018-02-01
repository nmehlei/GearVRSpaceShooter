using System;
using UnityEngine;

namespace Assets.Scripts.MovementControl
{
    public abstract class DeadzoneHandler
    {
        // Constructors

        protected DeadzoneHandler(float innerDeadzoneValue, float outerDeadzoneValue)
        {
            InnerDeadzoneValue = innerDeadzoneValue;
            OuterDeadzoneValue = outerDeadzoneValue;
        }

        // Fields

        protected float InnerDeadzoneValue;
        protected float OuterDeadzoneValue;

        // Methods

        public abstract float AdjustValue(float sourceValue, Vector3 rotationVector);
    }
}