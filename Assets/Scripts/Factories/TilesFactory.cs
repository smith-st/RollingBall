using GameEntities.Interfaces;
using Settings;
using UnityEngine;
using Zenject;

namespace Factories
{
    public class TilesFactory:BaseFactory<ITile>
    {
        [Inject]
        private GamePrefabs _prefabs;

        protected override GameObject GetPrefab()
        {
            return _prefabs.tile;
        }
    }
}
