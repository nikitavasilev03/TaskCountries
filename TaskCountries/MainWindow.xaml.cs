using DomainCore.Context;
using System;
using System.Configuration;
using System.Windows;
using TaskCountries.ViewModels;

namespace TaskCountries
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Объект для даботы со странами в БД
        private CountyManager cm;
        public MainWindow()
        {
            InitializeComponent();
            cm = new CountyManager(new CountriesDBContext(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridCountryInfo.Visibility = Visibility.Hidden;
            dgridCountries.Visibility = Visibility.Hidden;
        }
        
        //Обработка события для поска страны
        private async void btnFindCountry_Click(object sender, RoutedEventArgs e)
        {
            //Проверяем на есть ли в тексбоксе хоть что
            if (tbNameCountry.Text == "")
            {
                MessageBox.Show("Input country name");
                return;
            }
            
            try
            {
                //Пытаемся найти страну и поместить ее на форму
                var country = await cm.FindAPI(tbNameCountry.Text);
                spanelCountryInfo.DataContext = country;
            }
            catch (AppREstCountries.Helpers.CountryNotFoundException)
            {
                //Страна не найдена
                MessageBox.Show("Country not found");
                return;
            }
            catch (Exception ex)
            {
                //Иная ошибка
                MessageBox.Show(ex.Message, "Error!");
                return;
            }
            
            //Включаем форму для страны
            gridCountryInfo.Visibility = Visibility.Visible;
        }

        //Обработка события для отображения всех стран
        private async void btnShowCountries_Click(object sender, RoutedEventArgs e)
        {
            btnShowCountries.IsEnabled = false;

            //Получаем список стран
            var countries = await cm.GetAllCountriesAsync();
            if (countries == null)
            {
                //Вызывается диалог информирующий о проблеме с подключение к БД с возможностью изменить строку подключения
                ReconnectToDBDialog();
                btnShowCountries.IsEnabled = true;
                return;
            }

            dgridCountries.ItemsSource = countries;
            btnShowCountries.IsEnabled = true;
            dgridCountries.Visibility = Visibility.Visible;
        }
        //Сохранение изменений БД
        private async void btnSaveChanged_Click(object sender, RoutedEventArgs e)
        {
            btnSaveChanged.IsEnabled = false;

            //Получаем модель
            var model = (spanelCountryInfo.DataContext as CountryViewModel);
            //Проверка на валидность
            if (model == null || !model.IsValid())
            {
                MessageBox.Show("Model is not valid", "Error");
                return;
            }

            try
            {
                //Находи регион в БД
                var region = await cm.FindOrCreateRegionAsync(model.Region);
                //Находи город в БД
                var capital = await cm.FindOrCreateCityAsync(model.Capital);
                //Созадем или обновляем страну
                await cm.CreateOrUpdateCountryAsync(region, capital, model);
            }
            //Если произошла ощибка связаная с подключением к БД
            catch (Microsoft.Data.SqlClient.SqlException)
            {
                //Вызывается диалог информирующий о проблеме с подключение к БД с возможностью изменить строку подключения
                ReconnectToDBDialog();
                btnSaveChanged.IsEnabled = true;
                return;
            }
            //Иная ошибка
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
                btnSaveChanged.IsEnabled = true;
                return;
            }

            btnSaveChanged.IsEnabled = true;
        }

        //Вызывается диалог информирующий о проблеме с подключение к БД с возможностью изменить строку подключения
        public void ReconnectToDBDialog()
        {
            DataBaseConnectDialog dialog = new DataBaseConnectDialog();
            dialog.ConnectionString = cm.DB.ConnectionString;
            if (dialog.ShowDialog() == true)
                cm.ReconnectToDB(dialog.ConnectionString);
        }
    }
}
