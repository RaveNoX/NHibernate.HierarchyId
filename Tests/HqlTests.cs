using System.Linq;
using NHibernate.HierarchyId;
using Tests.NhibernateMappings;
using Xunit;

namespace Tests
{
    public class HqlTests : TestsBase
    {
        [Fact]
        public void IsDescendantOfTest()
        {
            using (var s = Config.SessionFactory.OpenSession())
            {
                var parent = Items.First();

                var childs = s.CreateQuery("SELECT Hid FROM HierarchyModel WHERE hid_IsDescendantOf(Hid, :hid) AND Hid != :hid")
                     .SetString("hid", parent.Key)
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

                var ancestor = s.CreateQuery("SELECT hid_GetAncestor(Hid, :level) FROM HierarchyModel WHERE Hid = :hid")
                                .SetString("hid", hid)
                                .SetInt32("level", 1)
                                .UniqueResult<string>();

                Assert.Equal("/", ancestor);
            }
        }

        [Fact]
        public void GetDescendantBetweenChildsTest()
        {
            using (var s = Config.SessionFactory.OpenSession())
            {
                var parent = Items.First();

                var descendant =
                    s.CreateQuery("SELECT hid_GetDescendant(Hid, :child1, :child2) FROM HierarchyModel WHERE Hid = :hid")
                     .SetString("hid", parent.Key)
                     .SetString("child1", parent.Value[0])
                     .SetString("child2", parent.Value[1])
                     .UniqueResult<string>();

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

                var reparented = s.CreateQuery("SELECT hid_GetReparentedValue(Hid, :old, :new) FROM HierarchyModel WHERE Hid = :hid")
                                  .SetString("hid", hid)
                                  .SetString("old", item.Key)
                                  .SetString("new", "/2/")
                                  .UniqueResult<string>();

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

                var shid = s.CreateQuery("SELECT to_string(Hid) FROM HierarchyModel WHERE Hid = :hid")
                            .SetString("hid", hid)
                            .UniqueResult<string>();                

                Assert.Equal(hid, shid);
            }
        }

        [Fact]
        public void HidParseTest()
        {
            using (var s = Config.SessionFactory.OpenSession())
            {
                var hid = Items.Select(x => x.Key).First();

                var hidValue = s.CreateQuery("SELECT Hid FROM HierarchyModel WHERE Hid = hid_Parse(:hid)")
                                .SetString("hid", hid)
                                .UniqueResult<string>();
                                    

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

                var lvl1 = s.CreateQuery("SELECT hid_GetLevel(Hid) FROM HierarchyModel WHERE Hid = :hid")
                            .SetString("hid", hid1)
                            .UniqueResult<int>();                

                Assert.Equal(1, lvl1);

                var lvl2 = s.CreateQuery("SELECT hid_GetLevel(Hid) FROM HierarchyModel WHERE Hid = :hid")
                            .SetString("hid", hid2)
                            .UniqueResult<int>();

                Assert.Equal(2, lvl2);
            }
        }
    }
}
