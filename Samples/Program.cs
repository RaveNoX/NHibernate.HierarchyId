using System;
using System.Collections.Generic;
using Samples.Samples;

namespace Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Adjusting console buffers...");
            Console.SetBufferSize(200, 2000);

            Console.WriteLine("Preparing NH config...");
            var cfg = PrepareConfig();

            Console.WriteLine("Updating database...");
            cfg.UpdateDatabase(true);

            Console.WriteLine("Populating data...");
            PopulateData(cfg);

            Console.WriteLine("Rdy, press [Enter] for run samples");
            Console.ReadLine();


            var samples = new List<SampleBase> { new HqlSample(cfg), new LinqSample(cfg), new QueryOverSample(cfg) };

            for (var i = 0;i<samples.Count;i++)
            {
                samples[i].Run();

                if (i != samples.Count - 1)
                {
                    Console.WriteLine("Press [Enter] to run next sample");
                    Console.ReadLine();
                }
            }


            Console.WriteLine();
            Console.WriteLine("All done, press [Enter] to exit");
            Console.ReadLine();
        }


        static NHibernateConfig PrepareConfig()
        {
            var cb = new System.Data.SqlClient.SqlConnectionStringBuilder
                {
                    DataSource = @"dev13-don\sandbox",
                    InitialCatalog = "NHHierarchy",
                    IntegratedSecurity = true
                };


            return new NHibernateConfig(cb.ConnectionString);
        }

        static void PopulateData(NHibernateConfig cfg)
        {
            using (var s = cfg.OpenSession())
            {
                using (var tr = s.BeginTransaction())
                {
                    for (var i = 0; i < 2; i++)
                    {
                        var p = new Models.Dictionary
                            {
                                Hid = string.Format("/{0}/", i + 1),
                                Name = string.Format("Parent {0}", i + 1)
                            };

                        s.Save(p);

                        for (var j = 0; j < 10; j++)
                        {
                            var c = new Models.Dictionary
                                {
                                    Hid = string.Format("/{0}/{1}/", i + 1, j + 1),
                                    Name = string.Format("Child {0}/{1}", i + 1, j + 1)
                                };

                            s.Save(c);
                        }
                    }

                    tr.Commit();
                }
            }
        }
    }
}
