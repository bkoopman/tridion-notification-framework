Type.registerNamespace("Tridion.WFN.Editors.Buttons.WFNControlRoom");

Tridion.WFN.Editors.Buttons.WFNControlRoom.WFNDashboardButton = function WFNDashboardButton()
{
	Tridion.OO.enableInterface(this, "Tridion.WFN.Editors.Buttons.WFNControlRoom.WFNDashboardButton");

	var p = this.properties;
	var c = p.controls = {};
	c.dashboardButton = undefined;
	c.controlRoomBodyDivs = undefined;
};

Tridion.WFN.Editors.Buttons.WFNControlRoom.WFNDashboardButton.prototype.initialize = function WFNDashboardButton$intialize() {
    var p = this.properties;
    var c = p.controls;

    var dashboardButtonEl = $("#WFNDashboardBtn");
    $css.addClass(dashboardButtonEl, "big");
    c.dashboardButton = $controls.getControl(dashboardButtonEl, "Tridion.Controls.Button");

    //add events
    $evt.addEventHandler(c.dashboardButton, "click", this.getDelegate(this._onClick));

    //var omeButtonEl = $("#OMEOnlineMarketingButton");
    var ugcButtonEl = $("#UGCCommunityBtn");
    //$css.addClass(omeButtonEl, "big");

    //c.omeButton = $controls.getControl(omeButtonEl, "Tridion.Controls.Button");

    //add events
    //$evt.addEventHandler(c.omeButton, "click", this.getDelegate(this._onClick));


    // ugc bug 
    if (ugcButtonEl) {
        c.ugcButton = $controls.getControl(ugcButtonEl, "Tridion.Controls.Button");
        $evt.addEventHandler(c.ugcButton, "click", this.getDelegate(this._onClickUGC));
    }

    //automatically show community if it's the first one	
    if (this._isFirstButton()) {
        this._switchToDashboard();
        return;
    } else if (this._isUgcFirstButton()) {
        this._switchToCommunity();
        return;
    }

    //this._switchToDashboard();
};

Tridion.WFN.Editors.Buttons.WFNControlRoom.WFNDashboardButton.prototype._isUgcFirstButton = function WFNDashboardButton$_isUgcFirstButton() {
    var p = this.properties;
    var c = p.controls;
    var widgetsViews = $$(".widgetsview", c.dashboardButton.getElement().parentNode);
    if (c.ugcButton && $$("#ControlRoomButtons>div")[0] === c.ugcButton.getElement()) {
        return true;
    }

    return false;
};

Tridion.WFN.Editors.Buttons.WFNControlRoom.WFNDashboardButton.prototype._isFirstButton = function WFNDashboardButton$_isFirstButton() {
    var p = this.properties;
    var c = p.controls;
    var widgetsViews = $$(".widgetsview", c.dashboardButton.getElement().parentNode);
    if (widgetsViews[0] == c.dashboardButton.getElement()) {
        return true;
    }

    return false;
};

Tridion.WFN.Editors.Buttons.WFNControlRoom.WFNDashboardButton.prototype._onClick = function WFNDashboardButton$_onClick(e) {
    
    this._switchToDashboard();
};

Tridion.WFN.Editors.Buttons.WFNControlRoom.WFNDashboardButton.prototype._switchToDashboard = function WFNDashboardButton$_switchToDashboard() {
    var p = this.properties;
    var c = p.controls;
    if (!c.controlRoomBodyDivs) {
        c.controlRoomBodyDivs = $$("#ControlRoomBody>.widgetsview");
    }

    var controlRoomBodyDiv;
    for (var i = 0, l = c.controlRoomBodyDivs.length; i < l; i++) {
        controlRoomBodyDiv = c.controlRoomBodyDivs[i];
        
        $css.addClass(controlRoomBodyDiv, "hidden");

        if (controlRoomBodyDiv.id === "WFNDashboardPages") {
            
            $css.removeClass(controlRoomBodyDiv, "hidden");
        }
    }



};

Tridion.WFN.Editors.Buttons.WFNControlRoom.WFNDashboardButton.prototype._onClickUGC = function WFNDashboardButton$_onClickUGC(e) {
    this._switchToCommunity();
};

Tridion.WFN.Editors.Buttons.WFNControlRoom.WFNDashboardButton.prototype._switchToCommunity = function WFNDashboardButton$_switchToCommunity() {
    var p = this.properties;
    var c = p.controls;
    if (!c.controlRoomBodyDivs) {
        c.controlRoomBodyDivs = $$("#ControlRoomBody>div");
    }

    var controlRoomBodyDiv;
    for (var i = 0, l = c.controlRoomBodyDivs.length; i < l; i++) {
        controlRoomBodyDiv = c.controlRoomBodyDivs[i];
        $css.addClass(controlRoomBodyDiv, "hidden");
        if (controlRoomBodyDiv.id === "CommunityWidgetsViewFilter" || controlRoomBodyDiv.id === "CommunityPages") {
            $css.removeClass(controlRoomBodyDiv, "hidden");
        }
    }
};

Tridion.Controls.Deck.registerInitializeExtender("ControlRoom_page", Tridion.WFN.Editors.Buttons.WFNControlRoom.WFNDashboardButton);

