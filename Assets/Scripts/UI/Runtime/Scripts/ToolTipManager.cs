namespace UI.ToolTip
{
    using UnityEngine;

    public class ToolTipManager : MonoBehaviour
    {
        [SerializeField] private ToolTip TooltipInstance;

        private static ToolTip tooltipInstance;

        public void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (TooltipInstance == null)
            {
                Debug.LogError("TooltipInstance is not assigned in the inspector!");
                return;
            }

            if (tooltipInstance == null)
            {
                tooltipInstance = TooltipInstance;
                tooltipInstance.Hide();
            }
        }

        public static void ShowTooltip(ToolTipLines a_config)
        {
            if (tooltipInstance != null)
            {
                tooltipInstance.Show(a_config);
            }
        }
        public static void ShowTooltip()
        {
            if (tooltipInstance != null)
            {
                tooltipInstance.Show();
            }
        }

        public static void HideTooltip()
        {
            if (tooltipInstance != null)
            {
                tooltipInstance.Hide();
            }
        }

    }

}
