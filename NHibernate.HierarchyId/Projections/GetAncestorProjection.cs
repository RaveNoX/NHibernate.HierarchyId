using System.Linq;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.SqlCommand;
using NHibernate.Type;

namespace NHibernate.HierarchyId.Projections
{
    public class GetAncestorProjection : SimpleProjection
    {
        private readonly IProjection _projection;
        private readonly int _level;        

        public GetAncestorProjection(IProjection projection, int level)
        {
            _projection = projection;
            _level = level;            
        }

       
        public override SqlString ToSqlString(ICriteria criteria, int position, ICriteriaQuery criteriaQuery)
        {
            var loc = position * GetHashCode();
            var val = _projection.ToSqlString(criteria, loc, criteriaQuery);
            val = RemoveAsAliasesFromSql(val);

            var lhs = new SqlStringBuilder();

            lhs.Add(val);
            lhs.Add(".GetAncestor(");
            lhs.Add(criteriaQuery.NewQueryParameter(new TypedValue(NHibernateUtil.Int32, _level,false)).Single());
            lhs.Add(") as ");
            lhs.Add(GetColumnAliases(position)[0]);

            var ret = lhs.ToSqlString();

            return ret;
        }
        public static SqlString RemoveAsAliasesFromSql(SqlString sql)
        {
            return sql.Substring(0, sql.LastIndexOfCaseInsensitive(" as "));
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
