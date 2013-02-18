using System;
using NHibernate.HierarchyId;

namespace Samples.Samples
{
    public class QueryOverSample : SampleBase
    {
        public QueryOverSample(NHibernateConfig cfg)
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
                var childs = s.QueryOver<Models.Dictionary>()
                              .Where(x => x.Hid.IsDescendantOf(p.Hid) && x.Hid != p.Hid)
                              .List();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("Childs:");
                foreach (var c in childs)
                {
                    Console.WriteLine("\t{0,3} | {1,-9} | {2}", c.Id, c.Hid, c.Name);
                }
                Console.WriteLine();
                
                Console.WriteLine("Getting ancestor of parent");
                var anc = s.QueryOver<Models.Dictionary>()
                           .Where(x => x.Hid == p.Hid)
                           .Select(x => x.Hid.GetAncestor(1))
                           .SingleOrDefault<string>();


                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("Ancestor: {0}", anc);
                Console.WriteLine();


                Console.WriteLine("Getting descendant for parent between child1 and child 2");
                var desc = s.QueryOver<Models.Dictionary>()
                            .Where(x => x.Hid == p.Hid)
                            .Select(x => x.Hid.GetDescendant(childs[0].Hid, childs[1].Hid))
                            .SingleOrDefault<string>();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("New child hid: {0}", desc);
                Console.WriteLine();
                
                Console.WriteLine("Getting reparented value for child1");
                var rep = s.QueryOver<Models.Dictionary>()
                           .Where(x => x.Id == childs[0].Id)
                           .Select(x => x.Hid.GetReparentedValue("/1/", "/2/"))
                           .SingleOrDefault<string>();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("Reparented hid: {0}", rep);
                Console.WriteLine();

                Console.WriteLine("Hid to string test (this is already making by NH, but may be need in queries)");
                var shid = s.QueryOver<Models.Dictionary>()
                            .Where(x => x.Id == p.Id)
                            .Select(x => x.Hid.SqlToString())
                            .SingleOrDefault<string>();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("String hid: {0}", shid);
                Console.WriteLine();

                Console.WriteLine("Hid::Parse test");
                var hid = s.QueryOver<Models.Dictionary>()
                           .Where(x => x.Hid.SqlToString().ToHierarchyId() == p.Hid)
                           .Select(x => x.Hid)
                           .SingleOrDefault<string>();

                Console.WriteLine("-------------------------------------------------------------------");
                Console.WriteLine("Hid: {0}", hid);
                Console.WriteLine();                 
            }
        }
    }
}
