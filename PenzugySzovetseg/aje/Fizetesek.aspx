<%@ Page Title="Fizetések nyomtatása" Language="C#" MasterPageFile="~/Site.Master" CodeBehind="Fizetesek.aspx.cs" Inherits="PenzugySzovetseg.aje.Fizetesek" %>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <hgroup class="title">
        <h1><%: Title %>.</h1>
        <h4>A fizetések kinyomtatásházoz, kattints a pdf logora.
            <br>A szűrés altt kijelölheted, hogy mely hónapokra szeretnéd a kimutatást késziteni.
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
                        <td><asp:CheckBox ID="chb" runat="server" AutoPostBack="True" OnDataBinding="chb_DataBinding"/></td>
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
                        <td><asp:CheckBox ID="chb" runat="server" AutoPostBack="True" OnDataBinding="chb_DataBinding"/></td>
                        <td><asp:Label runat="server" Width="40" ID="lbl" Font-Bold="true" Text="<%# Container.DataItem.ToString() %>"/></td>
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
                        <td><asp:Label runat="server" ID="lbl" Font-Bold="true" Text='<%# Eval("Kod") %>'/></td>
                </ItemTemplate>
                <FooterTemplate>
                    </tr></table>
                </FooterTemplate>
            </asp:Repeater>
             <hr />
             <asp:ImageButton ID="btnImgPDF" runat="server" ImageUrl="~/aje/pdf.gif" OnClick="btnImgPDF_Click"/>
     </div>
    </div>

         <div class="row">
        <div class="col-md-4">
            <asp:Repeater id="repOsszesites" runat="server">

                <HeaderTemplate>
                    <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
                </HeaderTemplate>

                <ItemTemplate>
                    <h3>
                        <asp:Label runat="server" ID="TitleLabel" Font-Bold="true" Font-Size="Large" Text='<%# Eval("Lelkesz") %>' />:
                    </h3>
                    <table>
                        <tr>
                            <td>&nbsp;</td>
                            <td>Gyülekezet: </td>
                            <td style="text-align:right"><asp:Label runat="server" ID="Label6" Font-Bold="true" Text='<%# Eval("Varos") %>' /></td>
                        </tr>                        
                        <tr>
                            <td>&nbsp;</td>
                            <td>Honoráriumok összesen: </td>
                            <td style="text-align:right"><asp:Label runat="server" ID="Label3" Font-Bold="true" Text='<%# Eval("Honorarium") %>' /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>Utazások összesen: </td>
                            <td style="text-align:right"><asp:Label runat="server" ID="Label4" Font-Bold="true" Text='<%# Eval("Utazas") %>' /></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td><b>Összesitve:</b></td>
                            <td style="text-align:right"><asp:Label runat="server" ID="Label5" Font-Bold="true" Font-Underline="true" Font-Size="Medium" Text='<%# Eval("Osszesen") %>' /></td>
                        </tr>
                    </table>
                </ItemTemplate>

                <FooterTemplate>
                    <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                </FooterTemplate>

            </asp:Repeater>
        </div>

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
