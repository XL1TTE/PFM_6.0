using Domain.Components;
using Domain.Map;
using Domain.Monster.Mono;
using Game;
using Persistence.Buiders;
using Persistence.Components;
using Persistence.DB;
using Persistence.DS;
using Scellecs.Morpeh;
using UnityEngine;

namespace Project
{
    public class LabMonsterCraftController : MonoBehaviour//, IDragHandler
    {
        private Filter all_body_parts;
        private GameObject partSlotPrefab;
        //private GameObject heldPartPrefab;


        private Transform monsterPreviewContainer;
        private Transform monsterSlotsContainer;
        private Transform craftingSlotsContainer;


        private Transform partStorageHeadsContainer;
        private Transform partStorageTorsosContainer;
        private Transform partStorageArmsContainer;
        private Transform partStorageLegsContainer;

        private bool hasPartHead = false;
        private bool hasPartTorso = false;
        private bool hasPartArm = false;
        private bool hasPartLeg = false;

        public BodyPartData[] initialResources;
        public int[] initialCounts;


        public bool isHoldingResource { get; private set; }
        public BodyPartData heldPartData { get; private set; }


        private LabBodyPartHeldMono heldPartMono;
        private LabBodyPartStorageMono storageSlot;


        public int currentPreviewPoints;
        private bool isShowingPreview;

        void Start()
        {
            all_body_parts = DataBase.Filter.With<TagBodyPart>().Build();


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

            InitializeResources();
            InitializeCraftingSlots();
            InitializeHeldObject();
            InitializeMonstersSlots();



            if (!hasPartHead)
            {
                LabReferences.Instance().headsTextRef.SetActive(false);
            }
            if (!hasPartTorso)
            {
                LabReferences.Instance().torsosTextRef.SetActive(false);
            }
            if (!hasPartArm)
            {
                LabReferences.Instance().armsTextRef.SetActive(false);
            }
            if (!hasPartLeg)
            {
                LabReferences.Instance().legsTextRef.SetActive(false);
            }

            isShowingPreview = false;
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
                BodyPartData data = new BodyPartData();

                BODYPART_TYPE tmp_type = BODYPART_TYPE.HEAD;

                var tmp_potential_event = all_body_parts.GetEntity(1);

                UnityEngine.Sprite sprite = null;

                if (DataBase.TryFindRecordByID(part.Key, out var e_record))
                {

                    if (DataBase.TryGetRecord<TagLeg>(e_record, out var e_tmh))
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
        }

        #endregion


        #region Picking and placing

        public void PickResourceFromStorage(LabBodyPartStorageMono slot)
        {
            heldPartData = slot.bodyPartData;
            isHoldingResource = true;
            storageSlot = slot;

            // ���������� � ����������� ������, ��������� �� ������
            heldPartMono.SetPart(heldPartData);

            heldPartMono.ShowSelf(true);

            // ������������ ���������� ����� ������
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

            // ���������� ��������� ���� ������
            ResetAllSlotsHighlight();


            AddPreviewPoint();

            storageSlot = null;
        }
        public void MoveResourceFromToSlot(LabCraftSlotMono from_slot, LabCraftSlotMono to_slot)
        {
            to_slot.SetOriginalSlot(from_slot.GetOriginalSlot());
            to_slot.UpdatePartData(from_slot.GetContainedResource());

            from_slot.ClearSlot();
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
                    // ���������� ������� ������, �� ������ ���� ���� ������
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


            var monsterData = GetMonsterDataFromCraftSlots(true);
            if (monsterData == null) { return; }

            var tmp_list_copy = monstersStorage.storage_monsters;

            tmp_list_copy.Add(monsterData);

            monstersStorage.storage_monsters = tmp_list_copy;

            UpdateMonsterSlots();
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

            MonsterData monsterData = new MonsterData("name", data_head, data_arml, data_armr, data_torso, data_legl, data_legr);
            return monsterData;
        }


        public void UpdateMonsterSlots()
        {
            var monster_slots = LabReferences.Instance().monsterSlotsContainer;

            ref var monstersStorage = ref DataStorage.GetRecordFromFile<Inventory, MonstersStorage>();

            var i = 0;
            var max = monstersStorage.storage_monsters.Count;

            foreach (Transform slot in monster_slots.transform)
            {
                LabMonsterSlotMono mono = slot.GetComponent<LabMonsterSlotMono>();
                if (mono != null)
                {
                    mono.OccupySelf(monstersStorage.storage_monsters[i]);
                    i++;
                    if (i >= max)
                    {
                        break;
                    }
                }
            }
        }

        #endregion
    }
}
