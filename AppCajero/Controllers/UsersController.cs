using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AppCajero.Models;

namespace AppCajero.Controllers
{
    public class UsersController : Controller
    {
        private readonly DbcajeroContext _context;

        public UsersController(DbcajeroContext context)
        {
            _context = context;
        }

        // GET: Users/Ingreso
        public IActionResult Ingreso()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ingreso(string nroTarjeta)
        {
            if (_context.Users.Any(u => u.NroTarjeta == nroTarjeta))
            {
                var user = _context.Users.FirstOrDefault(u => u.NroTarjeta == nroTarjeta);
                if (user != null && user.Bloqueado != true)
                    return View("Login", user);
                else
                {
                    ViewData["ErrorMessage"] = "La tarjeta esta bloqueada.";
                    return View("ErrorIngreso");
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "El nro de la tarjeta no es valido.";
                return View("ErrorIngreso");
            }
        }


        // GET: Users/Login
        public IActionResult Login(User user)
        {
            return View(user);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string nroTarjeta, string Password)
        {

            if (_context.Users.Any(u => u.NroTarjeta == nroTarjeta && u.Password == Password))
            {
                var user = _context.Users.FirstOrDefault(u => u.NroTarjeta == nroTarjeta && u.Password == Password);
                return View("Operaciones", user);
            }
            else
            {
                ViewData["ErrorMessage"] = "La contraseña no es correcta.";
                var user = _context.Users.FirstOrDefault(u => u.NroTarjeta == nroTarjeta);
                if (user != null)
                {
                    user.Cant += 1; // Sumar uno al campo "cant" del usuario
                    if (user.Cant == 4)
                    {
                        user.Bloqueado = true;
                        ViewData["ErrorMessage"] = "Ha ingresado 4 veces una contraseña incorrecta.\n Usuario BLOQUEADO";
                        _context.SaveChanges(); // Guardar los cambios en la base de datos
                        return View("Error", user);
                    }
                    _context.SaveChanges(); // Guardar los cambios en la base de datos
                }

                return View("Login", user);
            }
        }


        // GET: Users/Operaciones
        public IActionResult Operaciones(User user)
        {
             // Realiza una consulta para obtener un usuario (puedes personalizar esto según tus necesidades)
            if (user != null)
            {
                return View(user);
            }
            else
            {
                // Maneja el caso en el que no se encuentra ningún usuario en la base de datos
                ViewData["ErrorMessage"] = "No se encontró ningún usuario en la base de datos.";
                return View();
            }
        }
        

        // GET: Users/Balance
        public async Task<IActionResult> Balance(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            Operacion operacion = new Operacion
            {
                UserID = user.UserId,
                TipoOperacion = "Balance",
                FechaOperacion = DateTime.Now,
                CantidadDinero = user.Saldo
            };
            _context.Operaciones.Add(operacion);
            _context.SaveChanges();
            return View(user);
        }


        // GET: Users/Retiro
        public async Task<IActionResult> Retiro(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        // POST: Users/Aceptar

        [HttpPost]
        public IActionResult Aceptar(int userId, int monto)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound();
            }

            if (monto <= user.Saldo)
            {
                user.Saldo -= monto;
                _context.SaveChanges();

                // Registro de operación
                Operacion operacion = new Operacion
                {
                    UserID = user.UserId,
                    TipoOperacion = "Retiro",
                    FechaOperacion = DateTime.Now,
                    CantidadDinero = monto
                };
                _context.Operaciones.Add(operacion);
                _context.SaveChanges();

                ViewData["Monto"] = monto;
                return View(user);
            }
            else
            {
                TempData["MensajeError"] = "Saldo insuficiente para realizar el retiro. Ingrese otro monto menor";
                return View("Retiro", user);
            }
        }

    }
}
