namespace ApiActividadesMusicales.Model
{
    public class Actividad
    {
        public int IdActividad { get; set; }
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public int TipoActividadId { get; set; }
        public string? Lugar { get; set; }
    }
}
