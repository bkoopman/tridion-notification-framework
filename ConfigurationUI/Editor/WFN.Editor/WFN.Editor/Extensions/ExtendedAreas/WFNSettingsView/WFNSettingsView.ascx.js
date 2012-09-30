Type.registerNamespace("Tridion.WFN.Editors.Extensions.ExtendedAreas.Views");

Tridion.WFN.Editors.Extensions.ExtendedAreas.Views.WFNSettingsView = function WFNSettingsView() {
    Tridion.OO.enableInterface(this, "Tridion.WFN.Editors.Extensions.ExtendedAreas.WFNSettingsView");
    var p = this.properties;
    var c = p.controls = {};
};

Tridion.WFN.Editors.Extensions.ExtendedAreas.Views.WFNSettingsView.prototype.initialize = function WFNSettingsView$intialize() {
    var p = this.properties;
    var c = p.controls;
    c.WFNPages = $controls.getControl($("#WFNSettingsPages"), "Tridion.Controls.Deck");
    $controls.getControl($("#WFNSettingsPages"), "Tridion.Controls.Stack")
    c.WFNSettingsPage = c.WFNPages.getPage("WFNSettingsDeckPage");
    $css(c.WFNSettingsPage.getElement(), "padding", "0");
    c.WFNWidgetsView = $controls.getControl($("#WFNSettingsWidgetsView"), "Tridion.Controls.WidgetsView");
};

Tridion.Controls.Deck.registerInitializeExtender("Settings_page", Tridion.WFN.Editors.Extensions.ExtendedAreas.Views.WFNSettingsView);