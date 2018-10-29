using System;

namespace beaconMobile.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ItemsViewModel()
        {
            Title = "--Beacon Test--";
            Rut = "18.756.415-8";
            Nombre = "Juan Pablo Ibañez Dastres";
            Funcion = "Trabajador";

        }

        public ItemsViewModel(string t)
        {
            Tipo = t;
            Fecha = DateTime.Now.ToString();
        }

        public ItemsViewModel(string data, string prueba)
        {
            Title = data;
        }
    }


}
