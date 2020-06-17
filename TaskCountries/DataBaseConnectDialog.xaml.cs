using System.Windows;

namespace TaskCountries
{
    /// <summary>
    /// Interaction logic for DataBaseConnectDialog.xaml
    /// </summary>
    public partial class DataBaseConnectDialog : Window
    {
        public string ConnectionString
        {
            get => tbConnectionString.Text;
            set => tbConnectionString.Text = value;
        }

        public DataBaseConnectDialog()
        {
            InitializeComponent();
        }
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
