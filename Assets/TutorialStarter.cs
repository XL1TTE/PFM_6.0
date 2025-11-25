using UnityEngine;

namespace Project
{
    public class TutorialStarter : MonoBehaviour
    {
        public TutorialController tutorialController;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            tutorialController.BeginTutorial();
        }
    }
}
