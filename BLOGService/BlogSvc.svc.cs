using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using BLOG.Entidades;
using BLOG.Negocio;
using System.Xml.Serialization;
using System.IO;


namespace BLOGService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "BlogSvc" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select BlogSvc.svc or BlogSvc.svc.cs at the Solution Explorer and start debugging.
    public class BlogSvc : IBlogSvc
    {
        public bool Crear(string xmlPost)
        {
            XmlSerializer xmlSerial = new XmlSerializer(typeof(BLOG.Entidades.Post));
            StringReader xmlReader = new StringReader(xmlPost);
            BLOG.Entidades.Post oPost = (BLOG.Entidades.Post)xmlSerial.Deserialize(xmlReader);
            BLOG.Negocio.Post nPost = new BLOG.Negocio.Post();
            if (nPost.Crear(oPost))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ListarTodo()
        {
            List<BLOG.Entidades.Post> lPost = new List<BLOG.Entidades.Post>();
            XmlSerializer xmlSerial = new XmlSerializer(typeof(List<BLOG.Entidades.Post>));
            StringWriter xmlWrite = new StringWriter();
            BLOG.Negocio.Post nPost = new BLOG.Negocio.Post();
            lPost = nPost.ListarTodo();
            xmlSerial.Serialize(xmlWrite, lPost);
            return xmlWrite.ToString();
        }
    }
}
