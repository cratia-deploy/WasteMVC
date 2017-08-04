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
            UnitOfWork<SystemContext> _uow = new UnitOfWork<SystemContext>(_context);
            Random rnd = new Random();

            _uow.GetRepository<Waste>()
                .Add(new Waste
                {
                    WasteType = new WasteType
                    {
                        Description = "DESPERDICIO DE POLLO"
                    },
                    DateTime = DateTime.Now,
                    Weight = Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
                    Cost = Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
                    SalePrice = 2.0 * Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
                    Partners = new HashSet<Partner>
                    {
                        new Partner
                        {
                            Person = new Person
                            {
                                FirstName = "CARLOS",
                                LastName = "RATIA",
                            },
                            Percentage = 0.50,
                        },
                        new Partner
                        {
                            Person = new Person
                            {
                            FirstName = "PAOLA",
                            LastName = "CAMACHO",
                            },
                            Percentage = 0.50,
                        },
                    }
                });
            _uow.Commit();
            _uow.GetRepository<Waste>()
            .Add(new Waste
            {
                WasteType = new WasteType
                {
                    Description = "DESPERDICIO DE MIXTO"
                },
                DateTime = DateTime.Now,
                Weight = Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
                Cost = Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
                SalePrice = 2.0 * Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
                Partners = new HashSet<Partner>
                    {
                        new Partner
                        {
                            Person = new Person
                            {
                                FirstName = "INGRID",
                                LastName = "CAMACHO",
                            },
                            Percentage = 0.50,
                        },
                        new Partner
                        {
                            Person = new Person
                            {
                            FirstName = "IGNACIO",
                            LastName = "ALBERTO",
                            },
                            Percentage = 0.50,
                        },
                }
            });
            _uow.Commit();
            _uow.GetRepository<Waste>()
.Add(new Waste
{
    WasteType = new WasteType
    {
        Description = "DESPERDICIO DE BOVINO"
    },
    DateTime = DateTime.Now,
    Weight = Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
    Cost = Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
    SalePrice = 2.0 * Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
    Partners = new HashSet<Partner>
        {
                        new Partner
                        {
                            Person = _uow.GetRepository<Person>().Firts(),
                            Percentage = 0.60,
                        },
                        new Partner
                        {
                            Person = _uow.GetRepository<Person>().Last(),
                            Percentage = 0.40,
                        },
    }
});
            _uow.Commit();
            _uow.GetRepository<Waste>()
.Add(new Waste
{
    WasteType = _uow.GetRepository<WasteType>().Firts(),
    DateTime = DateTime.Now,
    Weight = Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
    Cost = Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
    SalePrice = 2.0 * Math.Round((double)rnd.Next(1, 10000) / (double)rnd.Next(1, 100), 2),
    Partners = new HashSet<Partner>
        {
                        new Partner
                        {
                            Person = _uow.GetRepository<Person>().Firts(),
                            Percentage = 0.70,
                        },
                        new Partner
                        {
                            Person = _uow.GetRepository<Person>().Last(),
                            Percentage = 0.30,
                        },
    }
});
            _uow.Commit();
        }
    }
}