using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.HierarchyId.Projections
{
    public class GetLevelProjection : SimpleProjection
    {
        private readonly IProjection _projection;


        public GetLevelProjection(IProjection projection)
        {
            _projection = projection;
        }

        public override SqlString ToSqlString(ICriteria criteria, int position, ICriteriaQuery criteriaQuery)
        {
            var loc = position * GetHashCode();
            var val = _projection.ToSqlString(criteria, loc, criteriaQuery);
            val = GetAncestorProjection.RemoveAsAliasesFromSql(val);

            var ret = new SqlStringBuilder()
                .Add(val)
                .Add(".GetLevel()")
                .Add(" as ")
                .Add(GetColumnAliases(position)[0])
                .ToSqlString();

            return ret;
        }

        public override SqlString ToGroupSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return _projection.ToGroupSqlString(criteria, criteriaQuery);
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
    }
}
