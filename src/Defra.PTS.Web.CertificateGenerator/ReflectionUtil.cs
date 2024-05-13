using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace Defra.PTS.Web.CertificateGenerator;

[ExcludeFromCodeCoverage]
internal static class ReflectionUtil
{
    public static MethodInfo GetGenericMethod<T>(Expression<Func<T, object>> expression) => GetGenericMethod(expression.Body);

    private static MethodInfo GetGenericMethod(Expression expression)
    {
        return expression switch
        {
            UnaryExpression unary when unary.NodeType is ExpressionType.Convert or ExpressionType.ConvertChecked => GetGenericMethod(unary.Operand),
            MethodCallExpression call when call.Method.IsGenericMethod => call.Method.GetGenericMethodDefinition(),
            MethodCallExpression call => call.Method,
            _ => throw new ArgumentException("Expression is not a simple method call expression")
        };
    }
}