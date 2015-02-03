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

        public void Intercept(IInvocation invocation)
        {
            if (_record)
            {
                _when.Name = invocation.Method.Name;
                _when.Arguments = invocation.Arguments;
            }
            else
            {
                When<T> when = _whens.SingleOrDefault(item => item.Name == invocation.Method.Name
                    && ArgumentsAreEqual(item.Arguments, invocation.Arguments));

                if (when == null)
                {
                    throw new Exception();
                }

                invocation.ReturnValue = when.ReturnValue;
            }
        }

        private bool ArgumentsAreEqual(object[] lhs, object[] rhs)
        {
            if (rhs.Length == rhs.Length)
            {
                return false;
            }

            for (int i = 0; i < rhs.Length; ++i)
            {
                if (lhs[i] != rhs[i])
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
        public object ReturnValue { get; set; }
        public object[] Arguments { get; set; }

        public When<T> Return(object returnValue)
        {
            ReturnValue = returnValue;
            return this;
        }
    }
}
