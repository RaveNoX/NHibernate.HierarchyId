using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.HierarchyId.Projections
{
    public class ToStringMethodProjection : SimpleProjection
    {
        private readonly IProjection _projection;


        public ToStringMethodProjection(IProjection projection)
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
                .Add(".ToString()")
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
    }
}
