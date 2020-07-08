using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BLOG.Entidades;
using System.Xml.Serialization;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;

namespace AppConWebService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LlenarGrilla();
        }

        private void LlenarGrilla()
        {
            XmlSerializer xmlSerial = new XmlSerializer(typeof(List<Post>));
            ServiceBlog.BlogSvcClient svcPost = new ServiceBlog.BlogSvcClient();
            string xmlLista = svcPost.ListarTodo();

            StringReader xmlRead = new StringReader(xmlLista);

            List<Post> lPost = (List<Post>)xmlSerial.Deserialize(xmlRead);

            dgPost.ItemsSource = lPost;

        }

        private void ExportarPDF()
        {
            try
            {
                XmlSerializer xmlSerial = new XmlSerializer(typeof(List<Post>));
                ServiceBlog.BlogSvcClient svcPost = new ServiceBlog.BlogSvcClient();
                string xmlLista = svcPost.ListarTodo();

                StringReader xmlRead = new StringReader(xmlLista);

                List<Post> lPost = (List<Post>)xmlSerial.Deserialize(xmlRead);


                var pdfDoc = new Document(PageSize.LETTER, 40f, 40f, 60f, 60f);
                string filePath = $"C://PDFs//report{DateTime.Now.ToString("yyyyMMddHHmmss")}.pdf";
                PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.OpenOrCreate));
                pdfDoc.Open();

                var columns = 3;
                var colWidth = new[] { 1f, 2f, 4f };
                var tabla = new PdfPTable(colWidth)
                {
                    WidthPercentage = 100,
                    DefaultCell = { MinimumHeight = 22f },
                    HorizontalAlignment = 0
                };

                var celdaT = new PdfPCell(new Phrase("REPORTE DE POSTS"))
                {
                    HorizontalAlignment = 1,
                    MinimumHeight = 20f,
                    Colspan = 3
                };

                tabla.AddCell(celdaT);

                foreach(Post p in lPost)
                {
                    tabla.AddCell(p.id_post.ToString());
                    tabla.AddCell(p.titulo_post);
                    tabla.AddCell(p.texto_post);
                }

                pdfDoc.Add(tabla);
                pdfDoc.Close();

                MessageBox.Show("Reporte Generado");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void SendMail()
        {
            try
            {
                MailMessage mail = new MailMessage();

                var SmtpCliente = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential("", "")



                };

                mail.From = new MailAddress("cesar2kx@gmail.com");
                mail.To.Add("c.pobletes@profesor.duoc.cl");
                mail.To.Add("javie.castrom@alumnos.duoc.cl");
                mail.To.Add("jord.lopezr@alumnos.duoc.cl");
                mail.To.Add("j.carcamoh@alumnos.duoc.cl");
                mail.To.Add("j.carcamoh@alumnos.duoc.cl");
                mail.Subject = "REPORTE BLOG";
                mail.Body = "Adjuntamos reporte de su blog";

                string fileName = cboReportes.SelectedItem.ToString();
                System.Net.Mail.Attachment att = new Attachment($"C://PDFs//{fileName}");
                mail.Attachments.Add(att);

                SmtpCliente.Send(mail);

                lblReportes.Visibility = Visibility.Hidden;
                cboReportes.Visibility = Visibility.Hidden;
                btnMail.Visibility = Visibility.Hidden;
                MessageBox.Show("Mail Enviado");
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }
       
        private void BtnGrabar_Click(object sender, RoutedEventArgs e)
        {
            XmlSerializer xmlSerial = new XmlSerializer(typeof(Post));
            ServiceBlog.BlogSvcClient svcPost = new ServiceBlog.BlogSvcClient();
            StringWriter xmlWrite = new StringWriter();
            Post oPost = new Post();

            oPost.titulo_post = txtTitulo.Text;
            oPost.texto_post = txtPost.Text;

            xmlSerial.Serialize(xmlWrite, oPost);

            if(svcPost.Crear(xmlWrite.ToString()))
            {
                MessageBox.Show("Datos Grabados!!! XD");
                LlenarGrilla();
            }
            else
            {
                MessageBox.Show("Ups!! Ocurrió un error! No se grabaron los datos");
            }

        }

        private void BtnPDF_Click(object sender, RoutedEventArgs e)
        {
            ExportarPDF();
        }

        private void BtnEnviarR_Click(object sender, RoutedEventArgs e)
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\PDFs");
            FileInfo[] files = d.GetFiles("*.pdf");

            foreach(FileInfo f in files)
            {
                cboReportes.Items.Add(f.Name);
            }


            lblReportes.Visibility = Visibility.Visible;
            cboReportes.Visibility = Visibility.Visible;
            btnMail.Visibility = Visibility.Visible;
        }

        private void BtnMail_Click(object sender, RoutedEventArgs e)
        {
            SendMail();
        }
    }
}
