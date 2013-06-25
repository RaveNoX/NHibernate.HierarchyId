using System.Collections.Generic;
using NHibernate;
using Tests.NhibernateMappings;
using Xunit;

namespace Tests
{
    public abstract class TestsBase
    {
        protected NHibernateConfig Config { get; private set; }
        protected Dictionary<string, IList<string>> Items { get; private set; }


        protected TestsBase()
        {
            Items = new Dictionary<string, IList<string>>();

            Assert.DoesNotThrow(Initialize);
        }

        private void Initialize()
        {
            var connStrBuilder = new System.Data.SqlClient.SqlConnectionStringBuilder
            {
                DataSource = @"dev13-don\SANDBOX",
                InitialCatalog = "NHHierarchy",
                IntegratedSecurity = true
            };

            // Chonfigure NH
            Config = new NHibernateConfig(connStrBuilder.ConnectionString);

            // Create database structure
            Config.UpdateDatabase(true);

            // Create test entries
            using (var s = Config.SessionFactory.OpenSession())
            {
                using (var tr = s.BeginTransaction())
                {
                    PopulateData(s);

                    tr.Commit();
                }
            }
        }

        private void PopulateData(ISession s)
        {
            for (var i = 0; i < 2; i++)
            {
                var p = new HierarchyModel
                {
                    Hid = string.Format("/{0}/", i + 1),
                    Name = string.Format("Parent {0}", i + 1)
                };

                s.Save(p);

                var childs = new List<string>();

                for (var j = 0; j < 10; j++)
                {
                    var c = new HierarchyModel
                        {
                        Hid = string.Format("/{0}/{1}/", i + 1, j + 1),
                        Name = string.Format("Child {0}/{1}", i + 1, j + 1)
                    };

                    childs.Add(c.Hid);
                    s.Save(c);
                }

                Items.Add(p.Hid, childs);
            }
        }
    }
}
