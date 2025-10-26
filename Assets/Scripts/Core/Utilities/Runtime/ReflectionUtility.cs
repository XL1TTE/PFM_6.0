using System;
using System.Linq;


namespace Core.Utilities
{
    public static class ReflectionUtility
    {
        public static Type[] FindAllSubclasses<T>(string assemblyName)
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName);

            if (assembly != null)
            {
                return assembly.DefinedTypes
                    .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract).ToArray();
            }
            else { return null; }
        }
        public static Type[] FindAllSubclasses<T>()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies.SelectMany(a => a.DefinedTypes)
                    .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract)
                    .ToArray();
        }
    }
}
