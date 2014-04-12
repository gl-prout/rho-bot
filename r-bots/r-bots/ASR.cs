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
        private Label recoText;
        private Label commandText;
        private Label devine;
        private Label affiche;

        /// <summary>
        /// Constructeur de l'ASR (Automatic Speech Recognition)
        /// </summary>
        public ASR(ref Label recoText, ref Label commandText, ref Label devine, ref Label affiche)
        {
            //Les 4 labels dont le texte devra être changé
            //en fonction de ce qui est reconnu
            this.recoText = recoText;
            this.commandText = commandText;
            this.devine = devine;
            this.affiche = affiche;
            //Démarrage du moteur de reconnaissance vocale
            StartEngine();
        }

        /// <summary>
        /// Démarrage du moteur de reconnaissance vocale et chargement du
        /// fichier de grammaire Grammaire.grxml
        /// </summary>
        private void StartEngine()
        {
            //Création d'un document de la norme SRGS à partir du fichier grxml
            SrgsDocument xmlGrammar = new SrgsDocument("Grammaire.grxml");
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
        }

        /// <summary>
        /// Méthode utilisée lorsque la reconnaissance vocale est réussi
        /// </summary>
        private void ASREngine_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
        }
    }
}
