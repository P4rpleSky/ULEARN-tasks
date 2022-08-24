using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Randomness
{
    public class FromDistributionAttribute : Attribute
    {
        public Type DistributionType { get; private set;}
        public IContinuousDistribution Distribution { get; private set; }

        public FromDistributionAttribute(Type distributionType, params object[] parameters)
        {
            DistributionType = distributionType;
            var constructor = DistributionType
                .GetConstructors()
                .FirstOrDefault(x => x.GetParameters().Length == parameters.Length);
            if (constructor is null)
                throw new ArgumentException(
                    $"Distribution { DistributionType.Name } with this number of parameters isn't defined!");
            var distribution = constructor.Invoke(parameters);
            var distrName = distribution.GetType().Name;
            Distribution = (IContinuousDistribution)distribution;
        }
    }

    public class Generator<T> where T : class
    {
        public T Instance { get; private set; }
        private Dictionary<PropertyInfo, FromDistributionAttribute> PropertyAttributeDict { get; set; }

        public Generator()
        {
            Instance = Activator.CreateInstance<T>();
            PropertyAttributeDict = new Dictionary<PropertyInfo, FromDistributionAttribute>();
            var properties = Instance.GetType().GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                var attribute = properties[i].GetCustomAttribute<FromDistributionAttribute>();
                if (attribute is null)
                    continue;
                PropertyAttributeDict[properties[i]] = attribute;
            }
        }

        public T Generate(Random rnd)
        {
            Instance = Activator.CreateInstance<T>();
            foreach (var property in PropertyAttributeDict.Keys)
            {
                var attribute = PropertyAttributeDict[property];
                property.SetValue(Instance, attribute.Distribution.Generate(rnd));
            }
            return Instance;
        }
    }
}