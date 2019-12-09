#region Usings

using NHibernate.Linq;
using NHibernate.Linq.Functions;

#endregion

namespace HierarchyId2.Linq
{
    public sealed class HierarchyHqlGeneratorRegistry : DefaultLinqToHqlGeneratorsRegistry
    {
        public HierarchyHqlGeneratorRegistry()
        {
            RegisterGenerator(NHibernate.Util.ReflectHelper.GetMethodDefinition(() => default(string).SqlToString()),
                              new ToStringGenerator());

            RegisterGenerator(
                NHibernate.Util.ReflectHelper.GetMethodDefinition(() => default(string).IsDescendantOf(default(string))),
                new IsDescendantOfGenerator());

            RegisterGenerator(
                NHibernate.Util.ReflectHelper.GetMethodDefinition(() => default(string).GetAncestor(default(int))),
                new GetAncestorGenerator());

            RegisterGenerator(
                NHibernate.Util.ReflectHelper.GetMethodDefinition(() => default(string).GetDescendant(null,null)),
                new GetDescendantGenerator());

            RegisterGenerator(
                NHibernate.Util.ReflectHelper.GetMethodDefinition(() => default(string).GetLevel()),
                new GetLevelGenerator());

            RegisterGenerator(
                NHibernate.Util.ReflectHelper.GetMethodDefinition(() => default(string).GetReparentedValue(null,null)),
                new GetReparentedValueGenerator());

            RegisterGenerator(
                NHibernate.Util.ReflectHelper.GetMethodDefinition(() => default(string).ToHierarchyId()),
                new HidParseGenerator());           
        }
    }
}