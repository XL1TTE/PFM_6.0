using System.Collections;
using System.Collections.Generic;
using Core.Utilities;
using Domain.Enemies.Tags;
using Domain.Extentions;
using Domain.Monster.Tags;
using Domain.StateMachine.Components;
using Domain.StateMachine.Events;
using Domain.StateMachine.Mono;
using Domain.TurnSystem.Requests;
using Domain.UI.Mono;
using Domain.UI.Requests;
using Game;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;

namespace Gameplay.StateMachine.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BattleStateEnterSystem : ISystem
    {
        public World World { get; set; }

        private Event<OnStateEnterEvent> evt_onStateEnter;

        private Stash<BattleState> stash_state;

        public void OnAwake()
        {
            evt_onStateEnter = SM.Value.GetEvent<OnStateEnterEvent>();

            stash_state = SM.Value.GetStash<BattleState>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var evt in evt_onStateEnter.publishedChanges)
            {
                if (IsValid(evt.StateEntity))
                {
                    Enter(evt.StateEntity);
                }
            }

        }

        public void Dispose()
        {

        }

        private void Enter(Entity stateEntity)
        {

            /* ########################################## */
            /*           Initialize turn system           */
            /* ########################################## */

            var f_monsters = World.Filter.With<TagMonster>().Build();
            var f_enemies = World.Filter.With<TagEnemy>().Build();

            var t_battleMembers = new List<Entity>();
            t_battleMembers.AddRange(f_monsters.AsEnumerable());
            t_battleMembers.AddRange(f_enemies.AsEnumerable());

            G.CreateTurnsQueue(t_battleMembers, World);

            G.NextTurn(World);
        }


        private bool IsValid(Entity stateEntity)
        {
            if (!stash_state.Has(stateEntity)) { return false; }
            else { return true; }
        }

    }

}

