using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Documentation
{
    public class Specifier<T> : ISpecifier
    {
        public string GetApiDescription()
        {
            var attribute = typeof(T).GetCustomAttribute<ApiDescriptionAttribute>();
            return attribute is null ? null : attribute.Description;
        }

        public string[] GetApiMethodNames()
        {
            var methods = typeof(T).GetMethods();
            var result = new List<string>();
            for (int i = 0; i < methods.Length; i++)
                if (!(methods[i].GetCustomAttribute<ApiMethodAttribute>() is null))
                    result.Add(methods[i].Name);
            return result.ToArray();
        }

        public string GetApiMethodDescription(string methodName)
        {
            var method = typeof(T).GetMethod(methodName);
            if (method is null) return null;
            var attribute = method.GetCustomAttribute<ApiDescriptionAttribute>();
            return attribute is null ? null : attribute.Description;
        }

        public string[] GetApiMethodParamNames(string methodName)
        {
            var method = typeof(T).GetMethod(methodName);
            if (method is null) return null;
            var paramsOfMethod = method.GetParameters();
            var result = new string[paramsOfMethod.Length];
            for (int i = 0; i < paramsOfMethod.Length; i++)
                result[i] = paramsOfMethod[i].Name;
            return result;
        }

        public string GetApiMethodParamDescription(string methodName, string paramName)
        {
            var method = typeof(T).GetMethod(methodName);
            if (method is null) return null;
            var paramsOfMethod = method.GetParameters();
            int index = 0;
            while (index < paramsOfMethod.Length)
            {
                if (paramsOfMethod[index].Name == paramName)
                {
                    var attribute = paramsOfMethod[index]
                        .GetCustomAttribute<ApiDescriptionAttribute>();
                    return attribute is null ? null : attribute.Description;
                }
                index++;
            }
            return null;
        }

        private ApiParamDescription GetDefaultDescription(string methodName, string paramName)
            => new ApiParamDescription
            {
                ParamDescription = new CommonDescription(
                    paramName,
                    GetApiMethodParamDescription(methodName, paramName))
            };

        public ApiParamDescription GetApiMethodParamFullDescription(string methodName, string paramName)
        {
            var method = typeof(T).GetMethod(methodName);
            if (method is null)
                return GetDefaultDescription(methodName, paramName);  
            var parameter = method.GetParameters().FirstOrDefault(x => x.Name == paramName);
            if (parameter is null)
                return GetDefaultDescription(methodName, paramName);
            var intValAttr = parameter.GetCustomAttribute<ApiIntValidationAttribute>();
            var requiredAttr = parameter.GetCustomAttribute<ApiRequiredAttribute>();
            var description = GetApiMethodParamDescription(methodName, paramName);
            return new ApiParamDescription
            {
                ParamDescription = new CommonDescription(paramName, description),
                MaxValue = intValAttr is null ? null : intValAttr.MaxValue,
                MinValue = intValAttr is null ? null : intValAttr.MinValue,
                Required = requiredAttr is null ? false : requiredAttr.Required
            };
        }

        public ApiMethodDescription GetApiMethodFullDescription(string methodName)
        {
            var method = typeof(T).GetMethod(methodName);
            if (method is null || method.GetCustomAttribute<ApiMethodAttribute>() is null) 
                return null;
            var parameters = method.GetParameters();
            if (parameters is null) return null;
            var intValAttr = method.ReturnParameter.GetCustomAttribute<ApiIntValidationAttribute>();
            var requiredAttr = method.ReturnParameter.GetCustomAttribute<ApiRequiredAttribute>();
            var returnDescription = intValAttr is null && requiredAttr is null ?
                null : new ApiParamDescription
                {
                    ParamDescription = new CommonDescription(),
                    MaxValue = intValAttr is null ? null : intValAttr.MaxValue,
                    MinValue = intValAttr is null ? null : intValAttr.MinValue,
                    Required = requiredAttr is null ? false : requiredAttr.Required
                };
            return new ApiMethodDescription
            {
                ParamDescriptions = parameters
                    .Select(x => GetApiMethodParamFullDescription(methodName, x.Name)).ToArray(),
                MethodDescription = new CommonDescription(methodName, GetApiMethodDescription(methodName)),
                ReturnDescription = returnDescription
            };
        }
    }
}