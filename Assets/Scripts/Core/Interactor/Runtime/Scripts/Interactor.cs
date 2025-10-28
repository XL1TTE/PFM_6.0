using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Utilities;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace Interactions
{
    public enum Priority : short
    {
        VERY_LOW = -100,
        LOW = -10,
        NORMAL = 0,
        HIGH = 10,
        VERY_HIGH = 100
    }

    public static class Interactor
    {
        private static IEnumerable<BaseInteraction> m_All;
        static Interactor()
        {
            var t_all = ReflectionUtility.FindAllSubclasses<BaseInteraction>();
            m_All = t_all.Select(
                t => Activator.CreateInstance(t) as BaseInteraction);
        }

        public static void Init()
        {
            // Just for trigger static constructor.
        }

        public static IEnumerable<T> GetAll<T>()
            => InteractorCache<T>.GetFromCache(m_All);


        public static async UniTask CallAll<T>(Func<T, UniTask> action)
        {
            foreach (var handler in GetAll<T>())
            {
                await action(handler);
            }
        }
    }

    internal static class InteractorCache<T>
    {
        private const int CACHE_SIZE = 64;

        private static List<T> m_Cache;
        public static IEnumerable<T> GetFromCache(IEnumerable<BaseInteraction> a_All)
        {
            if (m_Cache != null)
            {
                return m_Cache;
            }

            var m_filter = a_All
                .OfType<T>();
            if (m_filter.Count() > CACHE_SIZE)
            {
                throw new InvalidOperationException($"Cache size exceeded. Maximum: {CACHE_SIZE}, Actual: {m_filter.Count()}");
            }
            m_Cache = m_filter
                .OrderByDescending(x => (x as BaseInteraction)?.m_Priority)
                .ToList();

            return m_Cache;
        }
    }
}

