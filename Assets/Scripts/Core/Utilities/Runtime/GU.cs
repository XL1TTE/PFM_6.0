using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domain.Abilities.Components;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.Enemies.Tags;
using Domain.Extentions;
using Domain.Monster.Tags;
using Domain.Stats.Components;
using Domain.TargetSelection.Events;
using Scellecs.Morpeh;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.Utilities;

namespace Core.Utilities
{
    public static class GU
    {
        public static ref Transform GetTransform(Entity entity, World world)
        {
            var stash_transform = world.GetStash<TransformRefComponent>();
            if (stash_transform.Has(entity) == false) { throw new System.Exception("No transform component was found."); }

            return ref stash_transform.Get(entity).Value;
        }

        public static Vector2Int GetEntityPositionOnCell(Entity entity, World world)
        {
            var stash_Pos = world.GetStash<PositionComponent>();
            if (stash_Pos.Has(entity) == false) { return Vector2Int.zero; }

            return stash_Pos.Get(entity).m_GridPosition;
        }

        public static IEnumerable<Entity> GetCellsInArea(Entity a_cell, int a_area, World a_world)
        {
            var a_basis = GetCellGridPosition(a_cell, a_world);

            var t_shifts = Enumerable.Range(-a_area, a_area * 2 + 1)
                .SelectMany(x => Enumerable.Range(-a_area, a_area * 2 + 1)
                    .Select(y => new Vector2Int(x, y)))
                .Where(pos => pos != Vector2Int.zero)
                .ToList();

            return GU.GetCellsFromShifts(a_basis, t_shifts, a_world);
        }

        public static IEnumerable<Entity> GetCellsFromShifts(Vector2Int a_basis, IEnumerable<Vector2Int> a_shifts, World a_world)
        {
            if (a_shifts == null) { return default; }

            var f_cells = a_world.Filter
                .With<CellTag>()
                .With<PositionComponent>()
                .Build();
            var stash_pos = a_world.GetStash<PositionComponent>();

            IEnumerable<Vector2Int> t_calcPos = a_shifts.Select(x => x + a_basis);

            List<Entity> t_result = new(f_cells.GetLengthSlow());
            foreach (var cell in f_cells)
            {
                var cellPos = stash_pos.Get(cell);
                if (t_calcPos.Contains(cellPos.m_GridPosition))
                {
                    t_result.Add(cell);
                }
            }
            return t_result;
        }

        public static IEnumerable<Vector2Int> TransformShiftsFromSubjectLook(Entity a_subject, IEnumerable<Vector2Int> a_shifts, World a_world)
        {
            var stash_LookDir = a_world.GetStash<LookDirection>();
            if (stash_LookDir.Has(a_subject) == false) { return a_shifts; }

            IEnumerable<Vector2Int> result = a_shifts;

            switch (stash_LookDir.Get(a_subject).m_Value)
            {
                case Directions.LEFT:
                    result = a_shifts.Select(shift => shift * new Vector2Int(-1, 1));
                    break;
            }
            return result;
        }

        public static ref TargetSelectionResult GetTargetSelectionResult(Entity owner, World world)
        {
            var results = world.GetStash<TargetSelectionResult>();
            if (results.Has(owner))
            {
                return ref results.Get(owner);
            }
            throw new System.Exception($"Target selection result was not found on Entity: {owner.Id}.");
        }

        public static Entity GetCellOccupier(Entity cell, World world)
        {
            var occupiedCell = world.GetStash<TagOccupiedCell>();
            if (occupiedCell.Has(cell))
            {
                return occupiedCell.Get(cell).m_Occupier;
            }
            else
            {
                throw new System.Exception("You trying to get cell occupier from not occupied cell.");
            }
        }

        public static Vector2Int GetCellGridPosition(Entity a_cell, World a_world)
        {
            var stash_Position = a_world.GetStash<PositionComponent>();
            return stash_Position.Get(a_cell).m_GridPosition;
        }

        public static IEnumerable<Entity> GetCellOccupiers(IEnumerable<Entity> cells, World world)
        {
            var occupiedCell = world.GetStash<TagOccupiedCell>();
            var result = new List<Entity>();

            foreach (var cell in cells)
            {
                if (occupiedCell.Has(cell))
                {
                    result.Add(occupiedCell.Get(cell).m_Occupier);
                }
                else
                {
                    throw new System.Exception("You trying to get cell occupier from not occupied cell.");
                }
            }
            return result;
        }

