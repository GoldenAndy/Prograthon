using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Gestion_de_Labs.Models
{
    [Table("PROGRATHON_Laboratorio")]
    public class Laboratorio
    {


        [Key]
        public int Laboratorio_Id { get; set; }
        public string? Nombre { get; set; }
        public int Capacidad { get; set; }
        public string? Responsable { get; set; }
    }
}