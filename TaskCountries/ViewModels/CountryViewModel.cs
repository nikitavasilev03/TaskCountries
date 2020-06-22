using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskCountries.ViewModels
{
    //Класс для представления страны на View
    public class CountryViewModel : INotifyPropertyChanged
    {
        private string name;
        private string code;
        private string capital;
        private double area;
        private int population;
        private string region;

        public CountryViewModel()
        {

        }
        public CountryViewModel(RESTCountries.Models.Country country)
        {
            name = country.Name;
            area = country.Area.GetValueOrDefault();
            Capital = country.Capital + "";
            Code = country.NumericCode;
            Population = country.Population;
            Region = country.Region + "";
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }

        public string Capital
        {
            get { return capital; }
            set
            {
                capital = value;
                OnPropertyChanged("Capital");
            }
        }
        public double Area
        {
            get { return area; }
            set
            {
                area = value;
                OnPropertyChanged("Area");
            }
        }
        public int Population
        {
            get { return population; }
            set
            {
                population = value;
                OnPropertyChanged("Population");
            }
        }
        public string Region
        {
            get { return region; }
            set
            {
                region = value;
                OnPropertyChanged("Region");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        //Проверка на валидность
        public bool IsValid()
        {
            if (name == "" || code == "" || region == "" || capital == "")
                return false;
            return true;
        }
    }
}
