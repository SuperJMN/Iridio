using System.Threading.Tasks;

namespace SimpleScript
{
    public static class ReflectionMixin
    {
        public static async Task<object> ExecuteTask(this object instance, string methodName, params object[] parameters)
        {
            var meth = instance.GetType().GetMethod(methodName);
            return await (dynamic)meth.Invoke(instance, parameters);
        }

        public static object Execute(this object instance, string methodName, params object[] parameters)
        {
            var meth = instance.GetType().GetMethod(methodName);
            return meth.Invoke(instance, parameters);
        }
    }
}