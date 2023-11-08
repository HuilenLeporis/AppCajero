namespace AppCajero.Models
{
    public class Operacion
    {
        public int OperacionID { get; set; }
        public int UserID { get; set; }
        public string TipoOperacion { get; set; } = null!;
        public DateTime FechaOperacion { get; set; }
        public int CantidadDinero { get; set; }

        public User User { get; set; } = null!;
    }
}
