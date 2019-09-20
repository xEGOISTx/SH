using SH.Core.DevicesComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Communication
{
    internal class DeviceCommandEditor : IDeviceCommandEditor
    {
        private readonly DeviceCommandList _commands;
        private List<DeviceCommand> _commandsOrigCopies;

        public DeviceCommandEditor(DeviceCommandList commands)
        {
            _commands = commands;
        }

        public bool IsEditing { get; private set; }

        public List<IDeviceCommand> EditedCommands { get; private set; } = new List<IDeviceCommand>();


        public void ChangeDescription(IDeviceCommand command, string description)
        {
            (command as DeviceCommand).Description = description;

            if (!EditedCommands.Contains(command))
            {
                EditedCommands.Add(command);
            }
        }

        public void ChangeVoiceCommand(IDeviceCommand command, string voiceCommand)
        {
            (command as DeviceCommand).VoiceCommand = voiceCommand;

            if (!EditedCommands.Contains(command))
            {
                EditedCommands.Add(command);
            }
        }

        public void EndEditing(bool applyCancelChanges)
        {
            if (IsEditing)
            {
                if (applyCancelChanges)
                {
                    if (OnApply().Cancel)
                    {
                        CancelChanges();
                    }
                }
                else
                {
                    CancelChanges();
                }

                _commandsOrigCopies = null;
                EditedCommands.Clear();
                IsEditing = false;
            }
        }

        public void StartEditing()
        {
            if (!IsEditing)
            {
                _commandsOrigCopies = new List<DeviceCommand>();

                foreach (IDeviceCommand command in _commands)
                {
                    _commandsOrigCopies.Add((command as DeviceCommand).GetCopy());
                }

                IsEditing = true;
            }

        }

        private void CancelChanges()
        {
            foreach(IDeviceCommand commandCopy in _commandsOrigCopies)
            {
                DeviceCommand commandOrig = _commands.GetByID(commandCopy.ID) as DeviceCommand;

                commandOrig.Description = commandCopy.Description;
                commandOrig.VoiceCommand = commandCopy.VoiceCommand;
            }
        }

        private ApplyCommandsChangesEventArgs OnApply()
        {
            ApplyCommandsChangesEventArgs args = new ApplyCommandsChangesEventArgs(_commands.OwnerID, EditedCommands);
            Apply?.Invoke(this, args);
            return args;
        }

        public event ApplyCommandsChangesEventHandler Apply;
    }
}
