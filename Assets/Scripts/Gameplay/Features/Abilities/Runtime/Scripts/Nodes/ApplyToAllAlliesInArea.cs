using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;

namespace Gameplay.Abilities
{
    public class ApplyToAllAlliesInArea : IAbilityNode
    {
        public int m_Area;
        public ICollection<IAbilityNode> m_Nodes;

        public ApplyToAllAlliesInArea(ICollection<IAbilityNode> a_nodes, int a_area)
        {
            m_Nodes = a_nodes;
            m_Area = a_area;
        }

        public IAbilityNode Clone() => new ApplyToAllAlliesInArea(m_Nodes, m_Area);

        public UniTask Execute(AbilityContext context)
        {
            var t_target = context.m_Target;
            var t_caster = context.m_Caster;
            var t_world = context.m_World;

            var allies = F.IsMonster(t_caster, t_world) ? GU.GetAllMonstersOnField(t_world) : GU.GetAllEnemiesOnField(t_world);

            foreach (var cell in GU.GetCellsInArea(t_target, m_Area, t_world))
            {
                if (F.IsOccupiedCell(cell, t_world) == false) { continue; }
                var occupier = GU.GetCellOccupier(cell, t_world);

                if (allies.Any(a => a.Id == occupier.Id))
                {
                    foreach (var node in m_Nodes)
                    {
                        node.Execute(new AbilityContext(t_caster, occupier, t_world));
                    }
                }
            }

            return UniTask.CompletedTask;
        }
    }

}
