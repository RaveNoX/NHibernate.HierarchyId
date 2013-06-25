using NHibernate.Cfg;
using NHibernate.Dialect.Function;
using NHibernate.HierarchyId.Linq;
using NHibernate.Impl;

namespace NHibernate.HierarchyId
{
    public static class HierarchyIdExtensions
    {
        public static void RegisterTypes(Configuration cfg)
        {
            #region Criterion/QueryOver api
            // This is only awailable way to register Criteria/QueryOver extensions
            ExpressionProcessor.RegisterCustomProjection(() => default(string).CastAsString(), ProjectionsExtensionsHierarchy.CastAsString);
            ExpressionProcessor.RegisterCustomProjection(() => default(string).SqlToString(), ProjectionsExtensionsHierarchy.SqlToString);

            ExpressionProcessor.RegisterCustomProjection(() => default(string).GetAncestor(0), ProjectionsExtensionsHierarchy.GetAncestor);
            ExpressionProcessor.RegisterCustomProjection(() => default(string).GetDescendant(null, null), ProjectionsExtensionsHierarchy.GetDescendant);
            ExpressionProcessor.RegisterCustomProjection(() => default(string).GetReparentedValue(null, null), ProjectionsExtensionsHierarchy.GetReparentedValue);
            ExpressionProcessor.RegisterCustomProjection(() => default(string).GetLevel(), ProjectionsExtensionsHierarchy.GetLevel);
            ExpressionProcessor.RegisterCustomProjection(() => default(string).ToHierarchyId(), ProjectionsExtensionsHierarchy.ToHierarchyId);                        

            ExpressionProcessor.RegisterCustomMethodCall(() => default(string).IsDescendantOf(default(string)), RestrictionsExtensionsHierarchy.IsDescendantOf);
            #endregion

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
            cfg.SqlFunctions.Add("hid_GetLevel", new SQLFunctionTemplate(NHibernateUtil.Int16, "?1.GetLevel()"));

            // hid.GetReparentedValue(old, new)
            cfg.SqlFunctions.Add("hid_GetReparentedValue", new SQLFunctionTemplate(NHibernateUtil.String, "?1.GetReparentedValue(?2, ?3)"));            

            // hierarchyid::Parse
            cfg.SqlFunctions.Add("hid_Parse", new SQLFunctionTemplate(NHibernateUtil.String, "hierarchyid::Parse(?1)"));
            #endregion

            cfg.LinqToHqlGeneratorsRegistry<HierarchyHqlGeneratorRegistry>();            
        }        
    }
}
