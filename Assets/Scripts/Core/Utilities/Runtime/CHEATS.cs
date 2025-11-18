using Domain.Abilities.Components;
using Domain.Monster.Tags;
using Game;
using Gameplay.FloatingDamage.Systems;
using Scellecs.Morpeh;
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

    }

}


