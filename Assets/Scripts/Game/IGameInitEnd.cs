using Assets.Scripts.Observers.Events;

namespace Assets.Scripts.Game
{
    interface IGameInitEnd: IEventObserver
    {
        void OnGameInitEnd();
    }
}
