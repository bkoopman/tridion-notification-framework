Type.registerNamespace("Tridion.WFN.Editors.Buttons.WFNSettings");

Tridion.WFN.Editors.Buttons.WFNSettings.WFNSettingsButton = function WFNSettingsButton() {
    Tridion.OO.enableInterface(this, "Tridion.WFN.Editors.Buttons.WFNSettings.WFNSettingsButton");

	var p = this.properties;
	var c = p.controls = {};
	c.dashboardButton = undefined;
	c.controlRoomBodyDivs = undefined;
};

Tridion.WFN.Editors.Buttons.WFNSettings.WFNSettingsButton.prototype.initialize = function WFNSettingsButton$intialize() {
    var p = this.properties;
    var c = p.controls;

    var settingsButtonEl = $("#WFNSettingsBtn");
    $css.addClass(settingsButtonEl, "big");
    c.settingsButton = $controls.getControl(settingsButtonEl, "Tridion.Controls.Button");

    //add events
    $evt.addEventHandler(c.settingsButton, "click", this.getDelegate(this._onClick));

    // only if first button
    if (this._isFirstButton()) {
        this._switchToSettings();
        return;
    }
};

Tridion.WFN.Editors.Buttons.WFNSettings.WFNSettingsButton.prototype._onClick = function WFNSettingsButton$_onClick() {
    var selection = new Tridion.Cme.Selection();
    $commands.executeCommand('DoSomething', selection);
};

Tridion.WFN.Editors.Buttons.WFNSettings.WFNSettingsButton.prototype._switchToSettings = function WFNSettingsButton$_switchToSettings() {
    var p = this.properties;
    var c = p.controls;
    //alert('WFNSettingsButton$_switchToSettings()');
};

Tridion.WFN.Editors.Buttons.WFNSettings.WFNSettingsButton.prototype._isFirstButton = function WFNSettingsButton$_isFirstButton() {
    var p = this.properties;
    var c = p.controls;
    var widgetsViews = $$(".widgetsview", c.settingsButton.getElement().parentNode);
    if (widgetsViews[0] == c.settingsButton.getElement()) {
        //alert('_isFirstButton = true');
        return true;
    }

    //alert('_isFirstButton = false');
    return false;
};

Tridion.Controls.Deck.registerInitializeExtender("Settings_page", Tridion.WFN.Editors.Buttons.WFNSettings.WFNSettingsButton);
