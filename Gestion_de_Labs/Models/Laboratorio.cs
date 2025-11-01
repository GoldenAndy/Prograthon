using System.ComponentModel.DataAnnotations;
namespace Gestion_de_Labs.Models
{

    public class Laboratorio
    {


        [Key]
        public int Laboratorio_Id { get; set; }
        public string Nombre { get; set; }
        public int Capacidad { get; set; }
        public string Responsable { get; set; }
    }
}