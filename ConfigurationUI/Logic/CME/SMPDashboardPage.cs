using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tridion.Web.UI.Controls;
using Tridion.Web.UI.Core.Controls;

namespace Tridion.Extensions.SMP.CME
{
    [ControlResourcesDependency(new Type[] { typeof(Widget), typeof(Button) })]
    [ControlResources("Tridion.Extensions.SMP.Editors.Extensions.DeckPages.SMPDashboardPage")]
    public class SMPDashboardPage:TridionUserControl
    {
    }
}
