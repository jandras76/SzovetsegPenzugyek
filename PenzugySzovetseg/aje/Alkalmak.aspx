<%@ Page Title="Istentiszteletek és más alkalmak" Language="C#" MasterPageFile="~/Site.Master"CodeBehind="Alkalmak.aspx.cs" Inherits="PenzugySzovetseg.Alkalmak" %>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h4>Itt lehet rögziteni a különböző istentiszteletek és egyéb alkalmakat valamit a hozzájuk tartozó utazási összegeket.<br>
            A Kiadások tipusát a Kiadás típusa alatt rögzitheted.
        </h4>
        <h3>Figyelem:</h3>
        <h4>Azt a hónapot, illteve évet amihez adatot akarunk rögziteni, ki kell választani a Szűrés listában! 
            Ez egy kérés alapján van így és célja, hogy ne legyen túl sok adat a képernyőn, rögzitéskor. Köszönöm.
        </h4>
    </hgroup>

    <article>
    <div class="row">
        <div class="col-md-4">
                <h2>Szűrés</h2>

            <asp:Repeater id="repEvek" runat="server" >
                <HeaderTemplate>
                    <asp:Label ID="Label1" runat="server" Text="Évek:" ForeColor="BlueViolet" Font-Underline="False" Font-Size="X-Large" Font-Bold="True"></asp:Label>
                    <table style="border: 1px;border-color: brown"><tr>
                </HeaderTemplate>
                <ItemTemplate>
                        <td><asp:CheckBox ID="chb" runat="server" AutoPostBack="True" OnDataBinding="chb_OnDataBinding"/></td>
                        <td><asp:Label runat="server" Width="40" ID="lbl" Font-Bold="true" Text="<%# Container.DataItem.ToString() %>"/></td>
                </ItemTemplate>
                <FooterTemplate>
                    </tr></table>
                </FooterTemplate>
            </asp:Repeater>

            <asp:Repeater id="repHonapok" runat="server" >
                <HeaderTemplate>
                    <asp:Label ID="Label1" runat="server" Text="Hónapok:" ForeColor="Maroon" Font-Underline="False" Font-Size="X-Large" Font-Bold="True"></asp:Label>
                    <table style="border: 1px;border-color: brown"><tr>
                </HeaderTemplate>
                <ItemTemplate>
                        <td><asp:CheckBox ID="chb" runat="server" AutoPostBack="True" OnDataBinding="chb_OnDataBinding"/></td>
                        <td><asp:Label runat="server" Width="40" ID="lbl" Font-Bold="true" Text="<%# Container.DataItem.ToString() %>"/></td>
                </ItemTemplate>
                <FooterTemplate>
                    </tr></table>
                </FooterTemplate>
            </asp:Repeater>
         
            <asp:Repeater id="repVarosok" runat="server">
                <HeaderTemplate>
                    <asp:Label ID="Label1" runat="server" Text="Gyülekezetek:" ForeColor="#660066" Font-Underline="False" Font-Size="X-Large" Font-Bold="True"></asp:Label>
                    <table style="border: 1px;padding-left: 2px"><tr>
                </HeaderTemplate>
                <ItemTemplate>
                        <td><asp:CheckBox ID="chb" runat="server" AutoPostBack="True" Checked="True"/></td>
                        <td><asp:Label runat="server" ID="lbl" Font-Bold="true" Text='<%# Eval("Nev") %>'/></td>
                </ItemTemplate>
                <FooterTemplate>
                    </tr></table>
                </FooterTemplate>
            </asp:Repeater>
         
            <asp:Repeater id="repLelkeszek" runat="server">
                <HeaderTemplate>
                    <asp:Label ID="Label1" runat="server" Text="Lelkészek:" ForeColor="#003300" Font-Underline="False" Font-Size="X-Large" Font-Bold="True"></asp:Label>
                    <table style="border: 1px;padding-left: 2px"><tr>
                </HeaderTemplate>
                <ItemTemplate>
                        <td><asp:CheckBox ID="chb" runat="server" AutoPostBack="True"/></td>
                        <td><asp:Label Width="60" runat="server" ID="lbl" Font-Bold="true" Text='<%# Eval("Kod") %>'/></td>
                </ItemTemplate>
                <FooterTemplate>
                    </tr></table>
                </FooterTemplate>
            </asp:Repeater>
            
            <asp:Repeater id="repKiadasokTipusa" runat="server" >
                <HeaderTemplate>
                    <asp:Label ID="Label1" runat="server" Text="Honoráriumok:" ForeColor="#003300" Font-Underline="False" Font-Size="X-Large" Font-Bold="True"></asp:Label>
                    <table style="width: 400px;border: 1px;padding-left: 2px;"><tr>
                                        
                        <td>
                            <table>
                                <tr>
                </HeaderTemplate>
                    <ItemTemplate>
                        <td><asp:CheckBox ID="chb" runat="server" AutoPostBack="True"/></td>
                        <td><asp:Label Width="200" runat="server" ID="lbl" Font-Bold="true" Text='<%# Eval("Tipus") %>'/></td>
                        <td>&nbsp;</td>
                        <%# Container.ItemIndex > 0 && (Container.ItemIndex + 1) % 5 == 0 ? "</tr><tr>" : string.Empty %>
                    </ItemTemplate>

                <FooterTemplate>
                            </tr>
                        </table>
                    </td>
                    </tr></table>
                </FooterTemplate>
            </asp:Repeater>

         <hr />
     </div>
    </div>

      <div>
            <asp:GridView ID="gridView" DataKeyNames="id" runat="server"
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