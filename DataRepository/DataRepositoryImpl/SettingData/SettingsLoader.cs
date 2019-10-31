using SH.Core;
using SH.DataPorts;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;

namespace SH.DataRepository
{
	internal class SettingsLoader : ISettingsLoader
	{
		public IOperationResult DeleteAll()
		{
			OperationResult result = new OperationResult { Success = true };

			using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
			{
				SqliteCommand delAll = new SqliteCommand
				{
					Connection = db,
					CommandText = $"DELETE FROM {Consts.COON_SETTINGS_TABLE}"
				};

				try
				{
					delAll.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					result.Success = false;
					result.ErrorMessage = ex.Message;
				}
				finally
				{
					delAll.Dispose();
					db.Close();
				}
			}

			return result;
		}

		public ILoadSettingOperationResult Load()
		{
			LoadSettingOperationResult result = new LoadSettingOperationResult { Success = true };

			using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
			{
				SqliteCommand getConnSettingComm = new SqliteCommand
				{
					Connection = db,
					CommandText = $"SELECT * FROM {Consts.COON_SETTINGS_TABLE}"
				};

				try
				{
					List<IParameter> parameters = new List<IParameter>();
					SqliteDataReader reader = getConnSettingComm.ExecuteReader();

					while (reader.Read())
					{
						int paramIndex = reader.GetInt32(0);
						string val = reader.GetString(1);

						parameters.Add(new Parameter { Index = paramIndex, Value = val });
					}

					result.ConnectionSettings = new ConnectionSettings { Parameters = parameters.ToArray() };

				}
				catch (Exception ex)
				{
					result.Success = false;
					result.ErrorMessage = ex.Message;
				}
				finally
				{
					getConnSettingComm.Dispose();
					db.Close();
				}
			}

			return result;
		}

		public IOperationResult Save(IConnectionSettings settings)
		{
			OperationResult result = new OperationResult { Success = true };

			using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
			{
				SqliteCommand insertSettingComm = new SqliteCommand
				{
					Connection = db,
					CommandText = $"INSERT INTO {Consts.COON_SETTINGS_TABLE} VALUES (@ParamIndex, @Value)"
				};

				insertSettingComm.Parameters.Add("@ParamIndex", SqliteType.Integer);
				insertSettingComm.Parameters.Add("@Value", SqliteType.Text);

				try
				{
					foreach (IParameter parameter in settings.Parameters)
					{
						insertSettingComm.Parameters[0].Value = parameter.Index;
						insertSettingComm.Parameters[1].Value = parameter.Value;
						insertSettingComm.ExecuteNonQuery();
					}
				}
				catch (Exception ex)
				{
					result.Success = false;
					result.ErrorMessage = ex.Message;
				}
				finally
				{
					insertSettingComm.Dispose();
					db.Close();
				}
			}

			return result;
		}
	}
}
