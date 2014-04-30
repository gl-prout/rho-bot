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
        private Database database;
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
            ListeDesProcessus.Text += Application.LocalUserAppDataPath + "\n";
            ListeDesProcessus.Text += Application.ExecutablePath + "\n";
            ListeDesProcessus.Text += Application.StartupPath + "\n";
            database = new Database(ref this.ListeDesProcessus);
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
    }   
}
