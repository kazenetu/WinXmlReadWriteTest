Imports System.Text
Imports System.Xml

''' <summary>
''' XML制御ユーティリティ
''' </summary>
Public Class XmlUtil
    Private Const FILE_NAME As String = "test.xml"

    ''' <summary>
    ''' XMLの書き込み
    ''' </summary>
    ''' <param name="projectForms">プロジェクト・フォーム名のリスト</param>
    ''' <returns>書き込み成否</returns>
    Public Shared Function Write(ByVal projectForms As List(Of KeyValuePair(Of String, String))) As Boolean

        ' projectFormsが存在しない・ゼロ件の場合は終了
        If projectForms Is Nothing OrElse projectForms.Count <= 0 Then
            Return False
        End If

        ' プロジェクト名でグルーピング
        Dim q = projectForms.GroupBy(Function(ByVal keyValue As KeyValuePair(Of String, String)) keyValue.Key)

        ' XML設定
        Dim settings As New XmlWriterSettings()
        settings.Indent = True
        settings.IndentChars = vbTab
        settings.Encoding = System.Text.Encoding.UTF8

        ' XMLファイルを作成
        Using writer As XmlWriter = XmlWriter.Create(FILE_NAME, settings)
            Dim controlIds As New Dictionary(Of String, Integer)

            writer.WriteStartElement("root")
            For Each projectName In q
                ' プロジェクト名ごとに作成
                writer.WriteStartElement(projectName.[Key])

                ' 画面ごとにループ
                For Each keyValue In projectName
                    ' IDをユニークにするため連番を付与する
                    Dim baseCountrolId As String = keyValue.Key & keyValue.Value
                    If Not controlIds.Keys.Contains(baseCountrolId) Then
                        controlIds.Add(baseCountrolId, 0)
                    End If
                    Dim controlId = String.Format("{0}_{1:000000}", baseCountrolId, controlIds(baseCountrolId))
                    controlIds(baseCountrolId) = controlIds(baseCountrolId) + 1

                    ' XMLに書き出し
                    writer.WriteComment("コメント：" & controlId)
                    writer.WriteStartElement("item")
                    writer.WriteElementString("key", controlId)
                    writer.WriteElementString("value", controlId & "value!")
                    writer.WriteEndElement()
                Next

                writer.WriteEndElement()
            Next
            writer.WriteEndElement()

        End Using

        Return True
    End Function

    ''' <summary>
    ''' XMLのitemを文字列に変換して返す
    ''' </summary>
    ''' <returns>文字列に変換したitemのリスト</returns>
    Public Shared Function Read() As String
        Dim keyValues As New Dictionary(Of String, String)

        ' XMLファイルのitemを取得
        Dim items = XElement.Load(FILE_NAME).Descendants("item")

        ' XMLデータの読み込み
        For Each item In items
            keyValues.Add(item.Elements("key").Value, item.Elements("value").Value)
        Next

        '読み込んだXMLデータを文字列に変換して返す
        Dim result As New StringBuilder()
        For Each keyValue In keyValues
            result.AppendLine(keyValue.Key & vbTab & keyValue.Value)
        Next

        Return result.ToString()
    End Function
End Class
