using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.HierarchyId.Criterion;
using NHibernate.Impl;

namespace NHibernate.HierarchyId
{
    public static class RestrictionsExtensionsHierarchy
    {
        public static bool IsDescendantOf(this string hid, string parentHid)
        {
            throw new Exception("Not to be used directly - use inside QueryOver expression");
        }

        internal static ICriterion IsDescendantOf(MethodCallExpression methodCallExpression)
        {
            var projection = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
            var value = (string)ExpressionProcessor.FindValue(methodCallExpression.Arguments[1]);
            return new IsDescendantOfExpression(projection,value);
        }


    }
}
