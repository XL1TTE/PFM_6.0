using Domain.Monster.Mono;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project
{
    public class MonsterNamingPanel : MonoBehaviour
    {
        [Header("UI References")]
        public TMP_Text titleText;
        public TMP_InputField nameInputField;
        public Button acceptButton;

        private System.Action<string> onNameAccepted;
        private MonsterData currentMonsterData;

        void Start()
        {
            gameObject.SetActive(false);

            acceptButton.onClick.AddListener(OnAcceptClicked);

            nameInputField.characterLimit = 18;
            nameInputField.onValidateInput = ValidateEnglishInput;
        }

        private char ValidateEnglishInput(string text, int charIndex, char addedChar)
        {
            if (char.IsLetterOrDigit(addedChar) || addedChar == ' ' || addedChar == '_')
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(addedChar.ToString(), @"^[a-zA-Z0-9 _]+$"))
                {
                    return addedChar;
                }
            }
            return '\0';
        }

        public void ShowNamingPanel(MonsterData monsterData, System.Action<string> onAcceptedCallback)
        {
            currentMonsterData = monsterData;
            onNameAccepted = onAcceptedCallback;
            gameObject.SetActive(true);

            ForceVisualUpdate();

            if (nameInputField != null)
            {
                nameInputField.Select();
                nameInputField.ActivateInputField();
            }
        }

        private void ForceVisualUpdate()
        {
            Canvas parentCanvas = GetComponentInParent<Canvas>();
            if (parentCanvas != null)
            {
                transform.SetAsLastSibling();

                LayoutRebuilder.ForceRebuildLayoutImmediate(parentCanvas.GetComponent<RectTransform>());
            }

            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.localScale = Vector3.one;

                LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }

            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public void HideNamingPanel()
        {
            gameObject.SetActive(false);
            currentMonsterData = null;
            onNameAccepted = null;
        }

        private void OnAcceptClicked()
        {
            string monsterName = nameInputField.text.Trim();

            if (string.IsNullOrEmpty(monsterName))
            {
                return;
            }

            onNameAccepted?.Invoke(monsterName);

            HideNamingPanel();
        }

        public void OnEndEdit(string value)
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                OnAcceptClicked();
            }
        }

        void Update()
        {
            if (gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Escape))
            {
                HideNamingPanel();
            }
        }

        public void ClearInputField()
        {
            if (nameInputField != null)
            {
                nameInputField.text = "";
            }
        }
    }
}