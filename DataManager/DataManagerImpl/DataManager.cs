using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManager
{
    public class DataManager
    {
		private const string CONNECTION_STRING = "Filename=Data.db";
		private const string DEVICES_TABLE = "Devices";

		public void InitializeDatabase()
		{
			using (SqliteConnection db = new SqliteConnection(CONNECTION_STRING))
			{
				db.Open();

				string tableCommand = "CREATE TABLE IF NOT " +
					$"EXISTS {DEVICES_TABLE} (Id INTEGER NOT NULL UNIQUE PRIMARY KEY, " +
					"DeviceType INTEGER NULL, " +
					"FirmwareType INTEGER NULL, " +
					"Description NVARCHAR(2048) NULL)";

				SqliteCommand createTable = new SqliteCommand(tableCommand, db);

				createTable.ExecuteNonQuery();
			}
		}


		public void SaveDevice(DBDeviceInfo dBDeviceInfo)
		{
			using (SqliteConnection db = new SqliteConnection(CONNECTION_STRING))
			{
				db.Open();

				SqliteCommand insertDeviceCommand = new SqliteCommand();
				insertDeviceCommand.Connection = db;

				insertDeviceCommand.CommandText = $"INSERT INTO {DEVICES_TABLE} VALUES (@Id, @FirmwareType, @Description);";
				insertDeviceCommand.Parameters.AddWithValue("@Id", dBDeviceInfo.Id);
				insertDeviceCommand.Parameters.AddWithValue("@FirmwareType", dBDeviceInfo.FirmwareType);
				insertDeviceCommand.Parameters.AddWithValue("@Description", dBDeviceInfo.Description);

				insertDeviceCommand.ExecuteNonQuery();

				db.Close();
			};
		}

		public List<int> GetDevicesIDs()
		{
			List<int> iDs = new List<int>();

			using (SqliteConnection db = new SqliteConnection(CONNECTION_STRING))
			{
				db.Open();

				SqliteCommand getIDs = new SqliteCommand();
				getIDs.Connection = db;

				getIDs.CommandText = $"SELECT Id FROM {DEVICES_TABLE} ORDER BY Id";

				SqliteDataReader reader = getIDs.ExecuteReader();

				while(reader.Read())
				{
					iDs.Add(reader.GetInt32(0));
				}

				db.Close();
			}

			return iDs;
		}
	}
}
