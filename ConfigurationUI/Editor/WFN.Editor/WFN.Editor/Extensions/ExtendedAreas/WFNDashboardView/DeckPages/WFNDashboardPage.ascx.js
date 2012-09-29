Type.registerNamespace("Tridion.Extensions.WFN.Editors.DeckPages");

Tridion.Extensions.WFN.Editors.DeckPages.WFNDashboardPage = function WFNDashboardPage(element)
{
	Tridion.OO.enableInterface(this, "Tridion.Extensions.WFN.Editors.DeckPages.WFNDashboardPage");
	this.addInterface("Tridion.Controls.DeckPage", [element]);
};

Tridion.Extensions.WFN.Editors.DeckPages.WFNDashboardPage.prototype.initialize = function WFNDashboardPage$initialize() {
    this.callBase("Tridion.Controls.DeckPage", "initialize");
    var p = this.properties;


    // initialize controls
    var c = p.controls;

    //alert('WFNDashboardPage$initialize()');

};



Tridion.Controls.Deck.registerPageType(Tridion.Extensions.WFN.Editors.DeckPages.WFNDashboardPage, "WFNDashboardPage");