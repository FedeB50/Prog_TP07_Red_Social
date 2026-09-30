using Microsoft.Data.SqlClient;
using Dapper;
using System.Collections.Generic;

namespace TP07_Barg.Models;

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

    public List<Publicaciones> getPublicaciones()
    {
        List<Publicaciones> publicaciones = new List<Publicaciones>();
        string query = "SELECT p.id, username, Titulo, Descripcion, Imagen, FechaPublicacion FROM Publicaciones p INNER JOIN Usuarios u ON p.IdUsuario = u.id ORDER BY FechaPublicacion DESC";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            publicaciones = connection.Query<Publicaciones>(query).ToList();
        }
        return publicaciones;
    }

    public void DarLike(int IdPublicacion, int IdUsuario)
    {
        if (VerificarLike(IdPublicacion, IdUsuario) <= 0)
        {
            string SQL = "INSERT INTO PublicacionesMeGusta (IdPublicación, IdUsuario) VALUES (@pIdPublicacion, @pIdUsuario)"; 
            using(SqlConnection db = new SqlConnection(_connectionString))
            {
                db.Execute(SQL, new {pIdPublicacion = IdPublicacion, pIdUsuario = IdUsuario} ); 
            }
        }
        else
        {
            SacarLike(IdPublicacion, IdUsuario);
        }
        return;        
    }

    public void SacarLike(int IdPublicacion, int IdUsuario)
    {
        string SQL = "DELETE FROM PublicacionesMeGusta WHERE IdPublicación=@pIdPublicacion AND IdUsuario=@pIdUsuario"; 
        using(SqlConnection db = new SqlConnection(_connectionString))
        {
            db.Execute(SQL, new {pIdPublicacion = IdPublicacion, pIdUsuario = IdUsuario} ); 
        }
    }

    public int VerificarLike(int IdPublicacion, int IdUsuario)
    {
        string SQL = "SELECT COUNT(id) FROM PublicacionesMeGusta WHERE IdPublicación=@pIdPublicacion AND IdUsuario=@pIdUsuario"; 
        int likes = 0;
        using(SqlConnection db = new SqlConnection(_connectionString))
        {
            likes += db.QueryFirstOrDefault<int>(SQL, new {pIdPublicacion = IdPublicacion, pIdUsuario = IdUsuario} ); 
        }
        return likes;
    }

    public int GetLikes(int IdPublicacion)
    {
        int Likes = 0;
        string SQL = "SELECT COUNT(id) FROM PublicacionesMeGusta WHERE IdPublicación=@pIdPublicacion"; 
        using(SqlConnection db = new SqlConnection(_connectionString))
        {
            Likes = db.QueryFirstOrDefault<int>(SQL, new {pIdPublicacion = IdPublicacion} ); 
        } 
        return Likes;
    }

    public List<Comentarios> getComentarios()
    {
        List<Comentarios> comentarios = new List<Comentarios>();
        string query = "SELECT c.id, idPublicacion, username, Texto, FechaComentario FROM Comentarios c INNER JOIN Usuarios u ON IdUsuarioComenta = u.id ORDER BY FechaComentario DESC";
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            comentarios = connection.Query<Comentarios>(query).ToList();
        }
        return comentarios;
    }
}
