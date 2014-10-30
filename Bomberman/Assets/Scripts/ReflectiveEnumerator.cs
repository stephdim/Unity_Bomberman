using UnityEngine;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

public static class ReflectiveEnumerator {
	static ReflectiveEnumerator() {}

	public static IEnumerable<string> GetEnumerableOfType<T>() where T : class {
		List<string> objects = new List<string>();
		foreach (Type type in 
				 Assembly.GetAssembly(typeof(T)).GetTypes()
				 .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T)))) {
			objects.Add(type.Name);
		}
		return objects;
	}
}
