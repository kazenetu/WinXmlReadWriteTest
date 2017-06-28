Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'ダミーデータ作成
        Dim dummy As New List(Of KeyValuePair(Of String, String))
        dummy.Add(New KeyValuePair(Of String, String)("projA", "form1"))
        dummy.Add(New KeyValuePair(Of String, String)("projA", "form1"))
        dummy.Add(New KeyValuePair(Of String, String)("projA", "form1"))
        dummy.Add(New KeyValuePair(Of String, String)("projA", "form2"))
        dummy.Add(New KeyValuePair(Of String, String)("projA", "form3"))
        dummy.Add(New KeyValuePair(Of String, String)("projA", "form3"))
        dummy.Add(New KeyValuePair(Of String, String)("projB", "form1"))
        dummy.Add(New KeyValuePair(Of String, String)("projB", "form1"))
        dummy.Add(New KeyValuePair(Of String, String)("projA", "form3"))

        ' XML書き込み
        XmlUtil.Write(dummy)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.TextBox1.Text = XmlUtil.Read()
    End Sub
End Class
