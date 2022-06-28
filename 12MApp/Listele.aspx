<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Listele.aspx.cs" Inherits="_12MApp.Listele" %>
<%@ Register assembly="DevExpress.Web.v21.2, Version=21.2.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <dx:ASPxPanel ID="ASPxPanel1" runat="server" Height="510px" Width="1494px">
        <PanelCollection>
<dx:PanelContent runat="server">
            <div style="height: 124px">
                <br />
                <div style="height: 66px">
                    <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="İlk Tarih">
                    </dx:ASPxLabel>
                    <dx:ASPxDateEdit ID="dateIlkTarih" runat="server">
                    </dx:ASPxDateEdit>

                    <dx:ASPxLabel ID="ASPxLabel2" runat="server" Text="Son Tarih">
                    </dx:ASPxLabel>
                    <dx:ASPxDateEdit ID="dateSonTarih" runat="server">
                    </dx:ASPxDateEdit>
                </div>
                <br />
              
            </div>
                <div style=""height: 27px">

                    <dx:ASPxLabel ID="ASPxLabel3" runat="server" Text="Malzeme Kodu/Adı">
                    </dx:ASPxLabel>
                    <dx:ASPxTextBox ID="txtMalzemeKodu" runat="server" Width="170px">
                    </dx:ASPxTextBox>
                    <dx:ASPxButton ID="btnAra" runat="server" OnClick="btnAra_Click" Text="Ara">
                    </dx:ASPxButton>
                    <div style="height: 202px">
                        <dx:ASPxGridView ID="GridRapor" runat="server" AutoGenerateColumns="False" Width="1124px">
                            <SettingsPopup>
                                <FilterControl AutoUpdatePosition="False">
                                </FilterControl>
                            </SettingsPopup>
                            <Columns>
                                <dx:GridViewDataTextColumn Caption="Sıra No" FieldName="SiraNo" ShowInCustomizationForm="True" VisibleIndex="0">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="İşlem Tür" FieldName="IslemTur" ShowInCustomizationForm="True" VisibleIndex="1">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Evrak No" FieldName="EvrakNo" ShowInCustomizationForm="True" VisibleIndex="2">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Tarih" FieldName="Tarih" ShowInCustomizationForm="True" VisibleIndex="3">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Giriş Miktar" FieldName="GirisMiktar" ShowInCustomizationForm="True" VisibleIndex="4">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Çıkış Miktar" FieldName="CikisMiktar" ShowInCustomizationForm="True" VisibleIndex="5">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataTextColumn Caption="Stok Miktar" FieldName="StokMiktar" ShowInCustomizationForm="True" VisibleIndex="6">
                                </dx:GridViewDataTextColumn>
                            </Columns>
                        </dx:ASPxGridView>
                    </div>

                </div>
            </dx:PanelContent>
</PanelCollection>
    </dx:ASPxPanel>
</asp:Content>
