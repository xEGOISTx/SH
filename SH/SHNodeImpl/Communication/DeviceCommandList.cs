using SH.Core.DevicesComponents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SH.Communication
{
    internal class DeviceCommandList : IDeviceCommandList
    {
        private readonly Dictionary<int, IDeviceCommand> _commands = new Dictionary<int, IDeviceCommand>();

        public DeviceCommandList(int ownerID)
        {
            OwnerID = ownerID;
        }

        public int OwnerID { get; }

        public IDeviceCommandEditor Editor { get; set; }

        public IEnumerator<IDeviceCommand> GetEnumerator()
        {
            return _commands.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Add(IDeviceCommand command)
        {
            if(!_commands.ContainsKey(command.ID))
            {
                _commands.Add(command.ID, command);
            }
        }

        public void AddRange(IEnumerable<IDeviceCommand> commands)
        {
            foreach (IDeviceCommand command in commands.ToArray())
            {
                Add(command);
            }
        }

        public IDeviceCommand GetByID(int id)
        {
            if(_commands.ContainsKey(id))
            {
                return _commands[id];
            }
            else
            {
                return null;
            }
        }
    }
}
