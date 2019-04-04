<!-- default file list -->
*Files to look at*:

* [Default.aspx](./CS/WebSite/Default.aspx) (VB: [Default.aspx](./VB/WebSite/Default.aspx))
* [Default.aspx.cs](./CS/WebSite/Default.aspx.cs) (VB: [Default.aspx.vb](./VB/WebSite/Default.aspx.vb))
<!-- default file list end -->
# Optimistic concurrency in ASPxGridView without an additional "row version" column


<p>This approach does not require adding a row's version column to the underlying table. The main idea is to generate hash strings for rows on every server action. Then, you need to generate hash for the currently edited/deleted record and compare it with the stored string in the RowUpdating/RowDeleting events handlers.</p><p>Pros:</p><p>- You don't need to modify your database;</p><p>Cons:</p><p>- Hash strings should be calculated on every round-trip to server. However, this operation is rather quick, so it should not cause problems with performance. You can also increase hash generating speed by using fast hashing algorithms, like <a href="http://blog.teamleadnet.com/2012/08/murmurhash3-ultra-fast-hash-algorithm.html"><u>MurMurHash3 Algorithm</u></a><u>.</u></p>

<br/>


