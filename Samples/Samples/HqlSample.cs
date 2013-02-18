using System;
using System.Linq;

namespace Samples.Samples
{
    public class HqlSample : SampleBase
    {
        public HqlSample(NHibernateConfig cfg)
            : base(cfg)
        {
        }

        protected override void RunSample()
        {
            using (var s = _Cfg.OpenSession())
            {
                Console.WriteLine("Selecting one parent");
                var p = s.QueryOver<Models.Dictionary>()
                 .Take(1)
                 .SingleOrDefault();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("Parent: {0,3} | {1,-9} | {2}", p.Id, p.Hid, p.Name);
                Console.WriteLine();

                Console.WriteLine("Selecting parent's childs");
                var childs = s.CreateQuery("from Dictionary where hid_IsDescendantOf(Hid, :parent) and Hid != :parent")
                 .SetString("parent", p.Hid)
                 .List<Models.Dictionary>();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("Childs:");
                foreach (var c in childs)
                {
                    Console.WriteLine("\t{0,3} | {1,-9} | {2}", c.Id, c.Hid, c.Name);
                }
                Console.WriteLine();
                
                Console.WriteLine("Getting ancestor of parent");
                var anc = s.CreateQuery("select hid_GetAncestor(d.Hid, :level) as anc from Dictionary d where d.Hid = :parent ")
                           .SetString("parent", p.Hid)
                           .SetInt32("level", 1)                                                      
                           .UniqueResult<string>();


                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("Ancestor: {0}", anc);
                Console.WriteLine();


                Console.WriteLine("Getting descendant for parent between child1 and child 2");
                var desc = s.CreateQuery(
                    "select hid_GetDescendant(Hid, :child1, :child2) from Dictionary where Hid = :parent")
                            .SetString("parent", p.Hid)
                            .SetString("child1", childs[0].Hid)
                            .SetString("child2", childs[1].Hid)
                            .UniqueResult<string>();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("New child hid: {0}", desc);
                Console.WriteLine();
                
                Console.WriteLine("Getting reparented value for child1");
                var rep = s.CreateQuery("select hid_GetReparentedValue(Hid, :oldp, :newp) from Dictionary where Id = :id")
                           .SetInt64("id", childs[0].Id)
                           .SetString("oldp", "/1/")
                           .SetString("newp", "/2/")
                           .UniqueResult<string>();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("Reparented hid: {0}", rep);
                Console.WriteLine();

                Console.WriteLine("Hid to string test (this is already making by NH, but may be need in queries)");
                var shid = s.CreateQuery("select to_string(Hid) from Dictionary where Id = :id")
                            .SetInt64("id", p.Id)
                            .UniqueResult<string>();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("String hid: {0}", shid);
                Console.WriteLine();

                Console.WriteLine("Hid::Parse test");
                var hid = s.CreateQuery("select Hid from Dictionary where hid_Parse(to_string(Hid)) = :hid")
                           .SetString("hid", p.Hid)
                           .UniqueResult<string>();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("Hid: {0}", hid);
                Console.WriteLine();
            }
        }
    }
}
