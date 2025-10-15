namespace SpaceArcade.ObjectPool
{
    public interface IPoolable
    {
        void OnSpawn();
        void OnReturn();
    }
}
