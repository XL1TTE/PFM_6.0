using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Bootstrap
{
    public class InstructionsExecuter : MonoBehaviour
    {
        [SerializeField] private List<BootstrapInstruction> _instructions;

        void Awake()
        {
            foreach(var i in _instructions){
                i.Execute();
            }
        }
        

    }
}
