using Assets.Scripts.Observers.Events;
using Assets.Scripts.Observers.Methods;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public abstract class BaseController : MonoBehaviour, IEventObservable, IMethodObservable
    {
        private static Dictionary<Type, BaseController> Controllers = new Dictionary<Type, BaseController>();

        public static event Action<Action<IEventObserver>, Action<IMethodObserver>> OnPreInit = delegate { };

        private bool Initialized { get; set; } = false;
        protected bool AsyncLoad { get; set; } = true;

        private EventObserver EventObserver { get; set; }
        private MethodObserver MethodObserver { get; set; }

        protected virtual void Awake()
        {
            if (Controllers.ContainsKey(GetType()))
                throw new Exception("Doubled controller: " + GetType());

            EventObserver = new EventObserver(this);
            MethodObserver = new MethodObserver(this);
            Controllers.Add(GetType(), this);
            OnPreInit += OnPreInitInvoke;
            Initialized = false;
        }

        protected virtual IEnumerator Init(Action<double> onProgress)
        {
            Initialized = true;
#if UNITY_EDITOR || MMDevelop
            Debug.LogFormat("Init({0})", GetType());
#endif
            yield return null;
        }

        protected virtual void PostInit()
        {
        }

        private IEnumerator AddObservers()
        {
            foreach (Type key in Controllers.Keys)
            {
                if (this is IMethodObserver)
                    Controllers[key].AddObserver(this as IMethodObserver);

                if (this is IEventObserver)
                    Controllers[key].AddObserver(this as IEventObserver);
            }
            yield return null;
        }

        protected virtual void OnPreInitInvoke(Action<IEventObserver> addEventObserver, Action<IMethodObserver> addMethodObserver)
        {
        }

        protected virtual IEnumerator Bind()
        {
            yield return EventObserver.Bind();
            yield return MethodObserver.Bind();
        }

        protected internal IEnumerator WaitForInitialize()
        {
            while (!Initialized)
                yield return null;
        }

        protected virtual void Start()
        {
        }

        protected virtual void OnDestroy()
        {
            Controllers.Remove(GetType());
        }

        protected T GetController<T>() where T : BaseController
        {
            if (Controllers.ContainsKey(typeof(T)))
                return Controllers[typeof(T)] as T;

            return default(T);
        }

        public static IEnumerator InitAll(IEnumerator onInit, Action<double> onProgress)
        {
            double maxIter = Controllers.Count * 3;

            if (maxIter == 0)
                maxIter = 1;

            double currentIter = 0;
            double currentValue = 0;

            OnPreInit.Invoke(AddObservers, AddObservers);

            double part = 1 / maxIter;

            Action<double> initProgress = (progress) =>
            {
                onProgress(currentValue + (progress * part));
            };

            foreach (var controller in Controllers)
            {
                yield return controller.Value.AddObservers();
                onProgress(currentValue = (currentIter++ / maxIter));
            }

            foreach (var controller in Controllers)
            {
                yield return controller.Value.Bind();
                onProgress(currentValue = (currentIter++ / maxIter));
            }

            foreach (var controller in Controllers)
            {
                BaseController baseController = controller.Value;
                yield return baseController.Init(initProgress);
            }

            foreach (var controller in Controllers)
            {
                yield return controller.Value.WaitForInitialize();
                onProgress(currentValue = (currentIter++ / maxIter));
            }


            foreach (var controller in Controllers)
                controller.Value.PostInit();

            foreach (var controller in Controllers)
                controller.Value.enabled = true;

#if MMDevelop
            Debug.Log("BaseController.Init END");
#endif

            if (onInit != null)
                yield return onInit;
        }

        public static void Clear()
        {
            Controllers.Clear();
        }

        public void AddObserver(IEventObserver observer)
        {
            EventObserver.AddObserver(observer);
        }

        public void AddObserver(IMethodObserver target)
        {
            MethodObserver.AddObserver(target);
        }

        protected static void AddObservers(IMethodObserver target)
        {
            foreach (var controller in Controllers)
            {
                controller.Value.AddObserver(target);
                CrossAdd(target, controller.Value);
            }
        }

        protected static void AddObservers(IEventObserver target)
        {
            foreach (var controller in Controllers)
            {
                controller.Value.AddObserver(target);
                CrossAdd(target, controller.Value);
            }
        }

        private static void CrossAdd(Observers.IObserver observer, BaseController controller)
        {
            if (observer is IMethodObservable && controller is IMethodObserver)
                (observer as IMethodObservable).AddObserver(controller as IMethodObserver);

            if (observer is IEventObservable && controller is IEventObserver)
                (observer as IEventObservable).AddObserver(controller as IEventObserver);
        }
    }
}
