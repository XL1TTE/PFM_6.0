using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Abilities.Components;
using Domain.Abilities.Tags;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.Enemies.Tags;
using Domain.Extentions;
using Domain.HealthBars.Components;
using Domain.Monster.Tags;
using Domain.Stats.Components;
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
            var f_abilityButtons = world.Filter.With<AbilityButtonTag>().Build();
            var stash_abilityButtons = world.GetStash<AbilityButtonTag>();

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

        public static bool IsAbilityButton(Entity a_button, World a_world)
            => a_world.GetStash<AbilityButtonTag>().Has(a_button);
        public static Entity GetAbilityButtonOwner(Entity a_button, World a_world)
        {
            var stash_tag = a_world.GetStash<AbilityButtonTag>();
            //if (stash_tag.Has(a_button) == false) { return default; }
            return stash_tag.Get(a_button).m_ButtonOwner;
        }

        public static AbilityType GetAbilityType(Entity a_abilityButton, World a_world)
        {
            var stash_tag = a_world.GetStash<AbilityButtonTag>();
            return stash_tag.Get(a_abilityButton).m_Ability.m_AbilityType;
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


        public static bool IsDead(Entity entity, World world)
            => world.GetStash<DiedEntityTag>().Has(entity);
        public static bool IsMonster(Entity entity, World world)
            => world.GetStash<TagMonster>().Has(entity);
        public static bool IsEnemy(Entity entity, World world)
            => world.GetStash<TagEnemy>().Has(entity);
        public static bool IsOccupiedCell(Entity entity, World world)
            => world.GetStash<TagOccupiedCell>().Has(entity);
        public static bool IsCell(Entity entity, World world)
            => world.GetStash<CellTag>().Has(entity);

        public static IResistanceModiffier.Stage GetResistance<T>(Entity a_actor, World a_world)
        where T : struct, IResistanceModiffier
        {
            var stash = a_world.GetStash<T>();
            if (stash.Has(a_actor) == false) { return IResistanceModiffier.Stage.NONE; }

            return stash.Get(a_actor).m_Stage;
        }

    }

}


