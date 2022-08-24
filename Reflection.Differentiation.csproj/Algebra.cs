using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflection.Differentiation
{
    public static class Algebra
    {
        public static Expression<Func<double, double>> CastToLambda(Expression expr, ParameterExpression parameterExpr)
        {
            return Expression.Lambda<Func<double, double>>(expr, parameterExpr);
        }

        public static Expression GetDerivativeOfFunction(string funcName, Expression argument)
        {
            switch (funcName)
            {
                case "Cos":
                    return Expression.Multiply(
                    Expression.Constant(-1.0),
                    Expression.Call(typeof(Math).GetMethod("Sin"), argument));
                case "Sin":
                    return Expression.Call(typeof(Math).GetMethod("Cos"), argument);
                default:
                    throw new ArgumentException($"Unknown function: { funcName }");
            }
        }

        public static Expression<Func<double, double>> Differentiate(Expression<Func<double, double>> function)
        {
            var functionBody = function.Body;
            if (functionBody is ConstantExpression)
                return CastToLambda(Expression.Constant(0.0), function.Parameters[0]);
            else if (functionBody is ParameterExpression)
                return CastToLambda(Expression.Constant(1.0), function.Parameters[0]);
            else if (functionBody is MethodCallExpression methodCallBody)
            {
                var argument = methodCallBody.Arguments[0];
                return CastToLambda(
                    Expression.Multiply(
                        GetDerivativeOfFunction(methodCallBody.Method.Name, argument),
                        Differentiate(CastToLambda(argument, function.Parameters[0])).Body),
                    function.Parameters[0]);
            }
            else if (functionBody is BinaryExpression binaryExpression)
            {
                var right = Differentiate(
                CastToLambda(binaryExpression.Right, function.Parameters[0])).Body;
                var left = Differentiate(
                    CastToLambda(binaryExpression.Left, function.Parameters[0])).Body;
                if (binaryExpression.NodeType == ExpressionType.Multiply)
                {
                    var leftMult = Expression.Multiply(left, binaryExpression.Right);
                    var rightMult = Expression.Multiply(binaryExpression.Left, right);
                    return CastToLambda(Expression.Add(leftMult, rightMult), function.Parameters[0]);
                }
                return CastToLambda(
                    Expression.MakeBinary(binaryExpression.NodeType, left, right),
                    function.Parameters[0]
                    );
            }
            else if (functionBody is UnaryExpression unaryBody)
                throw new ArgumentException($"Method ToString is not available");
            else throw new ArgumentException();
        }
    }
}