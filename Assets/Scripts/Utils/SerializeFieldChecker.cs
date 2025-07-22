using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

public static class SerializeFieldChecker {
    public static bool HasSerializedFields(Type type) {
        // Find all instance (non-static), public and private fields
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        
        return fields.Any(field => (field.IsPublic && !field.IsNotSerialized) || field.GetCustomAttribute<SerializeField>() != null);
    }
}
