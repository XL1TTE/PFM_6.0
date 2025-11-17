using System.Linq;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using Domain.Abilities;
using Game;

namespace Gameplay.Abilities
{
    public class HealInArea : IAbilityNode
    {
        public int m_Amount;
        public int m_Area;

        public HealInArea(int a_amount, int a_area)
        {
            m_Amount = a_amount;
            m_Area = a_area;
        }

        public IAbilityNode Clone()
        {
            return new HealInArea(m_Amount, m_Area);
        }

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
                    G.Heal(t_caster, occupier, m_Amount, t_world);
                }
            }

            return UniTask.CompletedTask;
        }
    }

}
