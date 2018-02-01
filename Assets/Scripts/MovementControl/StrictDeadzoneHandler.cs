using System;

namespace Assets.Scripts.MovementControl
{
    public class StrictDeadzoneHandler : DeadzoneHandler
    {
        // Constructors

        public StrictDeadzoneHandler(float innerDeadzoneValue, float outerDeadzoneValue)
            : base(innerDeadzoneValue, outerDeadzoneValue)
        {

        }

        // Methods

        public override float AdjustValue(float sourceValue)
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