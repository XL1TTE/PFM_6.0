using Core.ECS.Modules;
using Domain.Extentions;
using Domain.StateMachine.Mono;
using Interactions;
using Scellecs.Morpeh;
using UnityEngine;

namespace Core.ECS
{
    public class ECS_Main : MonoBehaviour
    {
        public static World _defaultWorld;

        void Awake()
        {
            _defaultWorld = World.Default;

            _defaultWorld.UpdateByUnity = false;
        }

        void Start()
        {
            Interactor.Init();
            ConfigureSystems();
        }

        private void ConfigureSystems()
        {
            _defaultWorld.AddModule(new CoreModule());
            _defaultWorld.AddModule(new StateMachineModule());
            _defaultWorld.AddModule(new GameStatesModule());
            _defaultWorld.AddModule(new UILogicModule());
            _defaultWorld.AddModule(new AIAgentsModule());
            _defaultWorld.AddModule(new TurnModule());
            _defaultWorld.AddModule(new InputModule());
            _defaultWorld.AddModule(new DragAndDropLogicModule());
            _defaultWorld.AddModule(new GridLogicModule());
            _defaultWorld.AddModule(new MonstersLogicModule());
            _defaultWorld.AddModule(new EnemiesLogicModule());
            _defaultWorld.AddModule(new AbilitiesLogicModule());
            //_defaultWorld.AddModule(new AbilityGraphModule());
            _defaultWorld.AddModule(new GameEffectsModule());
            _defaultWorld.AddModule(new GameStatsModule());
            _defaultWorld.AddModule(new ServicesModule());
            _defaultWorld.AddModule(new VisualsModule());
            _defaultWorld.AddModule(new HealthBarsModule());
            _defaultWorld.AddModule(new PrefabInstantiationModule());
            _defaultWorld.AddModule(new CleanupModule());
        }


        void Update()
        {
            _defaultWorld.Update(Time.deltaTime);
            _defaultWorld.CleanupUpdate(Time.deltaTime);
            _defaultWorld.Commit();

            StateMachineWorld.Update();
        }

    }

}
