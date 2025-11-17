using Domain.Abilities.Components;
using Domain.Monster.Tags;
using Game;
using Scellecs.Morpeh;

namespace Core.Utilities
{
    public static class CHEATS
    {

        public static void EndlessTurnsForMonsters()
        {
            var world = ECS.m_CurrentWorld;

            if (world.IsDisposed || world == null) { return; }

            var filter = world.Filter.With<TagMonster>().With<InteractionsComponent>().Build();

            var stash = world.GetStash<InteractionsComponent>();

            foreach (var e in filter)
            {
                stash.Remove(e);
                G.RefreshInteractions(e, world);
            }

        }

    }

}


