using Assets.Scripts.Controllers;
using Assets.Scripts.Observers.Events;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game
{
    class GameController : BaseController
    {
        [EventObservable(typeof(IGameInitEnd))]
        private event Action onGameInitEnd = delegate { };


        protected override void Start()
        {
            Debug.LogFormat("Start");

            StartCoroutine(InitAll(OnInitEnd(), (progress) =>
            {
                Debug.LogFormat("Progress: {0:0.00}", progress);
            }));

            base.Start();
        }

        private IEnumerator OnInitEnd()
        {
            onGameInitEnd.Invoke();
            yield return null;
        }
    }
}
