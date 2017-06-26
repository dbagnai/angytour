<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CookiePolicy.aspx.cs" Inherits="CookiePolicy" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">


        <table>
            <tbody>
                <tr>
                    <td colspan="2">
                        <p><strong>Informazioni societarie </strong></p>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Website: <%= Request.Url.Host.ToString() %></strong><br />
                        <strong>Società: <%= Nome %></strong><br />
                        <%=  "<strong>Sede:</strong>" + references.ResMan("Common",Lingua,"TestoCredits1") %>
                        <br />

                        <br />

                    </td>
                </tr>
            </tbody>
        </table>
        <p><a name="privacy_policy"></a><strong>Privacy Policy</strong></p>
        <p>
            La presente informativa &egrave; stata redatta per far conoscere i principi in materia di riservatezza che regolano l&rsquo;uso da parte di&nbsp;<strong><%= Nome %></strong>&nbsp;dei dati forniti dai clienti, compresi i dati raccolti attraverso questo sito internet. In questa pagina si descrivono le modalit&agrave; di gestione del sito in riferimento al trattamento dei dati personali degli utenti che lo consultano. Si tratta di un'informativa resa anche ai sensi dell'<a href="http://www.garanteprivacy.it/web/guest/home/docweb/-/docweb-display/docweb/1311248#articolo13">art. 13 del d.lgs. n.196/2003</a>&nbsp;(Codice in materia di protezione dei dati personali) a coloro che interagiscono con i servizi Web accessibili per via telematica a partire dall'indirizzo:<strong> <%= Request.Url.Host.ToString()  %></strong>&nbsp;. L'informativa &egrave; resa solo per il presente sito internet e non anche per altri siti internet eventualmente consultati dall&rsquo;utente tramite link presenti su questo sito. Visitando il presente sito internet, si dichiara implicitamente di avere compreso ed accettato le modalit&agrave; di trattamento descritte nella presente informativa sulla privacy. La societ&agrave;&nbsp;<strong><%= Nome %></strong>&nbsp;con sede in&nbsp;<%= references.ResMan("Common", Lingua, "TestoCredits1");  %> &nbsp;&nbsp;in persona del suo legale rappresentante pro-tempore &egrave; titolare del trattamento dei dati suddetti, il quale viene effettuato presso la sede societaria per le finalit&agrave; previste nella presente informativa privacy, inclusa la finalit&agrave; di direct marketing in caso di consenso da parte dell&rsquo;utente. I trattamenti connessi ai servizi web di questo sito sono curati solo da personale interno alla societ&agrave; titolare, appositamente incaricato del trattamento, e da incaricati esterni di occasionali operazioni di manutenzione/gestione del presente sito internet. I dati personali forniti dagli utenti che inviano richieste di materiale informativo o che si iscrivono alle newsletter sono utilizzati al solo fine di eseguire il servizio o la prestazione richiesta. Tali dati possono essere utilizzati dalla societ&agrave; anche per il perseguimento delle seguenti finalit&agrave;: a) gestione del rapporto con il cliente; b) svolgimento della attivit&agrave; economica propria della societ&agrave;; c) altri scopi legati all&rsquo;attivit&agrave; svolta dalla societ&agrave; ed a questa connessi (a titolo esemplificativo migliorare i prodotti ed i servizi offerti, anche se non strettamente legati ai servizi richiesti dall&rsquo;interessato; ricerche di mercato sulla soddisfazione del cliente, etc.). Qualora sia necessario raccogliere dati che consentono l&rsquo;identificazione personale dell&rsquo;utente al fine di fornire a quest&rsquo;ultimo un determinato servizio, si richiede all&rsquo;utente di fornire volontariamente i dati necessari, che verranno trattati esclusivamente per le finalit&agrave; di erogazione del servizio.<br />
        </p>
        <p>Nel caso in cui l&rsquo;utente desideri effettuare una prenotazione presso la struttura ricettiva, verranno richieste all&rsquo;utente informazioni quali nome, indirizzo, numero di telefono, indirizzo e-mail, numero di carta di credito e preferenze riguardo alla camera, ci&ograve; al fine di elaborare e processare la prenotazione dell&rsquo;utente richiedente. La societ&agrave;&nbsp;<strong><%= Nome %></strong>&nbsp;utilizza l&rsquo;indirizzo e-mail per inviare la conferma della prenotazione e, se necessario, pu&ograve; servirsi delle altre informazioni per contattare l&rsquo;utente al fine di ottenerne alcuni dati necessari nell&rsquo;elaborazione della sua prenotazione. La societ&agrave;&nbsp;<strong><%= Nome %></strong>&nbsp;inoltre pu&ograve; utilizzare i dati di contatto per: inviare all&rsquo;utente un messaggio precedente all&rsquo;arrivo che riepiloghi i dati della conferma della prenotazione, e che fornisca ulteriori informazioni riguardo all&rsquo;area ed alla struttura ricettiva; comunicare all&rsquo;utente offerte speciali e promozioni secondo quanto previsto nella presente Informativa privacy; cos&igrave; come per inviare all&rsquo;utente sondaggi periodici relativi al grado di soddisfazione od a ricerche di mercato. L&rsquo;utente pu&ograve; revocare in qualsiasi momento il consenso prestato per l&rsquo;invio di comunicazioni di marketing. L&rsquo;utente pu&ograve; inoltre revocare il consenso prestato per la partecipazione a sondaggi.</p>
        <p><strong>Marketing Diretto</strong></p>
        <p>
            Come sopra specificato, vorremmo fornirLe informazioni su nuovi prodotti, promozioni, offerte speciali e altre informazioni che riteniamo possano interessarLe. Nel caso in cui ci abbia espressamente autorizzato a farlo barrando l&rsquo;apposito flag, Le potremo inviare dette informazioni per posta e telefonicamente, a meno che non abbia specificato diversamente nel corso delle registrazione. Qualora, in qualsiasi momento, desideri che i Suoi dati non vengano utilizzati per finalit&agrave; di marketing diretto, potr&agrave; comunicarcelo prendendo contatti con il nostro servizio clienti via email al seguente indirizzo<strong> <%= Email %></strong>&nbsp;e non Le invieremo pi&ugrave; alcuna comunicazione commerciale. Se ha inteso negare il consenso al trattamento dei Suoi dati, non ricever&agrave; mai alcuna comunicazione commerciale ed i dati da Lei forniti saranno utilizzati esclusivamente per rispondere alla Sua richiesta.<br />
        </p>
        <p><strong>Dati di Navigazione</strong></p>
        <p>
            I sistemi informatici e le procedure software preposte al funzionamento di questo sito internet acquisiscono, nel corso del loro normale funzionamento, alcuni dati personali la cui trasmissione &egrave; implicita nell&rsquo;uso dei protocolli di comunicazione di internet. Si tratta di informazioni che non sono raccolte per essere associate a interessati identificati, ma che per loro stessa natura potrebbero, attraverso elaborazioni ed associazioni con dati detenuti da terzi, permettere di identificare gli utenti. In questa categoria di dati rientrano gli indirizzi IP o i nomi a dominio dei computer utilizzati dagli utenti che si connettono al sito, gli indirizzi in notazione URI (Uniform Resource Identifier) delle risorse richieste, la data e l&rsquo;orario della richiesta, il metodo utilizzato nel sottoporre la richiesta, la dimensione del file di risposta, il codice numerico indicante lo stato della risposta data dal server ed altri parametri relativi al sistema operativo e all&rsquo;ambiente informatico dell&rsquo;utente. Questi dati vengono utilizzati al solo fine di ricavare informazioni statistiche anonime sull&rsquo;uso del sito e per controllarne il corretto funzionamento, e vengono cancellati immediatamente dopo l&rsquo;elaborazione. I dati potrebbero essere utilizzati, esclusivamente dall&rsquo;Autorit&agrave; Giudiziaria per l&rsquo;accertamento di responsabilit&agrave; in caso di ipotetici reati informatici ai danni del sito.<br />
        </p>
        <p><a name="cookie"></a><strong>Cookies</strong></p>
        <p>
            I cookies sono dati creati da un server che sono memorizzati in file di testo sull&rsquo;hard disk del Suo computer e consentono il funzionamento del presente sito internet, l&rsquo;uso di una specifica funzionalit&agrave; esplicitamente richiesta dall&rsquo;utente ovvero permettono di migliorare il funzionamento di questo sito, come i cookie che rendono la navigazione pi&ugrave; veloce o che mostrano i contenuti di maggiore interesse per l&rsquo;utente in funzione delle scelte precedenti. I cookies possono essere permanenti (c.d. cookies persistenti), ma possono anche avere una durata limitata (c.d. cookies di sessione). Questo sito utilizza sia cookies persistenti che di sessione. Quelli di sessione non vengono memorizzati in modo permanente sul Suo computer e svaniscono con la chiusura del browser. Quelli persistenti servono per personalizzare la navigazione in funzione dello strumento utilizzato dall&rsquo;utente (computer, tablet, smartphone) cos&igrave; come i cookies di terze parti che servono per analizzare gli accessi del sito (es. Google Analytics)&nbsp;e per permettere agli utenti di condividere i contenuti del sito attraverso social network (FB) o e-mail (Add This). Questi cookies, vengono memorizzati in modo permanente sul Suo computer e hanno una durata variabile. Il sito utilizza in modo particolare i seguenti cookies:
            <br />
            <strong>Cookies Sito</strong>
        </p>
        <h3>6 cookies</h3>
        <div class="web-developer-document">
            <div id="cookie-1" class="web-developer-cookie">
                <table class="table table-bordered table-striped">
                    <tbody>
                        <tr>
                            <td>Name</td>
                            <td class="web-developer-name">_ga</td>
                        </tr>


                        <tr>
                            <td>Path</td>
                            <td class="web-developer-path">/</td>
                        </tr>
                        <tr>
                            <td>Duration</td>
                            <td class="web-developer-expires">2 Years</td>
                        </tr>
                        <tr>
                            <td>Secure</td>
                            <td class="web-developer-secure">No</td>
                        </tr>
                        <tr>
                            <td>HttpOnly</td>
                            <td>No</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
        <div class="web-developer-separator">&nbsp;</div>
        <div id="cookie-2" class="web-developer-cookie">
            <table class="table table-bordered table-striped">
                <tbody>
                    <tr>
                        <td>Name</td>
                        <td class="web-developer-name">_gat</td>
                    </tr>


                    <tr>
                        <td>Path</td>
                        <td class="web-developer-path">/</td>
                    </tr>
                    <tr>
                        <td>Duration</td>
                        <td class="web-developer-expires">10 Minuti</td>
                    </tr>
                    <tr>
                        <td>Secure</td>
                        <td class="web-developer-secure">No</td>
                    </tr>
                    <tr>
                        <td>HttpOnly</td>
                        <td>No</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="web-developer-separator">&nbsp;</div>
        <div id="cookie-3" class="web-developer-cookie">
            <table class="table table-bordered table-striped">
                <tbody>
                    <tr>
                        <td>Name</td>
                        <td class="web-developer-name">.ASPXANONYMOUS</td>
                    </tr>


                    <tr>
                        <td>Path</td>
                        <td class="web-developer-path">/</td>
                    </tr>
                    <tr>
                        <td>Duration</td>
                        <td class="web-developer-expires">Fino al termine della sessione di navigazione</td>
                    </tr>
                    <tr>
                        <td>Secure</td>
                        <td class="web-developer-secure">No</td>
                    </tr>
                    <tr>
                        <td>HttpOnly</td>
                        <td>Yes</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="web-developer-separator">&nbsp;</div>
        <div id="cookie-4" class="web-developer-cookie">
            <table class="table table-bordered table-striped">
                <tbody>
                    <tr>
                        <td>Name</td>
                        <td class="web-developer-name">ASP.NET_SessionId</td>
                    </tr>


                    <tr>
                        <td>Path</td>
                        <td class="web-developer-path">/</td>
                    </tr>
                    <tr>
                        <td>Duration</td>
                        <td class="web-developer-expires">Fino al termine della sessione di navigazione</td>
                    </tr>
                    <tr>
                        <td>Secure</td>
                        <td class="web-developer-secure">No</td>
                    </tr>
                    <tr>
                        <td>HttpOnly</td>
                        <td>Yes</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="web-developer-separator">&nbsp;</div>
        <div id="cookie-5" class="web-developer-cookie">
            <table class="table table-bordered table-striped">
                <tbody>
                    <tr>
                        <td>Name</td>
                        <td class="web-developer-name">__atuvc</td>
                    </tr>


                    <tr>
                        <td>Path</td>
                        <td class="web-developer-path">/</td>
                    </tr>
                    <tr>
                        <td>Duration</td>
                        <td class="web-developer-expires">2 Anni</td>
                    </tr>
                    <tr>
                        <td>Secure</td>
                        <td class="web-developer-secure">No</td>
                    </tr>
                    <tr>
                        <td>HttpOnly</td>
                        <td>No</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="web-developer-separator">&nbsp;</div>
        <div id="cookie-6" class="web-developer-cookie">
            <table class="table table-bordered table-striped">
                <tbody>
                    <tr>
                        <td>Name</td>
                        <td class="web-developer-name">__atuvs</td>
                    </tr>


                    <tr>
                        <td>Path</td>
                        <td class="web-developer-path">/</td>
                    </tr>
                    <tr>
                        <td>Duration</td>
                        <td class="web-developer-expires">2 Ore</td>
                    </tr>
                    <tr>
                        <td>Secure</td>
                        <td class="web-developer-secure">No</td>
                    </tr>
                    <tr>
                        <td>HttpOnly</td>
                        <td>No</td>
                    </tr>
                </tbody>
            </table>
        </div>


        <p>
            <strong>Google Analytics Cookies</strong> Il nostro sito utilizza Google Analytics di Google, Inc., un servizio offre statistiche di misurazione ed analisi delle performance del sito, tramite l&rsquo;uso di Cookie. Per consultare l&rsquo;informativa privacy del servizio Google Analytics, visiti la pagina&nbsp;<a href="http://www.google.com/intl/en/analytics/privacyoverview.html">http://www.google.com/intl/en/analytics/privacyoverview.html</a>. Per le norme sulla privacy di Google, segnaliamo il seguente indirizzo&nbsp;<a href="http://www.google.com/intl/it/privacy/privacy-policy.html">http://www.google.com/intl/it/privacy/privacy-policy.html</a>.)&nbsp; <strong>Google Adwords &amp; Google Remarketing Cookies</strong> Il sito internet potrebbe &nbsp;utilizzare il programma Google Adwords e la tecnologia Google Remarketing. Entrambi sono gestiti da Google Inc.. Anche la funzione monitoraggio delle conversioni di AdWords utilizza i cookie. Per aiutarci a tenere traccia delle vendite e di altre conversioni, viene aggiunto un cookie al computer di un utente nel momento in cui quell&rsquo;utente fa clic su un annuncio. Questo cookie dura 30 giorni e non raccoglie, n&eacute; monitora informazioni in grado di identificare personalmente un utente. Gli utenti possono disabilitare i cookie del monitoraggio delle conversioni di Google nelle impostazioni del proprio browser Internet. In alcuni casi i cookie possono causare problemi al momento dell&rsquo;accesso o durante la navigazione all&rsquo;interno del tuo account AdWords. Quando ci&ograve; si verifica, il modo migliore per correggere il problema consiste nello svuotare la cache ed eliminare i cookie salvati per il tuo browser Internet. Per saperne di pi&ugrave; su&nbsp;<a href="https://support.google.com/adwords/topic/3121763?hl=it&amp;ref_topic=3119071">clicca qui</a>&nbsp;<a href="https://www.google.es/intl/it/policies/privacy/">https://www.google.es/intl/it/policies/privacy/</a>. L&rsquo;utente infine pu&ograve; disabilitare i cookies di Google Analytics scaricando uno specifico plug-in del browser reperibile al seguente url&nbsp;<a href="https://tools.google.com/dlpage/gaoptout">https://tools.google.com/dlpage/gaoptout</a>. <strong>Facebook Cookies</strong> Il sito potrebbe utilizzare cookie di Facebook Inc. per monitorare l&rsquo;andamento delle campagne Facebook Ads ed eventuali azioni di remarketing. Clicca qui per maggiori informazioni riguardo l&rsquo;utilizzo di cookie da parte di Facebook:&nbsp;<a href="https://www.facebook.com/help/cookies/">https://www.facebook.com/help/cookies/</a> L&rsquo;utente pu&ograve; opporsi alla registrazione di cookies persistenti sul Suo hard disk configurando il browser di navigazione in modo da disabilitare i cookies. Scopri come disabilitare i cookie nei principali browser:&nbsp;<a href="https://support.google.com/accounts/answer/61416?hl=it">Chrome</a>,&nbsp;<a href="https://support.mozilla.org/en-US/kb/enable-and-disable-cookies-website-preferences">Firefox</a>,&nbsp;<a href="http://windows.microsoft.com/it-it/windows-vista/block-or-allow-cookies">Internet Explorer</a>,&nbsp;<a href="https://support.apple.com/kb/PH5042?locale=en_US">Safari</a>,&nbsp;<a href="http://www.opera.com/help/tutorials/security/privacy/">Opera</a>. Dopo questa operazione, tuttavia, alcune funzioni delle pagine web potrebbero non essere eseguite correttamente.<br />
        </p>
        <p><strong>Dati Forniti Volontariamente dall&rsquo;utente</strong></p>
        <p>
            L&rsquo;invio facoltativo, esplicito e volontario di posta elettronica agli indirizzi indicati su questo sito, o la compilazione del form, comporta la successiva acquisizione dell&rsquo;indirizzo del mittente, necessario per rispondere alle richieste, nonch&eacute; degli eventuali altri dati personali inseriti nella missiva da parte del cliente.<br />
        </p>
        <p><strong>Conferimento dei Dati</strong></p>
        <p>
            L&rsquo;utente &egrave; libero di fornire i dati personali riportati nei moduli di richiesta o comunque indicati in apposite sezioni del sito internet per richiedere l&rsquo;invio di materiale informativo o di altre comunicazioni. Il loro mancato conferimento non comporta l&rsquo;impossibilit&agrave; di ottenere quanto richiesto in forma anche anonima. I dati personali sono trattati con strumenti automatizzati per il tempo strettamente necessario a conseguire gli scopi per cui sono stati raccolti. Specifiche misure di sicurezza sono osservate per prevenire la perdita dei dati, usi illeciti o non corretti ed accessi non autorizzati al database.<br />
        </p>
        <p><strong>Modalit&agrave; del Trattamento</strong></p>
        <p>
            I Suoi dati personali saranno trattati con strumenti automatizzati per il tempo strettamente necessario a conseguire gli scopi per cui sono stati raccolti. Specifiche misure di sicurezza sono osservate per prevenire la perdita dei dati, usi illeciti o non corretti ed accessi non autorizzati. L&rsquo;utente ha sempre diritto, in qualunque momento, di ottenere la conferma dell'esistenza o meno dei dati che si riferiscono alla sua persona e di conoscerne il contenuto e l'origine, verificarne l'esattezza o chiederne l'integrazione o l'aggiornamento, oppure la rettificazione (<a href="http://www.garanteprivacy.it/web/guest/home/docweb/-/docweb-display/docweb/1311248#articolo7">art. 7 del d.lgs. n. 196/2003</a>). Ai sensi del medesimo articolo Lei ha il diritto di chiedere la cancellazione, la trasformazione in forma anonima o il blocco dei dati trattati in violazione di legge, nonch&eacute; di opporsi in ogni caso, per motivi legittimi, al loro trattamento. Questa informativa sulla privacy pu&ograve; essere modificata periodicamente e l'uso delle informazioni raccolte &egrave; soggetto all'informativa sulla privacy in effetto al momento dell'uso. Il presente documento, pubblicato all&rsquo;indirizzo&nbsp;<strong> <%= Request.Url.Host.ToString() + "/CookiePolicy.aspx" %></strong>, costituisce l' Informativa sulla Privacy di questo sito internet e sar&agrave; soggetto ad aggiornamenti di volta in volta pubblicati e recanti la data di aggiornamento.<br />
        </p>
        <p>&nbsp;</p>

    </form>
</body>
</html>
