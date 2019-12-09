using NHibernate.Cfg;
using NHibernate.Dialect.Function;
using NHibernate.HierarchyId.Linq;
using NHibernate.Impl;

namespace NHibernate.HierarchyId
{
    public static class HierarchyIdExtensions
    {
        private static bool _criterionRegistered;
        private static readonly object LockObject = new object();

        public static void RegisterHierarchySupport(Configuration cfg)
        {
            if (!_criterionRegistered)
            {
                lock (LockObject)
                {
                    if (!_criterionRegistered)
                    {
                        RegisterCriterionSupport();
                        _criterionRegistered = true;
                    }
                }
            }

            #region HQL
            // hid.ToString()
            cfg.SqlFunctions.Add("to_string", new SQLFunctionTemplate(NHibernateUtil.String, "?1.ToString()"));            

            // hid.IsDescendantOf(parent) = 1
            cfg.SqlFunctions.Add("hid_IsDescendantOf", new SQLFunctionTemplate(NHibernateUtil.Boolean, "?1.IsDescendantOf(?2) = 1"));

            // hid.GetAncestor(level)
            cfg.SqlFunctions.Add("hid_GetAncestor", new SQLFunctionTemplate(NHibernateUtil.String, "?1.GetAncestor(?2)"));

            // hid.GetDescendant(child1, child2)
            cfg.SqlFunctions.Add("hid_GetDescendant", new SQLFunctionTemplate(NHibernateUtil.String, "?1.GetDescendant(?2, ?3)"));

            // hid.GetLevel()
            cfg.SqlFunctions.Add("hid_GetLevel", new SQLFunctionTemplate(NHibernateUtil.Int32, "?1.GetLevel()"));

            // hid.GetReparentedValue(old, new)
            cfg.SqlFunctions.Add("hid_GetReparentedValue", new SQLFunctionTemplate(NHibernateUtil.String, "?1.GetReparentedValue(?2, ?3)"));            

            // hierarchyid::Parse
            cfg.SqlFunctions.Add("hid_Parse", new SQLFunctionTemplate(NHibernateUtil.String, "hierarchyid::Parse(?1)"));
            #endregion

            cfg.LinqToHqlGeneratorsRegistry<HierarchyHqlGeneratorRegistry>();            
        }        

        private static void RegisterCriterionSupport()
        {
            // This is only awailable way to register Criteria/QueryOver extensions
            ExpressionProcessor.RegisterCustomProjection(() => default(string).CastAsString(), ProjectionsExtensionsHierarchy.CastAsString);
            ExpressionProcessor.RegisterCustomProjection(() => default(string).SqlToString(), ProjectionsExtensionsHierarchy.SqlToString);

            ExpressionProcessor.RegisterCustomProjection(() => default(string).GetAncestor(0), ProjectionsExtensionsHierarchy.GetAncestor);
            ExpressionProcessor.RegisterCustomProjection(() => default(string).GetDescendant(null, null), ProjectionsExtensionsHierarchy.GetDescendant);
            ExpressionProcessor.RegisterCustomProjection(() => default(string).GetReparentedValue(null, null), ProjectionsExtensionsHierarchy.GetReparentedValue);
            ExpressionProcessor.RegisterCustomProjection(() => default(string).GetLevel(), ProjectionsExtensionsHierarchy.GetLevel);
            ExpressionProcessor.RegisterCustomProjection(() => default(string).ToHierarchyId(), ProjectionsExtensionsHierarchy.ToHierarchyId);

            ExpressionProcessor.RegisterCustomMethodCall(() => default(string).IsDescendantOf(default(string)), RestrictionsExtensionsHierarchy.IsDescendantOf);
        }
    }
}
