
using System.Collections.Generic;
using Core.Utilities;
using UnityEngine;

namespace UI.Elements
{

    public abstract class ISingletonPool<T> : MonoBehaviour, IUIElementPool<T>
    where T : IUIElement, IPoolElement
    {
        private static ISingletonPool<T> m_instance;
        public void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
            }

            if (m_TextPrototype == null)
            {
                throw new System.Exception("Please, provide text prototype prefab for TextPool.");
            }

            if (m_PoolContainer == null)
            {
                m_PoolContainer = new GameObject("[POOL]").transform;
                m_PoolContainer.SetParent(transform);
            }
        }

        public static ISingletonPool<T> I() => m_instance;

        [SerializeField]
        private T m_TextPrototype;

        [SerializeField]
        private Transform m_PoolContainer;


        [HideInInspector]
        private Queue<T> m_FreeObjects = new(8);


        public T WarmupElement()
        {
            if (IsAnyFreeObjectExist())
            {
                return m_FreeObjects.Dequeue();
            }
            else
            {
                return CreateNewObject();
            }
        }

        public virtual void FreeElement(T element)
        {
            if (element == null) { return; }
            element.gameObject.SetActive(false);

            element.transform.SetParent(m_PoolContainer);

            if (m_FreeObjects.Contains(element) == false)
            {
                m_FreeObjects.Enqueue(element);
            }
        }

        private bool IsAnyFreeObjectExist() => m_FreeObjects.Count > 0;

        private T CreateNewObject()
        {
            var t_obj = Instantiate(m_TextPrototype, m_PoolContainer);
            t_obj.gameObject.SetActive(false);

            t_obj.OnFree += () => FreeElement(t_obj);

            return t_obj;
        }
    }

}
