using System;
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

        public static string GetCommentIdForType(this Type type)
        {
            var builder = new StringBuilder("T:");
            AppendFullTypeName(type, builder, expandGenericArgs: false);

            return builder.ToString();
        }

        public static string GetCommentIdForProperty(this PropertyInfo propertyInfo)
        {
            var builder = new StringBuilder("P:");
            AppendFullTypeName(propertyInfo.DeclaringType, builder);
            builder.Append(".");
            AppendPropertyName(propertyInfo, builder);

            return builder.ToString();
        }

        private static void AppendFullTypeName(Type type, StringBuilder builder, bool expandGenericArgs = false)
        {
            if (type.Namespace != null)
            {
                builder.Append(type.Namespace);
                builder.Append(".");
            }
            AppendTypeName(type, builder, expandGenericArgs);
        }

        private static void AppendTypeName(Type type, StringBuilder builder, bool expandGenericArgs)
        {
            if (type.IsNested)
            {
                AppendTypeName(type.DeclaringType, builder, false);
                builder.Append(".");
            }

            builder.Append(type.Name);

            if (expandGenericArgs)
                ExpandGenericArgsIfAny(type, builder);
        }

        public static void ExpandGenericArgsIfAny(Type type, StringBuilder builder)
        {
            if (type.IsGenericType)
            {
                string full = builder.ToString();
                int argPos = full.IndexOf('(');
                if (argPos > 0)
                {
                    var genericArgsBuilder = new StringBuilder("{");

                    var genericArgs = type.GetGenericArguments();
                    for (int i = 0; i < genericArgs.Length; i++)
                    {
                        if (type.BaseType != null && type.BaseType.Name == "Enum")
                            genericArgsBuilder.Append($"`{i}");
                        else
                            AppendFullTypeName(genericArgs[i], genericArgsBuilder, true);
                        genericArgsBuilder.Append(",");
                    }
                    genericArgsBuilder.Replace(",", "}", genericArgsBuilder.Length - 1, 1);

                    builder.Clear();
                    builder.Append(full.Substring(0, argPos));
                    string newValue = genericArgsBuilder.ToString();
                    string oldValue = string.Format("`{0}", genericArgs.Length);
                    builder.Append(full.Substring(argPos).Replace(oldValue, newValue));
                }
            }
            else if (type.IsArray)
                ExpandGenericArgsIfAny(type.GetElementType(), builder);
        }

        private static void AppendMethodName(MethodInfo methodInfo, StringBuilder builder)
        {
            builder.Append(methodInfo.Name);
            var declaringType = methodInfo.DeclaringType;
            if (declaringType.IsGenericType)
            {
                methodInfo = declaringType.GetGenericTypeDefinition().GetMethod(methodInfo.Name);
            }

            var parameters = methodInfo.GetParameters();
            if (parameters.Length == 0) return;

            builder.Append("(");
            int generic = 0;
            foreach (var param in parameters)
            {
                if (param.ParameterType.IsGenericParameter)
                {
                    builder.Append($"`{generic}");
                    generic++;
                }
                else
                {
                    AppendFullTypeName(param.ParameterType, builder, true);
                }
                builder.Append(",");
            }
            builder.Replace(",", ")", builder.Length - 1, 1);
        }

        private static void AppendPropertyName(PropertyInfo propertyInfo, StringBuilder builder)
        {
            builder.Append(propertyInfo.Name);
        }
    }
}
