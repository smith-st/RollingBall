using GameEntities.Interfaces.Common;
using UnityEngine;

namespace GameEntities.Interfaces
{
    public interface IBall:IBase
    {
        void Move(Vector2 direction, float distance);
    }
}