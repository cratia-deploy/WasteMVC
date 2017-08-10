using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WasteMVC.Data
{
    public class Singleton
    {
        /// <summary>
        /// Patrón Singleton
        /// </summary>
        protected static Singleton instance = null;
        protected static readonly object padlock = new object();

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new Singleton();
                        }
                    }
                }
                return instance;
            }
        }

        public Singleton() { }
    }
}
