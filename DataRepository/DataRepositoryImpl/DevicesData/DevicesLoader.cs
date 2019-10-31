using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using SH.Core;
using SH.DataPorts;

namespace SH.DataRepository
{
    internal class DevicesLoader : IDevicesLoader
    {
        public IOperationResultDevicesLoad LoadDevices()
        {
            OperationResultDevicesLoad result = new OperationResultDevicesLoad();

            using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
            {
                db.Open();

                SqliteCommand getDevicesComm = new SqliteCommand
				{
					Connection = db,
					CommandText = $"SELECT * FROM {Consts.DEVICES_TABLE}"
				};

                SqliteCommand getCommandsComm = new SqliteCommand 
				{
					Connection = db,
					CommandText = $"SELECT * FROM {Consts.COMMANDS_TABLE}"
				};

                try
                {
                    //выгружаем устройства
                    List<IDeviceData> devices = new List<IDeviceData>();
                    SqliteDataReader getDevicesQuery = getDevicesComm.ExecuteReader();

                    while (getDevicesQuery.Read())
                    {
                        DeviceData device = new DeviceData
                        {
                            ID = getDevicesQuery.GetInt32(0),
                            MacAddress = getDevicesQuery.GetString(1),
                            DeviceType = getDevicesQuery.GetInt32(2),
                            Description = getDevicesQuery.GetString(4)
                        };

                        devices.Add(device);
                    }

                    //выгружаем команды
                    Dictionary<int, List<IDeviceCommandData>> commands = new Dictionary<int, List<IDeviceCommandData>>();
                    SqliteDataReader getCommandsQuery = getCommandsComm.ExecuteReader();

                    while (getCommandsQuery.Read())
                    {
                        DeviceCommandData command = new DeviceCommandData
                        {
                            ID = getCommandsQuery.GetInt32(0),
                            OwnerID = getCommandsQuery.GetInt32(1),
                            VoiceCommand = getCommandsQuery.GetString(2),
                            Description = getCommandsQuery.GetString(3)
                        };

                        if (!commands.ContainsKey(command.OwnerID))
                        {
                            commands.Add(command.OwnerID, new List<IDeviceCommandData> { command });
                        }
                        else
                        {
                            commands[command.OwnerID].Add(command);
                        }
                    }

                    //передаём устройствам команды
                    foreach (IDeviceData deviceData in devices)
                    {
                        if (commands.ContainsKey(deviceData.ID))
                            (deviceData as DeviceData).Commands = commands[deviceData.ID].ToArray();
                    }

                    result.Success = true;
                    result.Devices = devices.ToArray();
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.Message;
                }
                finally
                {
                    getDevicesComm.Dispose();
                    getCommandsComm.Dispose();
                    db.Close();
                }
            }

            return result;
        }

		public IOperationResult RemoveDevice(int deviceID)
		{
			OperationResult result = new OperationResult { Success = true };

			using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
			{
				db.Open();
				SqliteCommand removeDeviceComm = new SqliteCommand
				{
					CommandText = $"DELETE FROM {Consts.DEVICES_TABLE} WHERE DeviceID = {deviceID}",//команды будут удалены каскадом
					Connection = db
				};

				try
				{
					removeDeviceComm.ExecuteNonQuery();
				}
				catch (Exception ex)
				{
					result.Success = false;
					result.ErrorMessage = ex.Message;
				}
				finally
				{
					removeDeviceComm.Dispose();
					db.Close();
				}
			}

			return result;
		}

