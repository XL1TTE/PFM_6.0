using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Domain.Abilities;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.Extentions;
using Domain.Stats.Components;
using DS.Files;
using Interactions;
using Persistence.DS;
using Scellecs.Morpeh;
using UnityEngine;

namespace Game
{
    public static partial class G
    {

        /// <summary>
        /// Function try to deal damage to target.
        /// 1) Validate target.
        /// 2) Calculates final damage.
        /// 3) Changes target health.
        /// 4) Sends damage dealt notification. 
        /// </summary>
        /// <param name="a_source">Damage source.</param>
        /// <param name="a_target">Target.</param>
        /// <param name="a_amount">Damage amount.</param>
        /// <param name="a_damageType">Damage type.</param>
        /// <param name="a_world">ECS world in which all affected entities leaves.</param>
        public static void DealDamage(
            Entity a_source,
            Entity a_target,
            int a_amount,
            DamageType a_damageType,
            World a_world)
        {
            DealDamageAsync(a_source, a_target, a_amount, a_damageType, a_world).Forget();
        }
        private static async UniTask DealDamageAsync(
            Entity a_source,
            Entity a_target,
            int a_amount,
            DamageType a_damageType,
            World a_world)
        {
            var t_canTakeDamage = true;
            foreach (var i in Interactor.GetAll<ICanTakeDamageValidator>())
            {
                t_canTakeDamage &= await i.Validate(a_target, a_world);
            }
            if (t_canTakeDamage == false) { return; }

            int t_damageCounter = a_amount;
            foreach (var i in Interactor.GetAll<ICalculateDamageInteraction>())
            {
                t_damageCounter = await i.Execute(
                    a_source, a_target, a_world, a_damageType, t_damageCounter);
            }

            a_world.GetStash<Health>().Get(a_target).ChangeHealth(-t_damageCounter);

            // On damage dealt notification
            foreach (var i in Interactor.GetAll<IOnDamageDealtInteraction>())
            {
                await i.Execute(a_source, a_target, a_world, t_damageCounter);
            }
        }

        public static void MoveToCell(Entity a_subject, Entity a_cell, World a_world)
        {
            MoveToCellAsync(a_subject, a_cell, a_world).Forget();
        }

        public async static UniTask MoveToCellAsync(Entity a_subject, Entity a_cell, World a_world)
        {
            var t_pCell = GU.GetOccupiedCell(a_subject, a_world);

            UnoccupyCell(t_pCell, a_world);
            OccupyCell(a_subject, a_cell, a_world);

            // Notify about entity's position changes.
            Interactor.CallAll<IOnEntityCellPositionChanged>(async handler =>
            {
                await handler.OnPositionChanged(t_pCell, a_cell, a_subject, a_world);
            }).Forget();


            A.KillSequenceFor(a_subject);

            // Default animation for any entity.
            var t_animation = A.ChessMovement(a_subject, a_cell, a_world);
            A.CacheSequenceFor(a_subject, t_animation);

            Interactor.CallAll<IOnAnimationStart>(async handler =>
            {
                await handler.OnAnimationStart(a_subject, a_world);
            }).Forget();

            t_animation.Play();

            await UniTask.WaitWhile(() => t_animation.IsActive());

            Interactor.CallAll<IOnAnimationEnd>(async handler =>
            {
                await handler.OnAnimationEnd(a_subject, a_world);
            }).Forget();
        }

        public static void TurnAround(Entity a_subject, World a_world)
        {
            TurnAroundAsync(a_subject, a_world).Forget();
        }

        public async static UniTask TurnAroundAsync(Entity a_subject, World a_world)
        {
            var t_animation = A.TurnAround(a_subject, a_world);
            t_animation.Play();

            Interactor.CallAll<IOnAnimationStart>(async handler =>
            {
                await handler.OnAnimationStart(a_subject, a_world);
            }).Forget();

            await UniTask.WaitWhile(() => t_animation.IsActive());

            var stash_lookDir = a_world.GetStash<LookDirection>();
            if (stash_lookDir.Has(a_subject))
            {
                switch (stash_lookDir.Get(a_subject).m_Value)
                {
                    case Directions.LEFT:
                        stash_lookDir.Get(a_subject).m_Value = Directions.RIGHT;
                        break;
                    case Directions.RIGHT:
                        stash_lookDir.Get(a_subject).m_Value = Directions.LEFT;
                        break;
                }
            }

            Interactor.CallAll<IOnAnimationEnd>(async handler =>
            {
                await handler.OnAnimationEnd(a_subject, a_world);
            }).Forget();
        }

