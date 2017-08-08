using System;
using System.Collections.Generic;
using WasteMVC.Models;

namespace WasteMVC.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SystemContext _context)
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            UnitOfWork<SystemContext> uow = new UnitOfWork<SystemContext>(_context);
            Random rnd = new Random();
            ///
            /// CREANDO LOS TIPOS DE DESPERDICIOS
            /// 
            List<WasteType> wt = new List<WasteType>{
                new WasteType
                {
                    Description = "DESPERDICIO DE BOVINO"
                },
                new WasteType
                {
                    Description = "DESPERDICIO DE POLLO"
                },
                new WasteType
                {
                    Description = "DESPERDICIO DE MIXTO"
                },
            };
            ///
            /// CREANDO LOS SOCIO
            /// 
            List<Person> p = new List<Person>
            {
                new Person
                {
                     FirstName ="CARLOS ALBERTO",
                     LastName = "RATIA VILORIA"
                },
                new Person
                {
                     FirstName ="INGRID",
                     LastName = "CAMACHO"
                },
                new Person
                {
                     FirstName ="PAOLA ANDREINA",
                     LastName = "CAMACHO MARQUEZ"
                },
                new Person
                {
                     FirstName ="IGNACIO ALBERTO",
                     LastName = "RATIA CAMACHO"
                },
            };
            ///
            /// SALVANDO NUEVOS OBJETOS
            /// 
            uow.GetRepository<WasteType>().Add(wt);
            uow.GetRepository<Person>().Add(p);
            uow.Commit();

            ///
            /// AGREGANDO DESPERDICIOS
            /// 

            int count_p = p.Count;
            int count_wt = wt.Count;
            List<Waste> ws = new List<Waste>();
            double _Cost = 0.00;
            int p1;
            int p2;
            double Percentage1;
            double Percentage2;

            for (int i = 0; i < 150; i++)
            {
                _Cost = Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2);
                p1 = rnd.Next(1, count_p);
                p2 = ((p1+1) % count_p) + 1;
                Percentage1 = Math.Round((1.00 / (double)rnd.Next(1, 10)), 2);
                Percentage2 = Math.Round((1.00 - Percentage1),2);
                ws.Add(new Waste
                {
                    WasteType = uow.GetRepository<WasteType>().Find(rnd.Next(1,count_wt+1)),
                    DateTime = DateTime.Now.AddDays(-1 * (i % 30)).Date,
                    Weight = Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
                    Cost = _Cost,
                    SalePrice = 2.0 * _Cost,
                    Partners = new HashSet<Partner>
                    {
                        new Partner
                        {
                            Person = uow.GetRepository<Person>().Find(p1),
                            Percentage = Percentage1,
                        },
                        new Partner
                        {
                            Person = uow.GetRepository<Person>().Find(p2),
                            Percentage = Percentage2,
                        }
                    },
                });
            }
            uow.GetRepository<Waste>().Add(ws);
            uow.Commit();
        }
    }
}