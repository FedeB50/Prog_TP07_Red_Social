namespace TP07_Barg.Models;

public class Comentarios
{
    public int id { get; set; }
    public int idPublicacion { get; set; }
    public string username { get; set; }
    public string texto { get; set; }
    public DateTime fechaPublicacion { get; set; }
}