using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tridion.Web.UI.Controls;
using Tridion.Web.UI.Core.Controls;

namespace Tridion.Extensions.UI.WFN.CME
{
    [ControlResourcesDependency(new Type[] { typeof(Widget), typeof(Button) })]
    [ControlResources("Tridion.Extensions.WFN.Editors.Extensions.DeckPages.WFNSettingsPage")]
    public class WFNSettingsPage:TridionUserControl
    {
    }
}
