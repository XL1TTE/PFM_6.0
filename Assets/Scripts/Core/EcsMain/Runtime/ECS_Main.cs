
using Codice.CM.WorkspaceServer.DataStore.WkTree;
using Core.ECS.Modules;
using Core.Utilities.Systems;
using CursorDetection.Systems;
using Domain.Extentions;
using Domain.StateMachine.Mono;
using Gameplay.Abilities.Systems;
using Gameplay.BattleField.Systems;
using Gameplay.DragAndDrop.Systems;
using Gameplay.DragAndDrop.Validators;
using Gameplay.EcsButtons.Systems;
using Gameplay.Enemies;
using Gameplay.Monster.Systems;
using Gameplay.StateMachine.Systems;
using Gameplay.TargetSelection.Systems;
using Gameplay.TurnSystem.Systems;
using Persistence.DB;
using Scellecs.Morpeh;
using UI.Systems;
using UnityEngine;

namespace Core.ECS{
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
            ConfigureSystems();
            DataBase.Initialize();
        }

        private void ConfigureSystems()
        {
            _defaultWorld.AddModule(new CoreModule());
            _defaultWorld.AddModule(new StateMachineModule());
            _defaultWorld.AddModule(new GameStatesModule());
            _defaultWorld.AddModule(new UILogicModule());
            _defaultWorld.AddModule(new TurnModule());
            _defaultWorld.AddModule(new InputModule());
            _defaultWorld.AddModule(new DragAndDropLogicModule());
            _defaultWorld.AddModule(new GridLogicModule());
            _defaultWorld.AddModule(new MonstersLogicModule());
            _defaultWorld.AddModule(new EnemiesLogicModule());
            _defaultWorld.AddModule(new AbilitiesLogicModule());
            _defaultWorld.AddModule(new VisualsModule());
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
