using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static beaconMobile.App;

namespace beaconMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewBeacon : ContentPage
    {

        public ListViewBeacon()
        {
            InitializeComponent();

            DatabaseManager baseDatos = new DatabaseManager();
            ltbeacon.ItemsSource = baseDatos.GetAllBeacon();

            ltbeacon.RefreshCommand = new Command(() => {
                //Do your stuff.
                RefreshData();
                ltbeacon.IsRefreshing = false;
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
            ltbeacon.ItemsSource = baseDatos.GetAllBeacon();
        }

    }
}
