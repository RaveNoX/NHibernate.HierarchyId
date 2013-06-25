using FluentNHibernate.Mapping;

namespace Tests.NhibernateMappings
{
    internal sealed class HierarchyModelMapper:ClassMap<HierarchyModel>
    {
        public HierarchyModelMapper()
        {
            Id(x => x.Hid)
                .Column("hid")
                .CustomSqlType("hierarchyid")
                .GeneratedBy.Assigned();

            Map(x => x.Name)
                .Column("name")
                .Length(256)
                .Not.Nullable();
        }
    }
}
