using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static beaconMobile.App;

namespace beaconMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewAcceso : ContentPage
    {
        public ObservableCollection<string> Items { get; set; }

        public ListViewAcceso()
        {
            InitializeComponent();

            DatabaseManager baseDatos = new DatabaseManager();
            ltacceso.ItemsSource = baseDatos.GetAllAcceso();

            ltacceso.RefreshCommand = new Command(() => {
                //Do your stuff.
                RefreshData();
                ltacceso.IsRefreshing = false;
            });
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                _isRefreshing = value;
                OnPropertyChanged(nameof(IsRefreshing));
            }
        }

        public void RefreshData()
        {
            DatabaseManager baseDatos = new DatabaseManager();
            ltacceso.ItemsSource = baseDatos.GetAllAcceso();
        }

    }
}
