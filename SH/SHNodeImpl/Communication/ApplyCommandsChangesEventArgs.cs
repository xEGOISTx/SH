using SH.Core.DevicesComponents;
using System;
using System.Collections.Generic;
using System.Text;

namespace SH.Communication
{
    internal delegate void ApplyCommandsChangesEventHandler(object sender, ApplyCommandsChangesEventArgs e);

    internal class ApplyCommandsChangesEventArgs : EventArgs
    {
        public ApplyCommandsChangesEventArgs(int ownerID, IEnumerable<IDeviceCommand> editedCommands)
        {
            OwnerID = ownerID;
            EditedCommands = editedCommands;
        }

        public int OwnerID { get; }

        public IEnumerable<IDeviceCommand> EditedCommands { get; }

        public bool Cancel { get; set; }
    }
}
