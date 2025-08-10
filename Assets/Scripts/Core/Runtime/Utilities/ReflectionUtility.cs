using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Core.Utilities
{
    public static class ReflectionUtility
    {
        public static Type[] FindAllSubclasses<T>(string assemblyName){
            var assembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == assemblyName);
            
            if(assembly != null){
                return assembly.DefinedTypes
                    .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract).ToArray();
            }
            else{return null;}
        } 
    }
}
