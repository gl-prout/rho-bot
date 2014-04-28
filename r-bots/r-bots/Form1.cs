using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Recognition;

namespace r_bots
{
    public partial class Form1 : Form
    {
        private ASR ASR;
        private Webcam camera;
        public Form1()
        {
            InitializeComponent();
            ASR = new ASR(ref this.ListeDesProcessus);
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            ListeDesProcessus.Text += "Programme initialisé...\n";

            //* DEBUT CODE FETRA /
            ListeDesProcessus.Text += "Initialisation de la camera...\n";
            try
            {
                camera = new Webcam(ref Camera1, ref Camera2, ref ListeDesProcessus);
                camera.start();
                ListeDesProcessus.Text += "Camera initialise...\n";
            }
            catch (Exception ex)
            {
                ListeDesProcessus.Text += "Erreur: " + ex.Message + "\n";
            }
            //* FIN CODE FETRA */

            //* DEBUT CODE MAHERY /
            ListeDesProcessus.Text += "Démarrage de la reconnaissance vocale...\n";
            try
            {
                //Activation de la reconnaissance vocale pour une commande
                ASR.ASREngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                ListeDesProcessus.Text += ex.Message + "\n";
            }

            //* FIN CODE MAHERY */

            
        }

        private void nouveauMail()
        {
            Synthese reponse = new Synthese();
            ListeDesProcessus.Text += "Envoie requete de demande de nouveau mail\n";
            ListeDesProcessus.Text += "====================================\n";
            try
            {
                //prepare pop client
                // TODO: Replace username and password with your own credentials.
                Pop3MailClient DemoClient = new Pop3MailClient("pop.gmail.com", 995, true, "projet.rhobots@gmail.com", "projet2014");
                DemoClient.IsAutoReconnect = true;

                //remove the following line if no tracing is needed
                DemoClient.Trace += new TraceHandler(Console.WriteLine);
                DemoClient.ReadTimeout = 60000; //give pop server 60 seconds to answer

                //establish connection
                DemoClient.Connect();

                //get mailbox statistics
                int NumberOfMails, MailboxSize;
                DemoClient.GetMailboxStats(out NumberOfMails, out MailboxSize);
                ListeDesProcessus.Text += "Données reçu " + NumberOfMails + " nouveaux mails\n";
                reponse.Parler("Vous avez " + NumberOfMails + " nouveaux mail");


                //close connection
                DemoClient.Disconnect();
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            nouveauMail();
        }
    }
}
