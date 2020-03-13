using System;
using DG.Tweening;
using Factories;
using GameEntities.Interfaces;
using Settings;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class BallManager :IInitializable, ITickable
    {

        public Vector2 Position => _ballTransform.position;

        [Inject]
        private GameSettings _settings;
        [Inject]
        private BallsFactory _ballsFactory;

        private IBall _ball;
        private Transform _ballTransform;
        private float _speed;
        private bool _needMoving;
        private Vector2 _direction;

        public void Initialize()
        {
            _ball = _ballsFactory.Create();
            _ballTransform = _ball.Transform;
            _speed = _settings.ballSpeed/100f;
            _direction = Vector2.up;
        }

        public void Tick()
        {
            if (!_needMoving) return;
            _ball.Move(_direction, _speed);
        }

        public void StartMove()
        {
            _needMoving = true;
        }

        public void StopMoveAndDestroy(Action onComplete)
        {
            _needMoving = false;
            _ballTransform.DOScale(0f, _settings.animationTime).OnComplete(onComplete.Invoke);
        }

        public void ChangeDirection()
        {
            _direction = _direction.y == 1f ? Vector2.right : Vector2.up;
        }

        public void Reset(Vector2 position)
        {
            _ballTransform.DOScale(1f, _settings.animationTime/2f);
            _ballTransform.position = position;
        }
    }
}
