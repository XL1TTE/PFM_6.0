using Persistence.DS;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class LabSceneChanger : MonoBehaviour
    {
        private LabUIController labUIController;

        void Start()
        {
            labUIController = FindObjectOfType<LabUIController>();
        }

        public void LoadPreparation()
        {
            if (labUIController != null)
            {
                labUIController.ShowPreparationScreen();
            }
        }

        public void LoadMap()
        {
            ref var storageMonsters = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            ref var crusadeMonsters = ref DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>();

            crusadeMonsters.crusade_monsters = storageMonsters.storage_monsters;

            LoadingScreen.Instance.LoadScene("MapGeneration");
        }

        public void ReturnToMainLab()
        {
            if (labUIController != null)
            {
                labUIController.ShowMainScreen();
            }
        }
    }
}