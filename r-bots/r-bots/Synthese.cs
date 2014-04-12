using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;

namespace r_bots
{
    class Synthese
    {
        private SpeechSynthesizer dire;
        private PromptBuilder texte;

        public Synthese()
        {
            dire = new SpeechSynthesizer();
        }

        public void Parler(string message)
        {
            texte = new PromptBuilder(System.Globalization.CultureInfo.GetCultureInfo("fr-FR"));
            texte.AppendText(message);
            dire.Speak(texte);
        }
    }
}
