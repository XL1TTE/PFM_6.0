using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;

namespace Domain.Abilities
{
    public sealed class Ability
    {
        public Ability(List<IAbilityEffect> effects)
        {
            m_Effects = effects;
        }

        private List<IAbilityEffect> m_Effects;

        public async UniTask Execute(Entity a_caster, Entity a_target, World a_world)
        {
            var t_context = new AbilityContext(a_caster, a_target, a_world);

            foreach (var effect in m_Effects)
            {
                await effect.Execute(t_context);
            }
        }

        public void AddEffect(IAbilityEffect effect) => m_Effects.Add(effect);
        public void RemoveEffect(IAbilityEffect effect) => m_Effects.Remove(effect);
        public void InsertEffect(int index, IAbilityEffect effect) => m_Effects.Insert(index, effect);
        public void ClearEffects() => m_Effects.Clear();

        public T GetEffect<T>() where T : IAbilityEffect => m_Effects.OfType<T>().FirstOrDefault();
        public T[] GetEffects<T>() where T : IAbilityEffect => m_Effects.OfType<T>().ToArray();
    }
}
