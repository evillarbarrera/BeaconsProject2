using Android.App;
using Android.Bluetooth;
using Android.Widget;
using beaconMobile.Models;
using beaconMobile.Views;
using Estimotes;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.Geolocator;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Application = Xamarin.Forms.Application;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace beaconMobile
{
    public partial class App : Xamarin.Forms.Application
    {
        private const string EstimoteUuid = @"B9407F30-F5F8-466E-AFF9-25556B57FE7D";

        IBluetoothLE bluetoothBLE;
        

        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override async void OnStart()
        {
            try
            {
                var hasPermission = await CheckGeolocationPermissions(Permission.Location);

                bluetoothBLE = CrossBluetoothLE.Current;
                var status = await EstimoteManager.Instance.Initialize();

                if (status == BeaconInitStatus.BluetoothOff || status == BeaconInitStatus.BluetoothMissing)
                { 
                    BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
                    bluetoothAdapter.Enable();

                    EstimoteManager.Instance.Ranged += Instance_Ranged;
                    EstimoteManager.Instance.RegionStatusChanged += Instance_RegionStatusChanged;
                    EstimoteManager.Instance.StartRanging(new BeaconRegion("beacon_Ksec", EstimoteUuid));

                }
                else
                {
                    EstimoteManager.Instance.Ranged += Instance_Ranged;
                    EstimoteManager.Instance.RegionStatusChanged += Instance_RegionStatusChanged;
                    EstimoteManager.Instance.StartRanging(new BeaconRegion("beacon_Ksec", EstimoteUuid));

                }

            }
            catch (Exception ex)
            {
                await Current.MainPage.DisplayAlert("Atención", "La aplicacion no se puede ejecutar si no se encuentra hablilitado y/o disponible el Bluetooth", "Ok");
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();

            }
        }

        private void Instance_Ranged(object sender, IEnumerable<IBeacon> e)
        {
            try
            {
                var data = string.Empty;
                foreach (var beacon in e)
                {
                    if (beacon.Major == 5555)
                    {
                        data = $@"Hora: {DateTime.Now.TimeOfDay}
                                Major: {beacon.Major}
                                Minor: {beacon.Minor}
                                Proximity: {beacon.Proximity}";

                        //Revisar conversion fecha a unix
                        var unixDateTime = new DateTimeOffset(DateTime.Now.AddHours(-3)).ToUnixTimeSeconds();

                        DatabaseManager baseDatos = new DatabaseManager();
                        Beacon Regbeacon = new Beacon
                        {
                            ui_beacon = beacon.Uuid,
                            mayor = beacon.Major,
                            minor = beacon.Minor,
                            fecha_lectura = unixDateTime
                        };

                        baseDatos.SaveBeacon(Regbeacon);

                    }
                    else
                    {
                        Current.MainPage.DisplayAlert("Atencion", "No se a detectado un dispositivo de validación cercano, mientras este no sea detectado no podra iniciar la aplicación, acerquese a un dispositivo y vuelva a iniciar la aplicación", "OK");
                        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                    }

                    break;
                }

            }
            catch (Exception ex)
            {


            }
        }

        private void Instance_RegionStatusChanged(object sender, BeaconRegionStatusChangedEventArgs e)
        {

        }

        protected override void OnSleep()
        {
            EstimoteManager.Instance.StopMonitoring(new BeaconRegion("beacon_Ksec", EstimoteUuid));
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public class DatabaseManager
        {
            SQLiteConnection dbConnection;
            public DatabaseManager()
            {
                dbConnection = DependencyService.Get<IDBInterface>().CreateConnection();
            }

            public List<Beacon> GetAllBeacon()
            {
                return dbConnection.Query<Beacon>("Select * From [beacon] order by id desc");
            }

            public List<Beacon> GetLastBeacon()
            {
                var fechaInicial = new DateTimeOffset(DateTime.Now.AddHours(-3).AddSeconds(-10)).ToUnixTimeSeconds();
                var fechaFinal = new DateTimeOffset(DateTime.Now.AddHours(-3).AddSeconds(10)).ToUnixTimeSeconds();

                string sql = "Select * From [beacon] WHERE fecha_lectura between '" + fechaInicial + "' AND '" + fechaFinal + "'";

                return dbConnection.Query<Beacon>(sql);
            }

            public int SaveBeacon(Beacon aBeacon)
            {
                return dbConnection.Insert(aBeacon);
            }

            public int SaveAcceso(Acceso aAcceso)
            {
                return dbConnection.Insert(aAcceso);
            }

            public List<Acceso> GetAllAcceso()
            {
                return dbConnection.Query<Acceso>("Select * From [acceso] order by id desc");
            }

        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static async Task<bool> CheckGeolocationPermissions(Permission permission)
        {
            var permissionStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

            bool request = false;

            if (request || permissionStatus != PermissionStatus.Granted)
            {
                var newStatus = await CrossPermissions.Current.RequestPermissionsAsync(permission);
                if (newStatus.ContainsKey(permission) && newStatus[permission] != PermissionStatus.Granted)
                {
                    var title = "Permisos de ubicación requeridos";
                    var question = "Para usar la aplicación es necesario que active los permisos de ubicación";
                    var positive = "Abrir Configuración";
                    var negative = "Cerrar Aplicación";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();

                    var result = await task;
                    if (result)
                    {
                        CrossPermissions.Current.OpenAppSettings();
                    }
                    return false;
                }
            }

            return true;
        }
        
    }
}
