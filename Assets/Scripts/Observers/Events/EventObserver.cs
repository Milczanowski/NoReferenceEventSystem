using Assets.Scripts.Controllers;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Assets.Scripts.Observers.Events
{
    public class EventObserver:Observer<IEventObserver, IEventObservable>
    {
        public EventObserver(IEventObservable target):base(target)
        {
        }
        public override IEnumerator Bind()
        {
            Type baseType = Target.GetType();

            while (baseType != null && baseType != typeof(BaseController))
            {
                yield return Bind(baseType);
                baseType = baseType.BaseType;
            }

            Observers.Clear();
        }

        private IEnumerator Bind(Type type)
        {
            foreach (EventInfo _event in type.GetEvents(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                if (_event.IsDefined(typeof(EventObservable), true))
                {
                    EventObservable observable = _event.GetCustomAttributes(typeof(EventObservable), true)[0] as EventObservable;

                    foreach (var observer in Observers)
                    {
                        if (observer.GetType().GetInterfaces().Contains(observable.Type))
                        {
                            MethodInfo addMethod = _event.GetAddMethod(true);
                            foreach (MethodInfo method in observable.Type.GetMethods())
                            {
                                addMethod.Invoke(Target, new[] { Delegate.CreateDelegate(_event.EventHandlerType, observer, method.Name) });
                                break;
                            }

                        }
                        else if (observer.GetType().GetInterface(observable.Type.FullName) != null)
                        {
                            MethodInfo addMethod = _event.GetAddMethod(true);
                            Type[] t = _event.EventHandlerType.GetMethod("Invoke").GetParameters().Select(x => x.ParameterType).ToArray();
                            foreach (MethodInfo method in observable.Type.GetMethods())
                            {
                                if (observer.GetType().GetMethod(method.Name, t) != null)
                                {
                                    addMethod.Invoke(Target, new[] { Delegate.CreateDelegate(_event.EventHandlerType, observer, method.Name) });
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            yield return null;
        }
    }
}
