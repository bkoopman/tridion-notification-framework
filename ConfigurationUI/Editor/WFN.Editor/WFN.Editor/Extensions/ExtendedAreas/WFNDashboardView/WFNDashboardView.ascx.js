Type.registerNamespace("Tridion.WFN.Editors.Extensions.ExtendedAreas.Views");

Tridion.WFN.Editors.Extensions.ExtendedAreas.Views.WFNDashboardView = function WFNDashboardView() {
    Tridion.OO.enableInterface(this, "Tridion.WFN.Editors.Extensions.ExtendedAreas.WFNDashboardView");
    var p = this.properties;
    var c = p.controls = {};
};

Tridion.WFN.Editors.Extensions.ExtendedAreas.Views.WFNDashboardView.prototype.initialize = function WFNDashboardView$intialize() {
    var p = this.properties;
    var c = p.controls;
    c.WFNPages = $controls.getControl($("#WFNDashboardPages"), "Tridion.Controls.Deck");
    $controls.getControl($("#WFNDashboardPages"), "Tridion.Controls.Stack")
    c.WFNDashboardPage = c.WFNPages.getPage("WFNDashboardDeckPage");
    $css(c.WFNDashboardPage.getElement(), "padding", "0");
    c.WFNWidgetsView = $controls.getControl($("#WFNDashboardWidgetsView"), "Tridion.Controls.WidgetsView");
};

Tridion.Controls.Deck.registerInitializeExtender("ControlRoom_page", Tridion.WFN.Editors.Extensions.ExtendedAreas.Views.WFNDashboardView);
