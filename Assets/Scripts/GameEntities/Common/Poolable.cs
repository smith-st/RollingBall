namespace GameEntities
{
    public class Poolable:Movable
    {
        public bool DestroyingAnimation { get; set; }
        public void Disable()
        {
            gameObject.SetActive(false);
            DestroyingAnimation = false;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}
