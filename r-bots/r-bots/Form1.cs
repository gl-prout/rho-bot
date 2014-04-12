using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace r_bots
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void quitterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListeDesProcessus.Text += "Programme initialisé...\n";
            
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
