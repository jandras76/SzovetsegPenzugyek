<%@ Page Title="Honoráriumok" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Honorarium.aspx.cs" Inherits="PenzugySzovetseg.aje.Honorarium" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h4> A munkaszerződésben elfogadott honoráriumi lista, amit a választmány jóváhagyott.<br>
            Amennyiben szükséges itt lehet az összegeken változtatni, vagy újat hozzáadni.
        </h4>
    </hgroup>

    <article>

      <div>
            <asp:GridView ID="gridView" DataKeyNames="Tipus" runat="server"
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
