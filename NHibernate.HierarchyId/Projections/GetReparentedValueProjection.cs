using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;
using NHibernate.Util;

namespace NHibernate.HierarchyId.Projections
{
    public class GetReparentedValueProjection : SimpleProjection
    {
        private readonly IProjection _projection;
        private readonly string _oldRoot, _newRoot;

        public GetReparentedValueProjection(IProjection projection, string oldRoot, string newRoot)
        {
            _projection = projection;
            _oldRoot = oldRoot;
            _newRoot = newRoot;
        }

        public override SqlString ToSqlString(ICriteria criteria, int position, ICriteriaQuery criteriaQuery, IDictionary<string, IFilter> enabledFilters)
        {
            var loc = position * GetHashCode();
            var val = _projection.ToSqlString(criteria, loc, criteriaQuery, enabledFilters);
            val = StringHelper.RemoveAsAliasesFromSql(val);

            var lhs = new SqlStringBuilder();

            lhs.Add(val);
            lhs.Add(".GetReparentedValue(");
            lhs.Add(criteriaQuery.NewQueryParameter(new TypedValue(NHibernateUtil.String, _oldRoot, EntityMode.Poco)).Single());
            lhs.Add(" , ");
            lhs.Add(criteriaQuery.NewQueryParameter(new TypedValue(NHibernateUtil.String, _newRoot, EntityMode.Poco)).Single());
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
