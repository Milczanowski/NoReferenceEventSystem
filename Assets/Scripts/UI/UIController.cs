using System;
using System.Collections;
using Assets.Scripts.Controllers;
using Assets.Scripts.Counters;
using Assets.Scripts.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    class UIController : BaseController
        ,ICounterChange
        ,ICounterResetable
        ,IGameInitEnd
    {
        public Action ResetCounter { get; set; } = delegate { };


        [SerializeField]
        private Button resetButton = null;
        [SerializeField]
        private Text counterText = null;
        [SerializeField]
        private RectTransform loadingPanel = null;


        protected override IEnumerator Init(Action<double> onProgress)
        {
            resetButton.onClick.AddListener(ResetCounter.Invoke);

            yield return base.Init(onProgress);
        }

        public void OnCounterChange(int value)
        {
            counterText.text = value.ToString();
        }

        public void OnGameInitEnd()
        {
            loadingPanel.gameObject.SetActive(false);
        }
    }
}
