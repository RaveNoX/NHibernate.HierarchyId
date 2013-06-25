using System.Collections.Generic;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.HierarchyId.Projections
{
    public class GetLevelProjection : SimpleProjection
    {
        private readonly IProjection _projection;


        public GetLevelProjection(IProjection projection)
        {
            _projection = projection;
        }

        public override SqlString ToSqlString(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
        {
            var loc = position * GetHashCode();
            var val = _projection.ToSqlString(criteria, loc, criteriaQuery, enabledFilters);
            val = StringHelper.RemoveAsAliasesFromSql(val);

            var ret = new SqlStringBuilder()
                .Add(val)
                .Add(".GetLevel()")
                .Add(" as ")
                .Add(GetColumnAliases(position)[0])
                .ToSqlString();

            return ret;
        }

        public override IType[] GetTypes(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return new IType[] { NHibernateUtil.Int32 };
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
