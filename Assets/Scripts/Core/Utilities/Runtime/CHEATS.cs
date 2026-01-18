using Domain.Abilities.Components;
using Domain.Map;
using Domain.Monster.Tags;
using Game;
using Gameplay.FloatingDamage.Systems;
using Persistence.DS;
using Scellecs.Morpeh;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Utilities
{
    public static class CHEATS
    {

        public static void EndlessTurnsForMonsters()
        {
            var world = ECS.m_CurrentWorld;

            if (world.IsDisposed || world == null) { return; }

            Debug.Log("ENDLESS TURNS CHEAT");
            Game.GUI.NotifyUnderCursor("ENDLESS TURNS CHEAT", Color.white);

            var filter = world.Filter.With<TagMonster>().With<InteractionsComponent>().Build();

            var stash = world.GetStash<InteractionsComponent>();

            foreach (var e in filter)
            {
                G.RefreshInteractions(e, world);
                stash.Remove(e);
            }

        }

        public static void GiveParts()
        {
            Dictionary<string, int> parts = new()
            {
                ///new test
                ///
                { "bp_bear-arm", 2 },
                { "bp_bear-leg", 2 },

                { "bp_bee-arm", 2 },
                { "bp_bee-torso", 2 },

                { "bp_cat-leg", 2 },
                { "bp_cat-arm", 2 },
                { "bp_cat-head", 2 },

                { "bp_cockroach-arm", 2 },

                { "bp_cow-arm", 2 },

                { "bp_dog-leg", 2 },
                { "bp_dog-torso", 2 },
                { "bp_dog-head", 2 },

                { "bp_dove-arm", 2 },

                { "bp_goat-leg", 2 },
                { "bp_goat-head", 2 },
                { "bp_goat-arm", 2 },

                { "bp_goose-head", 2 },
                { "bp_goose-leg", 2 },

                { "bp_horse-leg", 2 },
                { "bp_horse-head", 2 },

                { "bp_ladybug-leg", 2 },
                { "bp_ladybug-arm", 2 },

                { "bp_pig-head", 2 },
                { "bp_pig-torso", 2 },
                { "bp_pig-leg", 2 },

                { "bp_raccoon-arm", 2 },
                { "bp_raccoon-torso", 2 },
                { "bp_raccoon-leg", 2 },

                { "bp_rat-arm", 2 },
                { "bp_rat-leg", 2 },

                { "bp_raven-arm", 2 },
                { "bp_raven-torso", 2 },
                { "bp_raven-leg", 2 },

                { "bp_rooster-leg", 2 },
                { "bp_rooster-arm", 2 },
                { "bp_rooster-torso", 2 },
                { "bp_rooster-head", 2 },

                { "bp_sheep-leg", 2 },
                { "bp_sheep-arm", 2 },
                { "bp_sheep-torso", 2 },
                { "bp_sheep-head", 2 },
            };

            ref var bodyPartsStorage = ref DataStorage.GetRecordFromFile<Inventory, BodyPartsStorage>();

            bodyPartsStorage.parts = parts;

        }

    }

}