        public IOperationResult SaveDevice(IDeviceData device)
        {
            OperationResult result = new OperationResult();

            using (SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
            {
                db.Open();

                SqliteCommand insertDeviceComm = new SqliteCommand();
                SqliteCommand insertCommandsComm = new SqliteCommand();
                insertDeviceComm.Connection = db;
                insertCommandsComm.Connection = db;

                insertDeviceComm.CommandText = $"INSERT INTO {Consts.DEVICES_TABLE} VALUES (@DeviceID, @MacAddress, @DeviceType, @Description);";
                insertDeviceComm.Parameters.Add(new SqliteParameter("@DeviceID", SqliteType.Integer));
                insertDeviceComm.Parameters.Add(new SqliteParameter("@MacAddress", SqliteType.Text));
                insertDeviceComm.Parameters.Add(new SqliteParameter("@DeviceType", SqliteType.Integer));
                insertDeviceComm.Parameters.Add(new SqliteParameter("@Description", SqliteType.Text));

                insertCommandsComm.CommandText = $"INSERT INTO {Consts.COMMANDS_TABLE} VALUES (@CommandID, @DeviceID, @VoiceCommand, @Description);";
                insertCommandsComm.Parameters.Add(new SqliteParameter("@CommandID", SqliteType.Integer));
                insertCommandsComm.Parameters.Add(new SqliteParameter("@DeviceID", SqliteType.Integer));
                insertCommandsComm.Parameters.Add(new SqliteParameter("@VoiceCommand", SqliteType.Text));
                insertCommandsComm.Parameters.Add(new SqliteParameter("@Description", SqliteType.Text));

                try
                {
                    //сохраняем устройство
                    insertDeviceComm.Parameters[0].Value = device.ID;
                    insertDeviceComm.Parameters[1].Value = device.MacAddress;
                    insertDeviceComm.Parameters[2].Value = device.DeviceType;
                    insertDeviceComm.Parameters[3].Value = device.Description;
                    insertDeviceComm.ExecuteNonQuery();

                    //сохраняем команды
                    foreach(IDeviceCommandData cmd in device.Commands)
                    {
                        insertCommandsComm.Parameters[0].Value = cmd.ID;
                        insertCommandsComm.Parameters[1].Value = cmd.OwnerID;
                        insertCommandsComm.Parameters[2].Value = cmd.VoiceCommand;
                        insertCommandsComm.Parameters[3].Value = cmd.Description;
                        insertCommandsComm.ExecuteNonQuery();
                    }

                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = ex.Message;
                }
                finally
                {
                    insertDeviceComm.Dispose();
                    insertCommandsComm.Dispose();
                    db.Close();
                }
            };

            return result;
        }

        public IOperationResult UpdateDeviceCommands(IDeviceCommandData[] commands)
        {
            OperationResult result = new OperationResult { Success = false };

            using(SqliteConnection db = new SqliteConnection(Consts.CONNECTION_STRING))
            {
                db.Open();

                SqliteCommand updateCommandsComm = new SqliteCommand();
                updateCommandsComm.Connection = db;

                updateCommandsComm.CommandText = $"UPDATE {Consts.COMMANDS_TABLE} SET VoiceCommand = @VoiceCommand, Description = @Description, " +
                    "WHERE CommandID = @CommandID AND DeviceID = @DeviceID";

                updateCommandsComm.Parameters.Add("@VoiceCommand", SqliteType.Text);
                updateCommandsComm.Parameters.Add("@Description", SqliteType.Text);
                updateCommandsComm.Parameters.Add("@CommandID", SqliteType.Integer);
				updateCommandsComm.Parameters.Add("@DeviceID", SqliteType.Integer);

                try
                {
                    foreach(IDeviceCommandData cmd in commands)
                    {
                        updateCommandsComm.Parameters[0].Value = cmd.VoiceCommand;
                        updateCommandsComm.Parameters[1].Value = cmd.Description;
                        updateCommandsComm.Parameters[3].Value = cmd.ID;
						updateCommandsComm.Parameters[4].Value = cmd.OwnerID;
                        updateCommandsComm.ExecuteNonQuery();
                    }

                    result.Success = true;
                }
                catch(Exception ex)
                {                   
                    result.ErrorMessage = ex.Message;
                }
                finally
                {
                    updateCommandsComm.Dispose();
                    db.Close();
                }
            }

            return result;
        }
    }
}
