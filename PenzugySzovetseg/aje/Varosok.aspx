<%@ Page Title="Városok, Gemeindék" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Varosok.aspx.cs" Inherits="PenzugySzovetseg.aje.Varosok" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h4> Szövetségünk gyülekezetei városok szerint.
        </h4>
    </hgroup>

    <article>

      <div>
            <asp:GridView ID="gridView" DataKeyNames="Nev" runat="server"
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

<%--    <aside>
        <h3>Aside Title</h3>
        <p>        
            Use this area to provide additional information.
        </p>
        <ul>
            <li><a runat="server" href="~/">Home</a></li>
            <li><a runat="server" href="~/About">About</a></li>
            <li><a runat="server" href="~/Contact">Contact</a></li>
        </ul>
    </aside>--%>
</asp:Content>