using Core.Ecs.Modules;
using Domain.Extentions;
using Domain.Map;
using Domain.Map.Events;
using Domain.Map.Mono;
using Domain.StateMachine.Components;
using Domain.StateMachine.Mono;
using DS.Files;
using Gameplay.Map.Systems;
using Gameplay.MapEvents.Systems;
using Interactions;
using Persistence.DS;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{

    [DefaultExecutionOrder(ECS.SCENE_ENTRY_POINT)]
    public class ECS_Main_Map : MonoBehaviour
    {
        public static World m_mapWorld;

        // this is a fucking stupid fucking crutch to make it fucking stupid fucking work
        private bool flag_first_load = false;
        private bool flag_tutorial_load = false;

        private static ECS_Main_Map m_Instance;

        void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this;
            }

            m_mapWorld = World.Create();
            ECS.m_CurrentWorld = m_mapWorld;

            SM.EnterState<MapSceneState>();
            m_mapWorld.UpdateByUnity = false;
        }

        void Start()
        {
            Interactor.Init();



            ref var crusadeState = ref DataStorage.GetRecordFromFile<Crusade, CrusadeState>();

            switch (crusadeState.crusade_state)
            {
                case CRUSADE_STATE.NONE:
                    crusadeState.crusade_state = CRUSADE_STATE.CHOOSING;
                    SM.EnterState<MapDefaultState>();
                    flag_first_load = true;
                    //DataStorage.NewFile<BattleConfig>().WithRecord<LoadConfig>(new LoadConfig());
                    break;
                case CRUSADE_STATE.TEXT_EVENT:
                    SM.EnterState<MapTextEvState>();
                    break;
                case CRUSADE_STATE.TUTORIAL:
                    crusadeState.crusade_state = CRUSADE_STATE.CHOOSING;
                    SM.EnterState<MapDefaultState>();
                    flag_first_load = true;
                    flag_tutorial_load = true;
                    break;
                default:
                    SM.EnterState<MapDefaultState>();
                    break;
            }

            //flag_first_load = true;
            //flag_tutorial_load = true;

            if (flag_tutorial_load)
            {
                MapReferences.Instance().tutorialController.BeginTutorial();
            }

            ConfigureSystems();
        }

        private void ConfigureSystems()
        {
            m_mapWorld.AddModule(new CoreModule());
            m_mapWorld.AddModule(new StateMachineModule());

            m_mapWorld.AddModule(new InputModule());

            m_mapWorld.AddModule(new ServicesModule());

            m_mapWorld.AddModule(new CleanupModule());






            var scr_MapController = gameObject.AddComponent<Scr_MapController>();
            var mapTextEventHandlerSystem = gameObject.AddComponent<MapTextEventHandlerSystem>();
            var nodeDrawSystem = gameObject.AddComponent<MapDrawSystem>();

            m_mapWorld.AddModule(new MapInteractionBaseModule());
            m_mapWorld.AddModule(new MapNodeWaitModule());
            m_mapWorld.AddModule(new MapReqSystemsModule());

            SystemsGroup systemsMapContr = m_mapWorld.CreateSystemsGroup();
            SystemsGroup systemsMapGroup = m_mapWorld.CreateSystemsGroup();

            //systemsMapGroup.AddSystem(cursorDetectionSystem);
            //systemsMapGroup.AddSystem(mapClickObserveSystem);

            //systemsMapGroup.AddSystem(mapEvReqSystemGiveGold);
            //systemsMapGroup.AddSystem(mapEvReqSystemTakeGold);

            systemsMapContr.AddSystem(scr_MapController);

            systemsMapGroup.AddSystem(new MapEvReqSystemUpdateProgress());
            systemsMapGroup.AddSystem(mapTextEventHandlerSystem);
            systemsMapGroup.AddSystem(nodeDrawSystem);


            m_mapWorld.AddSystemsGroup(order: 201, systemsMapContr);
            m_mapWorld.AddSystemsGroup(order: 251, systemsMapGroup);
            //nodeWorld.RemoveSystemsGroup(systemsGroup);


            m_mapWorld.GetEvent<MapLoadSceneEvent>().NextFrame(new MapLoadSceneEvent() { is_first_load = flag_first_load, is_tutorial_load = flag_tutorial_load });
        }



        void Update()
        {
            m_mapWorld.Update(Time.deltaTime);
            m_mapWorld.CleanupUpdate(Time.deltaTime);
            m_mapWorld.Commit();

            SM.Update();
        }

        void OnDestroy()
        {

            if (SM.IsStateActive<MapDefaultState>(out var d_state)) { SM.ExitState<MapDefaultState>(); }
            if (SM.IsStateActive<MapTextEvState>(out var t_state)) { SM.ExitState<MapTextEvState>(); }

            SM.Dispose();

            m_mapWorld.Dispose();
        }


        public static void Pause()
        {
            m_Instance?.gameObject.SetActive(false);
        }
        public static void Unpause()
        {
            m_Instance?.gameObject.SetActive(true);
        }
    }

}
