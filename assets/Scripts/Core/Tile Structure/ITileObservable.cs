namespace Core.Tile_Structure
{
    public interface IObservable<T>
    {
        public void RegisterObserver(IObserver<T> observer);
        public void UnregisterObserver(IObserver<T> observer);
    }
    
    public interface IObserver<T>
    {
        public void Notify(T t);
    }
}