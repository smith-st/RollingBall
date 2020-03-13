using GameEntities.Interfaces;
using Settings;
using UnityEngine;
using Zenject;

namespace Factories
{
    public class BallsFactory:BaseFactory<IBall>
    {
        [Inject]
        private GamePrefabs _prefabs;

        protected override GameObject GetPrefab()
        {
            return _prefabs.ball;
        }
    }
}
