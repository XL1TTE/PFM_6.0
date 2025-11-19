using Core.Utilities;
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
                LoadingScreen.Instance.LoadScene("BattleField");
                //SceneManager.LoadScene("BattleField");
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                LoadingScreen.Instance.LoadScene("MapGeneration");
                //SceneManager.LoadScene("MapGeneration");
            }

            if (Input.GetKeyDown(KeyCode.E) && Input.GetKeyDown(KeyCode.T))
            {
                CHEATS.EndlessTurnsForMonsters();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                SceneManager.LoadScene("Laboratory");
            }

        }
    }
}
