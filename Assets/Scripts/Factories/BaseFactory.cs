using UnityEngine;
using Zenject;

namespace Factories
{
    public abstract class BaseFactory<T>: IFactory<T>
    {
        protected abstract GameObject GetPrefab();

        public T Create()
        {
            var element  = Object.Instantiate(GetPrefab(), Vector3.zero, Quaternion.identity).GetComponent<T>();
            return element;
        }
    }
}