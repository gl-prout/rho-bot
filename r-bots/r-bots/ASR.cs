using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Windows.Forms;
using System.Speech.Recognition.SrgsGrammar;
using System.Diagnostics;

namespace r_bots
{
    class ASR
    {
        public SpeechRecognitionEngine ASREngine;
        private RichTextBox message;

        /// <summary>
        /// Constructeur de l'ASR (Automatic Speech Recognition)
        /// </summary>
        public ASR(ref RichTextBox messages)
        {
            this.message = messages;
            StartEngine();
        }

        /// <summary>
        /// Démarrage du moteur de reconnaissance vocale et chargement du
        /// fichier de grammaire Grammaire.grxml
        /// </summary>
        private void StartEngine()
        {
            try
            {
                //Création d'un document de la norme SRGS à partir du fichier grxml
                SrgsDocument xmlGrammar = new SrgsDocument(@"H:\Projet\2014\rho-bot\r-bots\r-bots\Grammaire.xml");
                //SrgsDocument xmlGrammar = new SrgsDocument(@"H:\dev_software\projet\rho-bot\r-bots\r-bots\Grammaire.xml");
                //Création d'une grammaire depuis le fichier de grammaire
                Grammar grammar = new Grammar(xmlGrammar);
                //Création de l'objet traitant la reconnaissance vocale
                ASREngine = new SpeechRecognitionEngine();
                //Récupération du son du microphone
                ASREngine.SetInputToDefaultAudioDevice();
                //Chargement de la grammaire
                ASREngine.LoadGrammar(grammar);
                //Link des fonctions a appeler en cas de reconnaissance d'un texte
                ASREngine.SpeechRecognized += ASREngine_SpeechRecognized;
                ASREngine.SpeechRecognitionRejected += ASREngine_SpeechRecognitionRejected;
                ASREngine.SpeechHypothesized += ASREngine_SpeechHypothesized;
                //Spécification du nombre maximum d'alternatives
                //Par exemple : b ou p ou d ou t, t ou d, i ou j, etc.
                //Utile pour les sons qui se ressemblent
                ASREngine.MaxAlternates = 10;
            }
            catch (Exception ex)
            {
                this.message.Text += ex.Message + "\n";
            }
        }

        /// <summary>
        /// Méthode utilisée lorsque la reconnaissance vocale est en cours
        /// </summary>
        private void ASREngine_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
        }

        /// <summary>
        /// Méthode utilisée lorsque la reconnaissance vocale a échoué
        /// </summary>
        private void ASREngine_SpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
        {
            this.message.Text += "Reconnaissance impossible\n";
        }

        /// <summary>
        /// Méthode utilisée lorsque la reconnaissance vocale est réussi
        /// </summary>
        private void ASREngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            this.message.Text += "Texte reconnu: "+e.Result.Text + "\n";
            //Récupération de la commande de base utilisée (QUIT ou LEARN)
            string baseCommand = e.Result.Semantics["mouskie"].Value.ToString();
            this.message.Text += "Commande reçu: " +baseCommand+"\n";
            if (baseCommand.Equals("QUIT"))
            {
                //Environment.Exit(0);
            }
            else if (baseCommand.Equals("RESTART"))
            {
                //Process.Start("shutdown", "/r /t 0");
            }
            else if (baseCommand.Equals("HOUR"))
            {
                Synthese reponse = new Synthese();
                //reponse.Parler("il est " + DateTime.Now.Hour + " heures " + DateTime.Now.Minute + " minutes et " + DateTime.Now.Second + " Seconde");
            }
            else if (baseCommand.Equals("MESSAGE"))
            {
                this.nouveauMail();
            }
            else if (baseCommand.Equals("LEARN"))
            {
                string dataType = e.Result.Semantics["data_type"].Value.ToString();
                this.message.Text += " " + dataType + "\n";
                string node = "";
                //Choix du noeud en fonction de la commande trouvée
                if (dataType.Equals("NUMBER"))
                    node = "numbers";
                else if (dataType.Equals("LETTER"))
                    node = "letters";
                try
                {   //Parcours des alternatives pour toutes les afficher
                    for (int i = 0; i < e.Result.Alternates.ToArray().Length; i++)
                    {
                        string found = e.Result.Alternates.ToArray()[i].Semantics["data_type"][node].Value.ToString();
                        if (i != 0)
                            this.message.Text += " ou ";
                        this.message.Text += found;
                    }
                    this.message.Text += "\n";
                }
                catch { }
            }
        }

        private void nouveauMail()
        {
            Synthese reponse = new Synthese();
            this.message.Text += "Envoie requete de demande de nouveau mail\n";
            this.message.Text += "====================================\n";
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
                this.message.Text += "Données reçu " + NumberOfMails + " nouveaux mails\n";
                reponse.Parler("Vous avez " + NumberOfMails + " nouveaux mail");
                DemoClient.Disconnect();
            }
            catch (Exception ex)
            {
                this.message.Text += "ERREUR " + ex.Message + "\n";
            }
        }
    }
}

