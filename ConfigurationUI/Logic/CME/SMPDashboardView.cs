using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tridion.Web.UI.Controls;
using Tridion.Web.UI.Core.Controls;

namespace Tridion.Extensions.SMP.CME
{
    [ControlResourcesDependency(typeof(WidgetsView), typeof(Widget), typeof(Date), typeof(DatePicker), typeof(Deck), typeof(AddressBar))]
    [ControlResources("Tridion.Extensions.SMP.Editors.Extensions.ExtendedAreas.SMPDashboardView")]
    public class SMPDashboardView:TridionUserControl
    {
    }
}
