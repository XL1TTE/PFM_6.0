using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Scellecs.Morpeh;

namespace Domain.Ability
{
    public sealed class Ability
    {
        private List<IAbilityEffect> m_Effects = new();

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
        public IEnumerable<T> GetEffects<T>() where T : IAbilityEffect => m_Effects.OfType<T>();
    }
}
