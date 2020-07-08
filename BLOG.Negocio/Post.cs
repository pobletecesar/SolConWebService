using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLOG.Datos;
using BLOG.Entidades;

namespace BLOG.Negocio
{
    public class Post
    {
        private Datos.BLOGEntities conn = new BLOGEntities();

        public bool Crear(Entidades.Post oPost)
        {
            try
            {
                Datos.POST dPost = new POST();
                dPost.titulo_post = oPost.titulo_post;
                dPost.texto_post = oPost.texto_post;
                conn.POST.Add(dPost);
                conn.SaveChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }

        }

        public List<Entidades.Post> ListarTodo()
        {
            try
            {
                List<Entidades.Post> lPost = new List<Entidades.Post>();
                List<Datos.POST> dPost = conn.POST.ToList();
                foreach(POST dat in dPost)
                {
                    Entidades.Post p = new Entidades.Post();
                    p.id_post = dat.id_post;
                    p.titulo_post = dat.titulo_post;
                    p.texto_post = dat.texto_post;
                    lPost.Add(p);
                }
                return lPost;

            }
            catch(Exception e)
            {
                return new List<Entidades.Post>();
            }
        }





    }
}
