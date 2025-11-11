using Game;
using Scellecs.Morpeh;
using Scellecs.Morpeh.Collections;
using Scellecs.Morpeh.Providers;
using TriInspector;
using UnityEngine;

namespace Domain.Providers
{
    public abstract class ComponentProvider<T> : EntityProvider where T : struct, IComponent
    {
#pragma warning disable 0618
        [SerializeField]
        [HideInInspector]
        private T serializedData;
        private Stash<T> stash;
#if UNITY_EDITOR
        private string typeName = typeof(T).Name;

        [PropertySpace]
        [ShowInInspector]
        [PropertyOrder(1)]
        [HideLabel]
        [InlineProperty]
#endif
        private T Data
        {
            get
            {
                if (this.cachedEntity.IsNullOrDisposed() == false)
                {
                    var data = this.Stash.Get(this.cachedEntity, out var exist);
                    if (exist)
                    {
                        return data;
                    }
                }

                return this.serializedData;
            }
            set
            {
                if (this.cachedEntity.IsNullOrDisposed() == false)
                {
                    this.Stash.Set(this.cachedEntity, value);
                }
                else
                {
                    this.serializedData = value;
                }
            }
        }


        private World m_World;

        protected override void OnEnable()
        {
            m_World = ECS.m_CurrentWorld;

            GetEntityForComponent();

            base.OnEnable();
        }

        protected void GetEntityForComponent()
        {
            if (this.cachedEntity.IsNullOrDisposed())
            {
                var instanceId = this.gameObject.GetInstanceID();
                if (map.TryGetValue(instanceId, out var item))
                {
                    if (item.entity.IsNullOrDisposed())
                    {
                        this.cachedEntity = item.entity = m_World.CreateEntity();
                    }
                    else
                    {
                        this.cachedEntity = item.entity;
                    }
                    item.refCounter++;
                    map.Set(instanceId, item, out _);
                }
                else
                {
                    this.cachedEntity = item.entity = m_World.CreateEntity();
                    item.refCounter = 1;
                    map.Add(instanceId, item, out _);
                }
            }
        }

        public Stash<T> Stash
        {
            get
            {
                if (this.stash == null)
                {
                    this.stash = m_World.GetStash<T>();
                }
                return this.stash;
            }
        }

        public ref T GetSerializedData() => ref this.serializedData;

        public ref T GetData()
        {
            var ent = this.Entity;
            if (ent.IsNullOrDisposed() == false)
            {
                if (this.Stash.Has(ent))
                {
                    return ref this.Stash.Get(ent);
                }
            }

            return ref this.serializedData;
        }

        public ref T GetData(out bool existOnEntity)
        {
            if (this.cachedEntity.IsNullOrDisposed() == false)
            {
                return ref this.Stash.Get(this.cachedEntity, out existOnEntity);
            }

            existOnEntity = false;
            return ref this.serializedData;
        }

        protected virtual void OnValidate()
        {
            if (this.serializedData is IValidatable validatable)
            {
                validatable.OnValidate();
                this.serializedData = (T)validatable;
            }
            if (this.serializedData is IValidatableWithGameObject validatableWithGameObject)
            {
                validatableWithGameObject.OnValidate(this.gameObject);
                this.serializedData = (T)validatableWithGameObject;
            }
        }

        protected sealed override void PreInitialize()
        {
            this.Stash.Set(this.cachedEntity, this.serializedData);
        }

        protected sealed override void PreDeinitialize()
        {
            var ent = this.cachedEntity;
            if (ent.IsNullOrDisposed() == false)
            {
                this.Stash.Remove(ent);
            }
        }
#pragma warning restore 0618

    }
}
