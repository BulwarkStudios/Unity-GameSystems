using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Sirenix.Utilities;

namespace BulwarkStudios.GameSystems.Utils {

    public static class ReflectionUtils {

        public static HashSet<Type> ListAllDerviedTypes(Type type) {

            HashSet<Type> results = new HashSet<Type>();

            foreach (Assembly assembly in AssemblyUtilities.GetAllAssemblies()) {

                Type[] types = assembly.GetTypes();

                List<Type> data = new List<Type>();
                GetAllDerivedTypesRecursively(types, type, ref data);

                results.UnionWith(data);

            }

            return results;

        }

        private static void GetAllDerivedTypesRecursively(Type[] types, Type type1, ref List<Type> results) {
            if (type1.IsGenericType) {
                GetDerivedFromGeneric(types, type1, ref results);
            }
            else {
                GetDerivedFromNonGeneric(types, type1, ref results);
            }
        }

        private static void GetDerivedFromGeneric(Type[] types, Type type, ref List<Type> results) {
            var derivedTypes = types
                .Where(t => t.BaseType != null && t.BaseType.IsGenericType &&
                            t.BaseType.GetGenericTypeDefinition() == type).ToList();

            results.AddRange(derivedTypes);

            foreach (Type derivedType in derivedTypes) {
                GetAllDerivedTypesRecursively(types, derivedType, ref results);
            }
        }

        private static void GetDerivedFromNonGeneric(Type[] types, Type type, ref List<Type> results) {
            var derivedTypes = types.Where(t => t != type && type.IsAssignableFrom(t)).ToList();

            results.AddRange(derivedTypes);

            foreach (Type derivedType in derivedTypes) {
                GetAllDerivedTypesRecursively(types, derivedType, ref results);
            }
        }

    }

}