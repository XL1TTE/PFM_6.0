using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Domain.Abilities;
using Domain.Abilities.Components;
using Domain.Abilities.Tags;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.Components;
using Domain.Extentions;
using Domain.GameEffects;
using Domain.Stats.Components;
using Domain.UI.Tags;
using Domain.UI.Widgets;
using DS.Files;
using Interactions;
using Interactions.ICanBeHealedValidator;
using Interactions.IOnEntityDiedInteraction;
using Persistence.DS;
using Scellecs.Morpeh;
using Unity.VisualScripting;
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
            World a_world,
            IEnumerable<OnDamageTags> a_tags = null)
        {
            DealDamageAsync(a_source, a_target, a_amount, a_damageType, a_world, a_tags).Forget();
        }
        public static async UniTask DealDamageAsync(
            Entity a_source,
            Entity a_target,
            int a_amount,
            DamageType a_damageType,
            World a_world,
            IEnumerable<OnDamageTags> a_tags = null)
        {
            var t_canTakeDamage = true;
            foreach (var i in Interactor.GetAll<ICanTakeDamageValidator>())
            {
                t_canTakeDamage &= await i.Validate(a_target, a_world);
            }
            if (t_canTakeDamage == false) { return; }

            List<OnDamageTags> t_tags = new(2);

            int t_damageCounter = a_amount;
            foreach (var i in Interactor.GetAll<ICalculateDamageInteraction>())
            {
                await i.Execute(
                    a_source, a_target, a_world, a_damageType, ref t_damageCounter, t_tags);
            }

            a_world.GetStash<Health>().Get(a_target).AddHealth(-t_damageCounter);

            // Notify health changed.
            await Interactor.CallAll<IOnEntityHealthChanged>(async handler => await handler.OnChanged(a_target, a_world));

            // On damage dealt notification
            foreach (var i in Interactor.GetAll<IOnDamageDealtInteraction>())
            {
                await i.Execute(a_source, a_target, a_damageType, a_world, t_damageCounter, t_tags);
            }
        }

        public static void Heal(Entity a_source, Entity a_target, int a_amount, World a_world)
        {
            HealAsync(a_source, a_target, a_amount, a_world).Forget();
        }

        private static async UniTask HealAsync(Entity a_source, Entity a_target, int a_amount, World a_world)
        {
            var t_isCanBeHealed = true;
            var t_log = "";
            foreach (var i in Interactor.GetAll<ICanBeHealedValidator>())
            {
                t_isCanBeHealed &= await i.Validate(a_target, a_world, t_log);
                if (!t_isCanBeHealed) { break; }
            }
            if (t_isCanBeHealed)
            {
                int t_health = GU.GetHealth(a_target, a_world);
                int t_maxHealth = GU.GetMaxHealth(a_target, a_world);

                int t_finalHeal = Math.Min(a_amount, t_maxHealth - t_health);

                a_world.GetStash<Health>().Get(a_target).AddHealth(t_finalHeal);

                // Notify health changed.
                await Interactor.CallAll<IOnEntityHealthChanged>(async handler => await handler.OnChanged(a_target, a_world));

                // On Healed notification
                foreach (var i in Interactor.GetAll<IOnEntityHealedInteraction>())
                {
                    await i.Execute(a_source, a_target, t_finalHeal, a_world);
                }

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
            if (F.IsDead(a_subject, a_world)) { return; }

            a_world.GetStash<DiedEntityTag>().Set(a_subject, new DiedEntityTag());

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


        public static async UniTask ExecuteAbilityAsync(AbilityData abilityData, Entity a_caster, Entity a_target, World a_world)
        {
            await Interactor.CallAll<IOnAbiltiyExecutionStart>(
                async h => await h.OnExecutionStart(abilityData, a_caster, a_target, a_world));

            await abilityData.m_Value.Execute(a_caster, a_target, a_world);

            Interactor.CallAll<IOnAbiltiyExecutionEnd>(
                async h => await h.OnExecutionEnd(abilityData, a_caster, a_target, a_world)).Forget();
        }

        public static void RefreshInteractions(Entity a_subject, World a_world)
        {
            var stash_interactions = a_world.GetStash<InteractionsComponent>();
            if (stash_interactions.Has(a_subject) == false) { return; }

            ref var t_interactions = ref stash_interactions.Get(a_subject);

            t_interactions.m_InteractionsLeft = t_interactions.m_MaxInteractions;
            t_interactions.m_MoveInteractionsLeft = t_interactions.m_MaxMoveInteractions;

            Interactor.CallAll<IOnInteractionCountChanged>(
                async h => await h.OnChange(a_subject, stash_interactions.Get(a_subject).m_InteractionsLeft, a_world)).Forget();
        }


        public static void ConsumeAllInteractions(Entity a_subject, World a_world)
        {
            var stash_interactions = a_world.GetStash<InteractionsComponent>();
            if (stash_interactions.Has(a_subject) == false) { return; }

            ref var t_interactions = ref stash_interactions.Get(a_subject);

            t_interactions.m_InteractionsLeft = 0;
            t_interactions.m_MoveInteractionsLeft = 0;

            Interactor.CallAll<IOnInteractionCountChanged>(
                async h => await h.OnChange(a_subject, stash_interactions.Get(a_subject).m_InteractionsLeft, a_world)).Forget();
        }

        public static void ConsumeInteraction(Entity a_subject, World a_world)
        {
            ConsumeInteractionAsync(a_subject, a_world).Forget();
        }

        private static async UniTask ConsumeInteractionAsync(Entity a_subject, World a_world)
        {
            var stash_interactions = a_world.GetStash<InteractionsComponent>();
            if (stash_interactions.Has(a_subject) == false) { return; }

            var t_interactions = stash_interactions.Get(a_subject);

            stash_interactions.Get(a_subject).m_InteractionsLeft =
                Math.Clamp(t_interactions.m_InteractionsLeft - 1, 0, t_interactions.m_MaxInteractions);

            foreach (var i in Interactor.GetAll<IOnInteractionCountChanged>())
            {
                await i.OnChange(a_subject, stash_interactions.Get(a_subject).m_InteractionsLeft, a_world);
            }
        }
        public static void ConsumeMoveInteraction(Entity a_subject, World a_world)
        {
            ConsumeMoveInteractionAsync(a_subject, a_world).Forget();
        }

        private static async UniTask ConsumeMoveInteractionAsync(Entity a_subject, World a_world)
        {
            var stash_interactions = a_world.GetStash<InteractionsComponent>();
            if (stash_interactions.Has(a_subject) == false) { return; }

            var t_interactions = stash_interactions.Get(a_subject);

            stash_interactions.Get(a_subject).m_MoveInteractionsLeft =
                Math.Clamp(t_interactions.m_MoveInteractionsLeft - 1, 0, t_interactions.m_MaxMoveInteractions);

            foreach (var i in Interactor.GetAll<IOnInteractionCountChanged>())
            {
                await i.OnChange(a_subject, stash_interactions.Get(a_subject).m_MoveInteractionsLeft, a_world);
            }
        }

        public static void UpdateAbilityButtonsState(World a_world)
        {
            UpdateAbilityButtonsStateAsync(a_world).Forget();
        }

        private static async UniTask UpdateAbilityButtonsStateAsync(World a_world)
        {
            await UniTask.Yield();

            var filter = a_world.Filter.With<AbilityButtonTag>().With<ButtonTag>().Build();

            var t_btnStash = a_world.GetStash<ButtonTag>(); ;
            var t_abilityBtnStash = a_world.GetStash<AbilityButtonTag>(); ;

            foreach (var btn in filter)
            {
                bool t_canBeUsed = true;
                await Interactor.CallAll<IOnEvaluateAbilityButtonsState>(
                    async h => await h.OnEvaluate(btn, ref t_canBeUsed, a_world));

                if (t_canBeUsed)
                {
                    if (t_btnStash.Has(btn) == false) { continue; }
                    t_btnStash.Get(btn).m_State = ButtonTag.State.Enabled;
                    if (t_abilityBtnStash.Has(btn) == false) { continue; }
                    t_abilityBtnStash.Get(btn).m_View.DisableUnavaibleView();
                }
                else
                {
                    if (t_btnStash.Has(btn) == false) { continue; }
                    t_btnStash.Get(btn).m_State = ButtonTag.State.Disabled;
                    if (t_abilityBtnStash.Has(btn) == false) { continue; }
                    t_abilityBtnStash.Get(btn).m_View.EnableUnavaibleView();
                }
            }
        }

        public static class Statuses
        {
            public static void RemoveStunStack(Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
            {
                var stash_stacks = a_world.GetStash<StunStatusComponent>();
                ref var stacks = ref stash_stacks.Get(a_target);

                stacks.m_Stacks.Remove(a_stack);

                Interactor.CallAll<IOnStunRemoved>(async handler
                    => await handler.OnStunRemoved(a_target, a_stack, a_world)).Forget();

                if (stacks.m_Stacks.Count < 1)
                {
                    stash_stacks.Remove(a_target);
                }
            }
            public static void RemoveBleedingStack(Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
            {
                var stash_stacks = a_world.GetStash<BleedingStatusComponent>();
                ref var stacks = ref stash_stacks.Get(a_target);

                stacks.m_Stacks.Remove(a_stack);
                Interactor.CallAll<IOnBleedRemoved>(async handler
                    => await handler.OnOnBleedRemoved(a_target, a_stack, a_world)).Forget();

                if (stacks.m_Stacks.Count < 1)
                {
                    stash_stacks.Remove(a_target);
                }
            }
            public static void RemovePoisonStack(Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
            {
                var stash_stacks = a_world.GetStash<PoisonStatusComponent>();
                ref var stacks = ref stash_stacks.Get(a_target);

                stacks.m_Stacks.Remove(a_stack);

                Interactor.CallAll<IOnPoisonRemoved>(async handler
                    => await handler.OnPoisonRemoved(a_target, a_stack, a_world)).Forget();

                if (stacks.m_Stacks.Count < 1)
                {
                    stash_stacks.Remove(a_target);
                }
            }
            public static void RemoveBurningStack(Entity a_target, IStatusEffectComponent.Stack a_stack, World a_world)
            {
                var stash_stacks = a_world.GetStash<BurningStatusComponent>();
                ref var stacks = ref stash_stacks.Get(a_target);

                stacks.m_Stacks.Remove(a_stack);
                Interactor.CallAll<IOnBurningRemoved>(async handler
                    => await handler.OnBurningRemoved(a_target, a_stack, a_world)).Forget();

                if (stacks.m_Stacks.Count < 1)
                {
                    stash_stacks.Remove(a_target);
                }
            }

            public static void ApplyStun(Entity a_source, Entity a_target, int a_duration, World a_world)
            {
                ApplyStunAsync(a_source, a_target, a_duration, a_world).Forget();
            }
            public static async UniTask ApplyStunAsync(Entity a_source, Entity a_target, int a_duration, World a_world)
            {
                if (a_world.IsDisposed || a_world == null) { return; }
                if (a_world.GetStash<StunStatusComponent>().Has(a_target)) { return; }


                IStatusEffectComponent.Stack stack = new IStatusEffectComponent.Stack
                {
                    m_Duration = a_duration,
                    m_TurnsLeft = a_duration
                };

                var stash_stun = a_world.GetStash<StunStatusComponent>();
                if (stash_stun.Has(a_target))
                {
                    stash_stun.Get(a_target).m_Stacks.Add(stack);
                }
                else
                {
                    stash_stun.Set(a_target, new StunStatusComponent
                    {
                        m_Stacks = new List<IStatusEffectComponent.Stack>
                        {
                            stack
                        }
                    });
                }
                await UniTask.Yield();

                Interactor.CallAll<IOnStunApplied>(async handler
                    => await handler.OnStunApplied(a_source, a_target, stack, a_world)).Forget();
            }

            public static void ApplyBleeding(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world)
            {
                ApplyBleedingAsync(a_source, a_target, a_duration, a_damagePerTick, a_world).Forget();
            }
            private static async UniTask ApplyBleedingAsync(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world)
            {
                if (V.IsBuring(a_target, a_world)) { return; } // burning netrolize bleeding.

                IStatusEffectComponent.Stack stack = new IStatusEffectComponent.Stack
                {
                    m_DamagePerTurn = a_damagePerTick,
                    m_Duration = a_duration,
                    m_TurnsLeft = a_duration
                };

                var stash_bleeding = a_world.GetStash<BleedingStatusComponent>();
                if (stash_bleeding.Has(a_target))
                {
                    stash_bleeding.Get(a_target).m_Stacks.Add(stack);
                }
                else
                {
                    stash_bleeding.Set(a_target, new BleedingStatusComponent
                    {
                        m_Stacks = new List<IStatusEffectComponent.Stack>
                        {
                            stack
                        }
                    });
                }

                await UniTask.Yield();
                Interactor.CallAll<IOnBleedApplied>(async handler
                    => await handler.OnBleedApplied(a_source, a_target, stack, a_world)).Forget();
            }

            public static void ApplyPoison(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world)
            {
                ApplyPoisonAsync(a_source, a_target, a_duration, a_damagePerTick, a_world).Forget();
            }
            private static async UniTask ApplyPoisonAsync(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world)
            {
                if (V.IsBleeding(a_target, a_world)) { return; } // bleeding netrolize poison.

                IStatusEffectComponent.Stack stack = new IStatusEffectComponent.Stack
                {
                    m_DamagePerTurn = a_damagePerTick,
                    m_Duration = a_duration,
                    m_TurnsLeft = a_duration
                };

                var stash_poison = a_world.GetStash<PoisonStatusComponent>();
                if (stash_poison.Has(a_target))
                {
                    stash_poison.Get(a_target).m_Stacks.Add(stack);
                }
                else
                {
                    stash_poison.Set(a_target, new PoisonStatusComponent
                    {
                        m_Stacks = new List<IStatusEffectComponent.Stack>
                        {
                            stack
                        }
                    });
                }

                await UniTask.Yield();
                Interactor.CallAll<IOnPoisonApplied>(async handler
                    => await handler.OnPoisonApplied(a_source, a_target, stack, a_world)).Forget();
            }

            public static void ApplyBurning(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world)
            {
                ApplyBurningAsync(a_source, a_target, a_duration, a_damagePerTick, a_world).Forget();
            }
            private static async UniTask ApplyBurningAsync(Entity a_source, Entity a_target, int a_duration, int a_damagePerTick, World a_world)
            {
                if (V.IsPoisoned(a_target, a_world)) { return; } // poison netrolize burning.

                IStatusEffectComponent.Stack stack = new IStatusEffectComponent.Stack
                {
                    m_DamagePerTurn = a_damagePerTick,
                    m_Duration = a_duration,
                    m_TurnsLeft = a_duration
                };

                var stash_poison = a_world.GetStash<BurningStatusComponent>();

                if (stash_poison.Has(a_target))
                {
                    stash_poison.Get(a_target).m_Stacks.Add(stack);
                }
                else
                {
                    stash_poison.Set(a_target, new BurningStatusComponent
                    {
                        m_Stacks = new List<IStatusEffectComponent.Stack>
                        {
                            stack
                        }
                    });
                }

                await UniTask.Yield();
                Interactor.CallAll<IOnBurningApplied>(async handler
                    => await handler.OnBurningApplied(a_source, a_target, stack, a_world)).Forget();

            }

            public static void RemoveEffectFromPool(Entity a_subject, string a_EffectID, World a_world)
            {
                var effect_pool = a_world.GetStash<EffectsPoolComponent>();

                if (effect_pool.Has(a_subject))
                {

                    bool isRemoved = false;

                    ref var pool = ref effect_pool.Get(a_subject);
                    for (int i = 0; i < pool.m_PermanentEffects.Count; ++i)
                    {
                        if (pool.m_PermanentEffects[i].m_EffectId == a_EffectID)
                        {
                            pool.m_PermanentEffects.RemoveAt(i);
                            isRemoved = true;
                        }
                    }
                    for (int i = 0; i < pool.m_TemporalEffects.Count; ++i)
                    {
                        if (pool.m_TemporalEffects[i].m_EffectId == a_EffectID)
                        {
                            pool.m_TemporalEffects.RemoveAt(i);
                            isRemoved = true;
                        }
                    }
                    if (isRemoved)
                    {
                        Interactor.CallAll<IOnGameEffectRemove>(
                                            async handler => await handler.OnEffectRemove(a_EffectID, a_subject, a_world)).Forget();
                    }
                }
            }

            public static void AddEffectToPool(Entity a_subject, string a_effectID, int a_duration, World a_world)
            {
                var effect_pool = a_world.GetStash<EffectsPoolComponent>();
                if (effect_pool.Has(a_subject) == false)
                {
                    effect_pool.Set(a_subject, new EffectsPoolComponent
                    {
                        m_PermanentEffects = new(),
                        m_TemporalEffects = new()
                    });
                }

                ref var subjects_pool = ref effect_pool.Get(a_subject);

                if (a_duration <= -1)
                {
                    subjects_pool.m_PermanentEffects.Add(new PermanentEffect
                    {
                        m_EffectId = a_effectID
                    });
                }
                else
                {
                    subjects_pool.m_TemporalEffects.Add(new TemporalEffect
                    {
                        m_EffectId = a_effectID,
                        m_DurationInTurns = a_duration,
                        m_TurnsLeft = a_duration
                    });

                }

                Interactor.CallAll<IOnGameEffectApply>(
                    async handler => await handler.OnEffectApply(a_effectID, a_subject, a_world)).Forget();
            }
        }

        public static class gui
        {
            public static void UpdateEntityEffectsUI(Entity a_entity, World a_world)
            {
                var healthBar = F.GetActiveHealthBarFor(a_entity, a_world);

                if (healthBar == null) { return; }

                healthBar.ClearStatuses();

                if (V.IsBleeding(a_entity, a_world))
                {
                    var bleed = a_world.GetComponent<BleedingStatusComponent>(a_entity);
                    foreach (var stack in bleed.m_Stacks)
                    {
                        healthBar.AddStatusEffect(GR.SPR_UI_EFFECT_BLOOD, stack).Forget();
                    }
                }
                if (V.IsPoisoned(a_entity, a_world))
                {
                    var poison = a_world.GetComponent<PoisonStatusComponent>(a_entity);
                    foreach (var stack in poison.m_Stacks)
                    {
                        healthBar.AddStatusEffect(GR.SPR_UI_EFFECT_POISON, stack).Forget();
                    }
                }
                if (V.IsBuring(a_entity, a_world))
                {
                    var burns = a_world.GetComponent<BurningStatusComponent>(a_entity);
                    foreach (var stack in burns.m_Stacks)
                    {
                        healthBar.AddStatusEffect(GR.SPR_UI_EFFECT_FIRE, stack).Forget();
                    }
                }
                if (V.IsStuned(a_entity, a_world))
                {
                    var stuns = a_world.GetComponent<StunStatusComponent>(a_entity);
                    foreach (var stack in stuns.m_Stacks)
                    {
                        healthBar.AddStatusEffect(GR.SPR_UI_EFFECT_STUNNED, stack).Forget();
                    }
                }
            }

        }

    }

}
