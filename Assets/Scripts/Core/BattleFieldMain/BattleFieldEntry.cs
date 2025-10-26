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


        void Update()
        {
            m_World.Update(Time.deltaTime);
            m_World.CleanupUpdate(Time.deltaTime);

            m_World.Commit();
        }
    }
}
