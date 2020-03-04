using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Sirenix.Utilities;

namespace BulwarkStudios.GameSystems.Utils {

    public static class ReflectionUtils {

        public static HashSet<Type> ListAllDerivedTypes(Type type) {

            HashSet<Type> results = new HashSet<Type>();

            var baseAssembly = type.Assembly;
            var baseAssembyName = baseAssembly.GetName();

            foreach (Assembly assembly in AssemblyUtilities.GetAllAssemblies()) {

                var isDependent = (assembly == baseAssembly);

                if (!isDependent) {
                    foreach (var reference in assembly.GetReferencedAssemblies()) {
                        if (reference.FullName == baseAssembyName.FullName) {
                            isDependent = true;
                            break;
                        }
                    }
                }

                if (isDependent)
                    GetAllDerivedTypesRecursively(assembly.GetTypes(), type, results);
            }

            return results;

        }

        private static void GetAllDerivedTypesRecursively(Type[] types, Type type, HashSet<Type> results) {
            if (type.IsGenericType) {
                GetDerivedFromGeneric(types, type, results);
            }
            else {
                GetDerivedFromNonGeneric(types, type, results);
            }
        }

        private static void GetDerivedFromGeneric(Type[] types, Type type, HashSet<Type> results) {
            foreach (var t in types) {

                var baseType = t.BaseType;

                if ((baseType != null)
                    && baseType.IsGenericType
                    && (baseType.GetGenericTypeDefinition() == type)) {

                    GetAllDerivedTypesRecursively(types, t, results);
                    results.Add(t);
                }
            }
        }

        private static void GetDerivedFromNonGeneric(Type[] types, Type type, HashSet<Type> results) {
            foreach (var t in types) {
                if ((t != type)
                    && type.IsAssignableFrom(t)) {

                    GetAllDerivedTypesRecursively(types, t, results);
                    results.Add(t);
                }
            }
        }

    }

}