using System;
using System.Collections;
using Assets.Scripts.Controllers;
using Assets.Scripts.Game;
using Assets.Scripts.Observers.Events;
using Assets.Scripts.Observers.Methods;
using UnityEngine;

namespace Assets.Scripts.Counters
{
    class CounterController: BaseController
        ,IGameInitEnd
    {
        [EventObservable(typeof(ICounterChange))]
        private event Action<int> onCounterChange = delegate { };


        private int Counter { get; set; } = 0;



        [MethodObservable(typeof(ICounterResetable))]
        private void ResetCounter()
        {
            Counter = 0;
            onCounterChange.Invoke(Counter);
        }

        private IEnumerator IncCounter()
        {
            WaitForSeconds wait = new WaitForSeconds(1);

            while(true)
            {
                yield return wait;
                onCounterChange.Invoke(++Counter);
            }

        }

        public void OnGameInitEnd()
        {
            StartCoroutine(IncCounter());
        }
    }
}
