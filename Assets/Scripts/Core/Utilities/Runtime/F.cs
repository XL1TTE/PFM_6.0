using System.Collections.Generic;
using System.Linq;
using Domain.Abilities.Tags;
using Domain.BattleField.Tags;
using Domain.Enemies.Tags;
using Domain.Extentions;
using Domain.HealthBars.Components;
using Domain.Monster.Tags;
using Domain.UI.Widgets;
using Scellecs.Morpeh;

namespace Core.Utilities
{
    public static class F
    {
        public static IEnumerable<Entity> FilterEmptyCells(IEnumerable<Entity> cells, World world)
        {
            var occupiedCells = world.GetStash<TagOccupiedCell>();

            return cells.Where((e) => occupiedCells.Has(e) == false);
        }
        public static IEnumerable<Entity> FilterCellsWithEnemies(IEnumerable<Entity> cells, World world)
        {
            var occupiedCells = world.GetStash<TagOccupiedCell>();
            var enemies = world.GetStash<TagEnemy>();

            return cells.Where((e) =>
                occupiedCells.Has(e) && enemies.Has(occupiedCells.Get(e).m_Occupier)
            );
        }
        public static IEnumerable<Entity> FilterCellsWithMonsters(IEnumerable<Entity> cells, World world)
        {
            var occupiedCells = world.GetStash<TagOccupiedCell>();
            var monsters = world.GetStash<TagMonster>();

            return cells.Where((e) =>
                occupiedCells.Has(e) && monsters.Has(occupiedCells.Get(e).m_Occupier)
            );
        }


        public static IEnumerable<Entity> FindAbilityButtonsByOwner(Entity owner, World world)
        {
            var f_abilityButtons = world.Filter.With<AbiltiyButtonTag>().Build();
            var stash_abilityButtons = world.GetStash<AbiltiyButtonTag>();

            List<Entity> t_result = new(8);
            foreach (var e in f_abilityButtons)
            {
                if (stash_abilityButtons.Get(e).m_ButtonOwner.Id == owner.Id)
                {
                    t_result.Add(e);
                }
            }
            return t_result;
        }


        public static HealthBarView GetActiveHealthBarFor(Entity owner, World world)
        {
            var f_healthBars = world.Filter
                    .With<HealthBarTag>()
                    .With<HealthBarOwner>()
                    .With<HealthBarViewLink>()
                    .Build();

            var stash_healthBarOwner = world.GetStash<HealthBarOwner>();
            var stash_healthBarView = world.GetStash<HealthBarViewLink>();
            foreach (var bar in f_healthBars)
            {
                if (stash_healthBarOwner.Get(bar).Value.Id == owner.Id)
                {
                    return stash_healthBarView.Get(bar).Value;
                }
            }
            return null;
        }

        public static bool IsMonster(Entity entity, World world)
            => world.GetStash<TagMonster>().Has(entity);
        public static bool IsEnemy(Entity entity, World world)
            => world.GetStash<TagEnemy>().Has(entity);
        public static bool IsOccupiedCell(Entity entity, World world)
            => world.GetStash<TagOccupiedCell>().Has(entity);
    }

}


