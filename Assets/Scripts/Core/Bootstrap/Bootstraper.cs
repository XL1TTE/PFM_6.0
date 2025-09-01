using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Bootstrap
{
    public class Bootstraper : MonoBehaviour
    {
        private readonly static string BOOTSTRAP_SCENE_NAME = "Bootstrap";
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Boot(){
            
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == BOOTSTRAP_SCENE_NAME && scene.isLoaded) { return; }
            }
            SceneManager.LoadScene(BOOTSTRAP_SCENE_NAME, LoadSceneMode.Additive);
        }
    }
}
