using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using MoreLinq.Extensions;

namespace Iridio.Common.Utils
{
    public static class ReflectionMixin
    {
        public static async Task<object> InvokeTask(this MethodInfo method, object instance, object[] parameters)
        {
            var ctorParams = method.GetParameters();
            var injectableParameters = ctorParams.ZipLongest(parameters, SelectValue);

            if (method.ReturnType.IsGenericType)
            {
                return await (dynamic) method.Invoke(instance, injectableParameters.ToArray());
            }

            await (Task) method.Invoke(instance, injectableParameters.ToArray());
            return null;
        }

        public static object Call(this MethodInfo meth, object instance, params object[] parameters)
        {
            var invoke = meth.Invoke(instance, parameters);
            return invoke;
        }

        public static Task<object> InvokeTask(this object instance, string methodName, params object[] parameters)
        {
            var method = instance.GetType().GetMethod(methodName);
            var executeTask = method.InvokeTask(instance, parameters);
            return executeTask;
        }

        private static object SelectValue(ParameterInfo pi, object v)
        {
            return v == null && pi.HasDefaultValue ? pi.DefaultValue : v;
        }

        public static Delegate CreateDelegate(this MethodInfo methodInfo, object target)
        {
            Func<Type[], Type> getType;
            var isAction = methodInfo.ReturnType.Equals((typeof(void)));
            var types = methodInfo.GetParameters().Select(p => p.ParameterType);

            if (isAction)
            {
                getType = Expression.GetActionType;
            }
            else
            {
                getType = Expression.GetFuncType;
                types = types.Concat(new[] { methodInfo.ReturnType });
            }

            if (methodInfo.IsStatic)
            {
                return Delegate.CreateDelegate(getType(types.ToArray()), methodInfo);
            }

            return Delegate.CreateDelegate(getType(types.ToArray()), target, methodInfo.Name);
        }
    }
}