using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using NHibernate.Hql.Ast;
using NHibernate.Linq;
using NHibernate.Linq.Functions;
using NHibernate.Linq.Visitors;

namespace HierarchyId2.Linq
{
    public class GetAncestorGenerator : BaseHqlGeneratorForMethod
    {
        public GetAncestorGenerator()
        {
            SupportedMethods = new[]
            {
                NHibernate.Util.ReflectHelper.GetMethodDefinition(()=> default(string).GetAncestor(default(int)))
            };
        }

        public override HqlTreeNode BuildHql(MethodInfo method, Expression targetObject, ReadOnlyCollection<Expression> arguments, HqlTreeBuilder treeBuilder, IHqlExpressionVisitor visitor)
        {
            var arg = visitor.Visit(arguments[0]).AsExpression();
            var lvl = visitor.Visit(arguments[1]).AsExpression();

            var mt = treeBuilder.MethodCall("hid_GetAncestor", arg, lvl);

            return mt;
        }
    }
}
