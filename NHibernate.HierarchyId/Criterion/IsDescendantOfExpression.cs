#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;
using NHibernate.Engine;
using NHibernate.SqlCommand;

#endregion

namespace NHibernate.HierarchyId.Criterion
{
    [Serializable]
    public class IsDescendantOfExpression : AbstractCriterion
    {
        private readonly IProjection _projection;
        private readonly string _propertyName;
        private readonly TypedValue _typedValue;
        private readonly string _value;

        public IsDescendantOfExpression(string propertyName, string value)
        {
            _propertyName = propertyName;
            _projection = NHibernate.Criterion.Projections.Property(_propertyName);
            _value = value;
            _typedValue = new TypedValue(NHibernateUtil.String, _value, EntityMode.Poco);
        }

        public IsDescendantOfExpression(IProjection projection, string value)
        {
            _projection = projection;
            _value = value;
            _typedValue = new TypedValue(NHibernateUtil.String, _value, EntityMode.Poco);
        }

        public override SqlString ToSqlString(ICriteria criteria, ICriteriaQuery criteriaQuery,
                                              IDictionary<string, IFilter> enabledFilters)
        {
            var columns = CriterionUtil.GetColumnNames(_propertyName, _projection, criteriaQuery, criteria,
                                                       enabledFilters);
            if (columns.Length != 1)
                throw new HibernateException(
                    "IsDescendantOf may only be used with single-column properties / projections.");

            var lhs = new SqlStringBuilder(6);

            lhs.Add(columns[0]);
            lhs.Add(".IsDescendantOf(");
            lhs.Add(criteriaQuery.NewQueryParameter(_typedValue).Single());
            lhs.Add(") = 1");

            return lhs.ToSqlString();
        }

        public override TypedValue[] GetTypedValues(ICriteria criteria, ICriteriaQuery criteriaQuery)
        {
            return new[] {_typedValue};
        }

        public override IProjection[] GetProjections()
        {
            return new[] {_projection};
        }

        public override string ToString()
        {
            return _projection + ".IsDescendantOf('" + _value + "')";
        }
    }
}