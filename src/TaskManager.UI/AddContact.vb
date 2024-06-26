﻿Imports TaskManager.Core
Imports TaskManager.Data

Public Class AddContact
    Private Sub ButtonAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        Try
            Dim contact As New Contact() With {
            .Name = txtName.Text,
            .Email = txtEmail.Text,
            .Phone = txtPhone.Text
        }

            Using contactRepo As New ContactRepository()
                contactRepo.InsertContact(contact)
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