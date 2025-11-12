using Core.Ecs.Modules;
using Domain.Extentions;
using Domain.StateMachine.Mono;
using Interactions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{

    [DefaultExecutionOrder(ECS.SCENE_ENTRY_POINT)]
    public class ECS_Main : MonoBehaviour
    {
        public static World m_battleWorld;

        void Awake()
        {
            m_battleWorld = World.Create();
            ECS.m_CurrentWorld = m_battleWorld;
            m_battleWorld.UpdateByUnity = false;
        }

        void Start()
        {
            Interactor.Init();
            ConfigureSystems();
        }

        private void ConfigureSystems()
        {
            m_battleWorld.AddModule(new CoreModule());
            m_battleWorld.AddModule(new StateMachineModule());
            m_battleWorld.AddModule(new GameStatesModule());
            m_battleWorld.AddModule(new UILogicModule());
            m_battleWorld.AddModule(new AIAgentsModule());
            m_battleWorld.AddModule(new TurnModule());
            m_battleWorld.AddModule(new InputModule());
            m_battleWorld.AddModule(new DragAndDropLogicModule());
            m_battleWorld.AddModule(new GridLogicModule());
            m_battleWorld.AddModule(new MonstersLogicModule());
            m_battleWorld.AddModule(new EnemiesLogicModule());
            m_battleWorld.AddModule(new AbilitiesLogicModule());
            //_defaultWorld.AddModule(new AbilityGraphModule());
            m_battleWorld.AddModule(new GameEffectsModule());
            m_battleWorld.AddModule(new GameStatsModule());
            m_battleWorld.AddModule(new ServicesModule());
            m_battleWorld.AddModule(new VisualsModule());
            m_battleWorld.AddModule(new HealthBarsModule());
            m_battleWorld.AddModule(new PrefabInstantiationModule());
            m_battleWorld.AddModule(new CleanupModule());
        }


        void Update()
        {
            m_battleWorld.Update(Time.deltaTime);
            m_battleWorld.CleanupUpdate(Time.deltaTime);
            m_battleWorld.Commit();

            SM.Update();
        }

        void OnDestroy()
        {
            //m_battleWorld.Dispose();
        }
    }

}
