using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace beaconMobile
{
    public interface IDBInterface
    {
        SQLiteConnection CreateConnection();
    }
}
