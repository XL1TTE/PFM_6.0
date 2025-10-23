using Domain.Ability;
using Domain.Stats.Components;
using Gameplay.Abilities;
using Scellecs.Morpeh;
using UnityEngine;

namespace Core
{
    public class BattleFieldMain : MonoBehaviour
    {
        public static World m_World;

        async void Start()
        {
            m_World = World.Default;
            m_World.UpdateByUnity = false;

            var testAbility = new Ability();
            testAbility.AddEffect(new DealDamage(5, DamageType.PHYSICAL_DAMAGE));

            var testCaster = m_World.CreateEntity();

            var testTarget = m_World.CreateEntity();
            m_World.GetStash<Health>().Add(testTarget);

            await testAbility.Execute(testCaster, testTarget, m_World);
        }

        void Update()
        {
            m_World.Update(Time.deltaTime);
            m_World.CleanupUpdate(Time.deltaTime);

            m_World.Commit();
        }
    }
}
