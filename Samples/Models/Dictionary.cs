using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Samples.Models
{
    public class Dictionary:IComparer<Dictionary>
    {
        public virtual int Id { get; set; }
        public virtual string Hid { get; set; }        
        public virtual string Name { get; set; }        

        public sealed class Mapper:ClassMap<Dictionary>
        {
            public Mapper()
            {                
                Table("`Dictionary`");

                Id(x => x.Id)
                    .GeneratedBy.Identity();

                // Hierarchyid column
                Map(x => x.Hid)
                    .Generated.Never()
                    .Column("HID")                    
                    .CustomSqlType("hierarchyid")
                    .Not.Nullable()
                    .Unique()
                    .Length(4000);               

                Map(x => x.Name)
                    .Length(1024)
                    .Not.Nullable();               
            }
        }

        public virtual int Compare(Dictionary x, Dictionary y)
        {
            return x.Id.CompareTo(y.Id);
        }
    }
}
