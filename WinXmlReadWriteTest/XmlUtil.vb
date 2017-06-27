Imports System.Text
Imports System.Xml

Public Class XmlUtil
    Private Const FILE_NAME As String = "test.xml"

    Public Shared Function Write() As Boolean
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

        Dim q = dummy.GroupBy(Function(ByVal keyValue As KeyValuePair(Of String, String)) keyValue.Key)

        Dim settings As New XmlWriterSettings()
        settings.Indent = True
        settings.IndentChars = vbTab
        settings.Encoding = System.Text.Encoding.UTF8

        'Using writer As XmlWriter = XmlWriter.Create(System.Console.Out, settings)
        Using writer As XmlWriter = XmlWriter.Create(FILE_NAME, settings)
            Dim controlIds As New Dictionary(Of String, Integer)

            writer.WriteStartElement("root")
            For Each key In q
                ' projごとに作成
                writer.WriteStartElement(key.[Key])

                For Each keyValue In key

                    Dim baseCountrolId As String = keyValue.Key & keyValue.Value
                    If Not controlIds.Keys.Contains(baseCountrolId) Then
                        controlIds.Add(baseCountrolId, 0)
                    End If
                    Dim controlId = String.Format("{0}_{1:000000}", baseCountrolId, controlIds(baseCountrolId))
                    controlIds(baseCountrolId) = controlIds(baseCountrolId) + 1

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

    Public Shared Function Read() As String
        Dim keyValues As New Dictionary(Of String, String)

        Dim items = XElement.Load(FILE_NAME).Descendants("item")

        For Each item In items
            keyValues.Add(item.Elements("key").Value, item.Elements("value").Value)
        Next

        Dim result As New StringBuilder()
        For Each keyValue In keyValues
            result.AppendLine(keyValue.Key & vbTab & keyValue.Value)
        Next

        Return result.ToString()
    End Function
End Class
