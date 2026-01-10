using Domain.Components;
using Domain.Map;
using Domain.Monster.Mono;
using Domain.Stats.Components;
using Game;
using Persistence.Buiders;
using Persistence.Components;
using Persistence.DB;
using Persistence.DS;
using Scellecs.Morpeh;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class LabMonsterCraftController : MonoBehaviour
    {
        private LabReferences labRef;

        private GameObject partSlotPrefab;

        private bool hasPartHead = false;
        private bool hasPartTorso = false;
        private bool hasPartArm = false;
        private bool hasPartLeg = false;

        //public BodyPartData[] initialResources;
        //public int[] initialCounts;

        public bool isHoldingResource { get; private set; }
        public BodyPartData heldPartData { get; private set; }

        private LabBodyPartHeldMono heldPartMono;
        private LabBodyPartStorageMono storageSlot;

        public int currentPreviewPoints;
        private bool isShowingPreview;


        //private Transform monsterPreviewContainer;
        //private Transform monsterSlotsContainer;
        //private Transform craftingSlotsContainer;


        //private Transform partStorageHeadsContainer;
        //private Transform partStorageTorsosContainer;
        //private Transform partStorageArmsContainer;
        //private Transform partStorageLegsContainer;

        //public GameObject createButton;

        //public MonsterNamingPanel namingPanel;

        //public bool isHoldingResource { get; private set; }
        //public BodyPartData heldPartData { get; private set; }

        void Start()
        {
            labRef = LabReferences.Instance();

            partSlotPrefab = Resources.Load<GameObject>("Lab/Prefabs/LabBodyPartStoragePrefab");

            heldPartMono = labRef.heldPartMono;

            labRef.createButton.SetActive(false);

            //monsterPreviewContainer = LabReferences.Instance().monsterPreviewContainer.transform;
            //monsterSlotsContainer = LabReferences.Instance().monsterSlotsContainer.transform;
            //craftingSlotsContainer = LabReferences.Instance().craftSlotsContainer.transform;

            //partStorageHeadsContainer = LabReferences.Instance().headsGridRef.transform;
            //partStorageTorsosContainer = LabReferences.Instance().torsosGridRef.transform;
            //partStorageArmsContainer = LabReferences.Instance().armsGridRef.transform;
            //partStorageLegsContainer = LabReferences.Instance().legsGridRef.transform;

            InitializeResources();
            InitializeCraftingSlots();
            InitializeHeldObject();
            InitializeMonstersSlots();

            isShowingPreview = false;
        }

        private bool AreAllCraftSlotsFilled()
        {
            var craftingSlotsContainer = labRef.GetCraftSlotsContainer();
            if (craftingSlotsContainer == null) return false;

            foreach (Transform child in craftingSlotsContainer)
            {
                LabCraftSlotMono slot = child.GetComponent<LabCraftSlotMono>();
                if (slot != null && !slot.IsOccupied())
                {
                    return false;
                }
            }
            return true;
        }

        public void UpdateCreateButtonState()
        {
            bool allSlotsFilled = AreAllCraftSlotsFilled();
            labRef.createButton.SetActive(allSlotsFilled);
        }

        #region Initializing
        private void InitializeResources()
        {
            ref var bodyPartsStorage = ref DataStorage.GetRecordFromFile<Inventory, BodyPartsStorage>();

            foreach (var part in bodyPartsStorage.parts)
            {
                if (part.Value <= 0) continue;

                BodyPartData data = new BodyPartData();

                BODYPART_TYPE tmp_type = BODYPART_TYPE.HEAD;
                Sprite sprite = null;

                string part_name = "";
                string part_desc = "";

                IResistanceModiffier.Stage part_res_fire = IResistanceModiffier.Stage.NONE;
                IResistanceModiffier.Stage part_res_poison = IResistanceModiffier.Stage.NONE;
                IResistanceModiffier.Stage part_res_bleed = IResistanceModiffier.Stage.NONE;

                int hp_val = 0;
                int spd_val = 0;

                string abt_name = "";
                string abt_desc = "";
                Sprite abt_icon = null;
                //Sprite abt_shifts = null;
                Vector2Int[] abt_shifts = null;

                if (DataBase.TryFindRecordByID(part.Key, out var e_record))
                {
                    //if (DataBase.TryGetRecord<Name>(e_record, out var nameRecord))
                    //{
                    //    data.partName = nameRecord.m_Value;
                    //}
                    //else
                    //{
                    //    data.partName = data.type.ToString();
                    //}
                    //data.description = $"A {data.type.ToString().ToLower()} part for crafting";

                    // part type
                    if (DataBase.TryGetRecord<TagHead>(e_record, out var e_tmh))
                    {
                        tmp_type = BODYPART_TYPE.HEAD;
                    }
                    if (DataBase.TryGetRecord<TagTorso>(e_record, out var e_tmt))
                    {
                        tmp_type = BODYPART_TYPE.TORSO;
                    }
                    if (DataBase.TryGetRecord<TagArm>(e_record, out var e_tma))
                    {
                        tmp_type = BODYPART_TYPE.ARM;
                    }
                    if (DataBase.TryGetRecord<TagLeg>(e_record, out var e_tml))
                    {
                        tmp_type = BODYPART_TYPE.LEG;
                    }


                    // part icon
                    if (DataBase.TryGetRecord<IconUI>(e_record, out var e_icon))
                    {
                        sprite = e_icon.m_Value;
                    }

                    // part name and descr
                    if (DataBase.TryGetRecord<Name>(e_record, out var e_name))
                    {
                        part_name = LocalizationManager.Instance.GetLocalizedValue(e_name.m_Value, "Parts");
                    }
                    if (DataBase.TryGetRecord<Description>(e_record, out var e_desc))
                    {
                        part_desc = LocalizationManager.Instance.GetLocalizedValue(e_desc.m_Value, "Parts");
                    }


                    // part effects - hp, speed, resists
                    if (DataBase.TryGetRecord<EffectsProvider>(e_record, out var e_effects_record))
                    {
                        if (DataBase.TryFindRecordByID(e_effects_record.m_Effects[0], out var e_effect_pointer))
                        {
                            if (DataBase.TryGetRecord<MaxHealthModifier>(e_effect_pointer, out var e_effect_hp))
                            {
                                hp_val = e_effect_hp.m_Flat;
                            }
                            if (DataBase.TryGetRecord<SpeedModifier>(e_effect_pointer, out var e_effect_spd))
                            {
                                spd_val = e_effect_spd.m_Flat;
                            }
                            if (DataBase.TryGetRecord<BurningResistanceModiffier>(e_effect_pointer, out var e_effect_res_f))
                            {
                                part_res_fire = e_effect_res_f.m_Stage;
                            }
                            if (DataBase.TryGetRecord<PoisonResistanceModiffier>(e_effect_pointer, out var e_effect_res_p))
                            {
                                part_res_poison = e_effect_res_p.m_Stage;
                            }
                            if (DataBase.TryGetRecord<BleedResistanceModiffier>(e_effect_pointer, out var e_effect_res_b))
                            {
                                part_res_bleed = e_effect_res_b.m_Stage;
                            }
                        }
                    }


                    // part ability
                    if (DataBase.TryGetRecord<AbilityProvider>(e_record, out var e_abt_id))
                    {
                        string abt_id = e_abt_id.m_AbilityTemplateID;

                        if (DataBase.TryFindRecordByID(abt_id, out var e_abt_record))
                        {
                            if (DataBase.TryGetRecord<Name>(e_abt_record, out var e_abt_name))
                            {
                                abt_name = LocalizationManager.Instance.GetLocalizedValue(e_abt_name.m_Value, "Parts");
                            }
                            if (DataBase.TryGetRecord<Description>(e_abt_record, out var e_abt_desc))
                            {
                                abt_desc = LocalizationManager.Instance.GetLocalizedValue(e_abt_desc.m_Value, "Parts");
                            }
                            if (DataBase.TryGetRecord<IconUI>(e_abt_record, out var e_abt_icon))
                            {
                                abt_icon = e_abt_icon.m_Value;
                            }
                            //if (DataBase.TryGetRecord<AbilityShiftsSprite>(e_abt_record, out var e_abt_shifts))
                            //{
                            //    abt_shifts = e_abt_shifts.m_Value;
                            //}
                            if (DataBase.TryGetRecord<AbilityDefenition>(e_abt_record, out var e_abt_definitions))
                            {
                                abt_shifts = e_abt_definitions.m_Shifts;
                            }
                        }
                    }
                }

                data.type = tmp_type;
                data.db_id = part.Key;
                data.icon = sprite;

                data.partName = part_name;
                data.description = part_desc;

                data.hp_amount = hp_val;
                data.speed_amount = spd_val;

                data.res_fire = part_res_fire;
                data.res_poison = part_res_poison;
                data.res_bleed = part_res_bleed;

                data.ability_name = abt_name;
                data.ability_desc = abt_desc;
                data.ability_icon = abt_icon;
                data.ability_shifts = abt_shifts;



                CreateLabBodyPartStorageMono(data, part.Value);
            }
        }

        private void InitializeCraftingSlots()
        {
            foreach (Transform child in labRef.GetCraftSlotsContainer())
            {
                LabCraftSlotMono slot = child.GetComponent<LabCraftSlotMono>();
                if (slot != null)
                {
                    slot.Initialize();
                }
            }
        }

        private void CreateLabBodyPartStorageMono(BodyPartData data, int count)
        {
            Transform container = null;

            switch (data.type)
            {
                case BODYPART_TYPE.HEAD:
                    container = labRef.GetHeadsContainer();
                    hasPartHead = true;
                    break;
                case BODYPART_TYPE.TORSO:
                    container = labRef.GetTorsosContainer();
                    hasPartTorso = true;
                    break;
                case BODYPART_TYPE.ARM:
                    container = labRef.GetArmsContainer();
                    hasPartArm = true;
                    break;
                case BODYPART_TYPE.LEG:
                    container = labRef.GetLegsContainer();
                    hasPartLeg = true;
                    break;
            }

            GameObject slotObj = Instantiate(partSlotPrefab, container);
            LabBodyPartStorageMono slot = slotObj.GetComponent<LabBodyPartStorageMono>();
            slot.Initialize(data, count);
        }

        private void InitializeHeldObject()
        {
            heldPartMono.Initialize();

            heldPartMono.ShowSelf(false);
        }

        private void InitializeMonstersSlots()
        {
            var monsterSlotsContainer = labRef.GetMonsterSlotsContainer();
            if (monsterSlotsContainer == null) return;

            foreach (Transform child in monsterSlotsContainer)
            {
                LabMonsterSlotMono slot = child.GetComponent<LabMonsterSlotMono>();
                if (slot != null)
                {
                    slot.Initialize();
                }
            }

            UpdateMonsterSlots();
        }

        #endregion


        #region Picking and placing

        public void PickResourceFromStorage(LabBodyPartStorageMono slot)
        {
            heldPartData = slot.bodyPartData;
            isHoldingResource = true;
            storageSlot = slot;

            heldPartMono.SetPart(heldPartData);
            heldPartMono.ShowSelf(true);

            HighlightCompatibleSlots();
        }

        public void PickResourceFromCrafting(LabCraftSlotMono slot)
        {
            heldPartData = slot.GetContainedResource();
            storageSlot = slot.GetOriginalSlot();
            isHoldingResource = true;

            heldPartMono.SetPart(heldPartData);
            heldPartMono.gameObject.SetActive(true);

            HighlightCompatibleSlots();
            SubstrPreviewPoint();
        }

        public bool QuickPlaceResourceInSlot(LabBodyPartStorageMono storage_slot)
        {
            bool res = false;
            storageSlot = storage_slot;

            GameObject craft_slot = null;

            switch (storage_slot.part_type)
            {
                case BODYPART_TYPE.HEAD:
                    craft_slot = labRef.headCraftSlotRef;
                    break;
                case BODYPART_TYPE.TORSO:
                    craft_slot = labRef.torsoCraftSlotRef;
                    break;
                case BODYPART_TYPE.ARM:
                    craft_slot = labRef.armLCraftSlotRef;
                    if (craft_slot.GetComponent<LabCraftSlotMono>().IsOccupied())
                    {
                        craft_slot = labRef.armRCraftSlotRef;
                    }
                    break;
                case BODYPART_TYPE.LEG:
                    craft_slot = labRef.legLCraftSlotRef;
                    if (craft_slot.GetComponent<LabCraftSlotMono>().IsOccupied())
                    {
                        craft_slot = labRef.legRCraftSlotRef;
                    }
                    break;
            }

            if (craft_slot != null)
            {
                LabCraftSlotMono slot = craft_slot.GetComponent<LabCraftSlotMono>();
                if (slot != null)
                {
                    res = slot.TryPlaceResource(storage_slot.bodyPartData);
                }
            }

            return res;
        }

        public void PlaceResourceInSlot(LabCraftSlotMono slot)
        {
            slot.SetOriginalSlot(storageSlot);
            isHoldingResource = false;
            heldPartData = null;

            heldPartMono.ShowSelf(false);
            ResetAllSlotsHighlight();
            AddPreviewPoint();

            storageSlot = null;
            UpdateCreateButtonState();
        }

        public void MoveResourceFromToSlot(LabCraftSlotMono from_slot, LabCraftSlotMono to_slot)
        {
            to_slot.SetOriginalSlot(from_slot.GetOriginalSlot());
            to_slot.UpdatePartData(from_slot.GetContainedResource());
            from_slot.ClearSlot();
            UpdateCreateButtonState();
        }

        #endregion


        #region Dropping Held resourse

        public void ReturnHeldResource()
        {
            if (!isHoldingResource) return;

            if (storageSlot != null)
            {
                storageSlot.ReturnResource();

                AudioManager.Instance?.PlaySound(AudioManager.putSound);
            }
            CancelResourceHold();
        }

        public void CancelResourceHold()
        {
            isHoldingResource = false;
            heldPartData = null;

            heldPartMono.ShowSelf(false);
            ResetAllSlotsHighlight();
            storageSlot = null;
            UpdateCreateButtonState();
        }

        #endregion


        #region Highlighting stuff

        private void HighlightCompatibleSlots()
        {
            var craftingSlotsContainer = labRef.GetCraftSlotsContainer();
            if (craftingSlotsContainer == null) return;

            foreach (Transform child in craftingSlotsContainer)
            {
                LabCraftSlotMono slot = child.GetComponent<LabCraftSlotMono>();
                if (slot != null && slot.GetContainedResource() == null)
                {
                    if (slot.requiredType == heldPartData.type)
                    {
                        slot.HighlightSlot();
                    }
                }
            }
        }

        private void ResetAllSlotsHighlight()
        {
            var craftingSlotsContainer = labRef.GetCraftSlotsContainer();
            if (craftingSlotsContainer == null) return;

            foreach (Transform child in craftingSlotsContainer)
            {
                LabCraftSlotMono slot = child.GetComponent<LabCraftSlotMono>();
                if (slot != null && !slot.IsOccupied())
                {
                    slot.ClearSlot();
                }
            }
        }

        #endregion



        #region Monster Creation and visual update
        public void AddPreviewPoint()
        {
            currentPreviewPoints += 1;
            if (currentPreviewPoints >= 6)
            {
                currentPreviewPoints = 6;
                if (!isShowingPreview)
                {
                    PreviewMonsterUpdate();
                }
            }
        }

        public void SubstrPreviewPoint()
        {
            currentPreviewPoints -= 1;
            if (isShowingPreview && currentPreviewPoints < 6)
            {
                DeviewMonsterUpdate();
            }
            if (currentPreviewPoints <= 0)
            {
                currentPreviewPoints = 0;
            }
        }

        public void PreviewMonsterUpdate()
        {
            var monsterData = GetMonsterDataFromCraftSlots(false);
            if (monsterData == null) return;

            isShowingPreview = true;
            var monsterPreviewContainer = labRef.GetMonsterPreviewContainer();
            if (monsterPreviewContainer == null) return;

            var builder = new MonsterBuilder(ECS_Main_Lab.m_labWorld)
                .AttachHead(monsterData.Head_id)
                .AttachBody(monsterData.Body_id)
                .AttachFarArm(monsterData.FarArm_id)
                .AttachNearArm(monsterData.NearArm_id)
                .AttachNearLeg(monsterData.NearLeg_id)
                .AttachFarLeg(monsterData.FarLeg_id);

            var t_monsterEntity = builder.Build();
            var t_transform = ECS_Main_Lab.m_labWorld.GetStash<TransformRefComponent>();

            ref var monstTransf = ref t_transform.Get(t_monsterEntity).Value;
            monstTransf.position = monsterPreviewContainer.position;
            monstTransf.parent = monsterPreviewContainer;

            LabReferences.Instance().tutorialController.ContinueSpecial();
            Debug.Log("!!!!1");
        }

        public void DeviewMonsterUpdate()
        {
            isShowingPreview = false;
            var monsterPreviewContainer = labRef.GetMonsterPreviewContainer();
            if (monsterPreviewContainer != null && monsterPreviewContainer.childCount > 0)
            {
                Destroy(monsterPreviewContainer.GetChild(0).gameObject);
            }
        }

        public void CreateMonster()
        {
            AudioManager.Instance?.PlaySound(AudioManager.buttonClickSound);

            ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();
            if (monstersStorage.storage_monsters.Count >= monstersStorage.max_capacity)
            {
                return;
            }


            var monsterData = GetMonsterDataFromCraftSlots(false);
            if (monsterData == null) return;

            if (labRef.namingPanel != null)
            {
                labRef.namingPanel.ShowNamingPanel(monsterData, OnMonsterNamed);
            }
            else
            {
                FinalizeMonsterCreation(monsterData, "Unnamed Monster");
            }
        }

       

        private void OnMonsterNamed(string monsterName)
        {
            var monsterData = GetMonsterDataFromCraftSlots(false);
            if (monsterData != null)
            {
                FinalizeMonsterCreation(monsterData, monsterName);
            }
        }

        private void FinalizeMonsterCreation(MonsterData monsterData, string monsterName)
        {
            ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();

            // Проверка на уникальность имени
            if (IsMonsterNameExists(monsterName))
            {
                // Имя уже существует, показываем сообщение об ошибке
                Debug.LogWarning($"Monster with name '{monsterName}' already exists!");

                // Можно показать сообщение пользователю
                if (labRef.namingPanel != null)
                {
                    labRef.namingPanel.ShowErrorMessage($"Monster with name '{monsterName}' already exists! Please choose a different name.");
                }
                return;
            }

            AudioManager.Instance?.PlaySound(AudioManager.createMonsterSound);

            monsterData.SetName(monsterName);
            SpendBodyPartsForMonster(monsterData);

            var updatedMonsters = new List<MonsterData>(monstersStorage.storage_monsters);
            updatedMonsters.Add(monsterData);
            monstersStorage.storage_monsters = updatedMonsters;

            ClearAllCraftSlots();

            if (labRef.namingPanel != null)
            {
                labRef.namingPanel.ClearInputField();
                labRef.namingPanel.HideErrorMessage(); // Скрываем сообщение об ошибке если было показано
            }

            UpdateMonsterSlots();
            UpdateCreateButtonState();

            if (labRef.monstersController != null)
            {
                labRef.monstersController.SaveCurrentState();
            }

            LabReferences.Instance().tutorialController.ContinueSpecial();
            Debug.Log("!!!!2");
        }

        // Метод для проверки существования имени монстра
        public bool IsMonsterNameExists(string monsterName)
        {
            if (string.IsNullOrEmpty(monsterName))
                return false;

            ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();

            foreach (var monster in monstersStorage.storage_monsters)
            {
                if (monster.m_MonsterName != null && monster.m_MonsterName.Equals(monsterName, System.StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }


        private void SpendBodyPartsForMonster(MonsterData monsterData)
        {
            Debug.Log($"Spending body parts for monster creation: {monsterData.m_MonsterName}");

            SpendBodyPart(monsterData.Head_id);
            SpendBodyPart(monsterData.Body_id);
            SpendBodyPart(monsterData.NearArm_id);
            SpendBodyPart(monsterData.FarArm_id);
            SpendBodyPart(monsterData.NearLeg_id);
            SpendBodyPart(monsterData.FarLeg_id);

            RefreshInventoryDisplay();
        }

        private void SpendBodyPart(string bodyPartId)
        {
            if (string.IsNullOrEmpty(bodyPartId)) return;

            ref var bodyPartsStorage = ref DataStorage.GetRecordFromFile<Inventory, BodyPartsStorage>();
            if (bodyPartsStorage.parts.ContainsKey(bodyPartId) && bodyPartsStorage.parts[bodyPartId] > 0)
            {
                bodyPartsStorage.parts[bodyPartId]--;
                if (bodyPartsStorage.parts[bodyPartId] == 0)
                {
                    bodyPartsStorage.parts.Remove(bodyPartId);
                }
            }
        }

        private void ClearAllCraftSlots()
        {
            var craftingSlotsContainer = labRef.GetCraftSlotsContainer();
            if (craftingSlotsContainer == null) return;

            foreach (Transform child in craftingSlotsContainer)
            {
                LabCraftSlotMono slot = child.GetComponent<LabCraftSlotMono>();
                if (slot != null)
                {
                    SubstrPreviewPoint();
                    slot.ClearSlot();
                }
            }
            currentPreviewPoints = 0;
            DeviewMonsterUpdate();
        }


        public MonsterData GetMonsterDataFromCraftSlots(bool need_clear)
        {
            string data_head = "", data_torso = "", data_arml = "", data_armr = "", data_legl = "", data_legr = "";

            var craftingSlotsContainer = labRef.GetCraftSlotsContainer();
            if (craftingSlotsContainer == null) return null;

            foreach (Transform child in craftingSlotsContainer)
            {
                LabCraftSlotMono slot = child.GetComponent<LabCraftSlotMono>();
                if (slot != null && slot.IsOccupied())
                {
                    string res = slot.GetContainedResource().db_id;
                    switch (slot.requiredType)
                    {
                        case BODYPART_TYPE.HEAD:
                            data_head = res;
                            break;
                        case BODYPART_TYPE.TORSO:
                            data_torso = res;
                            break;
                        case BODYPART_TYPE.ARM:
                            if (slot.gameObject == labRef.armLCraftSlotRef)
                                data_arml = res;
                            else
                                data_armr = res;
                            break;
                        case BODYPART_TYPE.LEG:
                            if (slot.gameObject == labRef.legLCraftSlotRef)
                                data_legl = res;
                            else
                                data_legr = res;
                            break;
                    }
                }
                else
                {
                    return null;
                }
            }

            if (need_clear)
            {
                ClearAllCraftSlots();
            }

            return new MonsterData(data_head, data_arml, data_armr, data_torso, data_legl, data_legr);
        }


        public void UpdateMonsterSlots()
        {
            if (labRef.monstersController != null)
            {
                labRef.monstersController.UpdateMonsterSlotsFromCrafting();
            }
        }

        #endregion

        public void OnBodyPartHoverStart(BodyPartData bodyPart)
        {
            var allCraftSlots = FindObjectsOfType<LabCraftSlotMono>();
            foreach (var slot in allCraftSlots)
            {
                if (slot.requiredType == bodyPart.type && !slot.IsOccupied())
                {
                    slot.StartHighlightBlink();
                }
            }
        }

        public void OnBodyPartHoverEnd()
        {
            var allCraftSlots = FindObjectsOfType<LabCraftSlotMono>();
            foreach (var slot in allCraftSlots)
            {
                slot.StopHighlightBlink();
            }
        }

        public void DeleteMonsterAndReturnParts(MonsterData monsterData)
        {
            if (monsterData == null) return;

            ReturnBodyPartToInventory(monsterData.Head_id);
            ReturnBodyPartToInventory(monsterData.Body_id);
            ReturnBodyPartToInventory(monsterData.NearArm_id);
            ReturnBodyPartToInventory(monsterData.FarArm_id);
            ReturnBodyPartToInventory(monsterData.NearLeg_id);
            ReturnBodyPartToInventory(monsterData.FarLeg_id);

            RefreshInventoryDisplay();
        }

        private void ReturnBodyPartToInventory(string bodyPartId)
        {
            if (string.IsNullOrEmpty(bodyPartId)) return;

            ref var bodyPartsStorage = ref DataStorage.GetRecordFromFile<Inventory, BodyPartsStorage>();
            if (bodyPartsStorage.parts.ContainsKey(bodyPartId))
            {
                bodyPartsStorage.parts[bodyPartId]++;
            }
            else
            {
                bodyPartsStorage.parts[bodyPartId] = 1;
            }
        }

        private void RefreshInventoryDisplay()
        {
            ClearInventoryContainers();
            InitializeResources();
        }

        private void ClearInventoryContainers()
        {
            ClearContainer(labRef.GetHeadsContainer());
            ClearContainer(labRef.GetTorsosContainer());
            ClearContainer(labRef.GetArmsContainer());
            ClearContainer(labRef.GetLegsContainer());
        }

        private void ClearContainer(Transform container)
        {
            if (container == null) return;
            foreach (Transform child in container)
            {
                if (child.GetComponent<LabBodyPartStorageMono>() != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}
