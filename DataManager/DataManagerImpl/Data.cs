using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using SHToolKit.DataManagement;

namespace DataManager
{
	public static class Data
    {
		public static void InitializeDatabase()
		{
			using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
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

		public static IDataLoader DataLoader { get; } = new DataLoader();

		public static IDataEditor DataEditor { get; }

		//public IDBOperationResult RenameDevice(int devID, string newDescription)
		//{
		//	DBOperationResult result = new DBOperationResult();

		//	using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
		//	{
		//		db.Open();

		//		string rename = $"UPDATE {Consts.DEVICES_TABLE} SET Description = '{newDescription}' WHERE Id = {devID}";
		//		SqliteCommand renameDevice = new SqliteCommand(rename, db);

		//		try
		//		{
		//			renameDevice.ExecuteNonQuery();
		//			result.Success = true;
		//		}
		//		catch (SqliteException ex)
		//		{
		//			result.ErrorText = $"{Consts.DEVICES_TABLE} - {ex.Message}";
		//		}
		//		catch (Exception ex)
		//		{
		//			result.ErrorText = $"{Consts.DEVICES_TABLE} - {ex.Message}";
		//		}
		//		finally
		//		{
		//			db.Close();
		//		}
		//	}

		//	return result;
		//}

	}
}