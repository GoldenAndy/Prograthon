namespace Gestion_de_Labs.Models
{
    public class Usuario
    {
        public int Usuario_Id { get; set; }

        public string Nombre { get; set; }

        public int Tipo_Usuario_Id { get; set; }

        public string Tipo_Usuario_Nombre
        {
            get
            {
                switch (Especialidad)
                {
                    case 1:return "Estudiante";
                    case 2:return "Profesor";
                }
}
        }

        public string Correo { get; set; }

    }
}
