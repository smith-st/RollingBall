using GameEntities.Interfaces.Common;

namespace GameEntities.Interfaces
{
    public interface IBonus:IPoolable
    {
        bool IsCollected { get; }
        void Collect();
    }
}