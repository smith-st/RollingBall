using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;
using IPoolable = GameEntities.Interfaces.Common.IPoolable;

namespace Managers
{
    public abstract class PoolableManager<T> where T : IPoolable
    {
        private readonly Queue<T> _pool = new Queue<T>();
        protected List<T> _activeElements = new List<T>();
        private Transform _container;

        protected abstract IFactory<T> Factory();

        protected T GetElement()
        {
            T element;
            if (_pool.Count == 0)
            {
                element = Factory().Create();
                element.Transform.parent = _container;
            }
            else
            {
                element = _pool.Dequeue();
                element.Enable();
            }
            _activeElements.Add(element);
            return element;
        }

        protected void ReturnToPool(T element)
        {
            _activeElements.Remove(element);
            element.Transform.localScale = Vector3.one;
            element.Disable();
            _pool.Enqueue(element);
        }

        protected void DestroyAndReturnToPool(T element, float animationTime)
        {
            if (element.DestroyingAnimation)
            {
                return;
            }
            element.DestroyingAnimation = true;
            element.Transform.DOScale(0f, animationTime).OnComplete(() =>
            {
                ReturnToPool(element);
            });
        }

        public virtual void Initialize()
        {
            _container = new GameObject(typeof(T).ToString()).transform;
        }
    }
}