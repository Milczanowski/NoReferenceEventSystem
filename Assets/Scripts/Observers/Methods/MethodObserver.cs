using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Observers.Methods
{
    public class MethodObserver:Observer<IMethodObserver, IMethodObservable>
    {
        public MethodObserver(IMethodObservable target):base(target)
        {
        }

        public override IEnumerator Bind()
        {
            foreach(MethodInfo _method in Target.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if(_method.IsDefined(typeof(MethodObservable), true))
                {
                    MethodObservable observable = _method.GetCustomAttributes(typeof(MethodObservable), true)[0] as MethodObservable;

                    foreach(var observer in Observers)
                    {
                        if(observer.GetType().GetInterfaces().Contains(observable.Type))
                        {
                            PropertyInfo propertyInfo = observable.Type.GetProperties()[0];
                            propertyInfo.SetValue(observer, Delegate.CreateDelegate(propertyInfo.PropertyType, Target, _method.Name), null);
                        }
                    }
                }
            }
            yield return null;
            Observers.Clear();
        }
    }
}

