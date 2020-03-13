using DG.Tweening;
using Factories;
using GameEntities.Interfaces;
using Settings;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class BonusManager:PoolableManager<IBonus>, IInitializable
    {
        [Inject]
        private GameSettings _settings;
        [Inject]
        private BonusFactory _bonusFactory;

        protected override IFactory<IBonus> Factory()
        {
            return _bonusFactory;
        }

        public void BonusToPlace(Vector2 position)
        {
            var bonus = GetElement();
            bonus.ToPlace(position);
        }

        public void BallNewCheckpoint(Vector2 position)
        {
            position = position - Vector2.right - Vector2.up;
            var bonuses = _activeElements.FindAll(b=>b.LessPosition(position));
            foreach (var bonus in bonuses)
            {
                DestroyAndReturnToPool(bonus, _settings.animationTime);
            }
        }

        public bool CollectBonusOnTileIfExist(Vector2 position)
        {
            var bonus = _activeElements.Find(b => b.OnPosition(position) && !b.IsCollected);
            if (bonus == null)
            {
                return false;
            }
            bonus.Collect();
            bonus.Transform.DOScale(0f, _settings.animationTime).OnComplete(() => { ReturnToPool(bonus); });
            return true;
        }
    }
}
