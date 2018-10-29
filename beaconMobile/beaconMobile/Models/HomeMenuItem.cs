using System;
using System.Collections.Generic;
using System.Text;

namespace beaconMobile.Models
{
    public enum MenuItemType
    {
        Browse,
        Beacon,
        Acceso
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}
