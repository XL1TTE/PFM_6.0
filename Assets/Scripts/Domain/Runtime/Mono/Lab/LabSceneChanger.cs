using Persistence.DS;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project
{
    public class LabSceneChanger : MonoBehaviour
    {

        public void LoadPreparation()
        {

        }

        public void LoadMap()
        {

            ref var storageMonsters = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            ref var crusadeMonsters = ref DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>();

            crusadeMonsters.crusade_monsters = storageMonsters.storage_monsters;

            SceneManager.LoadScene("MapGeneration");
        }

    }
}
