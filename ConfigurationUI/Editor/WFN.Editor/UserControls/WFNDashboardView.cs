using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tridion.Web.UI.Controls;
using Tridion.Web.UI.Core.Controls;

namespace Tridion.Extensions.UI.WFN.CME
{
    [ControlResourcesDependency(typeof(WidgetsView), typeof(Widget), typeof(Date), typeof(DatePicker), typeof(Deck), typeof(AddressBar))]
    [ControlResources("Tridion.Extensions.WFN.Editors.Extensions.ExtendedAreas.WFNDashboardView")]
    public class WFNDashboardView:TridionUserControl
    {
    }
}
