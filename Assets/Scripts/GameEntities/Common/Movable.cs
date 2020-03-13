using UnityEngine;

namespace GameEntities
{
    public class Movable:Base
    {
        private Vector2 _position;
        public void ToPlace(Vector2 position)
        {
            _position = position;
            transform.position = _position;
        }

        public bool OnPosition(Vector2 position)
        {
            return _position == position;
        }

        public bool LessPosition(Vector2 position)
        {
            return _position.x < position.x || _position.y < position.y;
        }
    }
}