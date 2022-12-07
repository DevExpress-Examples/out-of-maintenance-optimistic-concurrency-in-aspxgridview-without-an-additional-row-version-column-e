<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.15.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ASPxGridView - How to implement optimistic concurrency for update/delete operations without an additional "row version" columnы</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxGridView ID="grid" runat="server" KeyFieldName="ProductID"
                OnRowUpdating="ASPxGridView2_RowUpdating" 
                OnRowDeleting="ASPxGridView2_RowDeleting" 
                OnCustomJSProperties="grid_CustomJSProperties"
                AutoGenerateColumns="False">
                <Columns>
                    <dx:GridViewCommandColumn ShowNewButtonInHeader="true" ShowEditButton="true" ShowDeleteButton="true"></dx:GridViewCommandColumn>
                    <dx:GridViewDataColumn FieldName="ProductID" EditFormSettings-Visible="False"></dx:GridViewDataColumn>
                    <dx:GridViewDataTextColumn FieldName="ProductName"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn FieldName="CategoryID"></dx:GridViewDataTextColumn>
                    <dx:GridViewDataSpinEditColumn FieldName="UnitPrice"></dx:GridViewDataSpinEditColumn>
                    <dx:GridViewDataSpinEditColumn FieldName="UnitsOnOrder"></dx:GridViewDataSpinEditColumn>
                </Columns>
                <SettingsPager PageSize="10"></SettingsPager>
            </dx:ASPxGridView>
             <dx:ASPxHiddenField ID="oldValuesStorage" runat="server"></dx:ASPxHiddenField>
             <asp:AccessDataSource runat="server" ID="dsProducts" SelectCommand="SELECT [ProductID], [ProductName], [CategoryID], [UnitPrice], [UnitsOnOrder] FROM [Products]" DataFile="~/App_Data/nwind.mdb" DeleteCommand="DELETE FROM [Products] WHERE [ProductID] = ?" InsertCommand="INSERT INTO [Products] ([ProductID], [ProductName], [CategoryID], [UnitPrice], [UnitsOnOrder]) VALUES (?, ?, ?, ?, ?)" UpdateCommand="UPDATE [Products] SET [ProductName] = ?, [CategoryID] = ?, [UnitPrice] = ?, [UnitsOnOrder] = ? WHERE [ProductID] = ?" >
                 <DeleteParameters>
                     <asp:Parameter Name="ProductID" Type="Int32" />
                 </DeleteParameters>
                 <InsertParameters>
                     <asp:Parameter Name="ProductID" Type="Int32" />
                     <asp:Parameter Name="ProductName" Type="String" />
                     <asp:Parameter Name="CategoryID" Type="Int32" />
                     <asp:Parameter Name="UnitPrice" Type="Decimal" />
                     <asp:Parameter Name="UnitsOnOrder" Type="Int16" />
                 </InsertParameters>
                 <UpdateParameters>
                     <asp:Parameter Name="ProductName" Type="String" />
                     <asp:Parameter Name="CategoryID" Type="Int32" />
                     <asp:Parameter Name="UnitPrice" Type="Decimal" />
                     <asp:Parameter Name="UnitsOnOrder" Type="Int16" />
                     <asp:Parameter Name="ProductID" Type="Int32" />
                 </UpdateParameters>
            </asp:AccessDataSource>
         <%--   <asp:SqlDataSource ID="dsProducts1" runat="server" ConnectionString="<%$ ConnectionStrings:Northwind %>"
                SelectCommand="SELECT [ProductID], [ProductName], [CategoryID], [UnitPrice], [UnitsOnOrder] FROM [Products]" DeleteCommand="DELETE FROM [Products] WHERE [ProductID] = @ProductID" InsertCommand="INSERT INTO [Products] ([ProductName], [CategoryID], [UnitPrice], [UnitsOnOrder]) VALUES (@ProductName, @CategoryID, @UnitPrice, @UnitsOnOrder)" ProviderName="System.Data.SqlClient" UpdateCommand="UPDATE [Products] SET [ProductName] = @ProductName, [CategoryID] = @CategoryID, [UnitPrice] = @UnitPrice, [UnitsOnOrder] = @UnitsOnOrder WHERE [ProductID] = @ProductID">
                <DeleteParameters>
                    <asp:Parameter Name="ProductID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="ProductName" Type="String" />
                    <asp:Parameter Name="CategoryID" Type="Int32" />
                    <asp:Parameter Name="UnitPrice" Type="Decimal" />
                    <asp:Parameter Name="UnitsOnOrder" Type="Int16" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="ProductName" Type="String" />
                    <asp:Parameter Name="CategoryID" Type="Int32" />
                    <asp:Parameter Name="UnitPrice" Type="Decimal" />
                    <asp:Parameter Name="UnitsOnOrder" Type="Int16" />
                    <asp:Parameter Name="ProductID" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>--%>
        </div>
    </form>
</body>
</html>
