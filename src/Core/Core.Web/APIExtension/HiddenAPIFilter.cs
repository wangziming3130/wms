using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Core.Web
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class HiddenAPIAttribute : Attribute
    {
    }

    public class HiddenAPIFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (ApiDescription apiDescription in context.ApiDescriptions)
            {
                if (apiDescription.TryGetMethodInfo(out MethodInfo method))
                {
                    if (method.ReflectedType.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenAPIAttribute))
                            || method.CustomAttributes.Any(t => t.AttributeType == typeof(HiddenAPIAttribute)))
                    {
                        string key = "/" + apiDescription.RelativePath;
                        if (key.Contains("?"))
                        {
                            int idx = key.IndexOf("?", System.StringComparison.Ordinal);
                            key = key.Substring(0, idx);
                        }
                        var targetItem = swaggerDoc.Paths.FirstOrDefault(s => s.Key == key);
                        if (!string.IsNullOrWhiteSpace(targetItem.Key))
                        {
                            var supportMethods = targetItem.Value.Operations;
                            if (supportMethods.Count <= 1)
                            {
                                swaggerDoc.Paths.Remove(key);
                            }
                            else
                            {
                                var operationType = ConvertToHttpOperationType(apiDescription.HttpMethod);
                                targetItem.Value.Operations.Remove(operationType);
                            }
                        }
                    }
                }
            }
        }
        private OperationType ConvertToHttpOperationType(string httpMethod)
        {
            var result = OperationType.Trace;
            switch (httpMethod.ToLower())
            {
                case "delete": { result = OperationType.Delete; } break;
                case "get": { result = OperationType.Get; } break;
                case "post": { result = OperationType.Post; } break;
                case "put": { result = OperationType.Put; } break;
                case "patch": { result = OperationType.Patch; } break;
            }
            return result;
        }
    }
}
