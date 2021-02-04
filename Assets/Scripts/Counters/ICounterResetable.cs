using Assets.Scripts.Observers.Methods;
using System;

namespace Assets.Scripts.Counters
{
    interface ICounterResetable : IMethodObserver
    {
        Action ResetCounter { get; set; }
    }
}
