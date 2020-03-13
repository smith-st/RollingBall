using GameEntities.Interfaces;
using Settings;
using UnityEngine;
using Zenject;

namespace Factories
{
    public class BonusFactory:BaseFactory<IBonus>
    {
        [Inject]
        private GamePrefabs _prefabs;

        protected override GameObject GetPrefab()
        {
            return _prefabs.bonus;
        }
    }
}