        /// <summary>
        /// Occupy cell by changing OccupiedCell tag's value.
        /// Set's subject position component to occupied cell values as well.
        /// Do not affect object render -> object will not be moved on the screen.
        /// </summary>
        public static void OccupyCell(Entity a_subject, Entity a_cell, World a_world)
        {
            var stash_occupiedCell = a_world.GetStash<TagOccupiedCell>();
            var stash_Position = a_world.GetStash<PositionComponent>();

            var cellPos = stash_Position.Get(a_cell);

            stash_occupiedCell.Set(a_cell, new TagOccupiedCell
            {
                m_Occupier = a_subject
            });

            stash_Position.Set(a_subject, new PositionComponent
            {
                m_GridPosition = cellPos.m_GridPosition,
                m_GlobalPosition = cellPos.m_GlobalPosition
            });
        }

        /// <summary>
        /// Frees cell -> OccupiedCell tag will be removed.
        /// </summary>
        public static void UnoccupyCell(Entity a_cell, World a_world)
        {
            var stash_occupiedCell = a_world.GetStash<TagOccupiedCell>();
            if (a_cell.isNullOrDisposed(a_world) == false)
            {
                stash_occupiedCell.Remove(a_cell);
            }
        }


        public static void Die(Entity a_subject, Entity a_cause, World a_world)
        {
            DieAsync(a_subject, a_cause, a_world).Forget();
        }

        public async static UniTask DieAsync(Entity a_subject, Entity a_cause, World a_world)
        {
            var t_dieSeq = A.Die(a_subject, 4, a_world);
            t_dieSeq.Play();

            await UniTask.WaitWhile(() => t_dieSeq.IsActive());
            t_dieSeq?.Kill();

            // 1. Call this first
            await Interactor.CallAll<IOnEntityDiedInteraction>(async t
                => await t.OnEntityDied(a_subject, a_cause, a_world));

            // 2. Destroy entity in the very end.
            if (GU.TryDestroyEntityTransform(a_subject, a_world))
            {

            }
        }


        /// <summary>
        /// Creates turn queue for battle.
        /// </summary>
        /// <param name="a_members">Entities which will be included in per turn process.</param>
        /// <param name="a_world"></param>
        /// <returns></returns>
        public static void CreateTurnsQueue(IEnumerable<Entity> a_members, World a_world)
        {
            var t_file = DataStorage.GetFile<BattleMeta>();
            if (t_file == null) { t_file = DataStorage.NewFile<BattleMeta>().Build(); }


            var stash_speed = a_world.GetStash<Speed>();

            var t_orderedBySpeed = a_members.OrderByDescending(x =>
                stash_speed.Has(x) ? stash_speed.Get(x).m_Value : 0);

            t_file.SetRecord<TurnsQueueRecord>(new TurnsQueueRecord { m_Queue = new List<Entity>(t_orderedBySpeed) });

            Interactor.CallAll<IOnTurnQueueCreatedInteraction>(handler => handler.IOnTurnQueueCreated(a_world)).Forget();
        }

        /// <summary>
        /// Ends current turn taker's turn and process to next turn.
        /// </summary>
        /// <param name="a_world"></param>
        public static void NextTurn(World a_world)
        {
            ref var t_queue = ref DataStorage.GetRecordFromFile<BattleMeta, TurnsQueueRecord>();

            if (t_queue.m_Queue.Count < 1) { return; }

            t_queue.m_LastTurnTaker = t_queue.m_CurrentTurnTaker;
            t_queue.m_CurrentTurnTaker = t_queue.m_Queue.PopFirst();
            t_queue.m_Queue.Add(t_queue.m_CurrentTurnTaker);

            NextTurnAsync(t_queue.m_LastTurnTaker, t_queue.m_CurrentTurnTaker, a_world).Forget();
        }

        private async static UniTask NextTurnAsync(Entity a_lastTurnTaker, Entity a_currentTurnTaker, World a_world)
        {
            await Interactor.CallAll<IOnTurnEndInteraction>(async handler => await handler.OnTurnEnd(a_lastTurnTaker, a_world));
            await Interactor.CallAll<IOnTurnStartInteraction>(async handler => await handler.OnTurnStart(a_currentTurnTaker, a_world));
        }

    }
}
