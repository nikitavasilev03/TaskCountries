using DomainCore.Context;
using DomainCore.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Windows;
using TaskCountries.ViewModels;

namespace TaskCountries
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CountriesDBContext db;

        public MainWindow()
        {
            InitializeComponent();
            ConfigureDB();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridCountryInfo.Visibility = Visibility.Hidden;
            dgridCountries.Visibility = Visibility.Hidden;
        }
        
        //Первоначальная настройка БД
        public void ConfigureDB()
        {
            //Берем строку подключения из конфига
            db = new CountriesDBContext(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }
        
        //Обработка события для поска страны
        private void btnFindCountry_Click(object sender, RoutedEventArgs e)
        {
            //Получаем строку для поиска
            string name = tbNameCountry.Text;
            
            Country country;
            Region region;
            City city;   
            try
            {
                //Ищем страну
                country = db.Countries.FirstOrDefault(c => c.Name == name);
                //Если такая страна не найдена
                if (country == null)
                {
                    MessageBox.Show("Country not found!");
                    return;
                }
                //Ищем соответствующие столицу и регион
                region = db.Regions.First(r => r.Id == country.RegionId);
                city = db.Cities.First(c => c.Id == country.CapitalId);
            }
            //Если произошла ощибка связаная с подключением к БД
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                //Вызывается диалог информирующий о проблеме с подключение к БД с возможностью изменить строку подключения
                DataBaseConnectDialog dialog = new DataBaseConnectDialog();
                dialog.ConnectionString = db.ConnectionString;
                if (dialog.ShowDialog() == true)
                    db = new CountriesDBContext(dialog.ConnectionString);
                return;
            }
            //Иная ошибка
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
                return;
            }

            //Создаем модель для представления на View
            CountryViewModel model = new CountryViewModel
            {
                Name = country.Name,
                Area = country.Area,
                Capital = city.Name,
                Code = country.Code,
                Population = country.Population,
                Region = region.Name
            };
            spanelCountryInfo.DataContext = model;
            
            gridCountryInfo.Visibility = Visibility.Visible;
        }
        //Обработка события для отображения всех стран
        private void btnShowCountries_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Получаем список стран
                var data = db.Countries.Select(country => new
                {
                    country.Name,
                    country.Code,
                    Capital = db.Cities.FirstOrDefault(c => c.Id == country.CapitalId).Name,
                    country.Area,
                    country.Population,
                    Region = db.Regions.FirstOrDefault(r => r.Id == country.RegionId).Name
                }).ToList();
                dgridCountries.ItemsSource = data;
            }
            //Если произошла ощибка связаная с подключением к БД
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                //Вызывается диалог информирующий о проблеме с подключение к БД с возможностью изменить строку подключения
                DataBaseConnectDialog dialog = new DataBaseConnectDialog();
                dialog.ConnectionString = db.ConnectionString;
                if (dialog.ShowDialog() == true)
                    db = new CountriesDBContext(dialog.ConnectionString);
                return;
            }
            //Иная ошибка
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
                return;
            }

            dgridCountries.Visibility = Visibility.Visible;
        }
        //Сохранение изменений БД
        private void btnSaveChanged_Click(object sender, RoutedEventArgs e)
        {
            //Получаем модель
            var model = (spanelCountryInfo.DataContext as CountryViewModel);
            //Проверка на валидность
            if (model == null || !model.IsValid)
            {
                MessageBox.Show("Model is not valid", "Error");
                return;
            }

            Country country;
            Region region;
            City city;
            try
            {
                //Находим страну по ее коду
                country = db.Countries.FirstOrDefault(c => c.Code == model.Code);
                //Находи регион в БД
                region = db.Regions.FirstOrDefault(r => r.Name == model.Region);
                //Находи город в БД
                city = db.Cities.FirstOrDefault(c => c.Name == model.Capital);
            }
            //Если произошла ощибка связаная с подключением к БД
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                //Вызывается диалог информирующий о проблеме с подключение к БД с возможностью изменить строку подключения
                DataBaseConnectDialog dialog = new DataBaseConnectDialog();
                dialog.ConnectionString = db.ConnectionString;
                if (dialog.ShowDialog() == true)
                    db = new CountriesDBContext(dialog.ConnectionString);
                return;
            }
            //Иная ошибка
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
                return;
            }

            //Если региона в БД нет, то создаем
            if (region == null)
            {
                region = new Region
                {
                    Id = db.NextSequence("SEQ_Regions"),
                    Name = model.Region
                };
                db.Regions.Add(region);
            }

            //Если города в БД нет, то создаем
            if (city == null)
            {
                city = new City
                {
                    Id = db.NextSequence("SEQ_Cities"),
                    Name = model.Capital
                };
                db.Cities.Add(city);
            }

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
               
            country.Name = model.Name;
            country.Population = model.Population;
            country.Area = model.Area;
            country.Code = model.Code;
            country.RegionId = region.Id;
            country.CapitalId = city.Id;

            if (newCountry)
                db.Countries.Add(country);
            else
                db.Countries.Update(country);

            db.SaveChanges();
        }
    }
}
