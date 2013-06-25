using NHibernate.HierarchyId;
using Tests.NhibernateMappings;
using System.Linq;
using Xunit;

namespace Tests
{
    public class QueryOverTests : TestsBase
    {
        [Fact]
        public void IsDescendantOfTest()
        {
            using (var s = Config.SessionFactory.OpenSession())
            {
                var parent = Items.First();

                var childs = s.QueryOver<HierarchyModel>()
                              .Where(x => x.Hid.IsDescendantOf(parent.Key) && x.Hid != parent.Key)
                              .Select(x => x.Hid)
                              .List<string>();

                Assert.Equal(parent.Value, childs);
            }
        }

        [Fact]
        public void GetAncestorTest()
        {
            using (var s = Config.SessionFactory.OpenSession())
            {
                var hid = Items.Select(x => x.Key).First();

                var ancestor = s.QueryOver<HierarchyModel>()
                           .Where(x => x.Hid == hid)
                           .Select(x => x.Hid.GetAncestor(1))
                           .SingleOrDefault<string>();

                Assert.Equal("/", ancestor);
            }
        }

        [Fact]
        public void GetDescendantBetweenChildsTest()
        {
            using (var s = Config.SessionFactory.OpenSession())
            {
                var parent = Items.First();

                var descendant = s.QueryOver<HierarchyModel>()
                           .Where(x => x.Hid == parent.Key)
                           .Select(x => x.Hid.GetDescendant(parent.Value[0], parent.Value[1]))
                           .SingleOrDefault<string>();

                Assert.Equal("/1/1.1/", descendant);
            }
        }

        [Fact]
        public void GetDescendantFirstTest()
        {
            using (var s = Config.SessionFactory.OpenSession())
            {
                var parent = Items.First();

                var descendant = s.QueryOver<HierarchyModel>()
                           .Where(x => x.Hid == parent.Key)
                           .Select(x => x.Hid.GetDescendant(null, null))
                           .SingleOrDefault<string>();

                Assert.Equal("/1/1/", descendant);
            }
        }

        [Fact]
        public void GetReparentedValueTest()
        {
            using (var s = Config.SessionFactory.OpenSession())
            {
                var item = Items.First();
                var hid = item.Value[0];

                var reparented = s.QueryOver<HierarchyModel>()
                           .Where(x => x.Hid == hid)
                           .Select(x => x.Hid.GetReparentedValue(item.Key, "/2/"))
                           .SingleOrDefault<string>();

                var mustBe = hid.Replace(item.Key, "/2/");

                Assert.Equal(mustBe, reparented);
            }
        }

        [Fact]
        public void Hid2StringTest()
        {
            using (var s = Config.SessionFactory.OpenSession())
            {
                var hid = Items.Select(x => x.Key).First();

                var shid = s.QueryOver<HierarchyModel>()
                            .Where(x => x.Hid == hid)
                            .Select(x => x.Hid.SqlToString())
                            .SingleOrDefault<string>();

                Assert.Equal(hid, shid);
            }
        }

        [Fact]
        public void HidParseTest()
        {
            using (var s = Config.SessionFactory.OpenSession())
            {
                var hid = Items.Select(x => x.Key).First();

                var hidValue = s.QueryOver<HierarchyModel>()
                         .Where(x => x.Hid == hid.ToHierarchyId())
                         .Select(x => x.Hid)
                         .SingleOrDefault<string>();

                Assert.Equal(hid, hidValue);
            }
        }

        [Fact]
        public void GetLevelTest()
        {
            using (var s = Config.SessionFactory.OpenSession())
            {
                var item = Items.First();
                var hid1 = item.Key;
                var hid2 = item.Value.First();

                var lvl1 = s.QueryOver<HierarchyModel>()
                            .Where(x => x.Hid == hid1)
                            .Select(x => x.Hid.GetLevel())
                            .SingleOrDefault<int>();

                Assert.Equal(1, lvl1);

                var lvl2 = s.QueryOver<HierarchyModel>()
                            .Where(x => x.Hid == hid2)
                            .Select(x => x.Hid.GetLevel())
                            .SingleOrDefault<int>();

                Assert.Equal(2, lvl2);
            }
        }
    }
}
