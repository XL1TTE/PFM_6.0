using Domain.Map;
using Domain.Monster.Mono;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project
{
    public class AbilityTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Tooltip Controller")]
        [SerializeField] private AbilityTooltipController tooltipController;

        [Header("Data Source")]
        [SerializeField] private DataSourceType dataSource = DataSourceType.PartId;

        [Header("Direct Data")]
        [SerializeField] private string partId;
        [SerializeField] private string abilityId;
        [SerializeField] private BodyPartData bodyPartData;
        [SerializeField] private MonsterData monsterData;
        [SerializeField] private BODYPART_TYPE specificPartType = BODYPART_TYPE.HEAD;

        [Header("Legs Combination")]
        [SerializeField] private string pairedLegId; // ID второй ноги для комбинации
        [SerializeField] private BodyPartData pairedLegData; // Готовая вторая нога
        [SerializeField] private bool showCombinedLegs = false;

        [Header("Manual Data")]
        [SerializeField] private string abilityName;
        [SerializeField][TextArea(3, 5)] private string abilityDescription;
        [SerializeField] private Sprite abilityIcon;
        [SerializeField] private Vector2Int[] abilityShifts;

        public bool isTurn = false;

        private bool isHovering = false;
        private string runtimePartId;
        private BodyPartData runtimeBodyPartData;
        private string runtimePairedLegId;
        private BodyPartData runtimePairedLegData;

        public enum DataSourceType
        {
            PartId,
            AbilityId,
            BodyPartData,
            MonsterData,
            Manual,
            CombinedLegs // НОВЫЙ тип
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (tooltipController == null)
            {
                tooltipController = AbilityTooltipController.Instance;

                if (tooltipController == null)

                {
                    tooltipController = FindObjectOfType<AbilityTooltipController>();
                }
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isHovering && tooltipController != null)
            {
                isHovering = true;
                ShowTooltip();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isHovering && tooltipController != null)
            {
                isHovering = false;
                tooltipController.HideTooltip();
            }
        }

        private void ShowTooltip()
        {
            if (tooltipController == null)
            {
                return;
            }
            Vector3 worldPosition = transform.position;

            if (showCombinedLegs || dataSource == DataSourceType.CombinedLegs)
            {
                ShowCombinedLegsTooltip(worldPosition);
                return;
            }

            switch (dataSource)
            {
                case DataSourceType.PartId:
                    string finalPartId = !string.IsNullOrEmpty(runtimePartId) ? runtimePartId : partId;
                    if (!string.IsNullOrEmpty(finalPartId))
                    {
                        string pairedId = !string.IsNullOrEmpty(runtimePairedLegId) ? runtimePairedLegId : pairedLegId;
                        if (!string.IsNullOrEmpty(pairedId))
                        {
                            tooltipController.ShowCombinedLegsTooltip(finalPartId, pairedId, worldPosition);
                        }
                        else
                        {
                            tooltipController.ShowTooltipForPart(finalPartId, worldPosition);
                        }
                    }
                    break;

                case DataSourceType.AbilityId:
                    if (!string.IsNullOrEmpty(abilityId))
                    {
                        tooltipController.ShowTooltip(abilityId, worldPosition);
                    }
                    break;

                case DataSourceType.BodyPartData:
                    BodyPartData finalData = runtimeBodyPartData ?? bodyPartData;
                    if (finalData != null && finalData.type != BODYPART_TYPE.TORSO)
                    {
                        BodyPartData pairedData = runtimePairedLegData ?? pairedLegData;
                        if (pairedData != null && finalData.type == BODYPART_TYPE.LEG && pairedData.type == BODYPART_TYPE.LEG)
                        {
                            tooltipController.ShowCombinedLegsTooltip(finalData, pairedData, worldPosition);
                        }
                        else
                        {
                            tooltipController.ShowTooltip(finalData, worldPosition);
                        }
                    }
                    break;

                case DataSourceType.MonsterData:
                    if (monsterData != null)
                    {
                        string targetPartId = GetPartIdFromMonster(monsterData);
                        if (!string.IsNullOrEmpty(targetPartId))
                        {
                            tooltipController.ShowTooltipForPart(targetPartId, worldPosition);
                        }
                    }
                    break;

                case DataSourceType.Manual:
                    if (!string.IsNullOrEmpty(abilityName))
                    {
                        tooltipController.ShowTooltip(abilityName, abilityDescription, abilityIcon, abilityShifts, worldPosition);
                    }
                    break;
            }
        }

        private void ShowCombinedLegsTooltip(Vector3 worldPosition)
        {
            BodyPartData leftLeg = runtimeBodyPartData ?? bodyPartData;
            BodyPartData rightLeg = runtimePairedLegData ?? pairedLegData;

            if (leftLeg != null && rightLeg != null)
            {
                tooltipController.ShowCombinedLegsTooltip(leftLeg, rightLeg, worldPosition);
            }
            else
            {
                Debug.LogWarning("Cannot show combined legs tooltip: missing leg data");
            }
        }

        // НОВЫЕ МЕТОДЫ ДЛЯ НАСТРОЙКИ КОМБИНИРОВАННЫХ НОГ
        public void SetCombinedLegsData(string leftLegId, string rightLegId)
        {
            runtimePartId = leftLegId;
            runtimePairedLegId = rightLegId;
            dataSource = DataSourceType.PartId;
            showCombinedLegs = true;
        }

        public void SetCombinedLegsData(BodyPartData leftLegData, BodyPartData rightLegData)
        {
            runtimeBodyPartData = leftLegData;
            runtimePairedLegData = rightLegData;
            dataSource = DataSourceType.CombinedLegs;
            showCombinedLegs = true;
        }

        public void UpdateRuntimePartId(string newPartId)
        {
            runtimePartId = newPartId;
            if (!string.IsNullOrEmpty(runtimePartId))
            {
                dataSource = DataSourceType.PartId;
            }
        }

        public void UpdateRuntimeBodyPartData(BodyPartData newData)
        {
            runtimeBodyPartData = newData;
            if (runtimeBodyPartData != null)
            {
                dataSource = DataSourceType.BodyPartData;
            }
        }

        // НОВЫЙ МЕТОД: Установить вторую ногу для комбинации
        public void SetPairedLegData(BodyPartData pairedData)
        {
            runtimePairedLegData = pairedData;
        }

        public void SetPairedLegId(string pairedId)
        {
            runtimePairedLegId = pairedId;
        }

        private string GetPartIdFromMonster(MonsterData monster)
        {
            return specificPartType switch
            {
                BODYPART_TYPE.HEAD => monster.Head_id,
                BODYPART_TYPE.ARM => monster.NearArm_id,
                BODYPART_TYPE.LEG => monster.NearLeg_id,
                _ => monster.Head_id
            };
        }

        // Публичные методы для динамического изменения данных
        public void SetTooltipController(AbilityTooltipController controller)
        {
            tooltipController = controller;
        }

        public void SetPartId(string newPartId)
        {
            partId = newPartId;
            dataSource = DataSourceType.PartId;
        }

        public void SetAbilityId(string newAbilityId)
        {
            abilityId = newAbilityId;
            dataSource = DataSourceType.AbilityId;
        }

        public void SetBodyPartData(BodyPartData data)
        {
            bodyPartData = data;
            dataSource = DataSourceType.BodyPartData;
        }

        public void SetMonsterData(MonsterData data, BODYPART_TYPE partType = BODYPART_TYPE.HEAD)
        {
            monsterData = data;
            specificPartType = partType;
            dataSource = DataSourceType.MonsterData;
        }

        public void SetManualData(string name, string description, Sprite icon, Vector2Int[] shifts)
        {
            abilityName = name;
            abilityDescription = description;
            abilityIcon = icon;
            abilityShifts = shifts;
            dataSource = DataSourceType.Manual;
        }

        public void SetDataSource(DataSourceType sourceType)
        {
            dataSource = sourceType;
        }

        private void OnDisable()
        {
            if (isHovering)
            {
                isHovering = false;
                if (tooltipController != null)
                    tooltipController.HideTooltip();
            }
        }
    }
}