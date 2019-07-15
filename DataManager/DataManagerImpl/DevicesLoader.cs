using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SHToolKit;
using SHToolKit.DataManagement;

namespace DataManager
{
	public class DevicesLoader : IDevicesLoader
	{
		public IOperationResultDevicesLoad LoadDevices(int devicesType)
		{
			OperationResultDevicesLoad result = new OperationResultDevicesLoad();
			List<IDevice> devices = new List<IDevice>();

			using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
			{
				db.Open();

				string getDevices = $"SELECT * FROM {Consts.DEVICES_TABLE} WHERE DeviceType = {devicesType}";
				SqliteCommand command = new SqliteCommand(getDevices, db);

				try
				{
					SqliteDataReader query = command.ExecuteReader();

					while (query.Read())
					{
						Device device = new Device
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
					result.Devices = devices.ToArray();
				}
				catch (Exception ex)
				{
					result.ErrorMessage = $"{Consts.DEVICES_TABLE} - {ex.Message}";
				}
				finally
				{
					db.Close();
				}
			}

			return result;
		}

		public IOperationResultSaveDevices SaveDevices(IDevice[] devices)
		{
			OperationResultSaveDevices result = new OperationResultSaveDevices();
			List<int> newIDs = GenerateIDs(devices);

			using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
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
					foreach (IDevice device in devices)
					{
						insertDeviceCommand.Parameters[0].Value = device.ID;
						insertDeviceCommand.Parameters[1].Value = device.MacAddress;
						insertDeviceCommand.Parameters[2].Value = device.DeviceType;
						insertDeviceCommand.Parameters[3].Value = device.FirmwareType;
						insertDeviceCommand.Parameters[4].Value = device.Description;

						insertDeviceCommand.ExecuteNonQuery();
					}

					result.Success = true;
					result.DevicesIDs = newIDs.ToArray();
				}
				catch (Exception ex)
				{
					result.ErrorMessage = $"{Consts.DEVICES_TABLE} - {ex.Message}";
				}
				finally
				{
					db.Close();
				}
			};

			return result;
		}

		private List<int> GenerateIDs(IDevice[] deviceInfos)
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
				(deviceInfos[i] as Device).ID = newIDs[i];
			}

			return newIDs;
		}

		private Dictionary<int, int> GetDevicesIDs()
		{
			Dictionary<int, int> iDs = new Dictionary<int, int>();

			using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
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
