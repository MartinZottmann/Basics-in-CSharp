using System;
using System.Collections.Generic;
using System.Reflection;

namespace MartinZottmann.Engine.UnitTests
{
    public class Helper
    {
        public bool EqualsDeep(object a, object b)
        {
            var terminators = new HashSet<object>();
            terminators.Add(a);
            terminators.Add(b);
            var result = EqualsDeep(a, b, terminators);
            terminators.Remove(b);
            terminators.Remove(a);
            return result;
        }

        protected bool EqualsDeep(object a, object b, HashSet<object> terminators)
        {
            if (null == a && null == b)
                return true;

            if (null == a || null == b)
                return false;

            if (a is IntPtr || b is IntPtr || a is UIntPtr || b is UIntPtr)
                return false;

            var type_a = a.GetType();
            var type_b = b.GetType();

            if (type_a != type_b)
                return false;

            if (type_a.IsPointer || type_b.IsPointer)
                return false;

            if (type_a.IsEnum || type_a.IsPrimitive || typeof(String) == type_a)
                return Object.Equals(a, b);

            var fields = type_a.GetFields(BindingFlags.Instance | BindingFlags.Public);

            foreach (var field in fields)
            {
                var value_a = field.GetValue(a);
                var value_b = field.GetValue(b);

                if (terminators.Contains(value_a) && terminators.Contains(value_b))
                    continue;

                terminators.Add(value_a);
                terminators.Add(value_b);
                var result = EqualsDeep(value_a, value_b, terminators);
                terminators.Remove(value_b);
                terminators.Remove(value_a);
                if (!result)
                    return false;
            }

            var properties = type_a.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var property in properties)
            {
                if (0 != property.GetIndexParameters().Length)
                    continue;

                if (!property.CanRead)
                    continue;

                if (!property.CanWrite)
                    continue;

                var value_a = property.GetValue(a);
                var value_b = property.GetValue(b);

                if (terminators.Contains(value_a) && terminators.Contains(value_b))
                    continue;

                terminators.Add(value_a);
                terminators.Add(value_b);
                var result = EqualsDeep(value_a, value_b, terminators);
                terminators.Remove(value_b);
                terminators.Remove(value_a);
                if (!result)
                    return false;
            }

            return true;
        }
    }
}
