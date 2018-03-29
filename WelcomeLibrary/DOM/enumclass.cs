using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WelcomeLibrary.DOM
{
    public class enumclass
    {
        public enum StatoCard
        {
            errore = 0,
            attiva = 1,
            nonattiva = 2,
            scaduta = 3,
            nonpresente = 4,
            attivata = 5
        }

        public enum TipoMailing
        {
            AvvisoScadenzaCard = 0,
            AvvisoInserimentoStruttura = 1,
            AvvisoNuovaofferta = 2,
            AvvisoNuovoAnnuncio = 4,
            Newsletter_tipo1 = 3
        }

        public enum TipoContatto
        {
            invioemail = 0,
            visitaurl = 1,
            visitapagina = 2
        }

    }
}
