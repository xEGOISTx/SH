using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataManager
{
	public class DataManager : IDataManager
    {
		private const string CONNECTION_STRING = "Filename=Data.db";

		public void InitializeDatabase()
		{
			using (SqliteConnection db = new SqliteConnection(CONNECTION_STRING))
			{
				db.Open();

				string tableCommand = "CREATE TABLE IF NOT " +
					$"EXISTS {Consts.DEVICES_TABLE} (Id INTEGER NOT NULL UNIQUE PRIMARY KEY, " +
					"MacAddress VARCHAR(17) NOT NULL, " +
					"DeviceType INTEGER NOT NULL, " +
					"FirmwareType INTEGER NOT NULL, " +
					"Description NVARCHAR(100) NULL)";

				SqliteCommand createTable = new SqliteCommand(tableCommand, db);

				createTable.ExecuteNonQuery();
			}
		}

		public IDeviceInfo CreateDeviceInfo(string description, int deviceType, int firmwareType, string macAddres, int id = 0)
		{
			return new DeviceInfo
			{
				Description = description,
				DeviceType = deviceType,
				FirmwareType = firmwareType,
				MacAddress = macAddres
			};
		}

		public IResultOperationLoad LoadDevices(int deviceType)
		{
			ResultOperationLoad result = new ResultOperationLoad();
			List<IDeviceInfo> devices = new List<IDeviceInfo>();

			using (SqliteConnection db = new SqliteConnection(CONNECTION_STRING))
			{
				db.Open();

				string getDevices = $"SELECT * FROM {Consts.DEVICES_TABLE} WHERE DeviceType = {deviceType}";
				SqliteCommand command = new SqliteCommand(getDevices, db);

				try
				{
					SqliteDataReader query = command.ExecuteReader();

					while (query.Read())
					{
						DeviceInfo device = new DeviceInfo
						{
							ID = query.GetInt32(0),
							MacAddress = query.GetString(1),
							DeviceType = query.GetInt32(2),
							FirmwareType = query.GetInt32(3),
							Description = query.GetString(4)
						};

						devices.Add(device);
					}

					result.Success = true;
					result.DeviceInfos = devices.ToArray();
				}
				catch (SqliteException ex)
				{
					result.ErrorText = $"{Consts.DEVICES_TABLE} - {ex.Message}";
				}
				catch (Exception ex)
				{
					result.ErrorText = $"{Consts.DEVICES_TABLE} - {ex.Message}";
				}
				finally
				{
					db.Close();
				}
			}

			return result;
		}

		public IDBOperationResult RenameDevice(IDeviceInfo device)
		{
			DBOperationResult result = new DBOperationResult();

			using (SqliteConnection db = new SqliteConnection(CONNECTION_STRING))
			{
				db.Open();

				string rename = $"UPDATE {Consts.DEVICES_TABLE} SET Description = '{device.Description}' WHERE Id = {device.ID}";
				SqliteCommand renameDevice = new SqliteCommand(rename, db);

				try
				{
					renameDevice.ExecuteNonQuery();
					result.Success = true;
				}
				catch (SqliteException ex)
				{
					result.ErrorText = $"{Consts.DEVICES_TABLE} - {ex.Message}";
				}
				catch (Exception ex)
				{
					result.ErrorText = $"{Consts.DEVICES_TABLE} - {ex.Message}";
				}
				finally
				{
					db.Close();
				}
			}

			return result;
		}

		public IResultOperationSave SaveDevices(IDeviceInfo[] devices)
		{
			ResultOperationSave result = new ResultOperationSave();
			List<int> newIDs = GenerateIDs(devices);

			using (SqliteConnection db = new SqliteConnection(CONNECTION_STRING))
			{
				db.Open();

				SqliteCommand insertDeviceCommand = new SqliteCommand();
				insertDeviceCommand.Connection = db;

				insertDeviceCommand.CommandText = $"INSERT INTO {Consts.DEVICES_TABLE} VALUES (@Id, @MacAddress, @DeviceType, @FirmwareType, @Description);";
				insertDeviceCommand.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer));
				insertDeviceCommand.Parameters.Add(new SqliteParameter("@MacAddress", SqliteType.Text));
				insertDeviceCommand.Parameters.Add(new SqliteParameter("@DeviceType", SqliteType.Integer));
				insertDeviceCommand.Parameters.Add(new SqliteParameter("@FirmwareType", SqliteType.Integer));
				insertDeviceCommand.Parameters.Add(new SqliteParameter("@Description", SqliteType.Text));

				try
				{
					foreach (IDeviceInfo deviceInfo in devices)
					{
						insertDeviceCommand.Parameters[0].Value = deviceInfo.ID;
						insertDeviceCommand.Parameters[1].Value = deviceInfo.MacAddress;
						insertDeviceCommand.Parameters[2].Value = deviceInfo.DeviceType;
						insertDeviceCommand.Parameters[3].Value = deviceInfo.FirmwareType;
						insertDeviceCommand.Parameters[4].Value = deviceInfo.Description;

						insertDeviceCommand.ExecuteNonQuery();

					}

					result.Success = true;
					result.NewIDs = newIDs.ToArray();
				}
				catch (SqliteException ex)
				{
					result.ErrorText = $"{Consts.DEVICES_TABLE} - {ex.Message}";
				}
				catch (Exception ex)
				{
					result.ErrorText = $"{Consts.DEVICES_TABLE} - {ex.Message}";
				}
				finally
				{
					db.Close();
				}
			};

			return result;
		}

		private List<int> GenerateIDs(IDeviceInfo[] deviceInfos)
		{
			List<int> newIDs = new List<int>();
			Dictionary<int, int> oldIDs = GetDevicesIDs();

			int howManyIDs = deviceInfos.Length;

			//если записи не первые
			if (oldIDs.Count > 0)
			{
				for (int i = 1; i < oldIDs.Last().Value; i++)
				{
					if (newIDs.Count != howManyIDs)
					{
						if (!oldIDs.ContainsKey(i))
						{
							newIDs.Add(i);
						}
					}
					else
					{
						break;
					}
				}
			}
			else
			{
				newIDs.Add(1);
			}

			//если не добавленно необходимое кол-во IDs
			if (newIDs.Count != howManyIDs)
			{
				//сколько осталось добавить
				int leftAdd = howManyIDs - newIDs.Count;

				//последний id
				int lastIdItem = oldIDs.Count > 0 ? oldIDs.Values.Last() : newIDs.Last();

				for (int i = 1; i <= leftAdd; i++)
				{
					int id = lastIdItem + i;
					newIDs.Add(id);
				}
			}

			for (int i = 0; i < deviceInfos.Length; i++)
			{
				(deviceInfos[i] as DeviceInfo).ID = newIDs[i];
			}

			return newIDs;
		}

		private Dictionary<int, int> GetDevicesIDs()
		{
			Dictionary<int, int> iDs = new Dictionary<int, int>();

			using (SqliteConnection db = new SqliteConnection(CONNECTION_STRING))
			{
				db.Open();

				SqliteCommand getIDs = new SqliteCommand();
				getIDs.Connection = db;

				getIDs.CommandText = $"SELECT Id FROM {Consts.DEVICES_TABLE} ORDER BY Id";

				SqliteDataReader reader = getIDs.ExecuteReader();

				while (reader.Read())
				{
					iDs.Add(reader.GetInt32(0), reader.GetInt32(0));
				}

				db.Close();
			}

			return iDs;
		}


	}
}


//IDeviceInfo[] devices = new IDeviceInfo[]
//{
//	new DeviceInfo
//	{
//		Description = "a",
//		DeviceType = 1,
//		FirmwareType = 1,
//		MacAddress = "mac"
//	}
//};


//IResultOperationSave resultS = Switches.SaveDevices(devices);

//IResultOperationLoad resultL = Switches.LoadDevices();

//(resultL.DeviceInfos[0] as DeviceInfo).Description = "aaa";

//IDBOperationResult resultR = Switches.RenameDevice(resultL.DeviceInfos[0]);