using DomainCore.Context;
using DomainCore.Models;
using RESTCountries.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataFillDB
{
    
    class Program
    { 
        static void Main(string[] args)
        {
            //Получим список всех стран
            var task = RESTCountriesAPI.GetAllCountriesAsync();
            while (!task.IsCompleted) { }
            List<RESTCountries.Models.Country> countries = task.Result;
            
            foreach (var country in countries)
                Console.WriteLine($"Name:{country.Name}   Code:{country.NumericCode}");
            
            //Укажем строку подключения к БД
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=CountriesDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //Заполняем данные в БД
            using (CountriesDBContext db = new CountriesDBContext(connectionString))
            {
                try
                {
                    foreach (var country in countries)
                    {
                        //Если отсутствуют данные
                        if (country.NumericCode == null || country.NumericCode == "" || country.Capital == null || country.Capital == "" || country.Region == null || country.Region == "")
                            continue;

                        //Add region

                        Region region = db.Regions.FirstOrDefault(r => r.Name == country.Region); 
                        if (region == null)
                        {
                            region = new Region()
                            {
                                Id = db.NextSequence("SEQ_Regions"),
                                Name = country.Region
                            };
                            db.Regions.Add(region);
                        }

                        //Add city
                        City city = db.Cities.FirstOrDefault(c => c.Name == country.Capital);
                        if (city == null)
                        {
                            city = new City()
                            {
                                Id = db.NextSequence("SEQ_Cities"),
                                Name = country.Capital
                            };
                            db.Cities.Add(city);
                        }

                        //Add country
                        Country c = new Country()
                        {
                            Id = db.NextSequence("SEQ_Countries"),
                            Name = country.Name,
                            Area = country.Area.GetValueOrDefault(),
                            Code = country.NumericCode + "",
                            Population = country.Population,
                            CapitalId = city.Id,
                            RegionId = region.Id
                        };
                        db.Countries.Add(c);

                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
                db.SaveChanges();
            }
        }
    }
}
