using System;
using System.Linq.Expressions;
using NHibernate.Criterion;
using NHibernate.HierarchyId.Projections;
using NHibernate.Impl;

namespace NHibernate.HierarchyId
{
    public static class ProjectionsExtensionsHierarchy
    {
        public static string GetAncestor(this string hid, int level)
        {
            throw new InvalidOperationException("Not to be used directly - use inside QueryOver or NH Linq expression");
        }

        public static IProjection GetAncestor(MethodCallExpression methodCallExpression)
        {
            var ishift = 0;
            IProjection property;

            if (methodCallExpression.Object == null)
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
            }
            else
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Object).AsProjection();
                ishift = -1;
            }

            var level = (int)ExpressionProcessor.FindValue(methodCallExpression.Arguments[ishift + 1]);
            return new GetAncestorProjection(property, level);
        }

        public static string GetDescendant(this string hid, string child1, string child2)
        {
            throw new InvalidOperationException("Not to be used directly - use inside QueryOver or NH Linq expression");
        }

        public static IProjection GetDescendant(MethodCallExpression methodCallExpression)
        {
            var ishift = 0;
            IProjection property;

            if (methodCallExpression.Object == null)
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
            }
            else
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Object).AsProjection();
                ishift = -1;
            }

            var child1 = (string)ExpressionProcessor.FindValue(methodCallExpression.Arguments[ishift + 1]);
            var child2 = (string)ExpressionProcessor.FindValue(methodCallExpression.Arguments[ishift + 2]);
            return new GetDescendantProjection(property, child1, child2);
        }

        public static string GetReparentedValue(this string hid, string oldParent, string newParent)
        {
            throw new InvalidOperationException("Not to be used directly - use inside QueryOver or NH Linq expression");
        }

        public static IProjection GetReparentedValue(MethodCallExpression methodCallExpression)
        {
            var ishift = 0;
            IProjection property;

            if (methodCallExpression.Object == null)
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
            }
            else
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Object).AsProjection();
                ishift = -1;
            }

            var oldRoot = (string)ExpressionProcessor.FindValue(methodCallExpression.Arguments[ishift + 1]);
            var newRoot = (string)ExpressionProcessor.FindValue(methodCallExpression.Arguments[ishift + 2]);
            return new GetReparentedValueProjection(property, oldRoot, newRoot);
        }

        public static int GetLevel(this string hid)
        {
            throw new InvalidOperationException("Not to be used directly - use inside QueryOver or NH Linq expression");
        }

        public static IProjection GetLevel(MethodCallExpression methodCallExpression)
        {
            IProjection property;

            if (methodCallExpression.Object == null)
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
            }
            else
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Object).AsProjection();
            }

            return new GetLevelProjection(property);
        }

        public static string CastAsString(this string hid)
        {
            throw new InvalidOperationException("Not to be used directly - use inside QueryOver or NH Linq expression");
        }

        public static IProjection CastAsString(MethodCallExpression methodCallExpression)
        {
            IProjection property;

            if (methodCallExpression.Object == null)
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
            }
            else
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Object).AsProjection();
            }

            return new CastProjection(NHibernateUtil.String, property);
        }

        public static string ToHierarchyId(this string hid)
        {
            throw new InvalidOperationException("Not to be used directly - use inside QueryOver or NH Linq expression");
        }

        public static IProjection ToHierarchyId(MethodCallExpression methodCallExpression)
        {
            IProjection property;

            if (methodCallExpression.Object == null)
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
            }
            else
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Object).AsProjection();
            }

            return new ToHierarchyIdProjection(property);
        }

        public static string SqlToString(this string hid)
        {
            throw new InvalidOperationException("Not to be used directly - use inside QueryOver or NH Linq expression");
        }

        public static IProjection SqlToString(MethodCallExpression methodCallExpression)
        {
            IProjection property;

            if (methodCallExpression.Object == null)
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Arguments[0]).AsProjection();
            }
            else
            {
                property = ExpressionProcessor.FindMemberProjection(methodCallExpression.Object).AsProjection();
            }

            return new ToStringMethodProjection(property);
        }
    }
}
