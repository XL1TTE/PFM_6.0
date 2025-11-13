using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Bootstrap
{

    public class LoadSharedUIScene : BootstrapInstruction
    {
        [SerializeField] private string SharedUISceneName;
        public override void Execute()
        {
            var load = SceneManager.LoadSceneAsync(SharedUISceneName, LoadSceneMode.Additive);

            load.completed += (load) => SceneManager.UnloadSceneAsync(SharedUISceneName);
        }
    }
}
