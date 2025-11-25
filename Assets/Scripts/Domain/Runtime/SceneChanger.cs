using Core.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class SceneChanger : MonoBehaviour
    {
        private bool allow_cheats = false;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (allow_cheats)
                {
                    LoadingScreen.Instance.LoadScene("BattleField");
                    //SceneManager.LoadScene("BattleField");
                }
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                if (allow_cheats)
                {
                    LoadingScreen.Instance.LoadScene("MapGeneration");
                    //SceneManager.LoadScene("MapGeneration");
                }
            }

            if (Input.GetKeyDown(KeyCode.E) && Input.GetKeyDown(KeyCode.T))
            {
                if (allow_cheats)
                {
                    CHEATS.EndlessTurnsForMonsters();
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                SceneManager.LoadScene("Laboratory");
            }

        }
    }
}
