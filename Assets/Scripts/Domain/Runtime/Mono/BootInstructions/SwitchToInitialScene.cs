using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Bootstrap
{
    public class SwitchToInitialScene : BootstrapInstruction
    {
        [SerializeField] private string InitialSceneName;

        [ContextMenu("Run")]
        public override void Execute()
        {
            if (SceneManager.GetSceneByName(InitialSceneName).isLoaded) { return; }

            SceneManager.LoadScene(InitialSceneName, LoadSceneMode.Single);
        }


        public void OnDestroy()
        {
            World.Default.Dispose();
        }
    }
}
