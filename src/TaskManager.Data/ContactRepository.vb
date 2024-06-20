Imports System.Data.SQLite
Imports TaskManager.Core

Public Class ContactRepository
    Implements IDisposable

    Public Sub Dispose() Implements IDisposable.Dispose
    End Sub

    Public Function InsertContact(contact As Contact) As Boolean
        Using connection As New SQLiteConnection(SQLiteHelper.DBPath)
            connection.Open()
            Dim insertQuery As String = "INSERT INTO Contact (Name, Email, Phone, IsActive) VALUES (@Name, @Email, @Phone,@IsActive)"
            Using command As New SQLiteCommand(insertQuery, connection)
                command.Parameters.AddWithValue("@Name", contact.Name)
                command.Parameters.AddWithValue("@Email", contact.Email)
                command.Parameters.AddWithValue("@Phone", contact.Phone)
                command.Parameters.AddWithValue("@IsActive", contact.IsActive)
                command.ExecuteNonQuery()
            End Using
        End Using

        Return False
    End Function

    Public Function GetContacts() As List(Of Contact)
        Dim contacts As New List(Of Contact)

        Using connection As New SQLiteConnection(SQLiteHelper.DBPath)
            connection.Open()
            Dim selectQuery As String = "SELECT Id,Name, Email, Phone, IsActive FROM Contact"
            Using command As New SQLiteCommand(selectQuery, connection)
                Using reader As SQLiteDataReader = command.ExecuteReader()
                    While reader.Read()
                        contacts.Add(New Contact() With {.Id = reader("Id"), .Name = reader("Name"), .Email = reader("Email"), .Phone = reader("Phone"), .IsActive = reader("IsActive")})
                    End While
                End Using
            End Using
        End Using

        Return contacts
    End Function

    Public Function SelectContact(ContactId As Integer) As Contact
        Dim contact As New Contact

        Using connection As New SQLiteConnection(SQLiteHelper.DBPath)
            connection.Open()
            Dim selectQuery As String = "SELECT Id,Name, Email, Phone, IsActive FROM Contact where Id=" & ContactId
            Using command As New SQLiteCommand(selectQuery, connection)
                Using reader As SQLiteDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        contact.Id = reader("Id")
                        contact.Name = reader("Name")
                        contact.Email = reader("Email")
                        contact.Phone = reader("Phone")
                        contact.IsActive = reader("IsActive")
                    End If
                End Using
            End Using
        End Using

        Return contact
    End Function

    Public Function UpdateContact(contact As Contact) As Boolean
        Using connection As New SQLiteConnection(SQLiteHelper.DBPath)
            connection.Open()

            Dim updateQuery As String = "UPDATE Contact SET Name=@Name, Email=@Email, Phone=@Phone,IsActive=@IsActive WHERE Id=" & contact.Id
            Using command As New SQLiteCommand(updateQuery, connection)
                command.Parameters.AddWithValue("@Name", contact.Name)
                command.Parameters.AddWithValue("@Email", contact.Email)
                command.Parameters.AddWithValue("@Phone", contact.Phone)
                command.Parameters.AddWithValue("@IsActive", contact.IsActive)
                command.ExecuteNonQuery()
            End Using
        End Using

        Return False
    End Function

    Public Function SearchContact(Name As String, isActive As Integer) As List(Of Contact)

        Dim contacts As New List(Of Contact)

        Using connection As New SQLiteConnection(SQLiteHelper.DBPath)
            connection.Open()

            Dim selectQuery As String = "SELECT Id,Name, Email, Phone, IsActive FROM Contact WHERE Name LIKE @SearchName AND IsActive =@IsActive;"
            Dim searchPattern As String = "%" & Name & "%"


            Using command As New SQLiteCommand(selectQuery, connection)
                ' Add parameter to the command
                command.Parameters.AddWithValue("@SearchName", searchPattern)
                command.Parameters.AddWithValue("@IsActive", isActive)

                ' Execute the query and process results
                Using reader As SQLiteDataReader = command.ExecuteReader()
                    While reader.Read()
                        contacts.Add(New Contact() With {.Id = reader("Id"), .Name = reader("Name"), .Email = reader("Email"), .Phone = reader("Phone"), .IsActive = reader("IsActive")})
                    End While
                End Using
            End Using
        End Using
        Return contacts

    End Function

End Class
