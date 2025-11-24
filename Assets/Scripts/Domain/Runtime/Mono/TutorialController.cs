using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class TutorialController : MonoBehaviour
    {
        private bool await_special_continue;
        private bool tutorial_active;

        public GameObject tutorialScreen;
        public GameObject tutorialNotificationMessage;
        public GameObject tutorialAllNotifications;

        [SerializeField]
        public List<TutorialPartWrapper> allScreensWithWhatTheyEnable;
        //public List<MyKeyValuePair<GameObject,List<GameObject>>> allScreensWithWhatTheyEnable;

        //public Vector2(GameObject obj, List<GameObject> enabled) allScreensWithWhatTheyEnable;

        private int tutorialCounter;
        private GameObject currentNotification;
        private List<GameObject> currentEnabledList;
        private List<EnabledPrevInfoWrapper> currentEnabledPrevParentAndIndex = new List<EnabledPrevInfoWrapper>();

        public void ShowTutorialNotification()
        {
            tutorialScreen.SetActive(true);
            tutorialNotificationMessage.SetActive(true);
        }


        public void OnClickAcceptTutorial()
        {
            tutorialNotificationMessage.SetActive(false);
            BeginTutorial();
        }
        public void OnClickDenyTutorial()
        {
            tutorialNotificationMessage.SetActive(false);
            EndTutorial();
        }

        public void BeginTutorial()
        {
            tutorial_active = true;

            tutorialCounter = -1;

            tutorialAllNotifications.SetActive(true);

            for (int i = 0, count = tutorialAllNotifications.transform.childCount; i < count; i++)
            {
                tutorialAllNotifications.transform.GetChild(i).gameObject.SetActive(false);
            }

            ContinueTutorial();
        }

        public void ContinueTutorial()
        {
            if (currentEnabledList != null)
            {
                for (int i = 0; i < currentEnabledList.Count; i++)
                {
                    currentEnabledList[i].transform.SetParent(currentEnabledPrevParentAndIndex[i].parent.transform);
                    currentEnabledList[i].transform.SetSiblingIndex(currentEnabledPrevParentAndIndex[i].sibling_pos);

                    Vector3 pos = currentEnabledList[i].transform.position;
                    pos.z = (currentEnabledPrevParentAndIndex[i].z_ind);

                    currentEnabledList[i].transform.position = pos;
                }

                currentEnabledList.Clear();
            }
            if (currentNotification != null) {
                currentNotification.SetActive(false);
            }
            await_special_continue = false;

            tutorialCounter ++;

            if (tutorialCounter >= allScreensWithWhatTheyEnable.Count) { EndTutorial(); return; }

            var currPart = allScreensWithWhatTheyEnable[tutorialCounter];

            await_special_continue = currPart.non_button_continue;

            currentNotification = currPart.notification;
            currentEnabledList = currPart.enable_list;

            if (currentEnabledList != null)
            {
                foreach (var obj in currentEnabledList)
                {
                    EnabledPrevInfoWrapper res = new(obj.transform.parent.gameObject, 
                        obj.transform.GetSiblingIndex(),
                        obj.transform.position.z);

                    currentEnabledPrevParentAndIndex.Add(res);


                    obj.transform.SetParent(tutorialScreen.transform);

                    Vector3 pos = obj.transform.position;
                    pos.z = 0;

                    obj.transform.position = pos;
                }
            }

            currentNotification.SetActive(true);
        }
    
        
        public void EndTutorial()
        {
            tutorial_active = false;
            tutorialScreen.SetActive(false);
        }


        public void ContinueSpecial()
        {
            if (!await_special_continue && !tutorial_active) { return; }

            ContinueTutorial();
        }


        public bool IsTutorialActive()
        {
            return tutorial_active;
        }
    }

    [Serializable]
    public class MyKeyValuePair<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public MyKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    [Serializable]
    public class TutorialPartWrapper
    {
        public GameObject notification;
        public bool non_button_continue = false;
        public List<GameObject> enable_list;

        public TutorialPartWrapper(GameObject g, List<GameObject> l, bool n)
        {
            notification = g;
            enable_list = l;
            non_button_continue = n;
        }
    }

    [Serializable]
    public class EnabledPrevInfoWrapper
    {
        public GameObject parent;
        public int sibling_pos;
        public float z_ind;
        public int ui_order;

        public EnabledPrevInfoWrapper(GameObject p, int s, float z)
        {
            parent = p;
            sibling_pos = s;
            z_ind = z; 
            //ui_order = u;
        }
    }
}
