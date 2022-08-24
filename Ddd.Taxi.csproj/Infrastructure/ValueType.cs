using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ddd.Infrastructure
{
	/// <summary>
	/// Базовый класс для всех Value типов.
	/// </summary>
	public class ValueType<T> where T : class
	{
		private static readonly List<PropertyInfo> properties;

		static ValueType()
        {
			properties = new List<PropertyInfo>(typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public));
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
                return true;
            else if (!(obj is T))
                return false;
            foreach (var property in properties)
            {
                var value1 = property.GetValue(this);
                var value2 = property.GetValue(obj as T);
                if (value1 is null && value2 is null)
                    continue;
                else if (value1 is null || value2 is null || !value1.Equals(value2))
                    return false;
            }
            return true;
        }

        public bool Equals(T obj)
        {
            return this.Equals(obj as object);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder(typeof(T).Name);
            result.Append("(");
            foreach (var property in properties.OrderBy(x => x.Name))
            {
                var valueString = "";
                if (!(property.GetValue(this) is null))
                    valueString = property.GetValue(this).ToString();
                result.Append($"{ property.Name }: { valueString }");
                result.Append("; ");
            }
            result.Remove(result.Length - 2, 2);
            result.Append(")");
            return result.ToString();
        }

        public override int GetHashCode()
        {
            int result = 0;
            unchecked
            {
                for (int i = 0; i < properties.Count; ++i)
                {
                    var value = properties[i].GetValue(this);
                    if (!(value is null))
                        result += (437 + i) * value.GetHashCode();
                    else
                        result += 0;
                }
            }
            return result;
        }
    }
}