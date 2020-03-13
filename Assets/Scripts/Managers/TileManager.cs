using System.Linq;
using Factories;
using GameEntities.Interfaces;
using Settings;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class TileManager: PoolableManager<ITile>, IInitializable
    {
        public Vector2 LastTilePosition => _lastTilePosition;

        [Inject] private GameSettings _settings;
        [Inject] private TilesFactory _tilesFactory;
        [Inject] private BonusManager _bonusManager;

        private Vector2 _nextTilePosition;
        private bool _pathDirection = true;//true - up/false - right
        private bool _prevDirection = true;
        private int _countTileInRow;
        private Vector2 _lastTilePosition;
        private int _tileCounter;
        private int _tileCounterForBonus = 1;

        public override void Initialize()
        {
            base.Initialize();
            _countTileInRow = (int) _settings.difficulty;
        }

        public void CreateStartupArea()
        {
            const int size = 3; //odd numbers preferred (3, 5, 7, 9, ...)
            const float halfSize = ((float)size + 1f) / 2f;
            for (var i = 1; i <= size; i++)
            {
                for (var j = 1; j <= size; j++)
                {
                    var position = new Vector2(j - halfSize, i - halfSize);
                    TileToPlace(position, true);
                }
            }
            _nextTilePosition = new Vector2(0f, halfSize-1f);
        }

        public void BuildPath(int length)
        {
            for (var i = 0; i < length; i++)
            {
                AddTailsDependsDifficulty(_nextTilePosition, _pathDirection);
                _prevDirection = _pathDirection;
                _pathDirection = Random.Range(1, 3) == 1;
                if (_pathDirection != _prevDirection)
                {
                    AddAdditionalTailsOnTurn();
                }

                FindNextTilePositionInDirection();
            }
        }

        public bool PresentTileOnPosition(Vector2 position)
        {
            return _activeElements.Any(t => t.OnPosition(position));
        }

        public void BallNewCheckpoint(Vector2 position)
        {
            _bonusManager.BallNewCheckpoint(position);
            position = position - Vector2.right - Vector2.up;
            var tiles = _activeElements.FindAll(t=>t.LessPosition(position));
            foreach (var tile in tiles)
            {
                DestroyAndReturnToPool(tile, _settings.animationTime);
            }
        }

        public bool CollectBonusOnTileIfExist(Vector2 position)
        {
            return _bonusManager.CollectBonusOnTileIfExist(position);
        }

        private void AddTailsDependsDifficulty(Vector2 position, bool pathDirection)
        {
            var half = Mathf.Floor(_countTileInRow / 2f);
            var from = 0f - half;
            var to = _countTileInRow - 1 - half;
            var direction = pathDirection ? Vector2.right : Vector2.up;
            for (var i = from; i <= to; i++)
            {
                TileToPlace(position + direction*i);
            }
        }

        private void AddAdditionalTailsOnTurn()
        {
            var steps = Mathf.Floor((_countTileInRow - 1f) / 2f);
            var direction = _prevDirection ? Vector2.up : Vector2.right;
            for (var i = 0; i < steps; i++)
            {
                var position = FindNextTilePositionInDirection(_nextTilePosition + direction * i, _prevDirection);
                AddTailsDependsDifficulty(position, _prevDirection);
            }
        }

        protected override IFactory<ITile> Factory()
        {
            return _tilesFactory;
        }

        private void TileToPlace(Vector2 position, bool forStartupArea = false)
        {
            if (PresentTileOnPosition(position))
            {
                return;
            }

            var tile = GetElement();
            tile.ToPlace(position);
            if (!forStartupArea)
            {
                GenerateBonusIfNeeded(position);
            }
        }

        private void GenerateBonusIfNeeded(Vector2 position)
        {
            _lastTilePosition = position;
            if (_settings.bonusAlgorithm == GameSettings.BonusAlgorithmType.Random)
            {
                if (Random.Range(1, 100) <= _settings.bonusRandomAlgorithmPercent)
                {
                    _bonusManager.BonusToPlace(position);
                }
            }
            else
            {
                SequenceAlgorithmIteration(position);
            }
        }

        private void SequenceAlgorithmIteration(Vector2 position)
        {
            _tileCounter++;
            var positionInBlock = _tileCounter % _settings.bonusSequenceAlgorithmBlockSize;
            if (positionInBlock == 0)
            {
                positionInBlock = _settings.bonusSequenceAlgorithmBlockSize;
            }

            if (positionInBlock == _tileCounterForBonus)
            {
                _bonusManager.BonusToPlace(position);
            }

            if (positionInBlock == _settings.bonusSequenceAlgorithmBlockSize)
            {
                _tileCounterForBonus++;
                if (_tileCounterForBonus > _settings.bonusSequenceAlgorithmBlockSize)
                {
                    _tileCounterForBonus = 1;
                }
            }
        }

        private void FindNextTilePositionInDirection()
        {
            _nextTilePosition = FindNextTilePositionInDirection(_nextTilePosition, _pathDirection);
        }

        private Vector2 FindNextTilePositionInDirection(Vector2 position, bool direction)
        {
            if (direction)//up
            {
                position += Vector2.up;
            }
            else//right
            {
                position += Vector2.right;
            }

            return position;
        }
    }
}
