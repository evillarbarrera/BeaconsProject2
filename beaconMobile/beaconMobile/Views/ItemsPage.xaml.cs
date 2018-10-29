using beaconMobile.Models;
using beaconMobile.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static beaconMobile.App;

namespace beaconMobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
    {
        ItemsViewModel viewModel;

        public ItemsPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();

        }

        public ItemsPage(string data)
        {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel(data, "a");
             
        }

        private void ButtonEntrada_Clicked(object sender, EventArgs e)
        {

            DatabaseManager baseDatos = new DatabaseManager();
            List<Beacon> beacons = baseDatos.GetLastBeacon();

            if (beacons.Count > 0)
            {
                Acceso acceso = new Acceso();
                acceso.tipo = "Persona";
                acceso.direccion = "Entrada";
                acceso.fecha_lectura = new DateTimeOffset(DateTime.Now.AddHours(-3)).ToUnixTimeSeconds();
                acceso.nombre_centro = beacons[0].mayor.ToString();
                acceso.nombre_persona = nombre.Text;
                acceso.rut_persona = viewModel.Rut;
                acceso.funcion_persona = viewModel.Funcion;
                acceso.id_beacon = beacons[0].minor;

                baseDatos.SaveAcceso(acceso);

                List<Acceso> accesos = baseDatos.GetAllAcceso();

                PopupNavigation.Instance.PushAsync(new PopupView("Entrada"));
            }

            else {

               DisplayAlert("Atencion", "Tenemos inconvenientes, por favor volver a intentarlo en un par de minutos", "OK");
            }

        }

        private void ButtonSalida_Clicked(object sender, EventArgs e)
        {
            DatabaseManager baseDatos = new DatabaseManager();
            List<Beacon> beacons = baseDatos.GetLastBeacon();

            if (beacons.Count > 0)
            {
                Acceso acceso = new Acceso();
                acceso.tipo = "Persona";
                acceso.direccion = "Salida";
                acceso.fecha_lectura = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                acceso.nombre_centro = beacons[0].mayor.ToString();
                acceso.nombre_persona = nombre.Text;
                acceso.rut_persona = viewModel.Rut;
                acceso.funcion_persona = viewModel.Funcion;
                acceso.id_beacon = beacons[0].minor;

                baseDatos.SaveAcceso(acceso);

                List<Acceso> accesos = baseDatos.GetAllAcceso();

                PopupNavigation.Instance.PushAsync(new PopupView("Salida"));
            }

            else
            {
                DisplayAlert("Atencion", "Tenemos inconvenientes, por favor volver a intentarlo en un par de minutos", "OK");
            }

        }
    }
}