        public static float GetHealthPercentFor(Entity a_subject, World a_world)
        {
            try
            {
                var stash_health = a_world.GetStash<Health>();
                var stash_maxHealth = a_world.GetStash<MaxHealth>();

                return (float)stash_health.Get(a_subject).GetHealth() / stash_maxHealth.Get(a_subject).m_Value;
            }
            catch
            {
                return 0;
            }
        }


        /// <summary>
        /// Automaticly updates health bar value for entity, if it has one.
        /// </summary>
        /// <param name="a_owner">Entity with active health bar.</param>
        /// <param name="a_world">ECS world in which entities leaves.</param>
        public static void UpdateHealthBarValueFor(Entity a_owner, World a_world)
        {
            var healthBar = F.GetActiveHealthBarFor(a_owner, a_world);
            var t_value = GetHealthPercentFor(a_owner, a_world);
            healthBar?.SetValue(t_value);
        }


        /// <summary>
        /// Find cell entity, which is occupied by other entity.
        /// </summary>
        /// <param name="a_occupier"></param>
        /// <param name="a_world"></param>
        /// <returns></returns>
        public static Entity GetOccupiedCell(Entity a_occupier, World a_world)
        {
            var t_filter = a_world.Filter
                .With<CellTag>()
                .With<TagOccupiedCell>()
                .Build();

            var stash_occupiedCells = a_world.GetStash<TagOccupiedCell>();
            foreach (var cell in t_filter)
            {
                if (stash_occupiedCells.Get(cell).m_Occupier.Id == a_occupier.Id)
                {
                    return cell;
                }
            }
            return default;
        }


        public static IEnumerable<Entity> GetAllMonstersOnField(World a_world)
        {
            return a_world.Filter.With<TagMonster>().With<PositionComponent>().Build().AsEnumerable();
        }
        public static IEnumerable<Entity> GetAllEnemiesOnField(World a_world)
        {
            return a_world.Filter.With<TagEnemy>().With<PositionComponent>().Build().AsEnumerable();
        }

        public static int GetHealth(Entity a_entity, World a_world)
        {
            if (a_world.TryGetComponent<Health>(a_entity, out var healthComponent))
            {
                return healthComponent.GetHealth();
            }
            return 0;
        }
        public static int GetMaxHealth(Entity a_entity, World a_world)
        {
            if (a_world.TryGetComponent<MaxHealth>(a_entity, out var healthComponent))
            {
                return healthComponent.m_Value;
            }
            return 100;
        }

        /// <summary>
        /// Destroys entity gameobject by transform ref.
        /// </summary>
        /// <param name="a_subject"></param>
        /// <param name="a_world"></param>
        /// <returns></returns>
        public static bool TryDestroyEntityTransform(Entity a_subject, World a_world)
        {
            var t_transformRef = a_world.GetStash<TransformRefComponent>();

            if (t_transformRef.Has(a_subject) == false) { return false; }

            UnityEngine.Object.Destroy(t_transformRef.Get(a_subject).Value.gameObject, 0.1f);
            return true;
        }


        public static void ApplyResistanceToDamage<T>(Entity a_target, ref int a_damage, World a_world) where T : struct, IResistanceModiffier
        {
            switch (F.GetResistance<T>(a_target, a_world))
            {
                case IResistanceModiffier.Stage.NONE:
                    break;
                case IResistanceModiffier.Stage.WEAKNESS:
                    a_damage *= 2;
                    break;
                case IResistanceModiffier.Stage.RESISTANT:
                    a_damage /= 2;
                    break;
                case IResistanceModiffier.Stage.IMMUNE:
                    a_damage = 0;
                    break;
            }
        }

        /// <summary>
        /// Will delete AI component. So you will need to manualy setup it again if you 
        /// would wanna to enable it again.
        /// </summary>
        /// <param name="a_agent"></param>
        /// <param name="a_world"></param>
        public static void DisposeAI(Entity a_agent, World a_world)
        {
            if (F.IsAiControlled(a_agent, a_world) == false) { return; }

            a_world.GetStash<AgentAIComponent>().Remove(a_agent);
        }
    }

}


