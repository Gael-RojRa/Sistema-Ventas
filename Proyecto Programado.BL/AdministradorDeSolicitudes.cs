using Proyecto_Programado.DA;
using Proyecto_Programado.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_Programado.BL
{
    public class AdministradorDeSolicitudes : IAdministradorDeSolicitudes
    {
        public DBContexto ElContexto;

        public AdministradorDeSolicitudes(DBContexto elContexto)
        {
            ElContexto = elContexto;
        }

        

        public bool SoliciteRegistro(string nombreUsuario, string email, string clave)
        {
            bool solicitudExitosa = false;
            try
            {
                SolicitudRegistro laNuevaSolicitud = new SolicitudRegistro();

                laNuevaSolicitud.Nombre = nombreUsuario;
                laNuevaSolicitud.correoElectronico = email;
                laNuevaSolicitud.Clave = clave;
                laNuevaSolicitud.EstadoRegistro = EstadoRegistro.Pendiente;
                laNuevaSolicitud.FechaSolicitud = DateTime.Now;
                

                ElContexto.SolicitudRegistro.Add(laNuevaSolicitud);
                ElContexto.SaveChanges();

                //NotificarAdministrador(laNuevaSolicitud);

                solicitudExitosa = true;

                return solicitudExitosa;
            }

            catch (Exception laExcepcion)
            {
                return solicitudExitosa;
            }
            
        }
        public void NotificarAdministrador(SolicitudRegistro solicitud)
        {
            string adminEmail = "vheinner@gmail.com";
            string subject = "Nueva solicitud de registro";
            string body = $"Se ha recibido una nueva solicitud de registro de {solicitud.Nombre} ({solicitud.correoElectronico}).";

            //EnvieElCorreoElectronico(adminEmail, subject, body);
        }

        public void EnvieElCorreoElectronico(string elDestinatario, string elAsunto, string elContenido)
        {
            try
            {
                var elSmtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("comercionoreply@gmail.com", "mbry uqen cudv mmxn"),
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
        public bool AprobarSolicitud(int solicitudId)
        {
            var solicitud = ElContexto.SolicitudRegistro.Find(solicitudId);

            if (solicitud == null)
            {
                return false;
            }


            solicitud.EstadoRegistro = EstadoRegistro.Aprobado;
            ElContexto.SaveChanges();


            var nuevoUsuario = new Usuario
            {
                Nombre = solicitud.Nombre,
                correoElectronico = solicitud.correoElectronico,
                Clave = solicitud.Clave,
                rol = Rol.UsuarioNormal
            };

            ElContexto.Usuario.Add(nuevoUsuario);
            ElContexto.SaveChanges();

            return true;
        }
        public void Active(int id)
        {
            Model.SolicitudRegistro solicitud;
            solicitud = ObtengaLaSolicitud(id);
            solicitud.EstadoRegistro = EstadoRegistro.Aprobado;
            ElContexto.SolicitudRegistro.Update(solicitud);
            ElContexto.SaveChanges();

        }
        public Model.SolicitudRegistro ObtengaLaSolicitud(int id)
        {
            List<Model.SolicitudRegistro> lista;
            lista = ObtengaLaLista();
            foreach (var solicitud in lista)
            {
                if (solicitud.Id == id)
                    return solicitud;
            }

            return null;

        }

        public List<SolicitudRegistro> ObtengaLaListaDeSolicitudes()
        {

            var laListaDeSolicitudes = ElContexto.SolicitudRegistro.ToList();

            return laListaDeSolicitudes;
        }


        public List<Model.SolicitudRegistro> ObtengaLaLista()
        {

            var resultado = from c in ElContexto.SolicitudRegistro
                            select c;
            return resultado.ToList();


        }

        public List<Model.SolicitudRegistro> ObtengaLaListaDePendientes()
        {
            var lista = from item in ObtengaLaLista()
                        where item.EstadoRegistro == EstadoRegistro.Pendiente
                        select item;
            return (List<Model.SolicitudRegistro>)lista.ToList();
        }
    }
}
