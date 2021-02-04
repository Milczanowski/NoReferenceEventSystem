using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Observers
{
    public abstract class Observer<T, Y>:IBindable, IObservable<T> where T : IObserver where Y : IObservable<T>
    {
        protected List<T> Observers { get; set; }
        protected Y Target { get; set; }

        public Observer(Y _target)
        {
            Observers = new List<T>();
            Target = _target;
        }

        public void AddObserver(T observer)
        {
            if (!Observers.Contains(observer))
                Observers.Add(observer);
            //else
             //   UnityEngine.Debug.LogFormat("Doubled observer: [{0}] - [{1}]", Target, observer);
        }

        public bool ContainsObserver(T target)
        {
            return Observers.Contains(target);
        }

        public abstract IEnumerator Bind();
    }
}
