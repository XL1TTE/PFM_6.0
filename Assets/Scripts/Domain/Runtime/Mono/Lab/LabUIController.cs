using UnityEngine;
using System.Collections;

namespace Project
{
    public class LabUIController : MonoBehaviour
    {
        [Header("UI Screens")]
        public GameObject mainScreen;
        public GameObject preparationScreen;

        [Header("Controllers")]
        public LabMonstersController monstersController;
        public PreparationMonsterPreviewController previewController;

        [Header("Transition Settings")]
        public float slideDuration = 0.5f;
        public AnimationCurve slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private RectTransform mainScreenRT;
        private RectTransform preparationScreenRT;
        private Vector2 screenSize;

        void Start()
        {

            mainScreenRT = mainScreen.GetComponent<RectTransform>();
            preparationScreenRT = preparationScreen.GetComponent<RectTransform>();
            screenSize = new Vector2(Screen.width, Screen.height);

            ResetScreens();

            if (monstersController == null)
                monstersController = FindObjectOfType<LabMonstersController>();
            if (previewController == null)
                previewController = FindObjectOfType<PreparationMonsterPreviewController>();
        }

        void ResetScreens()
        {
            mainScreenRT.anchoredPosition = Vector2.zero;
            mainScreen.SetActive(true);
            preparationScreenRT.anchoredPosition = Vector2.right * screenSize.x;
            preparationScreen.SetActive(true);
        }

        public void ShowPreparationScreen()
        {
            StartCoroutine(SlideToPreparation());
        }

        public void ShowMainScreen()
        {
            StartCoroutine(SlideToMain());
        }

        private IEnumerator SlideToPreparation()
        {
            if (monstersController != null)
            {
                monstersController.SwitchToPreparationScreen();
            }

            if (previewController != null)
            {
                previewController.OnPreparationScreenOpened();
            }

            float elapsedTime = 0f;
            Vector2 mainStartPos = mainScreenRT.anchoredPosition;
            Vector2 prepStartPos = preparationScreenRT.anchoredPosition;
            Vector2 mainEndPos = Vector2.left * screenSize.x;
            Vector2 prepEndPos = Vector2.zero;

            while (elapsedTime < slideDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = slideCurve.Evaluate(elapsedTime / slideDuration);

                mainScreenRT.anchoredPosition = Vector2.Lerp(mainStartPos, mainEndPos, progress);
                preparationScreenRT.anchoredPosition = Vector2.Lerp(prepStartPos, prepEndPos, progress);

                yield return null;
            }

            mainScreenRT.anchoredPosition = mainEndPos;
            preparationScreenRT.anchoredPosition = prepEndPos;

            if (monstersController != null)
            {
                monstersController.DebugCurrentState();
            }
        }

        private IEnumerator SlideToMain()
        {

            if (monstersController != null)
            {
                monstersController.SwitchToMainScreen();
            }

            float elapsedTime = 0f;
            Vector2 mainStartPos = mainScreenRT.anchoredPosition;
            Vector2 prepStartPos = preparationScreenRT.anchoredPosition;
            Vector2 mainEndPos = Vector2.zero;
            Vector2 prepEndPos = Vector2.right * screenSize.x;

            while (elapsedTime < slideDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = slideCurve.Evaluate(elapsedTime / slideDuration);

                mainScreenRT.anchoredPosition = Vector2.Lerp(mainStartPos, mainEndPos, progress);
                preparationScreenRT.anchoredPosition = Vector2.Lerp(prepStartPos, prepEndPos, progress);

                yield return null;
            }

            mainScreenRT.anchoredPosition = mainEndPos;
            preparationScreenRT.anchoredPosition = prepEndPos;

            if (monstersController != null)
            {
                monstersController.DebugCurrentState();
            }
        }

        public bool IsPreparationScreenActive()
        {
            bool isActive = preparationScreenRT.anchoredPosition.x == 0f;
            return isActive;
        }

        public bool IsMainScreenActive()
        {
            bool isActive = mainScreenRT.anchoredPosition.x == 0f;
            return isActive;
        }

        public void ForceDebug()
        {
            if (monstersController != null)
            {
                monstersController.DebugCurrentState();
            }
        }
    }
}