using Assets.Scripts.Observers.Events;

namespace Assets.Scripts.Counters
{
    interface ICounterChange: IEventObserver
    {
        void OnCounterChange(int value);
    }
}
