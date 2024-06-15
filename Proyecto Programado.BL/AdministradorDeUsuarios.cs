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

        public AdministradorDeUsuarios(DBContexto elContexto)
        {
            ElContexto = elContexto;
        }

        public bool RegistreElUsuario(string elNombre, string elCorreo, string laClave)
        {
            bool lasCredencialesCorrectas = false;
            try
            {
                Usuario elUsuarioNuevo = new Usuario();

                elUsuarioNuevo.Nombre = elNombre;
                elUsuarioNuevo.correoElectronico = elCorreo;
                elUsuarioNuevo.Clave = laClave;
                elUsuarioNuevo.rol = Rol.UsuarioNormal;

                ElContexto.Usuario.Add(elUsuarioNuevo);
                ElContexto.SaveChanges();

                string elAsunto = "Solicitud de creación de usuario.";
                string elContenido = "Cuenta de usuario creada satisfactoriamente para el usuario " + elUsuarioNuevo.Nombre;

                //EnvieCorreoElectronico(usuarioNuevo.correoElectronico, asunto, contenido);

                lasCredencialesCorrectas = true;

                return lasCredencialesCorrectas;
            }

            catch (Exception laExcepcion)
            {
                return lasCredencialesCorrectas;
            }
        }

        public void EnvieElCorreoElectronico(string elDestinatario, string elAsunto, string elContenido)
        {
            try
            {
                var elSmtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("comercionoreply@gmail.com", "ogobqqzaxkvpriic"),
                    EnableSsl = true,
                };

                var elMensajeDeCorreo = new MailMessage
                {
                    From = new MailAddress("comercionoreply@gmail.com"),
                    Subject = elAsunto,
                    Body = elContenido
                };

                elMensajeDeCorreo.To.Add(elDestinatario);

                elSmtpClient.Send(elMensajeDeCorreo);
            }
            catch (Exception laExcepcion)
            {
                throw new Exception($"Error al enviar el correo electrónico: {laExcepcion.Message}", laExcepcion);
            }
        }

        public bool VerifiqueCredenciales(string elNombre, string laClave)
        {
            var elUsuario = ElContexto.Usuario.FirstOrDefault(elElemento => elElemento.Nombre == elNombre);

            if (elUsuario == null)
                return false;

            if (elUsuario.EstaBloqueado && elUsuario.TiempoDesbloqueo > DateTime.Now)
            {
                string elAsunto = $"Intento de inicio de sesión del usuario {elUsuario.Nombre} bloqueado.";
                string elContenido = $"Le informamos que la cuenta del usuario {elUsuario.Nombre} se encuentra bloqueada por 10 minutos. Por favor ingrese el día {elUsuario.TiempoDesbloqueo?.ToString("dd/MM/yyyy")} a las {elUsuario.TiempoDesbloqueo?.ToString("HH:mm")}.";

                EnvieElCorreoElectronico(elUsuario.correoElectronico, elAsunto, elContenido);

                return false;
            }

            bool laCoincidenciaDeCredenciales = elUsuario.Clave == laClave;

            if (laCoincidenciaDeCredenciales)
            {
                elUsuario.IntentosFallidos = 0;
                elUsuario.EstaBloqueado = false;
                elUsuario.TiempoDesbloqueo = null;

                ElContexto.SaveChanges();

                string elDestinatario = elUsuario.correoElectronico;
                string elAsunto = "Inicio de sesión del usuario " + elNombre;
                string elCuerpo = "Usted inició sesión el día " + DateTime.Now.ToString("dd/MM/yyyy") + " a las " + DateTime.Now.ToString("HH:mm");

                //EnvieCorreoElectronico(destinatario, elAsunto, elCuerpo);

                return true;
            }
            else
            {
                elUsuario.IntentosFallidos++;

                if (elUsuario.IntentosFallidos >= 3)
                {
                    elUsuario.EstaBloqueado = true;
                    elUsuario.TiempoDesbloqueo = DateTime.Now.AddMinutes(10);

                    string elAsunto = $"Usuario Bloqueado.";
                    string elContenido = $"Le informamos que la cuenta del usuario {elUsuario.Nombre} se encuentra bloqueada por 10 minutos. Por favor ingrese el día {elUsuario.TiempoDesbloqueo?.ToString("dd/MM/yyyy")} a las {elUsuario.TiempoDesbloqueo?.ToString("HH:mm")}.";

                    EnvieElCorreoElectronico(elUsuario.correoElectronico, elAsunto, elContenido);
                }

                ElContexto.SaveChanges();

                return false;
            }
        }

        public Usuario ObtengaElUsuarioPorNombre(string elNombre)
        {
            return ElContexto.Usuario.SingleOrDefault(elElemento => elElemento.Nombre == elNombre);
        }

        public Rol ObtengaElRolDelUsuario(string elNombre)
        {
            var elUsuario = ElContexto.Usuario.SingleOrDefault(elElemento => elElemento.Nombre == elNombre);
            return elUsuario.rol;
        }

        public void CambieLaClave(Usuario elUsuario, string laClaveNueva)
        {
            elUsuario.Clave = laClaveNueva;
            ElContexto.SaveChanges();
        }

    }
}
