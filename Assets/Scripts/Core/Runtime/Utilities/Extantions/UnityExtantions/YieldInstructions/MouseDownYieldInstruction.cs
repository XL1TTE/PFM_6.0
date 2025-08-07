using UnityEngine;

namespace Core.Utilities.Extantions
{
    public class MouseDownYieldInstruction : CustomYieldInstruction
    {
        public override bool keepWaiting
        {
            get
            {
                return !Input.GetMouseButtonDown(0);   
            }
        }
    }
}
