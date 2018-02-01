using System;

namespace Assets.Scripts.MovementControl
{
    public class FlexibleDeadzoneHandler : DeadzoneHandler
    {
        // Constructors

        public FlexibleDeadzoneHandler(float innerDeadzoneValue, float outerDeadzoneValue)
            : base(innerDeadzoneValue, outerDeadzoneValue)
        {
            
        }

        // Methods

        public override float AdjustValue(float sourceValue)
        {
            throw new NotImplementedException();
        }
    }
}