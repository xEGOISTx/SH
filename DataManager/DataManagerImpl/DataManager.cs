using Microsoft.Data.Sqlite;

namespace DataManager
{
	public class DataManager
    {
		private const string CONNECTION_STRING = "Filename=Data.db";

		public void InitializeDatabase()
		{
			using (SqliteConnection db = new SqliteConnection(CONNECTION_STRING))
			{
				db.Open();

				string tableCommand = "CREATE TABLE IF NOT " +
					$"EXISTS {Consts.SWITCHES_TABLE} (Id INTEGER NOT NULL UNIQUE PRIMARY KEY, " +
					"MacAddress VARCHAR(17) NOT NULL, " +
					"DeviceType INTEGER NOT NULL, " +
					"FirmwareType INTEGER NOT NULL, " +
					"Description NVARCHAR(100) NULL)";

				SqliteCommand createTable = new SqliteCommand(tableCommand, db);

				createTable.ExecuteNonQuery();
			}
		}

		public IDataSwitches Switches
		{
			get { return new DataSwitches(CONNECTION_STRING); }
		}



		//public IResultOperationSave SaveSwitches(DeviceInfo[] deviceInfos)
		//{
		//	ResultOperationSave result = new ResultOperationSave();
		//	List<int> newIDs = GenerateIDs(deviceInfos, SWITCHES_TABLE);
			
		//	using (SqliteConnection db = new SqliteConnection(CONNECTION_STRING))
		//	{
		//		db.Open();

		//		SqliteCommand insertDeviceCommand = new SqliteCommand();
		//		insertDeviceCommand.Connection = db;

		//		insertDeviceCommand.CommandText = $"INSERT INTO {SWITCHES_TABLE} VALUES (@Id, @MacAddress, @DeviceType, @FirmwareType, @Description);";
		//		insertDeviceCommand.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer));
		//		insertDeviceCommand.Parameters.Add(new SqliteParameter("@MacAddress", SqliteType.Text));
		//		insertDeviceCommand.Parameters.Add(new SqliteParameter("@DeviceType", SqliteType.Integer));
		//		insertDeviceCommand.Parameters.Add(new SqliteParameter("@FirmwareType", SqliteType.Integer));
		//		insertDeviceCommand.Parameters.Add(new SqliteParameter("@Description", SqliteType.Text));

		//		try
		//		{
		//			foreach (DeviceInfo deviceInfo in deviceInfos)
		//			{
		//				insertDeviceCommand.Parameters[0].Value = deviceInfo.Id;
		//				insertDeviceCommand.Parameters[1].Value = deviceInfo.MacAddress;
		//				insertDeviceCommand.Parameters[2].Value = deviceInfo.DeviceType;
		//				insertDeviceCommand.Parameters[3].Value = deviceInfo.FirmwareType;
		//				insertDeviceCommand.Parameters[4].Value = deviceInfo.Description;

		//				insertDeviceCommand.ExecuteNonQuery();

		//			}
		//		}
		//		catch(Exception ex)
		//		{
		//			result.ErrorText = ex.Message;
		//		}

		//		if (result.ErrorText == null)
		//		{
		//			result.Success = true;
		//			result.NewIDs = newIDs.ToArray();
		//		}

		//		db.Close();
		//	};

		//	return result;
		//}

		//private List<int> GenerateIDs(DeviceInfo[] deviceInfos, string tableName)
		//{
		//	List<int> newIDs = new List<int>();
		//	Dictionary<int, int> oldIDs = GetDevicesIDs(tableName);

		//	int howManyIDs = deviceInfos.Length;

		//	//если записи не первые
		//	if (oldIDs.Count > 0)
		//	{
		//		for (int i = 1; i < oldIDs.Last().Value; i++)
		//		{
		//			if (newIDs.Count != howManyIDs)
		//			{
		//				if (!oldIDs.ContainsKey(i))
		//				{
		//					newIDs.Add(i);
		//				}
		//			}
		//			else
		//			{
		//				break;
		//			}
		//		}
		//	}
		//	else
		//	{
		//		newIDs.Add(1);
		//	}

		//	//если не добавленно необходимое кол-во IDs
		//	if (newIDs.Count != howManyIDs)
		//	{
		//		//сколько осталось добавить
		//		int leftAdd = howManyIDs - newIDs.Count;

		//		//последний id
		//		int lastIdItem = oldIDs.Count > 0 ? oldIDs.Values.Last() : newIDs.Last();

		//		for (int i = 1; i <= leftAdd; i++)
		//		{
		//			int id = lastIdItem + i;
		//			newIDs.Add(id);
		//		}
		//	}

		//	for (int i = 0; i< deviceInfos.Length; i++)
		//	{
		//		deviceInfos[i].Id = newIDs[i];
		//	}

		//	return newIDs;
		//}

		//private Dictionary<int, int> GetDevicesIDs(string tableName)
		//{
		//	Dictionary<int,int> iDs = new Dictionary<int,int>();

		//	using (SqliteConnection db = new SqliteConnection(CONNECTION_STRING))
		//	{
		//		db.Open();

		//		SqliteCommand getIDs = new SqliteCommand();
		//		getIDs.Connection = db;

		//		getIDs.CommandText = $"SELECT Id FROM {tableName} ORDER BY Id";

		//		SqliteDataReader reader = getIDs.ExecuteReader();

		//		while(reader.Read())
		//		{
		//			iDs.Add(reader.GetInt32(0), reader.GetInt32(0));
		//		}

		//		db.Close();
		//	}

		//	return iDs;
		//}
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