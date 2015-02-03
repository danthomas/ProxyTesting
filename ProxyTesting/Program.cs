using System;
using System.Collections.Generic;
using System.Linq;
using Castle.DynamicProxy;

namespace ProxyTesting
{
    public static class Mocker
    {
        public static Mock<T> Create<T>() where T : class
        {
            Mock<T> mock = new Mock<T>();

            mock.Object = new ProxyGenerator().CreateInterfaceProxyWithoutTarget<T>(mock);

            return mock;
        }
    }

    public class Mock<T> : IInterceptor where T : class
    {
        public T Object { get; set; }

        private When<T> _when;

        readonly List<When<T>> _whens = new List<When<T>>();
        private bool _record;

        public When<T> When(Action<T> action)
        {
            _when = new When<T>();

            _record = true;

            action(Object);
            
            _record = false;

            _whens.Add(_when);

            return _when;
        }

        public void Log<T1>(Action<T> action, Action<T1> log)
        {
        }

        public void Intercept(IInvocation invocation)
        {
            if (_record)
            {
                _when.Name = invocation.Method.Name;
                _when.Arguments = invocation.Arguments;
            }
            else
            {
                When<T> when = _whens.FirstOrDefault(item => item.Name == invocation.Method.Name
                    && ArgumentsAreEqual(item.Arguments, invocation.Arguments));

                if (when != null)
                {
                    _whens.Remove(when);
                    invocation.ReturnValue = when.ReturnValue;
                }


            }
        }

        private bool ArgumentsAreEqual(object[] lhs, object[] rhs)
        {
            if (lhs.Length != rhs.Length)
            {
                return false;
            }

            for (int i = 0; i < rhs.Length; ++i)
            {
                if (!lhs[i].Equals(rhs[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
    
    public class When<T>
    {
        public string Name { get; set; }
        internal object ReturnValue { get; set; }
        internal object[] Arguments { get; set; }

        public When<T> Return(object returnValue)
        {
            ReturnValue = returnValue;
            return this;
        }
    }

    public class Log<T>
    {
        public string Name { get; set; }
        
    }
}
