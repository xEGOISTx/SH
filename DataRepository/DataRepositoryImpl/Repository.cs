using Microsoft.Data.Sqlite;
using SH.DataPorts;

namespace SH.DataRepository
{
    public static class Repository
    {
        public static void InitializeDatabase()
        {
            using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
            {
                db.Open();

                string devicesTable = $"CREATE TABLE IF NOT EXISTS {Consts.DEVICES_TABLE} " +
                    "(DeviceID INTEGER NOT NULL UNIQUE PRIMARY KEY, " +
                    "MacAddress VARCHAR(17) NOT NULL, " +
                    "DeviceType INTEGER NOT NULL, " +
                    "Description NVARCHAR(100) NULL)";

                string commandsTable = $"CREATE TABLE IF NOT EXISTS {Consts.COMMANDS_TABLE} " +
                    "(CommandID INTEGER NOT NULL, " +
                    "DeviceID INTEGER NOT NULL, " +
                    "VoiceCommand VARCHAR(1000) NOT NULL, " +
                    "Description NVARCHAR(100) NULL, " +
                    "PRIMARY KEY (CommandID, DeviceID) " +
                    "CONSTRAINT fk_devices " +
                    "FOREIGN KEY(DeviceID) " +
                    "REFERENCES Devices(DeviceID) " +
                    "ON DELETE CASCADE) ";

                string connSettingsTable = $"CREATE TABLE IF NOT EXISTS {Consts.COON_SETTINGS_TABLE} " +
                    "(ParamIndex INTEGER NOT NULL, " +
                    "Value NVARCHAR(1000) NULL)";


                SqliteCommand createDevicesTable = new SqliteCommand(devicesTable, db);
                SqliteCommand createCommandsTable = new SqliteCommand(commandsTable, db);
                SqliteCommand createConnSettingsTable = new SqliteCommand(connSettingsTable, db);

                createDevicesTable.ExecuteNonQuery();
                createCommandsTable.ExecuteNonQuery();
                createConnSettingsTable.ExecuteNonQuery();

                createDevicesTable.Dispose();
                createCommandsTable.Dispose();
                createConnSettingsTable.Dispose();

                db.Close();
            }
        }

        public static IDataLoader DataLoader { get; } = new DataLoader();
    }
}
