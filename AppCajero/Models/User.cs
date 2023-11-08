using System;
using System.Collections.Generic;

namespace AppCajero.Models;

public partial class User
{
    public int UserId { get; set; }

    public string NroTarjeta { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Username { get; set; } = null!;

    public int Saldo { get; set; } = 0!;

    public bool Bloqueado { get; set; }

    public int Cant {  get; set; }

    public DateTime FechaVenc {  get; set; }

    public ICollection<Operacion> Operaciones { get; set; } = null!;
}
