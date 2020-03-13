using UnityEngine;

namespace GameEntities.Interfaces.Common
{
    public interface IMovable:IBase
    {
        void ToPlace(Vector2 position);
        bool OnPosition(Vector2 position);
        bool LessPosition(Vector2 position);
    }
}