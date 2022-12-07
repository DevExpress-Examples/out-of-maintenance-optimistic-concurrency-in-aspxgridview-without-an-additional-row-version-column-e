using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page {
    const string errorMessage = "This record is in use. Please refresh the page to obtain current data";
    protected void Page_Load(object sender, EventArgs e) {
        grid.DataSource = dsProducts;
        grid.DataBind();
    }
    protected Dictionary<object, string> RowHashes {
        get {
            if (Session[SessionKey] == null)
                Session[SessionKey] = new Dictionary<object, string>();
            return (Dictionary<object, string>)Session[SessionKey];
        }
    }
    protected string SessionKey {
        get {
            if (!oldValuesStorage.Contains("PageSessionKey"))
                oldValuesStorage["PageSessionKey"] = Guid.NewGuid().ToString();
            return oldValuesStorage["PageSessionKey"].ToString();
        }
    }
    protected void ASPxGridView2_RowUpdating(object sender, DevExpress.Web.Data.ASPxDataUpdatingEventArgs e) {
        CompareRowVersions(e.Keys[0]);
    }
    protected void ASPxGridView2_RowDeleting(object sender, DevExpress.Web.Data.ASPxDataDeletingEventArgs e) {
        CompareRowVersions(e.Keys[0]);
    }
    protected void CompareRowVersions(object key) {
        var newValues = GetRowHash(key);
        var oldValues = RowHashes[key];
        if (newValues != oldValues)
            throw new Exception(errorMessage);
    }
    protected string GetRowHash(object rowKey) {
        var values = grid.GetRowValuesByKeyValue(rowKey, grid.DataColumns.Select(c => c.FieldName).ToArray());
        return MD5Hash(values);
    }
    protected string MD5Hash(params object[] rowValues) {
        using (var provider = new MD5CryptoServiceProvider())
            return Convert.ToBase64String(provider.ComputeHash(GetByteArrayRepresentation(rowValues)));
    }
    protected byte[] GetByteArrayRepresentation(object[] rowValues) {
        if (rowValues == null)
            return new byte[0];
        var bf = new BinaryFormatter();
        using (var ms = new MemoryStream()) {
            foreach (object value in rowValues)
                bf.Serialize(ms, value);
            return ms.ToArray();
        }
    }
    protected void grid_CustomJSProperties(object sender, DevExpress.Web.ASPxGridViewClientJSPropertiesEventArgs e) {
        for (int i = 0; i < grid.VisibleRowCount; i++)
            RowHashes[grid.GetRowValues(i, grid.KeyFieldName)] = GetRowHash(grid.GetRowValues(i, grid.KeyFieldName));
    }
}