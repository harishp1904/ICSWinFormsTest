Imports System.ComponentModel
Imports System.Data.SQLite
Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports System.Security.Cryptography
Imports Newtonsoft
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports TaskManager.Core
Imports TaskManager.Data

Public Class ContactListing
    Private contacts As New List(Of Contact)()
    Private selectedIDValue As String

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            SQLiteHelper.SetupConnection()

            GetContacts()
        Catch ex As SQLiteException
            Console.WriteLine("SQLite Exception occurred: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub ButtonAdd_Click(sender As Object, e As EventArgs) Handles ButtonAdd.Click

        Try
            Using addContactForm As New AddContact()
                If addContactForm.ShowDialog() = DialogResult.OK Then
                    GetContacts()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

    End Sub

    Private Sub GetContacts() Handles ButtonAdd.Click
        Try
            Using contactRepo As New ContactRepository()
                contacts = contactRepo.GetContacts()
                grdContacts.DataSource = contacts
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

    End Sub

    Private Sub ButtonEdit_Click(sender As Object, e As EventArgs) Handles ButtonEdit.Click

        Try
            Using editContactForm As New EditContact()
                If String.IsNullOrEmpty(selectedIDValue) Then
                    MessageBox.Show("Please select the record to update.")
                Else
                    editContactForm.ID = selectedIDValue
                    editContactForm.ShowDialog()
                    GetContacts()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

    End Sub

    Private Sub grdContacts_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles grdContacts.CellClick
        Try
            If e.RowIndex >= 0 And e.ColumnIndex >= 0 Then
                selectedIDValue = grdContacts.Rows(e.RowIndex).Cells(0).Value.ToString()
            End If
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

    End Sub

    Private Sub ButtonImport_Click(sender As Object, e As EventArgs) Handles ButtonImport.Click

        Dim jsonFileName As String = "contacts.json"

        ' Get the path of the directory where the application is running
        Dim projectFolderPath As String = Application.StartupPath
        Try

            For i As Integer = 1 To 4
                projectFolderPath = Directory.GetParent(projectFolderPath).FullName
            Next

            projectFolderPath = projectFolderPath & "\JSonFile\"

            ' Combine the directory path with the file name to create the full path to the JSON file
            Dim jsonFilePath As String = Path.Combine(projectFolderPath, jsonFileName)


            Using contactRepo As New ContactRepository()
                contacts = contactRepo.GetContacts()
                Dim jsonText As String = JsonConvert.SerializeObject(contacts, Formatting.Indented)

                ' Parse the JSON content to validate it (for JSON arrays), if parsing succeeds the the Json is valid.
                Dim parsedJson As JArray = JArray.Parse(jsonText)

                If File.Exists(jsonFilePath) Then
                    File.Delete(jsonFilePath)
                End If

                File.WriteAllText(jsonFilePath, jsonText)

                MessageBox.Show("The JSON file with contacts detials is saved in the path: " & jsonFilePath)
            End Using
        Catch ex As JsonReaderException
            MessageBox.Show("The JSON file is invalid: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

    End Sub

    Private Sub SearchContact()
        Try
            Using contactRepo As New ContactRepository()
                contacts = contactRepo.SearchContact(txtSearchName.Text, Convert.ToInt32(IIf(chkIsActive.Checked, 1, 0)))
                grdContacts.DataSource = contacts
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

    End Sub

    Private Sub btnSearch_Click_1(sender As Object, e As EventArgs) Handles btnSearch.Click
        SearchContact()
    End Sub
End Class
