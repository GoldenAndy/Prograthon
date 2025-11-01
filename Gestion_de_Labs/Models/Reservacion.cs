namespace Gestion_de_Labs.Models
{
    public class Reservacion
    {
        public int Reserva_Id { get; set; }

        public int Usuario_Id { get; set; }
        public int Laboratorio_Id { get; set; }

        public DateOnly Fecha { get; set; }

        public TimeOnly Hora { get; set; }

        public Usuario? Usuario { get; set; }

        public Laboratorio? Laboratorio { get; set; }
    }
}


//Table: PROGRATHON_Reserva
//Columns:
//Reserva_Id int(11) AI PK 
//Usuario_Id int(11) 
//Laboratorio_Id int(11) 
//Fecha date 
//Hora time
