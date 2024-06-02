using Proyecto_Programado.DA;
using Proyecto_Programado.Model;
using System.Net.Mail;
using System.Net;

namespace Proyecto_Programado.BL
{
    public class AdministradorDeUsuarios : IAdministradorDeUsuarios
    {
        public DBContexto ElContexto;

        public AdministradorDeUsuarios(DBContexto contexto)
        {
            ElContexto = contexto;
        }

        public int RegistrarUsuario(string nombre, string correo, string clave)
        {
            Usuario usuarioNuevo = new Usuario();

            usuarioNuevo.IdUsuario = 0;
            usuarioNuevo.Nombre = nombre;
            usuarioNuevo.correoElectronico = correo;
            usuarioNuevo.Clave = clave;
            usuarioNuevo.rol = Rol.UsuarioNormal;

            ElContexto.Usuario.Add(usuarioNuevo);
            ElContexto.SaveChanges();

            string asunto = "Solicitud de creación de usuario.";
            string contenido = "Cuenta de usuario creada satisfactoriamente para el usuario " + usuarioNuevo.Nombre;

            EnvieCorreoElectronico(usuarioNuevo.correoElectronico, asunto, contenido);


            return usuarioNuevo.IdUsuario;
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
            bool CoincidenLasCredenciales;

            var resultado = ElContexto.Usuario.ToList();

            CoincidenLasCredenciales = resultado.Any(x => x.Nombre == nombre && x.Clave == clave);

            if (CoincidenLasCredenciales)
            {
                var usuario = resultado.First(x => x.Nombre == nombre);

                string destinatario = usuario.correoElectronico;
                string elAsunto = "Inicio de sesión del usuario " + nombre;
                string elCuerpo = "Usted inicio sesión el día " + DateTime.Now.ToString("dd/MM/yyyy") + " a las " + DateTime.Now.ToString("hh:mm");

                EnvieCorreoElectronico(destinatario, elAsunto, elCuerpo);
            }

            return CoincidenLasCredenciales;
        }
    }
}
