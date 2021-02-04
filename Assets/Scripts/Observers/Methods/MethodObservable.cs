using System;

namespace Assets.Scripts.Observers.Methods
{
    class MethodObservable:Attribute
    {
        public Type Type { get; private set; }

        public MethodObservable(Type type)
        {
            Type = type;
        }
    }
}
