using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataManager
{
	public class DataSwitches : IDataSwitches
	{
		private readonly string _connectionString;

		internal DataSwitches() { }

		public DataSwitches(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IResultOperationLoad LoadDevices()
		{
			ResultOperationLoad result = new ResultOperationLoad();
			List<IDeviceInfo> devices = new List<IDeviceInfo>();

			using (SqliteConnection db = new SqliteConnection(_connectionString))
			{
				db.Open();

				string getDevices = $"SELECT * FROM {Consts.SWITCHES_TABLE}";
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
				catch(SqliteException ex)
				{
					result.ErrorText = $"{Consts.SWITCHES_TABLE} - {ex.Message}";
				}
				catch(Exception ex)
				{
					result.ErrorText = $"{Consts.SWITCHES_TABLE} - {ex.Message}";
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

			using (SqliteConnection db = new SqliteConnection(_connectionString))
			{
				db.Open();

				string rename = $"UPDATE {Consts.SWITCHES_TABLE} SET Description = '{device.Description}' WHERE Id = {device.ID}";
				SqliteCommand renameDevice = new SqliteCommand(rename, db);

				try
				{
					renameDevice.ExecuteNonQuery();
					result.Success = true;
				}
				catch (SqliteException ex)
				{
					result.ErrorText = $"{Consts.SWITCHES_TABLE} - {ex.Message}";
				}
				catch (Exception ex)
				{
					result.ErrorText = $"{Consts.SWITCHES_TABLE} - {ex.Message}";
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
			List<int> newIDs = GenerateIDs(devices, Consts.SWITCHES_TABLE);

			using (SqliteConnection db = new SqliteConnection(_connectionString))
			{
				db.Open();

				SqliteCommand insertDeviceCommand = new SqliteCommand();
				insertDeviceCommand.Connection = db;

				insertDeviceCommand.CommandText = $"INSERT INTO {Consts.SWITCHES_TABLE} VALUES (@Id, @MacAddress, @DeviceType, @FirmwareType, @Description);";
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
					result.ErrorText = $"{Consts.SWITCHES_TABLE} - {ex.Message}";
				}
				catch (Exception ex)
				{
					result.ErrorText = $"{Consts.SWITCHES_TABLE} - {ex.Message}";
				}
				finally
				{
					db.Close();
				}
			};

			return result;
		}

		private List<int> GenerateIDs(IDeviceInfo[] deviceInfos, string tableName)
		{
			List<int> newIDs = new List<int>();
			Dictionary<int, int> oldIDs = GetDevicesIDs(tableName);

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
				(deviceInfos[i]  as DeviceInfo).ID = newIDs[i];
			}

			return newIDs;
		}

		private Dictionary<int, int> GetDevicesIDs(string tableName)
		{
			Dictionary<int, int> iDs = new Dictionary<int, int>();

			using (SqliteConnection db = new SqliteConnection(_connectionString))
			{
				db.Open();

				SqliteCommand getIDs = new SqliteCommand();
				getIDs.Connection = db;

				getIDs.CommandText = $"SELECT Id FROM {tableName} ORDER BY Id";

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
