﻿#region Usings

using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;

#endregion

namespace HierarchyId2.Linq
{
    public class GetLevelGenerator : BaseHqlGeneratorForMethod
    {
        public GetLevelGenerator()
        {
            SupportedMethods = new[]
                {
                    NHibernate.Util.ReflectHelper.GetMethodDefinition(() => default(string).GetLevel())
                };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject,
                                             ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder,
                                             IHqlExpressionVisitor visitor)
        {
            var arg = visitor.Visit(arguments[0]).AsExpression();
            var mt = treeBuilder.MethodCall("hid_GetLevel", arg);

            return mt;
        }
    }
}