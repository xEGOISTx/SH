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

        public IDeviceCommandEditor Editor { get; set; }

        public IEnumerator<IDeviceCommand> GetEnumerator()
        {
            return _commands.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<IDeviceCommand> commands)
        {
            foreach (IDeviceCommand command in commands.ToArray())
            {
                _commands.Add(command.ID, command);
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
