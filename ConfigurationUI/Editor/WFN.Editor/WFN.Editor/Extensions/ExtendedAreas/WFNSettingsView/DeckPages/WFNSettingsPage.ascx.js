Type.registerNamespace("Tridion.Extensions.WFN.Editors.DeckPages");

Tridion.Extensions.WFN.Editors.DeckPages.WFNSettingsPage = function WFNSettingsPage(element) {
    Tridion.OO.enableInterface(this, "Tridion.Extensions.WFN.Editors.DeckPages.WFNSettingsPage");
	this.addInterface("Tridion.Controls.DeckPage", [element]);
};

Tridion.Extensions.WFN.Editors.DeckPages.WFNSettingsPage.prototype.initialize = function WFNSettingsPage$initialize() {
    this.callBase("Tridion.Controls.DeckPage", "initialize");
    var p = this.properties;

    // initialize controls
    var c = p.controls;

    alert('WFNSettingsPage$initialize()');

};



Tridion.Controls.Deck.registerPageType(Tridion.Extensions.WFN.Editors.DeckPages.WFNSettingsPage, "WFNSettingsPage");