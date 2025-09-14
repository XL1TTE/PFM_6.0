using System;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using DG.Tweening;
using Domain.Abilities.Components;
using Domain.AbilityGraph;
using Domain.AI.Components;
using Domain.BattleField.Components;
using Domain.BattleField.Tags;
using Domain.Commands.Components;
using Domain.Commands.Requests;
using Domain.Components;
using Domain.Enemies.Tags;
using Domain.Monster.Tags;
using Domain.Stats.Components;
using Domain.TurnSystem.Events;
using Domain.TurnSystem.Requests;
using Domain.TurnSystem.Tags;
using Persistence.DB;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Gameplay.Enemies
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DefaultAIAgent : ISystem
    {
        public World World { get; set; }
        
        private Filter f_aiTurnTaker;
        private Request<MoveToCellRequest> req_moveToCell;
        private Request<ProcessTurnRequest> req_processTurn;
        private Request<AbilityUseRequest> req_abilityUse;
        private Stash<TagMonster> stash_monster;
        private Stash<AIAgentType> stash_aiType;
        private Stash<Actions> stash_actions;
        private Stash<Health> stash_health;
        private Stash<TransformRefComponent> stash_transformRef;

        public void OnAwake()
        {
            f_aiTurnTaker = World.Filter
                .With<CurrentTurnTakerTag>()
                .With<AIAgentType>()
                .Without<IsMoving>()
                .Without<IsAttacking>()
                .Build();


            req_moveToCell = World.GetRequest<MoveToCellRequest>();
            req_processTurn = World.GetRequest<ProcessTurnRequest>();
            req_abilityUse = World.GetRequest<AbilityUseRequest>();

            stash_monster = World.GetStash<TagMonster>();
            stash_aiType = World.GetStash<AIAgentType>();
            stash_actions = World.GetStash<Actions>();
            stash_health = World.GetStash<Health>();
            stash_transformRef = World.GetStash<TransformRefComponent>();
        }

        public void OnUpdate(float deltaTime)
        {    
            foreach(var turnTaker in f_aiTurnTaker){
                ProcessAI(turnTaker);
            }

        }

        public void Dispose()
        {

        }
        
        private void ProcessAI(Entity agent)
        {
            var actions = stash_actions.Get(agent);
            if(actions.AvaibleInteractionActions > 0){
                TryMakeAttack(agent);
            }
            if(actions.AvaibleMoveActions > 0){
                TryMakeMove(agent);
            }
            
            if(actions.AvaibleInteractionActions <= 0 && actions.AvaibleMoveActions <= 0){
                req_processTurn.Publish(new ProcessTurnRequest());
            }
        }

        private void TryMakeMove(Entity agent)
        {
            var options = GameLogicUtility.FindMoveOptionsFor(agent, World);

            if (options.Count > 0) {
                MakeRandomMove(agent, options);
                ConsumeMoveAction(agent);
            }
            else{
                ConsumeMoveAction(agent);
            }
        }

        private void TryMakeAttack(Entity agent)
        {
            var attackOptions = ScanForAttack(agent);

            if (attackOptions.Count() > 0)
            {
                MakeAttack(agent, attackOptions);
                ConsumeInteractionAction(agent);
            }
            else{
                ConsumeInteractionAction(agent);
            }
        }
        
        private void ConsumeInteractionAction(Entity agent){
            stash_actions.Get(agent).AvaibleInteractionActions--;
        }
        private void ConsumeMoveAction(Entity agent){
            stash_actions.Get(agent).AvaibleMoveActions--;
        }

        private void MakeAttack(Entity agent, IEnumerable<Entity> attackOptions)
        {
            if (DataBase.TryFindRecordByID("abt_TestRat", out var abilityTemplate) == false) { return; }
            
            var targets = new List<Entity>();

            if (DataBase.TryGetRecord<AbilityTargetsComponent>(abilityTemplate, out var targetsRec)){
                targets = attackOptions.ToList().GetRange(0, targetsRec.TargetCount);
            }
            else{
                targets = attackOptions.ToList();
            }
            
            req_abilityUse.Publish(new AbilityUseRequest{
                Caster = agent,
                AbilityTemplate = abilityTemplate,
                Targets = targets
            });
        }

        private void MakeRandomMove(Entity agent, List<Entity> moveOptions)
        {
            var cell = moveOptions[UnityEngine.Random.Range(0, moveOptions.Count)];
            var cellPos = stash_transformRef.Get(cell).Value.position;
            
            var turnTakerTransform = stash_transformRef.Get(agent).Value;
    
            var moveSeq = GetMoveSequence(turnTakerTransform, cellPos);
            
            req_moveToCell.Publish(new MoveToCellRequest{
                MoveSequence = moveSeq,
                Subject = agent,
                TargetCell = cell
            });
        }

        private Sequence GetAttackSequence(Entity target, Transform attackerTransform, Transform targetTransform)
        {

            var raiseHeight = 20;

            Sequence seq = DOTween.Sequence();

            var originalPosition = attackerTransform.position;
            var targetPos = targetTransform.position;

            seq.Append(attackerTransform.DOMoveZ(originalPosition.z - 1.0f, 0.1f));

            seq.Append(attackerTransform.DOMoveY(originalPosition.y
                + raiseHeight, 0.5f).SetEase(Ease.OutQuad));

            seq.Append(attackerTransform.DOMove(targetPos, 0.25f)
                .SetEase(Ease.InExpo).OnComplete(
                    () => DoDamageToTarget(target) // Do damage
                ));

            var attackPos = new Vector3(originalPosition.x,
                originalPosition.y + raiseHeight, originalPosition.z - 1.0f);

            seq.Append(attackerTransform.DOMove(attackPos, 0.25f)
                .SetEase(Ease.OutQuad));

            seq.Append(attackerTransform.DOMove(originalPosition, 0.5f)
                .SetEase(Ease.InQuad));


            return seq;
        }

        private void DoDamageToTarget(Entity target)
        {
            if (stash_health.Has(target))
            {
                ref var c_health = ref stash_health.Get(target);
                c_health.Value--;
                Debug.Log($"Target {target.Id} now have {c_health.Value} hp.");
            }
        }

        private Sequence GetMoveSequence(Transform executerTransform, Vector3 targetPosition)
        {
            var raiseHeight = 20;

            var targetPosWithHeight =
                new Vector3(targetPosition.x, targetPosition.y + raiseHeight, targetPosition.z);

            var originalPosition = executerTransform.position;

            Sequence seq = DOTween.Sequence();

            seq.Append(executerTransform.DOMoveY(originalPosition.y
                + raiseHeight, 0.25f).SetEase(Ease.OutQuad));

            seq.Append(executerTransform.DOMove(targetPosWithHeight, 0.5f)
                .SetEase(Ease.OutQuad));

            seq.Append(executerTransform.DOMoveY(targetPosition.y, 0.25f)
                .SetEase(Ease.InQuad));

            return seq;
        }
        private IEnumerable<Entity> ScanForAttack(Entity attacker){
            var options = GameLogicUtility.FindAttackOptionsFor(attacker, World);
            List<Entity> validatedOptions = new();
            
            foreach(var opt in options){
                if(stash_monster.Has(opt) == false){continue;}

                validatedOptions.Add(opt);
            }
            
            return options;
        }
        
        private bool ValidateAIAgentType(){
            if(f_aiTurnTaker.IsEmpty()){return false;}
            var turnTaker = f_aiTurnTaker.First();
            if(stash_aiType.Get(turnTaker).Value != AIAgentType.AgentType.Default){return false;}

            return true;
        }
    }
}

