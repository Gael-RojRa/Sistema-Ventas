using Proyecto_Programado.DA;
using Proyecto_Programado.Model;
using System.Net.Mail;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto_Programado.BL
{
    public class AdministradorDeUsuarios : IAdministradorDeUsuarios
    {
        public DBContexto ElContexto;

        public AdministradorDeUsuarios(DBContexto contexto)
        {
            ElContexto = contexto;
        }

        public bool RegistrarUsuario(string nombre, string correo, string clave)
        {
            bool CredencialesCorrectas = false;
            try
            {
                Usuario usuarioNuevo = new Usuario();

                usuarioNuevo.Nombre = nombre;
                usuarioNuevo.correoElectronico = correo;
                usuarioNuevo.Clave = clave;
                usuarioNuevo.rol = Rol.UsuarioNormal;

                ElContexto.Usuario.Add(usuarioNuevo);
                ElContexto.SaveChanges();

                string asunto = "Solicitud de creación de usuario.";
                string contenido = "Cuenta de usuario creada satisfactoriamente para el usuario " + usuarioNuevo.Nombre;

                //EnvieCorreoElectronico(usuarioNuevo.correoElectronico, asunto, contenido);

                CredencialesCorrectas = true;

                return CredencialesCorrectas;
            }

            catch (Exception ex)
            {
                return CredencialesCorrectas;
            }
        }

        public void EnvieCorreoElectronico(string destinatario, string asunto, string contenido)
        {
            try
            {
                // Configura el cliente SMTP de Gmail
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("comercionoreply@gmail.com", "ogobqqzaxkvpriic"),
                    EnableSsl = true,
                };

                // Crea el correo electrónico
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("comercionoreply@gmail.com"),
                    Subject = asunto,
                    Body = contenido
                };

                // Agrega el destinatario
                mailMessage.To.Add(destinatario);

                // Envía el correo electrónico
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                // Puedes manejar el error aquí o lanzarlo para que sea manejado por el código que llama a este método.
                throw new Exception($"Error al enviar el correo electrónico: {ex.Message}", ex);
            }
        }

        public bool VerifiqueCredenciales(string nombre, string clave)
        {
            var usuario = ElContexto.Usuario.FirstOrDefault(x => x.Nombre == nombre);

            if (usuario == null)
                return false;

            if (usuario.EstaBloqueado && usuario.TiempoDesbloqueo > DateTime.Now)
            {
                string asunto = $"Intento de inicio de sesión del usuario {usuario.Nombre} bloqueado.";
                string contenido = $"Le informamos que la cuenta del usuario {usuario.Nombre} se encuentra bloqueada por 10 minutos. Por favor ingrese el día {usuario.TiempoDesbloqueo?.ToString("dd/MM/yyyy")} a las {usuario.TiempoDesbloqueo?.ToString("HH:mm")}.";

                EnvieCorreoElectronico(usuario.correoElectronico, asunto, contenido);

                return false;
            }

            bool CoincidenLasCredenciales = usuario.Clave == clave;

            if (CoincidenLasCredenciales)
            {
                usuario.IntentosFallidos = 0;
                usuario.EstaBloqueado = false;
                usuario.TiempoDesbloqueo = null;

                ElContexto.SaveChanges();

                string destinatario = usuario.correoElectronico;
                string elAsunto = "Inicio de sesión del usuario " + nombre;
                string elCuerpo = "Usted inició sesión el día " + DateTime.Now.ToString("dd/MM/yyyy") + " a las " + DateTime.Now.ToString("HH:mm");

                //EnvieCorreoElectronico(destinatario, elAsunto, elCuerpo);

                return true;
            }
            else
            {
                usuario.IntentosFallidos++;

                if (usuario.IntentosFallidos >= 3)
                {
                    usuario.EstaBloqueado = true;
                    usuario.TiempoDesbloqueo = DateTime.Now.AddMinutes(10);

                    string asunto = $"Usuario Bloqueado.";
                    string contenido = $"Le informamos que la cuenta del usuario {usuario.Nombre} se encuentra bloqueada por 10 minutos. Por favor ingrese el día {usuario.TiempoDesbloqueo?.ToString("dd/MM/yyyy")} a las {usuario.TiempoDesbloqueo?.ToString("HH:mm")}.";

                    EnvieCorreoElectronico(usuario.correoElectronico, asunto, contenido);
                }

                ElContexto.SaveChanges();

                return false;
            }
        }

        public Usuario ObtenerUsuarioPorNombre(string nombre)
        {
            return ElContexto.Usuario.SingleOrDefault(u => u.Nombre == nombre);
        }

        public Rol ObtengaElRolDelUsuario(string nombre)
        {
            var ElUsuario = ElContexto.Usuario.SingleOrDefault(u => u.Nombre == nombre);
            return ElUsuario.rol;
        }

        public void CambiarClave(Usuario usuario, string claveNueva)
        {
            usuario.Clave = claveNueva;
            ElContexto.SaveChanges();
        }

    }
}
