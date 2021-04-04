
<%@ Page Title="Zárás" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Zaras.aspx.cs" Inherits="PenzugySzovetseg.aje.Zaras" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
<%--        <h2>Your app description page.</h2>--%>
    </hgroup>

    <article>
      <div>
            <asp:GridView ID="gridView" DataKeyNames="Id" runat="server"
                AutoGenerateColumns="False" ShowFooter="True" HeaderStyle-Font-Bold="true"
                onrowcancelingedit="gridView_RowCancelingEdit"
                onrowdeleting="gridView_RowDeleting"
                onrowediting="gridView_RowEditing"
                onrowupdating="gridView_RowUpdating"
                onrowcommand="gridView_RowCommand"
                OnRowDataBound="gridView_RowDataBound">
        <Columns>

         </Columns>

            <HeaderStyle Font-Bold="True"></HeaderStyle>
        </asp:GridView>
            </div>
        <div >
        <br />&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblmsg" runat="server"></asp:Label>
        </div>
    </article>

</asp:Content>