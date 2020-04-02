using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MoreLinq.Extensions;

namespace SimpleScript
{
    public static class ReflectionMixin
    {
        public static async Task<object> ExecuteTask(this object instance, string methodName, params object[] parameters)
        {
            var meth = instance.GetType().GetMethod(methodName);

            var ctorParams = meth.GetParameters();
            var injectableParameters = ctorParams.ZipLongest(parameters, SelectValue);

            if (!meth.ReturnType.ContainsGenericParameters)
            {
                await (dynamic) meth.Invoke(instance, injectableParameters.ToArray());
                return new object();
            }

            return await (dynamic)meth.Invoke(instance, injectableParameters.ToArray());
        }

        private static object SelectValue(ParameterInfo pi, object v)
        {
            return pi.HasDefaultValue ? pi.DefaultValue : v;
        }

        public static object Execute(this object instance, string methodName, params object[] parameters)
        {
            var meth = instance.GetType().GetMethod(methodName);
            return meth.Invoke(instance, parameters);
        }
    }
}