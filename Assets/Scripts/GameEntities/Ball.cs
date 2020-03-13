using GameEntities.Interfaces;
using UnityEngine;

namespace GameEntities
{
    public class Ball : Base, IBall
    {
        public void Move(Vector2 direction, float distance)
        {
            transform.Translate(direction*distance);
        }
    }
}
