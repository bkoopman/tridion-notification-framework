using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tridion.Notification.Framework.Contracts
{
    public interface INotificationMessage
    {
        string AppData { get; }
    }
}
