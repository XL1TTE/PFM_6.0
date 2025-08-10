using UnityEngine;

namespace Core.Utilities.Extantions
{
    public class WaitForMouseDown : CustomYieldInstruction
    {
        public WaitForMouseDown(int keyIndex){
            this.keyIndex = keyIndex;
        }
        private int keyIndex;
        
        public override bool keepWaiting
        {
            get
            {
                return !Input.GetMouseButtonDown(keyIndex);   
            }
        }
    }
}
