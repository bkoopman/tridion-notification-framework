<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WFNSettingsView.ascx.cs" Inherits="Tridion.Extensions.UI.WFN.CME.WFNSettingsPage" ClassName="bobobo" %>
<c:deck runat="server" id="WFNSettingsPages" class="stack horizontal">
    <c:DeckPage runat="server" id="WFNSettingsDeckPage" SourceEditor="WFN" ExternalControl="~/Extensions/ExtendedAreas/WFNSettingsView/DeckPages/WFNSettingsPage.ascx" PageType="WFNSettingsPage" />    
</c:deck>