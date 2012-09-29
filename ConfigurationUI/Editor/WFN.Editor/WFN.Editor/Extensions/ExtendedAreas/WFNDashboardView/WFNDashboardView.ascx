<%@ Control Language="C#" AutoEventWireup="true"  Inherits="Tridion.Extensions.UI.WFN.CME.WFNDashboardView" %>
<c:deck runat="server" id="WFNDashboardPages" class="stack horizontal">
    <c:DeckPage runat="server" id="WFNDashboardDeckPage" SourceEditor="WFN" ExternalControl="~/Extensions/ExtendedAreas/WFNDashboardView/DeckPages/WFNDashboardPage.ascx" PageType="WFNDashboardPage" />    
</c:deck>
