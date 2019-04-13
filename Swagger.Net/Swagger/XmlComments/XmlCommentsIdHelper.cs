using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Swagger.Net.XmlComments
{
    public static class XmlCommentsIdHelper
    {
        public static string GetCommentIdForMethod(this MethodInfo methodInfo)
        {
            var builder = new StringBuilder("M:");
            AppendFullTypeName(methodInfo.DeclaringType, builder);
            builder.Append(".");
            AppendMethodName(methodInfo, builder);

            return builder.ToString();
        }

        public static string GetCommentId(this Type type)
        {
            var builder = new StringBuilder("T:");
            AppendFullTypeName(type, builder, expandGenericArgs: false);

            return builder.ToString();
        }

        public static string GetCommentId(this PropertyInfo propertyInfo)
        {
            var builder = new StringBuilder("P:");
            AppendFullTypeName(propertyInfo.DeclaringType, builder);
            builder.Append(".");
            builder.Append(propertyInfo.Name);
            return builder.ToString();
        }

        public static string GetCommentId(this FieldInfo fieldInfo)
        {
            var builder = new StringBuilder("F:");
            AppendFullTypeName(fieldInfo.DeclaringType, builder);
            builder.Append(".");
            builder.Append(fieldInfo.Name);
            return builder.ToString();
        }

        private static void AppendFullTypeName(Type type, StringBuilder builder, bool expandGenericArgs = false, IDictionary<string, int> genericParametersPositions = null)
        {
            if (type.Namespace != null)
            {
                builder.Append(type.Namespace);
                builder.Append(".");
            }
            AppendTypeName(type, builder, expandGenericArgs, genericParametersPositions);
        }

        public static string GetNameWithoutGenericArity(Type t)
        {
            string name = t.Name;
            int index = name.IndexOf('`');
            return index == -1 ? name : name.Substring(0, index);
        }

        private static void AppendTypeName(Type type, StringBuilder builder, bool expandGenericArgs, IDictionary<string, int> genericParametersPositions = null)
        {
            if (type.IsNested)
            {
                AppendTypeName(type.DeclaringType, builder, true, genericParametersPositions);
                builder.Append(".");
            }

            if ((type.IsGenericType && expandGenericArgs) || type.IsArray)
            {
                var nonGenericName = GetNameWithoutGenericArity(type);
                builder.Append(nonGenericName);
                if (type.IsEnum)
                    return;
            }
            else
                builder.Append(type.Name);

            if (expandGenericArgs)
                ExpandGenericArgsIfAny(type, builder, genericParametersPositions);
            if (type.IsArray)
                builder.Append("[]");
        }

        public static void ExpandGenericArgsIfAny(Type type, StringBuilder builder, IDictionary<string, int> genericParametersPositions = null)
        {
            if (type.IsGenericType)
            {
                var genericArgs = type.GetGenericArguments();
                builder.Append("{");
                for (int i = 0; i < genericArgs.Length; i++)
                {
                    if (type.IsEnum || ((type.IsClass || type.IsInterface) && genericArgs[i].FullName == null))
                        builder.Append($"`{genericParametersPositions[genericArgs[i].Name]}");
                    else
                        AppendFullTypeName(genericArgs[i], builder, true, genericParametersPositions);
                    builder.Append(",");
                }
                builder.Replace(",", "}", builder.Length - 1, 1);
            }
            else if (type.IsArray)
                ExpandGenericArgsIfAny(type.GetElementType(), builder, genericParametersPositions);
        }

        private static void AppendMethodName(MethodInfo methodInfo, StringBuilder builder)
        {
            var methodName = methodInfo.Name;
            builder.Append(methodName);
            var declaringType = methodInfo.DeclaringType;
            if (declaringType.IsGenericType)
            {
                methodInfo = declaringType.GetGenericTypeDefinition().GetMethod(methodInfo.Name);
            }

            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 0) return;

            builder.Append("(");
            var genericParametersPositions = GetTypeParameterPositions(declaringType);
            foreach (var param in parameters)
            {
                if (param.ParameterType.IsGenericParameter)
                    builder.Append($"`{genericParametersPositions[param.ParameterType.Name]}");
                else
                    AppendFullTypeName(param.ParameterType, builder, true, genericParametersPositions);
                builder.Append(",");
            }
            builder.Replace(",", ")", builder.Length - 1, 1);
        }

        private static IDictionary<string, int> GetTypeParameterPositions(Type type)
        {
            var result = new Dictionary<string, int>();
            if (!type.IsGenericType)
                return result;
            var genericTypeDefinition = type.GetGenericTypeDefinition();
            foreach (var genericArgument in genericTypeDefinition.GetGenericArguments())
                result.Add(genericArgument.Name, genericArgument.GenericParameterPosition);
            return result;
        }
    }
}
