Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Xml
Imports System.Xml.XPath

Public Class DataItem
	Private path As String

	Private _ID As Integer
	Private _Name As String
	Private _Type As String

	Public ReadOnly Property ID() As Integer
		Get
			Return Me._ID
		End Get
	End Property
	Public ReadOnly Property Name() As String
		Get
			Return Me._Name
		End Get
	End Property
	Public ReadOnly Property Type() As String
		Get
			Return Me._Type
		End Get
	End Property

	Public Sub New()
		Dim basePath As String = AppDomain.CurrentDomain.BaseDirectory
		If basePath.EndsWith("\") Then
			basePath = basePath.TrimEnd(New Char() { "\"c })
		End If
		Me.path = String.Format("{0}/XMLFile.xml", basePath)
	End Sub

	Public Sub New(ByVal id As Integer, ByVal name As String, ByVal type As String)
		Me._ID = id
		Me._Name = name
		Me._Type = type
	End Sub

	Public Function SelectAllItems() As DataTable
		Dim dataTable As New DataTable("DataTable")

		dataTable.Columns.Add("ID", GetType(Integer))
		dataTable.Columns.Add("Name", GetType(String))
		dataTable.Columns.Add("Type", GetType(String))

		dataTable.PrimaryKey = New DataColumn() { dataTable.Columns("ID") }

		Dim document As New XmlDocument()
		document.Load(Me.path)

		Dim navigator As XPathNavigator = document.CreateNavigator()
		Dim nodes As XPathNodeIterator = navigator.Select("descendant::Package")

		Do While nodes.MoveNext()
			Dim id As Integer = nodes.Current.SelectSingleNode("ID").ValueAsInt
			Dim name As String = nodes.Current.SelectSingleNode("Name").Value
			Dim type As String = nodes.Current.SelectSingleNode("Type").Value
			dataTable.Rows.Add(New Object() { id, name, type })
		Loop

		Return dataTable
	End Function
End Class