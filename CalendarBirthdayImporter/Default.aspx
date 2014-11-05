<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CalendarBirthdayImporter._Default" %>
<%@ Register Src="~/Controls/ErrorSuccessNotifier/ErrorSuccessNotifier.ascx" TagPrefix="ucl" TagName="ErrorSuccessNotifier" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Birthday calendar</h1>
    </div>
    <ucl:errorsuccessnotifier ID="ESN" runat="server" />
    <asp:Panel ID="pnlLogin" runat="server">
        <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-default" OnClick="btnLogin_Click" />
    </asp:Panel>
    <asp:Panel ID="pnlImport" runat="server" Visible="false">
        <asp:FileUpload ID="FileUploadControl" runat="server" CssClass="pull-left" />
        <asp:Button ID="btnImport" runat="server" Text="Import" CssClass="btn btn-default" OnClick="btnImport_Click" />
    </asp:Panel>

</asp:Content>
