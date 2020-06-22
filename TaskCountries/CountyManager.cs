using DomainCore.Context;
using DomainCore.Models;
using Microsoft.EntityFrameworkCore;
using RESTCountries.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskCountries.ViewModels;

namespace TaskCountries
{
    public class CountyManager
    {
        CountriesDBContext db = null;

        public CountriesDBContext DB { get => db; }

        public CountyManager(CountriesDBContext context)
        {
            db = context;
        }
        //Поиск страны по совпадениям по имени через RESTCountriesAPI
        public async Task<CountryViewModel> FindAPI(string name)
        {
            //Получим список всех стран
            var countries = await RESTCountriesAPI.GetCountriesByNameContainsAsync(name);
            var country = countries.FirstOrDefault();
            if (country == null)
                return null;
            return new CountryViewModel(country);
        }

        //Получить все страны из БД
        public async Task<List<CountryViewModel>> GetAllCountriesAsync()
        {

            return await Task.Run(() =>
            {
                try
                {
                    return db.Countries.Select(country => new CountryViewModel
                    {
                        Name = country.Name,
                        Code = country.Code,
                        Capital = db.Cities.FirstOrDefault(c => c.Id == country.CapitalId).Name,
                        Area = country.Area,
                        Population = country.Population,
                        Region = db.Regions.FirstOrDefault(r => r.Id == country.RegionId).Name
                    }).ToList();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        //Найти или создать регион
        public async Task<Region> FindOrCreateRegionAsync(string name)
        {
            //Находи регион в БД
            var region = await db.Regions.FirstOrDefaultAsync(r => r.Name == name);
            //Если региона в БД нет, то создаем
            if (region == null)
            {
                region = new Region
                {
                    Id = db.NextSequence("SEQ_Regions"),
                    Name = name
                };
                db.Regions.Add(region);
            }
            //Сохраняем изменения
            await db.SaveChangesAsync();
            return region;
        }

        //Найти или создать город
        public async Task<City> FindOrCreateCityAsync(string name)
        {
            //Находи город в БД
            var city = await db.Cities.FirstOrDefaultAsync(c => c.Name == name);
            //Если города в БД нет, то создаем
            if (city == null)
            {
                city = new City
                {
                    Id = db.NextSequence("SEQ_Cities"),
                    Name = name
                };
                db.Cities.Add(city);
            }
            //Сохраняем изменения
            await db.SaveChangesAsync();
            return city;
        }

        public async Task CreateOrUpdateCountryAsync(Region region, City capital, CountryViewModel model)
        {
            //Находим страну по ее коду
            var country = await db.Countries.FirstOrDefaultAsync(c => c.Code == model.Code);
            //Пердположим что страна уже есть в БД
            bool newCountry = false;
            //Если страны нет в БД, то создаем
            if (country == null)
            {
                country = new Country
                {
                    Id = db.NextSequence("SEQ_Countries"),
                };
                //Новая страна
                newCountry = true;
            }

            //Сохраняем новый данные
            country.Name = model.Name;
            country.Population = model.Population;
            country.Area = model.Area;
            country.Code = model.Code;
            country.RegionId = region.Id;
            country.CapitalId = capital.Id;

            //Если страна новая то
            if (newCountry)
                //То добавляем
                db.Countries.Add(country);
            else
                //Если нет, то обновляем
                db.Countries.Update(country);

            await db.SaveChangesAsync();
        }

        //Переподключение к БД
        public void ReconnectToDB(string connectionStr)
        {
            db = new CountriesDBContext(connectionStr);
        }
    }
}
