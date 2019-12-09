#region Usings

using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;

#endregion

namespace NHibernate.HierarchyId.Linq
{
    public class IsDescendantOfGenerator : BaseHqlGeneratorForMethod
    {
        public IsDescendantOfGenerator()
        {
            SupportedMethods = new[]
                {
                    NHibernate.Util.ReflectHelper.GetMethodDefinition(() => default(string).IsDescendantOf(default(string)))
                };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject,
                                             ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder,
                                             IHqlExpressionVisitor visitor)
        {
            var arg = visitor.Visit(arguments[0]).AsExpression();
            var parent = visitor.Visit(arguments[1]).AsExpression();
            var mt = treeBuilder.BooleanMethodCall("hid_IsDescendantOf", new[] {arg, parent});

            return mt;
        }
    }
}