namespace TP07_Barg.Models;
using Microsoft.Data.SqlClient;
using Dapper;
using TP07_Barg.Models;
public class DB
{
    string _connectionString = @"Server=localhost;DataBase=DBRedSocial;Integrated Security=True;TrustServerCertificate=True;";
    
    public void Registrar(string username, string password, string nombre, string apellido)
    {
        string query = "INSERT INTO Usuarios (username, password, nombre, apellido) VALUES (@Username, @Password, @Nombre, @Apellido)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { Username = username, Password = password, Nombre = nombre, Apellido = apellido });
        }
    }
    public Usuarios getUsuario(string username)
    {
        Usuarios usuario = new Usuarios();
        string query = "SELECT * FROM Usuarios WHERE username = @Username";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            usuario = connection.QueryFirstOrDefault<Usuarios>(query, new { Username = username });
        }
        return usuario;
    }
    public bool getUsername(string username)
    {
        string usuario;
        string query = "SELECT username FROM Usuarios WHERE username = @Username";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            usuario = connection.QueryFirstOrDefault<string>(query, new { Username = username });
        }
        if (usuario != null) {return true;}
        return false;
    }
    public bool getPassword(string username, string password)
    {
        string pass;
        string query = "SELECT password FROM Usuarios WHERE username = @Username";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            pass = connection.QueryFirstOrDefault<string>(query, new { Username = username });
        }
        if (pass == password) {return true;}
        return false;
    }
    public void EliminarCuenta(string username)
    {
        string query = "DELETE FROM Usuarios WHERE username = @Username";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { Username = username });
        }
    }

    public void CrearPublicacion(string imagen, int idUsuario, string descripcion, string titulo, DateTime fechaHora)
    {
        string query = "INSERT INTO Publicaciones (Imagen, IdUsuario, Descripcion, Titulo, FechaPublicacion) VALUES (@Imagen, @IdUsuario, @Descripcion, @Titulo, @FechaHora)";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { Imagen = imagen, IdUsuario = idUsuario, Descripcion = descripcion, Titulo = titulo, FechaPublicacion = fechaHora });
        }
    }
    
}
