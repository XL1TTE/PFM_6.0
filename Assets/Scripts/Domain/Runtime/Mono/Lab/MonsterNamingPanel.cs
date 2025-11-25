using Domain.Map;
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
        public TMP_Text errorText; 

        private System.Action<string> onNameAccepted;
        private MonsterData currentMonsterData;

        [SerializeField] private string collidersTag;

        private BoxCollider2D[] iconColliders;

        void Start()
        {
            GameObject[] iconObjects = GameObject.FindGameObjectsWithTag(collidersTag);
            iconColliders = new BoxCollider2D[iconObjects.Length];

            for (int i = 0; i < iconObjects.Length; i++)
            {
                iconColliders[i] = iconObjects[i].GetComponent<BoxCollider2D>();
            }

            gameObject.SetActive(false);

            acceptButton.onClick.AddListener(OnAcceptClicked);

            nameInputField.characterLimit = 18;
            nameInputField.onValidateInput = ValidateEnglishInput;

            nameInputField.onValueChanged.AddListener(OnNameInputChanged);

            HideErrorMessage();
        }

        private char ValidateEnglishInput(string text, int charIndex, char addedChar)
        {
            if (char.IsLetterOrDigit(addedChar) || addedChar == ' ' || addedChar == '_')
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(addedChar.ToString(), @"^[a-zA-Z0-9 _]+$"))
                {
                    return addedChar;
                }
                else
                {
                    ShowErrorMessage("Please use only English letters, numbers, spaces and underscores!");
                    return '\0';
                }
            }
            return '\0';
        }

        private void OnNameInputChanged(string newName)
        {
            HideErrorMessage();

            if (!string.IsNullOrEmpty(newName) && ContainsRussianCharacters(newName))
            {
                ShowErrorMessage("Please use only English letters, numbers, spaces and underscores!");
                return;
            }

            if (!string.IsNullOrEmpty(newName))
            {
                if (IsMonsterNameExists(newName.Trim()))
                {
                    ShowErrorMessage($"Monster name '{newName.Trim()}' is already taken!");
                }
            }
        }

        private bool ContainsRussianCharacters(string text)
        {
            foreach (char c in text)
            {
                if ((c >= 'À' && c <= 'ß') || (c >= 'à' && c <= 'ÿ') || c == '¨' || c == '¸')
                {
                    return true;
                }
            }
            return false;
        }

        public void ShowNamingPanel(MonsterData monsterData, System.Action<string> onAcceptedCallback)
        {
            foreach (BoxCollider2D collider in iconColliders)
            {
                if (collider != null)
                    collider.enabled = false;
            }

            currentMonsterData = monsterData;
            onNameAccepted = onAcceptedCallback;
            gameObject.SetActive(true);

            HideErrorMessage();

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

            foreach (BoxCollider2D collider in iconColliders)
            {
                if (collider != null)
                    collider.enabled = true;
            }
        }

        private void OnAcceptClicked()
        {
            AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

            string monsterName = nameInputField.text.Trim();

            if (string.IsNullOrEmpty(monsterName))
            {
                ShowErrorMessage("Monster name cannot be empty!");
                return;
            }

            if (ContainsRussianCharacters(monsterName))
            {
                ShowErrorMessage("Please use only English letters, numbers, spaces and underscor!");
                return;
            }

            if (IsMonsterNameExists(monsterName))
            {
                ShowErrorMessage($"Monster with this name already exists! Please choose a different name.");
                return;
            }

            onNameAccepted?.Invoke(monsterName);
            HideNamingPanel();
        }

        public void ShowErrorMessage(string message)
        {
            AudioManager.Instance?.PlaySound(AudioManager.buttonErrorSound);

            if (errorText != null)
            {
                errorText.text = message;
                errorText.gameObject.SetActive(true);

                errorText.color = Color.red;
            }

            ShakePanel();
        }

        public void HideErrorMessage()
        {
            if (errorText != null)
            {
                errorText.gameObject.SetActive(false);
            }
        }

        private void ShakePanel()
        {
            StartCoroutine(ShakeCoroutine());
        }

        private System.Collections.IEnumerator ShakeCoroutine()
        {
            RectTransform rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null) yield break;

            Vector3 originalPosition = rectTransform.anchoredPosition;
            float shakeDuration = 0.5f;
            float shakeMagnitude = 2f;
            float elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                float x = Random.Range(-1f, 1f) * shakeMagnitude;
                float y = Random.Range(-1f, 1f) * shakeMagnitude;
                rectTransform.anchoredPosition = originalPosition + new Vector3(x, y, 0);
                elapsed += Time.deltaTime;
                yield return null;
            }

            rectTransform.anchoredPosition = originalPosition;
        }

        private bool IsMonsterNameExists(string monsterName)
        {
            if (string.IsNullOrEmpty(monsterName))
                return false;

            var craftController = LabReferences.Instance()?.craftController;
            if (craftController != null)
            {
                return craftController.IsMonsterNameExists(monsterName);
            }

            return false;
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
            HideErrorMessage();
        }
    }
}