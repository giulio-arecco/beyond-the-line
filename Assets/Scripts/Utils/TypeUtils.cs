using System;

namespace Utils {
    public static class TypeUtils {
        public static T ConvertTo<T>(object value) {
            try {
                return value switch {
                    null => throw new ArgumentNullException(nameof(value)),
                    T t => t,
                    _ => (T)Convert.ChangeType(value, typeof(T))
                };
            }
            catch (Exception ex) {
                throw new ArgumentException(
                    $"Cannot convert value '{value}' (type {(value == null ? "null" : value.GetType().ToString())}) to {typeof(T).Name}", ex);
            }
        }
    }
}
