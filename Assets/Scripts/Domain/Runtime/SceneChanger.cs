using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class SceneChanger : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                SceneManager.LoadScene("BattleField");
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                SceneManager.LoadScene("MapGeneration");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                SceneManager.LoadScene("Laboratory");
            }

        }
    }
}
