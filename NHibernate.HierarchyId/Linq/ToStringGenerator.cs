#region Usings

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
    public class ToStringGenerator : BaseHqlGeneratorForMethod
    {
        public ToStringGenerator()
        {
            SupportedMethods = new[]
                {
                    NHibernate.Util.ReflectHelper.GetMethodDefinition(() => default(string).SqlToString())
                };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject,
                                             ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder,
                                             IHqlExpressionVisitor visitor)
        {
            var arg = visitor.Visit(arguments[0]).AsExpression();
            var mt = treeBuilder.MethodCall("to_string", arg);

            return mt;
        }
    }
}