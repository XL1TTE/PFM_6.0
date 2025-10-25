using System.Collections.Generic;
using System.Linq;
using Domain.Abilities.Tags;
using Domain.BattleField.Tags;
using Domain.Enemies.Tags;
using Domain.Extentions;
using Domain.Monster.Tags;
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
                occupiedCells.Has(e) && enemies.Has(occupiedCells.Get(e).Occupier)
            );
        }
        public static IEnumerable<Entity> FilterCellsWithMonsters(IEnumerable<Entity> cells, World world)
        {
            var occupiedCells = world.GetStash<TagOccupiedCell>();
            var monsters = world.GetStash<TagMonster>();

            return cells.Where((e) =>
                occupiedCells.Has(e) && monsters.Has(occupiedCells.Get(e).Occupier)
            );
        }


        public static Entity FindAbilityButtonByOwner(Entity owner, World world)
        {
            var f_abilityButtons = world.Filter.With<AbiltiyButtonTag>().Build();
            var stash_abilityButtons = world.GetStash<AbiltiyButtonTag>();
            foreach (var e in f_abilityButtons)
            {
                if (stash_abilityButtons.Get(e).m_ButtonOwner.Id == owner.Id)
                {
                    return e;
                }
            }
            return default;
        }



        public static bool IsMonster(Entity entity, World world)
            => world.GetStash<TagMonster>().Has(entity);
        public static bool IsEnemy(Entity entity, World world)
            => world.GetStash<TagEnemy>().Has(entity);
        public static bool IsOccupiedCell(Entity entity, World world)
            => world.GetStash<TagOccupiedCell>().Has(entity);



    }

}


