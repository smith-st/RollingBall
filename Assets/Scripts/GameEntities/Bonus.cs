using GameEntities.Interfaces;

namespace GameEntities
{
    public class Bonus : Poolable, IBonus
    {
        public bool IsCollected => DestroyingAnimation;

        public void Collect()
        {
            DestroyingAnimation = true;
        }
    }
}
