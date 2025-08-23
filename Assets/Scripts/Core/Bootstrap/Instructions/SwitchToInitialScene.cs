using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Bootstrap
{
    public class SwitchToInitialScene : BootstrapInstruction
    {
        [SerializeField] private string InitialSceneName;

        public override void Execute()
        {
            if(SceneManager.GetSceneByName(InitialSceneName).isLoaded){return;}
            
            SceneManager.LoadScene(InitialSceneName, LoadSceneMode.Single);
        }
    }
}
