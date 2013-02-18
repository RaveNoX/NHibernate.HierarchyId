using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.HierarchyId.Projections
{
    public class GetDescendantProjection : SimpleProjection
    {
        private readonly IProjection _projection;
        private readonly string _child1, _child2;

        public GetDescendantProjection(IProjection projection, string child1, string child2)
        {
            _projection = projection;
            _child1 = child1;
            _child2 = child2;
        }

        public override SqlString ToSqlString(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
        {
            var loc = position * GetHashCode();
            var val = _projection.ToSqlString(criteria, loc, criteriaQuery, enabledFilters);
            val = StringHelper.RemoveAsAliasesFromSql(val);

            var lhs = new SqlStringBuilder();

            lhs.Add(val);
            lhs.Add(".GetDescendant(");
            lhs.Add(criteriaQuery.NewQueryParameter(new TypedValue(NHibernateUtil.String, _child1, EntityMode.Poco)).Single());
            lhs.Add(" , ");
            lhs.Add(criteriaQuery.NewQueryParameter(new TypedValue(NHibernateUtil.String, _child2, EntityMode.Poco)).Single());
            lhs.Add(") as ");
            lhs.Add(GetColumnAliases(position)[0]);

            var ret =  lhs.ToSqlString();

            return ret;
        }

        public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return new IType[] { NHibernateUtil.String };
        }

        public override bool IsGrouped
        {
            get { return _projection.IsGrouped; }
        }

        public override bool IsAggregate
        {
            get { return false; }
        }

        public override SqlString ToGroupSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
        {
            return _projection.ToGroupSqlString(criteria, criteriaQuery, enabledFilters);
        }
    }
}
