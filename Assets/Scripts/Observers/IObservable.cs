
namespace Assets.Scripts.Observers
{
    public interface IObservable<T> where T: IObserver
    {
        void AddObserver(T target);
    }
}
