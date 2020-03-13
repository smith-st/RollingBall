namespace GameEntities.Interfaces.Common
{
    public interface IPoolable:IMovable
    {
        bool DestroyingAnimation { get; set; }
        void Disable();
        void Enable();
    }
}
