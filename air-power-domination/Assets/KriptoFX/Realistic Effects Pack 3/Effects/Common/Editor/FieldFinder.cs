using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GameNet.KriptoFX.Realistic_Effects_Pack_3.Effects.Common.Editor
{
    public static class FieldFinder<T>
    {
        public static FieldInfo GetField<TValue>(Expression<Func<T, TValue>> selector)
        {
            Expression body = selector;
            if (body is LambdaExpression)
            {
                body = ((LambdaExpression)body).Body;
            }
            switch (body.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return (FieldInfo)((MemberExpression)body).Member;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
