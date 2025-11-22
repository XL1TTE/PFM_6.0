using Domain.Components;
using Domain.Map;
using Domain.Monster.Mono;
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
        private GameObject partSlotPrefab;

        private Transform monsterPreviewContainer;
        private Transform monsterSlotsContainer;
        private Transform craftingSlotsContainer;


        private Transform partStorageHeadsContainer;
        private Transform partStorageTorsosContainer;
        private Transform partStorageArmsContainer;
        private Transform partStorageLegsContainer;

        public GameObject createButton;

        private bool hasPartHead = false;
        private bool hasPartTorso = false;
        private bool hasPartArm = false;
        private bool hasPartLeg = false;

        public BodyPartData[] initialResources;
        public int[] initialCounts;

        public MonsterNamingPanel namingPanel;


        public bool isHoldingResource { get; private set; }
        public BodyPartData heldPartData { get; private set; }


        private LabBodyPartHeldMono heldPartMono;
        private LabBodyPartStorageMono storageSlot;


        public int currentPreviewPoints;
        private bool isShowingPreview;

        void Start()
        {
            partSlotPrefab = Resources.Load<GameObject>("Lab/Prefabs/LabBodyPartStoragePrefab");
            //heldPartPrefab = Resources.Load<GameObject>("Lab/Prefabs/LabBodyPartHeldPrefab");

            heldPartMono = LabReferences.Instance().heldPartMono;

            monsterPreviewContainer = LabReferences.Instance().monsterPreviewContainer.transform;
            monsterSlotsContainer = LabReferences.Instance().monsterSlotsContainer.transform;
            craftingSlotsContainer = LabReferences.Instance().craftSlotsContainer.transform;

            partStorageHeadsContainer = LabReferences.Instance().headsGridRef.transform;
            partStorageTorsosContainer = LabReferences.Instance().torsosGridRef.transform;
            partStorageArmsContainer = LabReferences.Instance().armsGridRef.transform;
            partStorageLegsContainer = LabReferences.Instance().legsGridRef.transform;

            createButton.SetActive(false);

            InitializeResources();
            InitializeCraftingSlots();
            InitializeHeldObject();
            InitializeMonstersSlots();

            //if (!hasPartHead)
            //{
            //    LabReferences.Instance().headsTextRef.SetActive(false);
            //}
            //if (!hasPartTorso)
            //{
            //    LabReferences.Instance().torsosTextRef.SetActive(false);
            //}
            //if (!hasPartArm)
            //{
            //    LabReferences.Instance().armsTextRef.SetActive(false);
            //}
            //if (!hasPartLeg)
            //{
            //    LabReferences.Instance().legsTextRef.SetActive(false);
            //}

            isShowingPreview = false;
        }

        private bool AreAllCraftSlotsFilled()
        {
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
            createButton.SetActive(allSlotsFilled);
        }


        //void Update()
        //{
        //    if (isHoldingResource && heldPartMono != null)
        //    {
        //        heldPartMono.FollowMouse();
        //    }
        //}

        //public void OnDrag(PointerEventData eventData)
        //{
        //    if (isHoldingResource && heldPartMono != null)
        //    {
        //        heldPartMono.FollowMouse();
        //    }
        //}

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

                if (DataBase.TryFindRecordByID(part.Key, out var e_record))
                {
                    if (DataBase.TryGetRecord<Name>(e_record, out var nameRecord))
                    {
                        data.partName = nameRecord.m_Value;
                    }
                    else
                    {
                        data.partName = data.type.ToString(); //јЋя–ћ ”ƒјЋ»“№
                    }

                    data.description = $"A {data.type.ToString().ToLower()} part for crafting";

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

                    if (DataBase.TryGetRecord<IconUI>(e_record, out var e_icon))
                    {
                        sprite = e_icon.m_Value;
                    }
                }

                data.type = tmp_type;
                data.db_id = part.Key;
                data.icon = sprite;

                CreateLabBodyPartStorageMono(data, part.Value);
            }
        }

        private void InitializeCraftingSlots()
        {
            foreach (Transform child in craftingSlotsContainer)
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
            Transform container = partStorageHeadsContainer;

            switch (data.type)
            {
                case BODYPART_TYPE.HEAD:
                    container = partStorageHeadsContainer;
                    hasPartHead = true;
                    break;
                case BODYPART_TYPE.TORSO:
                    container = partStorageTorsosContainer;
                    hasPartTorso = true;
                    break;
                case BODYPART_TYPE.ARM:
                    container = partStorageArmsContainer;
                    hasPartArm = true;
                    break;
                case BODYPART_TYPE.LEG:
                    container = partStorageLegsContainer;
                    hasPartLeg = true;
                    break;
            }

            GameObject slotObj = Instantiate(partSlotPrefab, container);
            LabBodyPartStorageMono slot = slotObj.GetComponent<LabBodyPartStorageMono>();
            slot.Initialize(data, count);
        }

        private void InitializeHeldObject()
        {
            //GameObject heldObj = Instantiate(heldPartPrefab, LabReferences.Instance().heldPartContainer.transform);
            //heldPartMono = heldObj.GetComponent<LabBodyPartHeldMono>();
            heldPartMono.Initialize();

            heldPartMono.ShowSelf(false);
        }

        private void InitializeMonstersSlots()
        {
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
                    craft_slot = LabReferences.Instance().headCraftSlotRef;
                    break;

                case BODYPART_TYPE.TORSO:
                    craft_slot = LabReferences.Instance().torsoCraftSlotRef;
                    break;

                case BODYPART_TYPE.ARM:
                    craft_slot = LabReferences.Instance().armLCraftSlotRef;

                    if (craft_slot.GetComponent<LabCraftSlotMono>().IsOccupied())
                    {
                        craft_slot = LabReferences.Instance().armRCraftSlotRef;
                    }
                    break;

                case BODYPART_TYPE.LEG:
                    craft_slot = LabReferences.Instance().legLCraftSlotRef;

                    if (craft_slot.GetComponent<LabCraftSlotMono>().IsOccupied())
                    {
                        craft_slot = LabReferences.Instance().legRCraftSlotRef;
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
            foreach (Transform child in craftingSlotsContainer)
            {
                LabCraftSlotMono slot = child.GetComponent<LabCraftSlotMono>();
                if (slot != null)
                {
                    // ¬озвращаем обычный спрайт, но только если слот пустой
                    if (!slot.IsOccupied())
                    {
                        slot.ClearSlot();
                    }
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
            if (monsterData == null) { return; }


            isShowingPreview = true;


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
        }

        public void DeviewMonsterUpdate()
        {
            isShowingPreview = false;

            if (monsterPreviewContainer.childCount > 0)
            {
                Destroy(monsterPreviewContainer.GetChild(0).gameObject);
            }
        }

        public void CreateMonster()
        {
            ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();


            if (monstersStorage.storage_monsters.Count >= monstersStorage.max_capacity)
            {
                return;
            }

            var monsterData = GetMonsterDataFromCraftSlots(false);
            if (monsterData == null) { return; }
            ShowNamingPanel(monsterData);
        }

        private void ShowNamingPanel(MonsterData monsterData)
        {
            if (namingPanel != null)
            {
                namingPanel.ShowNamingPanel(monsterData, OnMonsterNamed);
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

            Debug.Log($"Storage before adding monster: {monstersStorage.storage_monsters.Count} monsters");

            monsterData.SetName(monsterName);

            SpendBodyPartsForMonster(monsterData);

            var updatedMonsters = new List<MonsterData>(monstersStorage.storage_monsters);
            updatedMonsters.Add(monsterData);
            monstersStorage.storage_monsters = updatedMonsters;

            ClearAllCraftSlots();

            if (namingPanel != null)
            {
                namingPanel.ClearInputField();
                Debug.Log("Input field cleared after monster creation");
            }

            UpdateMonsterSlots();
            createButton.SetActive(false);

            Debug.Log($"Monster created: {monsterName}");

            var monstersController = FindObjectOfType<LabMonstersController>();
            if (monstersController != null)
            {
                monstersController.SaveCurrentState();
            }
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
            if (string.IsNullOrEmpty(bodyPartId))
            {
                Debug.LogWarning("Attempted to spend null or empty body part ID");
                return;
            }

            ref var bodyPartsStorage = ref DataStorage.GetRecordFromFile<Inventory, BodyPartsStorage>();

            int currentCount = bodyPartsStorage.parts.ContainsKey(bodyPartId) ? bodyPartsStorage.parts[bodyPartId] : 0;

            if (bodyPartsStorage.parts.ContainsKey(bodyPartId) && bodyPartsStorage.parts[bodyPartId] > 0)
            {
                bodyPartsStorage.parts[bodyPartId]--;
                int newCount = bodyPartsStorage.parts[bodyPartId];
                Debug.Log($"Spent body part: {bodyPartId} (was: {currentCount}, now: {newCount})");

                if (newCount == 0)
                {
                    bodyPartsStorage.parts.Remove(bodyPartId);
                }
            }
            else
            {
                Debug.LogError($"Cannot spend body part {bodyPartId} - not enough in inventory!");
            }
        }

        private void ClearAllCraftSlots()
        {
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
            string data_head = "";
            string data_torso = "";
            string data_arml = "";
            string data_armr = "";
            string data_legl = "";
            string data_legr = "";


            foreach (Transform child in craftingSlotsContainer)
            {
                LabCraftSlotMono slot = child.GetComponent<LabCraftSlotMono>();
                if (slot != null)
                {
                    if (slot.IsOccupied())
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
                                if (slot == LabReferences.Instance().armLCraftSlotRef.GetComponent<LabCraftSlotMono>())
                                {
                                    data_arml = res;
                                }
                                else
                                {
                                    data_armr = res;
                                }
                                break;
                            case BODYPART_TYPE.LEG:
                                if (slot == LabReferences.Instance().legLCraftSlotRef.GetComponent<LabCraftSlotMono>())
                                {
                                    data_legl = res;
                                }
                                else
                                {
                                    data_legr = res;
                                }
                                break;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }

            if (need_clear) 
            {            
                foreach (Transform child in craftingSlotsContainer)
                {
                    LabCraftSlotMono slot = child.GetComponent<LabCraftSlotMono>();
                    if (slot != null)
                    {
                        SubstrPreviewPoint();
                        slot.ClearSlot();
                    }
                }
            }

            MonsterData monsterData = new(data_head, data_arml, data_armr, data_torso, data_legl, data_legr);
            return monsterData;
        }


        public void UpdateMonsterSlots()
        {
            var monstersController = FindObjectOfType<LabMonstersController>();
            if (monstersController != null)
            {
                monstersController.UpdateMonsterSlotsFromCrafting();
            }
            else
            {
                Debug.LogError("LabMonstersController not found!");
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
            Debug.Log($"Deleting monster: {monsterData.m_MonsterName} and returning body parts");

            if (monsterData == null)
            {
                Debug.LogError("MonsterData is null! Cannot return parts.");
                return;
            }

            Debug.Log($"Returning parts for monster: Head={monsterData.Head_id}, Body={monsterData.Body_id}, " +
                      $"Arms=({monsterData.NearArm_id}, {monsterData.FarArm_id}), " +
                      $"Legs=({monsterData.NearLeg_id}, {monsterData.FarLeg_id})");

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
            if (string.IsNullOrEmpty(bodyPartId))
            {
                Debug.LogWarning("Attempted to return null or empty body part ID");
                return;
            }

            ref var bodyPartsStorage = ref DataStorage.GetRecordFromFile<Inventory, BodyPartsStorage>();

            int currentCount = bodyPartsStorage.parts.ContainsKey(bodyPartId) ? bodyPartsStorage.parts[bodyPartId] : 0;

            if (bodyPartsStorage.parts.ContainsKey(bodyPartId))
            {
                bodyPartsStorage.parts[bodyPartId]++;
            }
            else
            {
                bodyPartsStorage.parts[bodyPartId] = 1;
            }

            int newCount = bodyPartsStorage.parts[bodyPartId];
            Debug.Log($"Returned body part to inventory: {bodyPartId} (was: {currentCount}, now: {newCount})");
        }

        private void RefreshInventoryDisplay()
        {
            ClearInventoryContainers();

            InitializeResources();

            Debug.Log("Inventory refreshed after monster deletion");
        }

        private void ClearInventoryContainers()
        {
            ClearContainer(partStorageHeadsContainer);
            ClearContainer(partStorageTorsosContainer);
            ClearContainer(partStorageArmsContainer);
            ClearContainer(partStorageLegsContainer);
        }

        private void ClearContainer(Transform container)
        {
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
