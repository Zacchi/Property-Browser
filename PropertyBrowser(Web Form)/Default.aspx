<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PropertyBrowser_Web_Form_._Default" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    


    <asp:GridView ID="GridView1" runat="server" DataSourceID="AllPropertyWeb">
    </asp:GridView>
    <asp:SqlDataSource ID="AllPropertyWeb" runat="server"></asp:SqlDataSource>
    <asp:SqlDataSource ID="AllProperty" runat="server"></asp:SqlDataSource>
    <p>
    </p>
    


</asp:Content>
