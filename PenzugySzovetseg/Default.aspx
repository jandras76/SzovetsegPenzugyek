<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="PenzugySzovetseg._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1 style="font-family: Arial, Helvetica, sans-serif; color: #800000">Áttekintés</h1>
        <p class="lead">Összegek, kiadások rövid áttekintése. Adat felvitelhez válasz a többi menüpont közül.<br />
            Az istentiszteleti és egyéb alkalmakat a <a runat="server" href="~/aje/Alkalmak">"Istentiszteletek/Alkalmak"</a> menüpont alatt lehet rögziteni.
          
            A lelkészek fizetésének összesítése és nyomtatása a <a runat="server" href="~/aje/Fizetesek">"Fizetések nyomtatása"</a>-re kattintva érhető el.
        </p>

    </div>
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
                            <td><asp:CheckBox ID="chb" runat="server" AutoPostBack="True" /></td>
                            <td><asp:Label runat="server" ID="lbl" Font-Bold="true" Text='<%# Eval("Nev") %>'/></td>
                    </ItemTemplate>
                    <FooterTemplate>
                        </tr></table>
                    </FooterTemplate>
                </asp:Repeater>
         
    <%--            <asp:Repeater id="repLelkeszek" runat="server">
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
                </asp:Repeater>--%>
             <hr />
             
             
            <table style="width: 100%;">
                <tr>
                    <td><asp:ImageButton ID="btnImgPDF" runat="server" ImageUrl="~/aje/pdf.gif" Visible="true" OnClick="btnImgPDF_Click"/></td>
                    <td><asp:CheckBox ID="chbIncludeTagdij" runat="server" /></td>
                    <td><asp:Label runat="server" ID="Label6" Font-Bold="true" Text="Éves tagdíj ('<%=PenzugySzovetseg.aje.Nyomtatas.Tagdij%>' CHF) hozzáadása a pdf kivonathoz"/></td>
                </tr>
            </table>
            
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
                        <asp:Label runat="server" ID="TitleLabel" Font-Bold="true" Font-Size="Large" Text='<%# Eval("Varos") %>' />:
                    </h3>
                    <table border ="0">
                        <tr>
                            <td>&nbsp;</td>
                            <td>Honoráriumok összesen: </td>
                            <td style="text-align:right"><asp:Label runat="server" ID="Label3" Font-Bold="true" Text='<%# Eval("Honorarium") %>' /><b>.00</b></td>
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

</asp:Content>
