using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Interactions;
using Scellecs.Morpeh;

namespace Domain.Abilities
{
    public sealed class Ability
    {
        public Ability(List<IAbilityNode> effects)
        {
            m_Effects = effects;
        }

        private List<IAbilityNode> m_Effects;

        public async UniTask Execute(Entity a_caster, Entity a_target, World a_world)
        {
            var t_context = new AbilityContext(a_caster, a_target, a_world);

            foreach (var effect in m_Effects)
            {
                await effect.Execute(t_context);
            }
        }

        public Ability Clone()
        {
            var clonedEffects = new List<IAbilityNode>();
            foreach (var effect in m_Effects)
            {
                clonedEffects.Add(effect.Clone());
            }
            return new Ability(clonedEffects);
        }
        public void AddEffect(IAbilityNode effect) => m_Effects.Add(effect);
        public void RemoveEffect(IAbilityNode effect) => m_Effects.Remove(effect);
        public void InsertEffect(int index, IAbilityNode effect) => m_Effects.Insert(index, effect);
        public void ClearEffects() => m_Effects.Clear();

        public T GetEffect<T>() where T : IAbilityNode => m_Effects.OfType<T>().FirstOrDefault();
        public T[] GetEffects<T>() where T : IAbilityNode => m_Effects.OfType<T>().ToArray();
    }
}
