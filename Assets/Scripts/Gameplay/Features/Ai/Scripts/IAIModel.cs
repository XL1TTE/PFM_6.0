using System.Linq;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities.Components;
using Game;
using Scellecs.Morpeh;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.AI
{
    public interface IAIModel
    {
        UniTask Process(Entity a_Agent, World a_world);
    }

    public sealed class RandomAIModel : IAIModel
    {
        public async UniTask Process(Entity a_agent, World a_world)
        {
            var context = new AIExecutionContext(a_agent, a_world);

            var t_MoveToCellJob = new MoveToCell();
            var t_EndTurnJob = new EndTurn();

            var stash_abilities = a_world.GetStash<AbilitiesComponent>();

            var t_abilities = stash_abilities.Get(a_agent);

            var t_moveShifts = t_abilities.m_LegsAbility.m_Shifts;
            var t_agentPos = GU.GetEntityPositionOnCell(a_agent, a_world);

            var t_moveOptions = GU.GetCellsFromShifts(t_agentPos, t_moveShifts, a_world)
                .Where(opt => !F.IsOccupiedCell(opt, a_world)).ToList();

            var t_cell = t_moveOptions.ElementAt(Random.Range(0, t_moveOptions.Count()));

            await t_MoveToCellJob.SetupJob(t_cell).DoJob(context);
            await t_EndTurnJob.DoJob(context);
        }
    }
}
