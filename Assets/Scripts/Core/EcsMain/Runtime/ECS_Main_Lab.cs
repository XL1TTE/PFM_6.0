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
                LabReferences.Instance().tutorialController.ShowTutorialNotification();
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
                // release starter parts
                { "bp_dog-head", 1 },
                { "bp_pig-head", 1 },
                { "bp_cat-head", 1 },
                { "bp_sheep-head", 1 },

                { "bp_pig-torso", 1 },
                { "bp_rooster-torso", 2 },
                { "bp_raccoon-torso", 1 },

                { "bp_sheep-arm", 1 },
                { "bp_raven-arm", 1 },
                { "bp_cat-arm", 2 },
                { "bp_rat-arm", 2 },
                { "bp_ladybug-arm", 1 },
                { "bp_raccoon-arm", 1 },

                { "bp_bear-leg", 1 },
                { "bp_rat-leg", 2 },
                { "bp_rooster-leg", 1 },
                { "bp_dog-leg", 2 },
                { "bp_cat-leg", 1 },
                { "bp_sheep-leg", 1 },

                ///new test
                ///
                //{ "bp_bear-arm", 2 },
                //{ "bp_bear-leg", 2 },

                //{ "bp_bee-arm", 2 },
                //{ "bp_bee-torso", 2 },

                //{ "bp_cat-leg", 2 },
                //{ "bp_cat-arm", 2 },
                //{ "bp_cat-head", 2 },

                //{ "bp_cockroach-arm", 2 },

                //{ "bp_cow-arm", 2 },

                //{ "bp_dog-leg", 2 },
                //{ "bp_dog-torso", 2 },
                //{ "bp_dog-head", 2 },

                //{ "bp_dove-arm", 2 },

                //{ "bp_goat-leg", 2 },
                //{ "bp_goat-head", 2 },
                //{ "bp_goat-arm", 2 },

                //{ "bp_goose-head", 2 },
                //{ "bp_goose-leg", 2 },

                //{ "bp_horse-leg", 2 },
                //{ "bp_horse-head", 2 },

                //{ "bp_ladybug-leg", 2 },
                //{ "bp_ladybug-arm", 2 },

                //{ "bp_pig-head", 2 },
                //{ "bp_pig-torso", 2 },
                //{ "bp_pig-leg", 2 },

                //{ "bp_raccoon-arm", 2 },
                //{ "bp_raccoon-torso", 2 },
                //{ "bp_raccoon-leg", 2 },

                //{ "bp_rat-arm", 2 },
                //{ "bp_rat-leg", 2 },

                //{ "bp_raven-arm", 2 },
                //{ "bp_raven-torso", 2 },
                //{ "bp_raven-leg", 2 },

                //{ "bp_rooster-leg", 2 },
                //{ "bp_rooster-arm", 2 },
                //{ "bp_rooster-torso", 2 },
                //{ "bp_rooster-head", 2 },

                //{ "bp_sheep-leg", 2 },
                //{ "bp_sheep-arm", 2 },
                //{ "bp_sheep-torso", 2 },
                //{ "bp_sheep-head", 2 },

                ///old
                //{ "bp_pig-head", 2 },
                //{ "bp_sheep-head", 2 },
                //{ "bp_rat-arm", 2 },
                //{ "bp_cockroach-arm",2 },
                //{ "bp_cow-arm",2 },
                //{ "bp_sheep-arm",2 },
                //{ "bp_rat-leg", 4 },
                //{ "bp_pig-leg", 2 },
                //{ "bp_sheep-leg", 2 },
                //{ "bp_pig-torso", 2 },
                //{ "bp_sheep-torso", 2 }
            };

            return parts;
        }
    }

}
