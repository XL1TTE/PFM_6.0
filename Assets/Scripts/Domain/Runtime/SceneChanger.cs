using Core.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class SceneChanger : MonoBehaviour
    {
        private bool allow_cheats = true;

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Keypad1))
            {
                //SceneManager.LoadScene("Laboratory");
                if (allow_cheats)
                {
                    LoadingScreen.Instance.LoadScene("Laboratory");
                    //SceneManager.LoadScene("MapGeneration");
                }
            }
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (allow_cheats)
                {
                    LoadingScreen.Instance.LoadScene("MapGeneration");
                    //SceneManager.LoadScene("MapGeneration");
                }
            }
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (allow_cheats)
                {
                    LoadingScreen.Instance.LoadScene("BattleField");
                    //SceneManager.LoadScene("BattleField");
                }
            }




            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Keypad6))
            {
                if (allow_cheats)
                {
                    CHEATS.EndlessTurnsForMonsters();
                }
            }

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Keypad4))
            {
                if (allow_cheats)
                {
                    CHEATS.GiveParts();
                }
            }

        }
    }
}
