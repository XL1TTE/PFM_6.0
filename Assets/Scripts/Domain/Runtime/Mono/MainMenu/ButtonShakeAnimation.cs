using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class AdvancedButtonShake : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Shake Behavior")]
    [SerializeField] private ShakeProfile normalProfile;
    [SerializeField] private ShakeProfile hoverProfile;
    [SerializeField] private float transitionSpeed = 3f;

    [Header("Additional Effects")]
    [SerializeField] private bool scaleEffect = true;
    [SerializeField] private float hoverScale = 1.05f;
    [SerializeField] private bool colorEffect = true;
    [SerializeField] private Color hoverColor = new Color(1.1f, 1.1f, 1.1f, 1f);

    [System.Serializable]
    public class ShakeProfile
    {
        public float intensity = 2f;
        public float speed = 50f;
        public float frequency = 0.2f;
    }

    private RectTransform rectTransform;
    private Image buttonImage;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Color originalColor;

    private bool isHovered = false;
    private float individualTime;
    private Coroutine shakeCoroutine;

    private float currentIntensity;
    private float currentSpeed;
    private float currentFrequency;
    private float timeUntilNextShake;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        buttonImage = GetComponent<Image>();
        originalPosition = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;

        if (buttonImage != null)
            originalColor = buttonImage.color;

        individualTime = Random.Range(0f, 10f);

        currentIntensity = normalProfile.intensity;
        currentSpeed = normalProfile.speed;
        currentFrequency = normalProfile.frequency;
        timeUntilNextShake = Random.Range(0f, currentFrequency);

        StartCoroutine(ShakeUpdateRoutine());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;

        if (scaleEffect)
        {
            rectTransform.localScale = originalScale * hoverScale;
        }

        if (colorEffect && buttonImage != null)
        {
            buttonImage.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;

        if (scaleEffect)
        {
            rectTransform.localScale = originalScale;
        }

        if (colorEffect && buttonImage != null)
        {
            buttonImage.color = originalColor;
        }
    }

    private IEnumerator ShakeUpdateRoutine()
    {
        while (true)
        {
            ShakeProfile targetProfile = isHovered ? hoverProfile : normalProfile;

            currentIntensity = Mathf.Lerp(currentIntensity, targetProfile.intensity, Time.deltaTime * transitionSpeed);
            currentSpeed = Mathf.Lerp(currentSpeed, targetProfile.speed, Time.deltaTime * transitionSpeed);
            currentFrequency = Mathf.Lerp(currentFrequency, targetProfile.frequency, Time.deltaTime * transitionSpeed);

            timeUntilNextShake -= Time.deltaTime;
            if (timeUntilNextShake <= 0f)
            {
                StartCoroutine(SingleShakeRoutine());
                timeUntilNextShake = currentFrequency + Random.Range(-0.5f, 0.5f);
            }

            yield return null;
        }
    }

    private IEnumerator SingleShakeRoutine()
    {
        float elapsed = 0f;
        float duration = 0.8f;

        float noiseOffsetX = Random.Range(0f, 100f);
        float noiseOffsetY = Random.Range(0f, 100f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            individualTime += Time.deltaTime;

            float x = Mathf.PerlinNoise(individualTime * currentSpeed + noiseOffsetX, 0) * 2 - 1;
            float y = Mathf.PerlinNoise(0, individualTime * currentSpeed + noiseOffsetY) * 2 - 1;

            float decay = 1f - (elapsed / duration);
            decay = decay * decay;

            Vector3 shakeOffset = new Vector3(x, y, 0) * currentIntensity * decay;
            rectTransform.anchoredPosition = originalPosition + shakeOffset;

            yield return null;
        }

        rectTransform.anchoredPosition = originalPosition;
    }
}