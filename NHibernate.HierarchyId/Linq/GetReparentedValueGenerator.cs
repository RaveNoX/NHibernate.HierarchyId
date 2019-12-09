using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;

namespace NHibernate.HierarchyId.Linq
{
    public class GetReparentedValueGenerator : BaseHqlGeneratorForMethod
    {
        public GetReparentedValueGenerator()
        {
            SupportedMethods = new[]
            {
                NHibernate.Util.ReflectHelper.GetMethodDefinition(()=> default(string).GetReparentedValue(default(string), default(string)))
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            var arg = visitor.Visit(arguments[0]).AsExpression();
            var c1 = visitor.Visit(arguments[1]).AsExpression();
            var c2 = visitor.Visit(arguments[2]).AsExpression();

            var mt = treeBuilder.MethodCall("hid_GetReparentedValue", arg, c1, c2);

            return mt;
        }
    }
}
