using System;
using System.Data;
using System.Xml;
using System.Xml.XPath;

public class DataItem {
    private string path;

    private int _ID;
    private string _Name;
    private string _Type;

    public int ID { get { return this._ID; } }
    public string Name { get { return this._Name; } }
    public string Type { get { return this._Type; } }

    public DataItem() {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        if (basePath.EndsWith("\\")) 
            basePath = basePath.TrimEnd(new char[] { '\\' });
        this.path = string.Format("{0}/XMLFile.xml", basePath);
    }

    public DataItem(int id, string name, string type) {
        this._ID = id;
        this._Name = name;
        this._Type = type;
    }

    public DataTable SelectAllItems() {
        DataTable dataTable = new DataTable("DataTable");

        dataTable.Columns.Add("ID", typeof(int));
        dataTable.Columns.Add("Name", typeof(string));
        dataTable.Columns.Add("Type", typeof(string));

        dataTable.PrimaryKey = new DataColumn[] { dataTable.Columns["ID"] };

        XmlDocument document = new XmlDocument();
        document.Load(this.path);

        XPathNavigator navigator = document.CreateNavigator();
        XPathNodeIterator nodes = navigator.Select("descendant::Package");

        while (nodes.MoveNext()) {
            int id = nodes.Current.SelectSingleNode("ID").ValueAsInt;
            string name = nodes.Current.SelectSingleNode("Name").Value;
            string type = nodes.Current.SelectSingleNode("Type").Value;
            dataTable.Rows.Add(new object[] { id, name, type });
        }

        return dataTable;
    }
}