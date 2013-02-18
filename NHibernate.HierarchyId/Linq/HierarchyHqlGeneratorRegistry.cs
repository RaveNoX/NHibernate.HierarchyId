#region Usings

using NHibernate.Linq;
using NHibernate.Linq.Functions;

#endregion

namespace NHibernate.HierarchyId.Linq
{
    public sealed class HierarchyHqlGeneratorRegistry : DefaultLinqToHqlGeneratorsRegistry
    {
        public HierarchyHqlGeneratorRegistry()
        {
            RegisterGenerator(ReflectionHelper.GetMethodDefinition(() => default(string).SqlToString()),
                              new ToStringGenerator());

            RegisterGenerator(
                ReflectionHelper.GetMethodDefinition(() => default(string).IsDescendantOf(default(string))),
                new IsDescendantOfGenerator());

            RegisterGenerator(
                ReflectionHelper.GetMethodDefinition(() => default(string).GetAncestor(default(int))),
                new GetAncestorGenerator());

            RegisterGenerator(
                ReflectionHelper.GetMethodDefinition(() => default(string).GetDescendant(null,null)),
                new GetDescendantGenerator());

            RegisterGenerator(
                ReflectionHelper.GetMethodDefinition(() => default(string).GetLevel()),
                new GetLevelGenerator());

            RegisterGenerator(
                ReflectionHelper.GetMethodDefinition(() => default(string).GetReparentedValue(null,null)),
                new GetReparentedValueGenerator());

            RegisterGenerator(
                ReflectionHelper.GetMethodDefinition(() => default(string).ToHierarchyId()),
                new HidParseGenerator());           
        }
    }
}