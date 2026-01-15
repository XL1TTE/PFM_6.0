using Domain.Map;
using Persistence.DS;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Project
{
    public class LabSceneChanger : MonoBehaviour
    {
        private LabReferences labRef;

        void Start()
        {
            labRef = LabReferences.Instance();
        }

        public void LoadPreparation()
        {
            AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

            LabReferences.Instance().tutorialController.ContinueSpecial();
            Debug.Log("!!!!4");
            labRef.uiController?.ShowPreparationScreen();
        }

        public void LoadMap()
        {
            AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

            if (labRef.expeditionController == null) return;

            int expeditionMonsterCount = labRef.expeditionController.GetExpeditionMonsterCount();
            if (expeditionMonsterCount == 0)
            {
                return;
            }

            var expeditionMonsters = labRef.expeditionController.GetExpeditionMonsters();
            ref var storageMonsters = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            ref var crusadeMonsters = ref DataStorage.GetRecordFromFile<Crusade, CrusadeMonsters>();
            ref var crusadeState = ref DataStorage.GetRecordFromFile<Crusade, CrusadeState>();

            if (expeditionMonsters != null && expeditionMonsters.Count > 0)
            {
                crusadeMonsters.crusade_monsters = expeditionMonsters;
            }
            else
            {
                crusadeMonsters.crusade_monsters = storageMonsters.storage_monsters;
            }

            if (LabReferences.Instance().tutorialController.IsTutorialActive())
            {
                crusadeState.crusade_state = CRUSADE_STATE.TUTORIAL;
            }

            LoadingScreen.Instance.LoadScene("MapGeneration");
        }

        public void ReturnToMainLab()
        {
            AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

            labRef.uiController?.ShowMainScreen();
        }

        public bool CanStartExpedition()
        {
            return labRef.expeditionController != null && labRef.expeditionController.GetExpeditionMonsterCount() > 0;
        }

        public string GetExpeditionStatus()
        {
            if (labRef.expeditionController != null)
            {
                int count = labRef.expeditionController.GetExpeditionMonsterCount();
                return count > 0 ? $"Готово к походу ({count} монстров)" : "Выберите монстров для похода";
            }
            return "Система экспедиции не найдена";
        }
    }
}