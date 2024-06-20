Imports System.Data.SQLite
Imports System.IO
Imports TaskManager.Core
Imports TaskManager.Data


Public Class EditContact

    Private contact As New Contact

    Private _Id As Integer
    Public WriteOnly Property ID() As Integer
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property

    Private Sub EditContact_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            SQLiteHelper.SetupConnection()

            GetContact()
        Catch ex As SQLiteException
            MessageBox.Show("SQLite Exception occurred: " & ex.Message)
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try
    End Sub

    Private Sub GetContact()
        Try
            Using contactRepo As New ContactRepository()
                contact = contactRepo.SelectContact(_Id)

                txtName.Text = contact.Name
                txtEmail.Text = contact.Email
                txtPhone.Text = contact.Phone

            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        Try

            Dim contact As New Contact() With {
            .Id = _Id,
            .Name = txtName.Text,
            .Email = txtEmail.Text,
            .Phone = txtPhone.Text,
            .IsActive = Convert.ToInt32(IIf(chkIsActive.Checked, 1, 0))
            }

            Using contactRepo As New ContactRepository()
                contactRepo.UpdateContact(contact)
            End Using
        Catch ex As Exception
            MessageBox.Show("An error occurred: " & ex.Message)
        End Try

        ClearTextBoxes()
        Close()

    End Sub


    Private Sub ClearTextBoxes()
        txtName.Clear()
        txtEmail.Clear()
        txtPhone.Clear()
    End Sub

End Class