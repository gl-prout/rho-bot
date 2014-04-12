using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Windows.Forms;
using System.Speech.Recognition.SrgsGrammar;

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
                SrgsDocument xmlGrammar = new SrgsDocument("H:\\Projet\\2014\\rho-bot\\r-bots\\Grammaire.xml");
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
                ASREngine.MaxAlternates = 4;
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
            this.message.Text += "Commande reçu: " +baseCommand;
            if (baseCommand.Equals("QUIT"))
                Environment.Exit(0);
            else if (baseCommand.Equals("LEARN"))
            {
                string dataType = e.Result.Semantics["data_type"].Value.ToString();
                this.message.Text += " " + dataType+"\n";
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
    }
}
