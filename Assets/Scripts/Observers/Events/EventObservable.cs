using System;

namespace Assets.Scripts.Observers.Events
{
    class EventObservable: Attribute
    {
        public Type Type { get; private set; }

        public EventObservable(Type type)
        {
            Type = type;  
        }
    }
}
