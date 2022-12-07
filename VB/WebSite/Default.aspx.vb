Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Security.Cryptography
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class _Default
	Inherits System.Web.UI.Page
	Private Const errorMessage As String = "This record is in use. Please refresh the page to obtain current data"
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
		grid.DataSource = dsProducts
		grid.DataBind()
	End Sub
	Protected ReadOnly Property RowHashes() As Dictionary(Of Object, String)
		Get
			If Session(SessionKey) Is Nothing Then
				Session(SessionKey) = New Dictionary(Of Object, String)()
			End If
			Return CType(Session(SessionKey), Dictionary(Of Object, String))
		End Get
	End Property
	Protected ReadOnly Property SessionKey() As String
		Get
			If (Not oldValuesStorage.Contains("PageSessionKey")) Then
				oldValuesStorage("PageSessionKey") = Guid.NewGuid().ToString()
			End If
			Return oldValuesStorage("PageSessionKey").ToString()
		End Get
	End Property
	Protected Sub ASPxGridView2_RowUpdating(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataUpdatingEventArgs)
		CompareRowVersions(e.Keys(0))
	End Sub
	Protected Sub ASPxGridView2_RowDeleting(ByVal sender As Object, ByVal e As DevExpress.Web.Data.ASPxDataDeletingEventArgs)
		CompareRowVersions(e.Keys(0))
	End Sub
	Protected Sub CompareRowVersions(ByVal key As Object)
		Dim newValues = GetRowHash(key)
		Dim oldValues = RowHashes(key)
		If newValues IsNot oldValues Then
			Throw New Exception(errorMessage)
		End If
	End Sub
	Protected Function GetRowHash(ByVal rowKey As Object) As String
		Dim values = grid.GetRowValuesByKeyValue(rowKey, grid.DataColumns.Select(Function(c) c.FieldName).ToArray())
		Return MD5Hash(values)
	End Function
	Protected Function MD5Hash(ParamArray ByVal rowValues() As Object) As String
		Using provider = New MD5CryptoServiceProvider()
			Return Convert.ToBase64String(provider.ComputeHash(GetByteArrayRepresentation(rowValues)))
		End Using
	End Function
	Protected Function GetByteArrayRepresentation(ByVal rowValues() As Object) As Byte()
		If rowValues Is Nothing Then
			Return New Byte(){}
		End If
		Dim bf = New BinaryFormatter()
		Using ms = New MemoryStream()
			For Each value As Object In rowValues
				bf.Serialize(ms, value)
			Next value
			Return ms.ToArray()
		End Using
	End Function
	Protected Sub grid_CustomJSProperties(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs)
		For i As Integer = 0 To grid.VisibleRowCount - 1
			RowHashes(grid.GetRowValues(i, grid.KeyFieldName)) = GetRowHash(grid.GetRowValues(i, grid.KeyFieldName))
		Next i
	End Sub
End Class