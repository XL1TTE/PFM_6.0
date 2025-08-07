using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Bootstrap
{
    
    public class LoadSharedUIScene : BootstrapInstruction
    {
        [SerializeField] private string SharedUISceneName;
        public override void Execute()
        {
            SceneManager.LoadScene(SharedUISceneName, LoadSceneMode.Single);
        }
    }
}
