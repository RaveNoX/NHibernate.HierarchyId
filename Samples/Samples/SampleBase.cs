using System;

namespace Samples.Samples
{
    public abstract class SampleBase
    {
        protected readonly NHibernateConfig _Cfg;

        public SampleBase(NHibernateConfig cfg)
        {
            _Cfg = cfg;
        }

        public void Run()
        {
            Console.WriteLine("Running sample {0}", GetType().Name);
            Console.WriteLine();

            RunSample();

            Console.WriteLine();
            Console.WriteLine("Running sample {0} done", GetType().Name);
        }

        protected abstract void RunSample();
    }
}
