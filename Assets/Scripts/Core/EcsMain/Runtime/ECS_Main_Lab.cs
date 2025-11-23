using Core.Ecs.Modules;
using Domain.Extentions;
using Domain.Map;
using Domain.StateMachine.Mono;
using Interactions;
using Persistence.DS;
using Scellecs.Morpeh;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    [DefaultExecutionOrder(ECS.SCENE_ENTRY_POINT)]
    public class ECS_Main_Lab : MonoBehaviour
    {
        public static World m_labWorld;

        void Awake()
        {
            m_labWorld = World.Create();
            ECS.m_CurrentWorld = m_labWorld;
            m_labWorld.UpdateByUnity = false;
        }

        void Start()
        {
            Interactor.Init();


            DataStorage.NewFile<Inventory>().WithRecord<BodyPartsStorage>(new BodyPartsStorage());

            ref var bodyPartsStorage = ref DataStorage.GetRecordFromFile<Inventory, BodyPartsStorage>();

            if (bodyPartsStorage.parts == null)
            {
                bodyPartsStorage.parts = CreateStartParts();
            }

            ConfigureSystems();
        }

        private void ConfigureSystems()
        {
            m_labWorld.AddModule(new CoreModule());
            m_labWorld.AddModule(new StateMachineModule());

            m_labWorld.AddModule(new InputModule());

            m_labWorld.AddModule(new ServicesModule());

            m_labWorld.AddModule(new CleanupModule());

        }


        void Update()
        {
            m_labWorld.Update(Time.deltaTime);
            m_labWorld.CleanupUpdate(Time.deltaTime);
            m_labWorld.Commit();

            SM.Update();
        }

        void OnDestroy()
        {
            m_labWorld.Dispose();
        }

        private Dictionary<string, int> CreateStartParts()
        {
            Dictionary<string, int> parts = new()
            {
                { "mp_DinHead", 1 },
                { "mp_DinArm", 2 },
                { "mp_Din2Arm", 0 },
                { "mp_DinLeg", 2 },
                { "mp_DinTorso", 1 }
            };

            return parts;
        }
    }

}
