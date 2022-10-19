var cookieconsentinitialized = false;

window.addEventListener('load', function () {

    // obtain cookieconsent plugin
    var cc = initCookieConsent();

    // example logo
    var titolomodal = 'Preferenze sul consenso dei dati personali';
    var cookie = '🍪';
    var privacypage = "/it/privacy-policy-6";
    var cookiepage = "/it/politica-cookie-5";
    var consolemsgactivate = true;
    var locallng = 'it';
    if (lng != null && lng != undefined) {
        if (lng == 'I') {
            locallng = 'it';
            privacypage = "/it/web/privacy-policy-6";
            cookiepage = "/it/web/politica-cookie-5";
        }
        if (lng == 'GB') {
            locallng = 'en';
            privacypage = "/en/web/privacy-policy-6";
            cookiepage = "/en/web/politica-cookie-5";
        }
    }


    // run plugin with config object
    cc.run({
        current_lang: locallng,
        autoclear_cookies: true,                   // default: false
        cookie_name: 'cc_cookie',             // default: 'cc_cookie'
        cookie_expiration: 182,                    // default: 182
        page_scripts: true,                         // default: false

        // mode: 'opt-in'                          // default: 'opt-in'; value: 'opt-in' or 'opt-out'
        // delay: 0,                               // default: 0
        // auto_language: '',                      // default: null; could also be 'browser' or 'document'
        // autorun: true,                          // default: true
        // force_consent: false,                   // default: false
        // hide_from_bots: false,                  // default: false
        // remove_cookie_tables: false             // default: false 
        // cookie_necessary_only_expiration: 182   // default: disabled
        // cookie_domain: location.hostname,       // default: current domain
        // cookie_path: '/',                       // default: root
        // cookie_same_site: 'Lax',                // default: 'Lax'
        // use_rfc_cookie: false,                  // default: false
        // revision: 0,                            // default: 0

        gui_options: {
            consent_modal: {
                layout: 'cloud',                      // box,cloud,bar
                position: 'middle center',           // bottom,middle,top + left,right,center
                transition: 'slide'                 // zoom,slide
            },
            settings_modal: {
                layout: 'box',                      // box,bar
                // position: 'left',                // right,left (available only if bar layout selected)
                transition: 'slide'                 // zoom,slide
            }
        },

        onFirstAction: function () {
            // callback triggered only once on the first accept/reject action
            if (consolemsgactivate) console.log('onFirstAction fired');
            update_registro();
        },

        onAccept: function (cookie) {
            // callback triggered on the first accept/reject action, and after each page load
            if (consolemsgactivate) console.log('onAccept fired ...');

            //FOR GTM CONSENT MODE -----------------
            if (cc.allowedCategory('analytics')) {
                gtag('consent', 'update', {
                    'analytics_storage': 'granted'
                });
            } else {
                gtag('consent', 'update', {
                    'analytics_storage': 'denied'
                });
            }
            if (cc.allowedCategory('targeting')) {
                gtag('consent', 'update', {
                    'ad_storage': 'granted'
                });
            } else {
                gtag('consent', 'update', {
                    'ad_storage': 'denied'
                });
            }
            //END FOR GTM CONSENT MODE -----
             //cookieconsentinitialized = true;
            (function () {
                setTimeout(function () {
                    cookieconsentinitialized = true;
                }, 1500);
            })();
        },

        onChange: function (cookie, changed_preferences) {
            // callback triggered when user changes preferences after consent has already been given
            if (consolemsgactivate) console.log('onChange fired ...');


            //FOR GTM CONSENT MODE ----------------------
            if (cc.allowedCategory('analytics')) {
                gtag('consent', 'update', {
                    'analytics_storage': 'granted'
                });
            } else {
                gtag('consent', 'update', {
                    'analytics_storage': 'denied'
                });
            }
            if (cc.allowedCategory('targeting')) {
                gtag('consent', 'update', {
                    'ad_storage': 'granted'
                });
            } else {
                gtag('consent', 'update', {
                    'ad_storage': 'denied'
                });
            }
            //END FOR GTM CONSENT MODE -----
            //cookieconsentinitialized = true;
            (function () {
                setTimeout(function () {
                    cookieconsentinitialized = true;
                }, 1500);
            })();
            update_registro();

        },

        languages: {
            'it': {
                consent_modal: {
                    title: cookie + ' Utilizziamo i cookies ',
                    description: 'Questo sito utilizza cookie essenziali per garantirne il corretto funzionamento e cookie di tracciamento per capire come interagisci con esso. Questi ultimi saranno impostati solo previo consenso. <button type="button" data-cc="c-settings" class="cc-link">Scopri e personalizza</button>',
                    primary_btn: {
                        text: 'Accetta tutti',
                        role: 'accept_all'              // 'accept_selected' or 'accept_all'
                    },
                    //secondary_btn: {
                    //    text: 'Scopri di più e personalizza',
                    //    role: 'settings'        // 'settings' or 'accept_necessary'
                    //}
                     secondary_btn: {
                     text: 'Rifiuta tutti',
                     role: 'accept_necessary'        // 'settings' or 'accept_necessary'
                     }
                },
                settings_modal: {
                    title: titolomodal,
                    save_settings_btn: 'Salva e procedi',
                    accept_all_btn: 'Accetta tutto',
                    reject_all_btn: 'Rifiuta tutto',
                    close_btn_label: 'Chiudi',
                    cookie_table_headers: [
                        { col1: 'Nome' },
                        { col2: 'Dominio' },
                        { col3: 'Scadenza' },
                        { col4: 'Descrizione' },
                        { col5: 'Tipo' }
                    ],
                    blocks: [
                        {
                            title: 'Utilizzo dei cookies',
                            description: 'Questo pannello ti consente di esprimere le tue preferenze di consenso alle tecnologie di tracciamento che adottiamo per offrire le funzionalità e svolgere le attività sotto descritte. Per ottenere ulteriori informazioni in merito all\'utilità e al funzionamento di tali strumenti di tracciamento, fai riferimento alla <a href="' + cookiepage + '" class="cc-link">cookie policy</a>. Puoi rivedere e modificare le tue scelte in qualsiasi momento. Tieni presente che il rifiuto del consenso per una finalità particolare può rendere le relative funzioni non disponibili.'
                        }, {
                            title: 'Strettamente necessari',
                            description: 'Questi strumenti di tracciamento sono strettamente necessari per garantire il funzionamento e la fornitura del servizio che ci hai richiesto e, pertanto, non richiedono il tuo consenso.',
                            toggle: {
                                value: 'necessary',   // there are no default categories => you specify them
                                enabled: true,  // default status
                                readonly: true  // allow to enable/disable
                            },
                            cookie_table: [
                                {
                                    col1: 'ASP.NET_SessionId',
                                    col2: 'site domain',
                                    col3: 'durata della sessione',
                                    col4: 'codice id sessione utilizzato dal sito web per memorizzare le scelte utente durante la navigazione',
                                    col5: 'Functional'
                                },
                                {
                                    col1: '.ASPXANONYMOUS',
                                    col2: 'site domain',
                                    col3: 'durata della sessione',
                                    col4: 'valore usato dal sito per funzioni interne e gestione accessi alle varie sezioni del sito',
                                    col5: 'Functional'
                                },
                                {
                                    col1: 'cc_cookie',
                                    col2: 'site domain',
                                    col3: '1 anno',
                                    col4: 'cookie usato per la registrazione delle categorie di cookie scelte dall\'utente per permettere o bloccare le varie tipologie di cookie a seconda della finalità',
                                    col5: 'Functional'
                                },
                                {
                                    col1: '__stripe_mid',
                                    col2: 'stripe.com',
                                    col3: '1 anno',
                                    col4: 'Stripe viene utilizzato per i pagamenti con carta di credito. Stripe utilizza un cookie per ricordare chi sei e per consentire al nostro sito Web di elaborare i pagamenti senza memorizzare alcuna informazione sulla carta di credito sui suoi server.',
                                    col5: 'Functional'
                                },
                                {
                                    col1: '__stripe_sid',
                                    col2: 'stripe.com',
                                    col3: '1 ora',
                                    col4: 'Stripe viene utilizzato per i pagamenti con carta di credito. Stripe utilizza un cookie per ricordare chi sei e per consentire al nostro sito Web di elaborare i pagamenti senza memorizzare alcuna informazione sulla carta di credito sui suoi server.',
                                    col5: 'Functional'
                                }


                            ]
                        }, {
                            title: 'Statistiche di utilizzo, misurazione e miglioramento dell\'esperienza',
                            description: 'Questi strumenti di tracciamento ci permettono di offrire una user experience personalizzata migliorando la gestione delle impostazioni e consentendo l\'interazione con network e piattaforme esterne. Questi strumenti di tracciamento ci permettono di misurare il traffico e analizzare il tuo comportamento con l\'obiettivo di migliorare il nostro servizio e per aumentare la sicurezza durante la navigazione',
                            toggle: {
                                value: 'analytics',     // there are no default categories => you specify them
                                enabled: false,
                                readonly: false
                            },
                            cookie_table: [
                                {
                                    col1: 'VISITOR_INFO1_LIVE',
                                    col2: 'youtube.com',
                                    col3: '6 mesi',
                                    col4: 'utilizzato per fornire stime della larghezza di banda a YouTube, utilizzato per rilevare e risolvere problemi con il servizio',
                                    col5: 'functional, statistics'
                                },
                                {
                                    col1: 'PREF',
                                    col2: 'youtube.com',
                                    col3: '8 mesi',
                                    col4: 'YouTube utilizza il cookie "PREF" per memorizzare informazioni come la configurazione della pagina preferita dell\'utente e le preferenze di riproduzione come la riproduzione automatica, il contenuto casuale e le dimensioni del lettore.Per YouTube Music, queste preferenze includono volume, modalità di ripetizione e riproduzione automatica.',
                                    col5: 'functional'
                                },
                                {
                                    col1: 'LOGIN_INFO',
                                    col2: 'youtube.com',
                                    col3: '2 anni',
                                    col4: 'Questo cookie viene utilizzato per riprodurre i video di YouTube incorporati nel sito web.',
                                    col5: 'functional, security'
                                },
                                {
                                    col1: 'YSC',
                                    col2: 'youtube.com',
                                    col3: 'durata della sessione',
                                    col4: '"YSC" viene utilizzato da YouTube per ricordare l\'input dell\'utente e associare le azioni di un utente. Questo cookie dura fintanto che l\'utente mantiene aperto il proprio browser. Serve ad assicurare che le richieste all\'interno di una sessione di navigazione vengano effettuate dall\'utente e non da altri siti.Questi cookie impediscono ai siti dannosi di agire per conto di un utente a sua insaputa.',
                                    col5: 'functional, security'
                                },
                                {
                                    col1: 'NID',
                                    col2: 'google.com',
                                    col3: '6 mesi',
                                    col4: 'utilizzato per ricordare le tue preferenze e altre informazioni, come la tua lingua preferita, quanti risultati preferisci visualizzare in una pagina dei risultati di ricerca (ad esempio, 10 o 20) e se desideri che il filtro SafeSearch di Google sia attivato',
                                    col5: 'functional'
                                },
                                {
                                    col1: 'CONSENT',
                                    col2: 'google.com',
                                    col3: '2 anni',
                                    col4: 'utilizzato per memorizzare lo stato di un utente in merito alle sue scelte sui cookie.',
                                    col5: 'functional'
                                },
                                {
                                    col1: 'SIDCC',
                                    col2: 'google.com',
                                    col3: 'nessuna scandenza',
                                    col4: 'Questo cookie di sicurezza protegge i dati di un utente da accessi non autorizzati.',
                                    col5: 'security'
                                },
                                {
                                    col1: 'AEC',
                                    col2: 'google.com',
                                    col3: '6 mesi',
                                    col4: 'utilizzato per garantire che le richieste all\'interno di una sessione di navigazione vengano effettuate dall\'utente e non da altri siti. Questi cookie impediscono ai siti dannosi di agire per conto di un utente a sua insaputa.',
                                    col5: 'functional, security'
                                },
                                {
                                    col1: 'test_cookie',
                                    col2: 'doubleclick.net',
                                    col3: '15 minuti',
                                    col4: 'This cookie is set by doubleclick.net. The purpose of the cookie is to determine if the users’ browser supports cookies.',
                                    col5: 'functional'
                                },
                                {
                                    col1: '_gid',
                                    col2: 'google.com',
                                    col3: '1 giorno',
                                    col4: 'Google Analytics (Google Ireland Limited) lo utilizza per distiguere gli utenti trmite id univoco per le statistiche di navigazione. Luogo del trattamento: Irlanda – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://tools.google.com/dlpage/gaoptout?hl=it%22">Opt Out</a>.',
                                    col5: 'Statistics'
                                },
                                {
                                    col1: '__utma',
                                    col2: 'google.com',
                                    col3: '2 anni',
                                    col4: 'Google Analytics (Google Ireland Limited) lo utilizza  per dati statistici di utilizzo del sito. Luogo del trattamento: Irlanda – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://tools.google.com/dlpage/gaoptout?hl=it%22">Opt Out</a>.',
                                    col5: 'Statistics'
                                },
                                {
                                    col1: '_gac_*',
                                    col2: 'google.com',
                                    col3: '3 mesi',
                                    col4: 'Contiene informazioni statistiche relative alla campagna per l\'utente.Se hai collegato i tuoi account Google Analytics e Google Ads. Luogo del trattamento: Irlanda – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://tools.google.com/dlpage/gaoptout?hl=it%22">Opt Out</a>.',
                                    col5: 'Statistics',
                                    is_regex: true
                                }, {
                                    col1: '_ga',
                                    col2: 'google.com',
                                    col3: '2 anni',
                                    col4: 'Usato da google Analytics 4 per registrare id univoco dell\'utente per dati statistici di utilizzo del sito. Luogo del trattamento: Irlanda – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://tools.google.com/dlpage/gaoptout?hl=it%22">Opt Out</a>.',
                                    col5: 'Statistics',
                                }, {
                                    col1: '_ga_*',
                                    col2: 'google.com',
                                    col3: '2 anni',
                                    col4: 'Usato per memorizzare lo stato della sessione utente in google Analytics 4. Luogo del trattamento: Irlanda – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://tools.google.com/dlpage/gaoptout?hl=it%22">Opt Out</a>.',
                                    col5: 'Statistics',
                                    is_regex: true
                                }, {
                                    col1: '_gcl_*',
                                    col2: 'google.com',
                                    col3: '3 mesi',
                                    col4: 'È il cookie di prima parte per la funzionalità "Linker di conversione": prende le informazioni nei clic sugli annunci e le memorizza in un cookie di prima parte in modo che le conversioni possano essere attribuite al di fuori della pagina di destinazione.',
                                    col5: 'Statistics',
                                    is_regex: true
                                }, {
                                    col1: '_fbp',
                                    col2: 'facebook.com',
                                    col3: '3 mesi',
                                    col4: 'Integrando il pixel di Meta, Meta Events Manager può dare al Titolare delle informazioni sul traffico e sulle interazioni su questo Sito Web e sulle azioni compiute all\'interno del medesimo, in paricolare pagine visitate, azioni compiute quali conversioni specifiche.  Gli Utenti possono scegliere di non utilizzare i Strumenti di Tracciamento di Facebook per la personalizzazione degli annunci visitando – <a href="https://www.facebook.com/about/privacy/">Privacy Policy</a> – <a href="https://www.aboutads.info/choices/">Opt Out</a>. ',
                                    col5: 'Statistics'
                                }
                            ]
                        }, {
                            title: 'Pubblicità e Targeting di pubblico',
                            description: 'Questi strumenti di tracciamento ci consentono di fornirti contenuti commerciali personalizzati in base al tuo comportamento ed interessi oltre a gestire, fornire e tracciare gli annunci pubblicitari.',
                            toggle: {
                                value: 'targeting',   // there are no default categories => you specify them
                                enabled: false,
                                readonly: false
                            },
                            cookie_table: [
                                {
                                    col1: 'IDE',
                                    col2: 'doubleclick.net',
                                    col3: '24 mesi',
                                    col4: 'Usato per fornire pubblicazione annunci pubblicitari o per visualizzare pubblicità personalizzata tramite retargeting. Gli Utenti possono vedere e scegliere di non utilizzare i Strumenti di Tracciamento di google per la personalizzazione degli annunci visitando – <a href="https://myactivity.google.com/myactivity">La mia attvità</a> – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://adssettings.google.com/authenticated">Opt Out</a>',
                                    col5: 'marketing, tracking'
                                },
                                {
                                    col1: '1P_JAR',
                                    col2: 'google.com',
                                    col3: '1 mese',
                                    col4: 'Usato per fornire pubblicazione annunci pubblicitari o per visualizzare pubblicità personalizzata tramite retargeting',
                                    col5: 'marketing, tracking'
                                },
                                {
                                    col1: 'NID',
                                    col2: 'google.com',
                                    col3: '1 mese',
                                    col4: 'Viene utilizzato per memorizzare le preferenze in un ID Google univoco per ricordare le tue informazioni. Ciò include la tua lingua preferita (ad esempio,italiano, inglese), quanti risultati di ricerca desideri visualizzare per pagina (ad esempio 10 o 20) e se desideri attivare o meno il filtro SafeSearch di Google. Queste preferenze possono essere utilizzati per pubblicità ottimizzata e/o personalizzata sulle reti di Google',
                                    col5: 'marketing, tracking'
                                },
                                {
                                    col1: 'UULE',
                                    col2: 'google.com',
                                    col3: '6 ore',
                                    col4: 'Invia informazioni precise sulla posizione dal tuo browser ai server di Google in modo che Google possa mostrarti risultati pertinenti alla tua posizione. L\'uso di questo cookie dipende dalle impostazioni del tuo browser e dal fatto che tu abbia scelto di attivare la posizione per il tuo browser',
                                    col5: 'marketing, tracking'
                                },
                                {
                                    col1: 'VISITOR_INFO1_LIVE',
                                    col2: 'youtube.com',
                                    col3: '6 mesi',
                                    col4: 'Utilizzato per fornire stime di larghezza di banda a YouTube, utilizzato per rilevare e risolvere problemi con il servizio e per pubblicità mirata',
                                    col5: 'marketing'
                                },
                                {
                                    col1: '__Secure-*',
                                    col2: 'google.com',
                                    col3: '2 anni',
                                    col4: 'Questo è un cookie di targeting. Costruisce un profilo degli interessi del visitatore del sito web al fine di mostrare pubblicità di Google pertinente e personalizzata.',
                                    col5: 'marketing',
                                    is_regex: true
                                },

                                {
                                    col1: 'SID',
                                    col2: 'google.com',
                                    col3: '1 anno',
                                    col4: 'Viene utilizzato per motivi di sicurezza per archiviare record crittografati e firmati digitalmente dell\'ID dell\'account Google di un utente e dell\'ora di accesso più recente. Consente a Google di autenticare gli utenti, prevenire l\'uso fraudolento delle credenziali di accesso e proteggere i dati degli utenti da parti non autorizzate. Può essere utilizzato anche per scopi di targeting per mostrare contenuti pubblicitari pertinenti e personalizzati.',
                                    col5: 'marketing'
                                },
                                {
                                    col1: 'SSID',
                                    col2: 'google.com',
                                    col3: '1 anno',
                                    col4: 'Viene utilizzato per memorizzare informazioni su come utilizzi il sito Web e qualsiasi pubblicità che potresti aver visto prima di visitare questo sito Web. Aiuta anche a personalizzare la pubblicità sulle proprietà di Google ricordando le tue ricerche più recenti, le tue precedenti interazioni con gli annunci di un inserzionista o i risultati di ricerca e le tue visite al sito web di un inserzionista.',
                                    col5: 'marketing'
                                },
                                {
                                    col1: 'APISID',
                                    col2: 'google.com, youtube.com',
                                    col3: '2 anni',
                                    col4: 'Viene utilizzato per scopi di targeting per costruire un profilo degli interessi del visitatore del sito Web al fine di mostrare pubblicità di Google pertinente e personalizzata.',
                                    col5: 'marketing'
                                },
                                {
                                    col1: 'SAPISID',
                                    col2: 'google.com',
                                    col3: '2 anni',
                                    col4: 'Viene utilizzato per scopi di targeting per costruire un profilo degli interessi del visitatore del sito Web al fine di mostrare pubblicità di Google pertinente e personalizzata.',
                                    col5: 'marketing'
                                },
                                {
                                    col1: 'HSID',
                                    col2: 'google.com, youtube.com',
                                    col3: '1 anno',
                                    col4: 'Viene utilizzato a fini di sicurezza per archiviare record crittografati e firmati digitalmente dell\'ID dell\'account Google di un utente e dell\'ora di accesso più recente che consente a Google di autenticare gli utenti, prevenire l\'uso fraudolento delle credenziali di accesso e proteggere i dati degli utenti da parti non autorizzate .Questo può essere utilizzato anche per scopi di targeting per mostrare contenuti pubblicitari pertinenti e personalizzati.',
                                    col5: 'marketing'
                                },
                                {
                                    col1: '_fr',
                                    col2: 'facebook.com',
                                    col3: '3 mesi',
                                    col4: 'Fornito da Facebook, questo pixel abbina i profili utente di Facebook in modo anonimo, consente il retargeting sulla piattaforma Facebook e la creazione di un pubblico simile. Questo pixel consente a Macular Society di analizzare gli sforzi di marketing e di offrire la pubblicità più pertinente al pubblico corretto. Gli Utenti possono scegliere di non utilizzare i Strumenti di Tracciamento di Facebook per la personalizzazione degli annunci visitando – <a href="https://www.facebook.com/about/privacy/">Privacy Policy</a> – <a href="https://www.aboutads.info/choices/">Opt Out</a>. ',
                                    col5: 'marketing'
                                },
                                {
                                    col1: '_fbp',
                                    col2: 'facebook.com',
                                    col3: '3 mesi',
                                    col4: 'Pubblico simile di Facebook, pubblico personalizzato, facebook remarketing e simili utilizzano questo cookie per un servizio di advertising e di targeting comportamentale che utilizza i Dati raccolti attraverso il servizio di Facebook al fine di mostrare annunci pubblicitari a Utenti con comportamenti simili a Utenti che sono già in una lista di Pubblico personalizzato sulla base del loro precedente utilizzo di questo Sito Web o della loro interazione con contenuti rilevanti attraverso le applicazioni e i servizi di Facebook. Sulla base di questi Dati, gli annunci personalizzati saranno mostrati agli Utenti suggeriti da Pubblico simile di Facebook. Gli Utenti possono scegliere di non utilizzare i Strumenti di Tracciamento di Facebook per la personalizzazione degli annunci visitando – <a href="https://www.facebook.com/about/privacy/">Privacy Policy</a> – <a href="https://www.aboutads.info/choices/">Opt Out</a>. ',
                                    col5: 'marketing'
                                }
                            ]
                        }, {
                            title: 'Altre informazioni',
                            description: 'Per qualsiasi informazioni sulle policy sui cookie e le nostre scelte, puoi contattarci ai riferimenti indicati nella pagina <a class="cc-link" href="' + privacypage + '">Privacy Policy</a>.',
                        }
                    ]
                }
            },
            'en': {
                consent_modal: {
                    title: cookie + 'We use cookies ',
                    description: 'This site uses essential cookies to ensure its proper functioning and tracking cookies to understand how you interact with it. The latter will be set only with prior consent. <button type="button" data-cc="c-settings" class="cc-link">Discover and personalize</button>',
                    primary_btn: {
                        text: 'Accept all',
                        role: 'accept_all'              // 'accept_selected' or 'accept_all'
                    },
                    //secondary_btn: {
                    //    text: 'Personalize',
                    //    role: 'settings'        // 'settings' or 'accept_necessary'
                    //}
                     secondary_btn: {
                     text: 'Reject all',
                     role: 'accept_necessary'        // 'settings' or 'accept_necessary'
                     }
                },
                settings_modal: {
                    title: titolomodal,
                    save_settings_btn: 'Save and proceed',
                    accept_all_btn: 'Accept all',
                    reject_all_btn: 'Reject all',
                    close_btn_label: 'Close',
                    cookie_table_headers: [
                        { col1: 'Name' },
                        { col2: 'Domain' },
                        { col3: 'Expiration' },
                        { col4: 'Description' },
                        { col5: 'Type' }
                    ],
                    blocks: [
                        {
                            title: 'Utilizzo dei cookies',
                            description: 'This panel allows you to express your consent preferences to the tracking technologies we adopt to offer the features and carry out the activities described below. For more information on the usefulness and operation of these tracking tools, please refer to <a href="' + cookiepage + '" class="cc-link">cookie policy</a>. You can review and change your choices at any time. Keep in mind that refusing consent for a particular purpose can make the related functions unavailable.'
                        }, {
                            title: 'Strictly necessary',
                            description: 'These tracking tools are strictly necessary to ensure the functioning and provision of the service you have requested from us and, therefore, do not require your consent..',
                            toggle: {
                                value: 'necessary',   // there are no default categories => you specify them
                                enabled: true,  // default status
                                readonly: true  // allow to enable/disable
                            },
                            cookie_table: [
                                {
                                    col1: 'ASP.NET_SessionId',
                                    col2: 'site domain',
                                    col3: 'duration of the session',
                                    col4: 'Session Id value used by the website to store user choices while browsing',
                                    col5: 'Functional'
                                },
                                {
                                    col1: '.ASPXANONYMOUS',
                                    col2: 'site domain',
                                    col3: 'duration of the session',
                                    col4: 'values used by the site for internal functions and access management to the various sections of the site',
                                    col5: 'Functional'
                                },
                                {
                                    col1: 'cc_cookie',
                                    col2: 'site domain',
                                    col3: '1 year',
                                    col4: 'cookie usato per la registrazione delle categorie di cookie scelte dall\'utente per permettere o bloccare le varie tipologie di cookie a seconda della finalità',
                                    col5: 'Functional'
                                },
                                {
                                    col1: '__stripe_mid',
                                    col2: 'stripe.com',
                                    col3: '1 year',
                                    col4: 'Stripe is used for credit card payments. Stripe uses a cookie to remember who you are and to enable our website to process payments without storing any credit card information on its our servers.',
                                    col5: 'Functional'
                                },
                                {
                                    col1: '__stripe_sid',
                                    col2: 'stripe.com',
                                    col3: '1 hour',
                                    col4: 'Stripe is used for credit card payments. Stripe uses a cookie to remember who you are and to enable our website to process payments without storing any credit card information on its our servers..',
                                    col5: 'Functional'
                                }


                            ]
                        }, {
                            title: 'Statistics of use, measurement and improvement of the experience',
                            description: 'These tracking tools allow us to offer a personalized user experience by improving the management of settings and allowing interaction with external networks and platforms. These tracking tools allow us to measure traffic and analyze your behavior with the aim of improving our service and to increase safety while browsing',
                            toggle: {
                                value: 'analytics',     // there are no default categories => you specify them
                                enabled: false,
                                readonly: false
                            },
                            cookie_table: [
                                {
                                    col1: 'VISITOR_INFO1_LIVE',
                                    col2: 'youtube.com',
                                    col3: '6 month',
                                    col4: 'to provide bandwidth estimations to youtube, used to detect and resolve problems with the service',
                                    col5: 'functional, statistics'
                                },
                                {
                                    col1: 'PREF',
                                    col2: 'youtube.com',
                                    col3: '8 month',
                                    col4: 'YouTube uses the ‘PREF’ cookie to store information such as a user’s preferred page configuration and playback preferences like autoplay, shuffle content, and player size. For YouTube Music, these preferences include volume, repeat mode, and autoplay. This cookie expires 8 months from a user’s last use.',
                                    col5: 'functional'
                                },
                                {
                                    col1: 'LOGIN_INFO',
                                    col2: 'youtube.com',
                                    col3: '2 anni',
                                    col4: 'This cookie is used to play YouTube videos embedded on the website.',
                                    col5: 'functional, security'
                                },
                                {
                                    col1: 'YSC',
                                    col2: 'youtube.com',
                                    col3: 'duration of the session',
                                    col4: '‘YSC’ is used by YouTube to remember user input and associate a user’s actions. This cookie lasts for as long as the user keeps their browser open.ensure that requests within a browsing session are made by the user, and not by other sites. These cookies prevent malicious sites from acting on behalf of a user without that user’s knowledge.',
                                    col5: 'functional, security'
                                },
                                {
                                    col1: 'NID',
                                    col2: 'google.com',
                                    col3: '6 month',
                                    col4: 'Used to remember your preferences and other information, such as your preferred language, how many results you prefer to have shown on a search results page (for example, 10 or 20), and whether you want to have Google’s SafeSearch filter turned on',
                                    col5: 'functional'
                                },
                                {
                                    col1: 'CONSENT',
                                    col2: 'google.com',
                                    col3: '2 year',
                                    col4: 'Used to store a user’s state regarding their cookies choices.',
                                    col5: 'functional'
                                },
                                {
                                    col1: 'SIDCC',
                                    col2: 'google.com',
                                    col3: 'duration of the session',
                                    col4: 'This security cookie protects a user\'s data from unauthorized access.',
                                    col5: 'security'
                                },
                                {
                                    col1: 'AEC',
                                    col2: 'google.com',
                                    col3: '6 mesi',
                                    col4: 'used to ensure that requests within a browsing session are made by the user, and not by other sites. These cookies prevent malicious sites from acting on behalf of a user without that user’s knowledge.',
                                    col5: 'functional, security'
                                },
                                {
                                    col1: 'test_cookie',
                                    col2: 'doubleclick.net',
                                    col3: '15 minuti',
                                    col4: 'This cookie is set by doubleclick.net. The purpose of the cookie is to determine if the users’ browser supports cookies.',
                                    col5: 'functional'
                                },
                                {
                                    col1: '_gid',
                                    col2: 'google.com',
                                    col3: '1 day',
                                    col4: 'Google Analytics (Google Ireland Limited) uses it to distinguish users through a unique id for navigation statistics. Place of processing: Irlanda – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://tools.google.com/dlpage/gaoptout?hl=it%22">Opt Out</a>.',
                                    col5: 'Statistics'
                                },
                                {
                                    col1: '__utma',
                                    col2: 'google.com',
                                    col3: '2 years',
                                    col4: 'Google Analytics (Google Ireland Limited) uses it for statistical data on the use of the site. Place of processing: Irlanda – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://tools.google.com/dlpage/gaoptout?hl=it%22">Opt Out</a>.',
                                    col5: 'Statistics'
                                },
                                {
                                    col1: '_gac_*',
                                    col2: 'google.com',
                                    col3: '3 months',
                                    col4: 'Contains campaign related statistics information for the user. If you have linked your Google Analytics and Google Ads accounts. Place of processing: Irlanda – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://tools.google.com/dlpage/gaoptout?hl=it%22">Opt Out</a>.',
                                    col5: 'Statistics',
                                    is_regex: true
                                }, {
                                    col1: '_ga',
                                    col2: 'google.com',
                                    col3: '2 years',
                                    col4: 'Used by google Analytics 4 to record the unique id of the user for statistical data on the use of the site. Place of processing: Irlanda – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://tools.google.com/dlpage/gaoptout?hl=it%22">Opt Out</a>.',
                                    col5: 'Statistics',
                                }, {
                                    col1: '_ga_*',
                                    col2: 'google.com',
                                    col3: '2 years',
                                    col4: 'Used to persist session state in google Analytics 4. Luogo del trattamento: Irlanda – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://tools.google.com/dlpage/gaoptout?hl=it%22">Opt Out</a>.',
                                    col5: 'Statistics',
                                    is_regex: true
                                }, {
                                    col1: '_gcl_*',
                                    col2: 'google.com',
                                    col3: '3 months',
                                    col4: 'It\'s the first party cookie for "Conversion Linker" functionality - it takes information in ad clicks and stores it in a firs-party cookie so that conversions can be attributed outside the landing page.',
                                    col5: 'Statistics',
                                    is_regex: true
                                }, {
                                    col1: '_fbp',
                                    col2: 'facebook.com',
                                    col3: '3 months',
                                    col4: 'By integrating the Meta pixel, Meta Events Manager can give the Owner information on traffic and interactions on this Website and on the actions performed within it, in particular pages visited, actions performed such as specific conversions. Users can choose not to use Facebook Tracking Tools for ad personalization by visiting – <a href="https://www.facebook.com/about/privacy/">Privacy Policy</a> – <a href="https://www.aboutads.info/choices/">Opt Out</a>. ',
                                    col5: 'Statistics'
                                }
                            ]
                        }, {
                            title: 'Advertising and audience targeting',
                            description: 'These tracking tools allow us to provide you with customized commercial content based on your behavior and interests as well as manage, deliver and track advertisements.',
                            toggle: {
                                value: 'targeting',   // there are no default categories => you specify them
                                enabled: false,
                                readonly: false
                            },
                            cookie_table: [
                                {
                                    col1: 'IDE',
                                    col2: 'doubleclick.net',
                                    col3: '24 months',
                                    col4: 'Used to display advertisements or to display personalized advertising through retargeting. Users can see and choose not to use Google\'s Tracking Tools for ad personalization by visiting – <a href="https://myactivity.google.com/myactivity">La mia attvità</a> – <a href="https://policies.google.com/privacy">Privacy Policy</a> – <a href="https://adssettings.google.com/authenticated">Opt Out</a>',
                                    col5: 'marketing, tracking'
                                },
                                {
                                    col1: '1P_JAR',
                                    col2: 'google.com',
                                    col3: '1 month',
                                    col4: 'Used to deliver advertisements or to display personalized advertising through retargeting',
                                    col5: 'marketing, tracking'
                                },
                                {
                                    col1: 'NID',
                                    col2: 'google.com',
                                    col3: '1 month',
                                    col4: 'This is used to store preferences in a unique Google ID to remember your information. This includes your preferred language (for example, English), how many search results you wish to have shown per page (such as 10 or 20), and whether or not you wish to have Google\'s SafeSearch filter turned on.These preferences can be used for optimised and/ or personalised advertising on Google networks',
                                    col5: 'marketing, tracking'
                                },
                                {
                                    col1: 'UULE',
                                    col2: 'google.com',
                                    col3: '6 hours',
                                    col4:  'Sends precise location information from your browser to Google’s servers so that Google can show you results that are relevant to your location. The use of this cookie depends on your browser settings and whether you have chosen to have location turned on for your browser',
                                    col5: 'marketing, tracking'
                                },
                                {
                                    col1: 'VISITOR_INFO1_LIVE',
                                    col2: 'youtube.com',
                                    col3: '6 months',
                                    col4: 'Used to provide bandwidth estimations to youtube, used to detect and resolve problems with the service, and for target advertising',
                                    col5: 'marketing'
                                },
                                {
                                    col1: '__Secure-*',
                                    col2: 'google.com',
                                    col3: '2 years',
                                    col4: 'This is a targeting cookie. It builds a profile of the website visitor\'s interests in order to show relevant and personalised Google advertising.',
                                    col5: 'marketing',
                                    is_regex: true
                                },

                                {
                                    col1: 'SID',
                                    col2: 'google.com',
                                    col3: '1 anno',
                                    col4: 'This is used for security purposes to store digitally signed and encrypted records of a user\'s Google account ID and most recent sign-in time. It allows Google to authenticate users, prevent fraudulent use of login credentials, and protect user data from unauthorised parties.This can also be used for targeting purposes to show relevant and personalised ad content.',
                                    col5: 'marketing'
                                },
                                {
                                    col1: 'SSID',
                                    col2: 'google.com',
                                    col3: '1 anno',
                                    col4: 'This is used to store information about how you use the website and any advertising that you may have seen before visiting this website. It also helps to customise advertising on Google properties by remembering your most recent searches, your previous interactions with an advertiser\'s ads or search results and your visits to an advertisers website.',
                                    col5: 'marketing'
                                },
                                {
                                    col1: 'APISID',
                                    col2: 'google.com, youtube.com',
                                    col3: '2 anni',
                                    col4: 'This is used by for targeting purposes to build a profile of the website visitor\'s interests in order to show relevant and personalised Google advertising.',
                                    col5: 'marketing'
                                },
                                {
                                    col1: 'SAPISID',
                                    col2: 'google.com',
                                    col3: '2 anni',
                                    col4: 'This is used by for targeting purposes to build a profile of the website visitor\'s interests in order to show relevant and personalised Google advertising.',
                                    col5: 'marketing'
                                },
                                {
                                    col1: 'HSID',
                                    col2: 'google.com, youtube.com',
                                    col3: '1 anno',
                                    col4: 'This is used for security purposes to store digitally signed and encrypted records of a user\'s Google account ID and most recent sign-in time which allows Google to authenticate users, prevent fraudulent use of login credentials, and protect user data from unauthorised parties.This can also be used for targeting purposes to show relevant and personalised ad content..',
                                    col5: 'marketing'
                                },
                                {
                                    col1: '_fr',
                                    col2: 'facebook.com',
                                    col3: '3 mesi',
                                    col4: 'Provided by Facebook, this pixel matches Facebook user profiles anonymously, enable retargeting on the Facebook platform, as well as the creation of lookalike audiences. This pixel allows Macular Society to analyse marketing efforts, and serve the most relevant advertising to the correct audience. Users can choose not to use Facebook Tracking Tools for ad personalization by visiting – <a href="https://www.facebook.com/about/privacy/">Privacy Policy</a> – <a href="https://www.aboutads.info/choices/">Opt Out</a>. ',
                                    col5: 'marketing'
                                },
                                {
                                    col1: '_fbp',
                                    col2: 'facebook.com',
                                    col3: '3 mesi',
                                    col4: 'Facebook similar audience, custom audience, facebook remarketing and the like use this cookie for an advertising and behavioral targeting service that uses the Data collected through the Facebook service in order to show advertisements to Users with similar behavior to Users who are already in a Custom Audience list based on their previous use of this Website or their interaction with relevant content through Facebook applications and services. Based on this Data, personalized ads will be shown to Users suggested by Facebook Similar Audience. Users can choose not to use Facebook Tracking Tools for ad personalization by visiting – <a href="https://www.facebook.com/about/privacy/">Privacy Policy</a> – <a href="https://www.aboutads.info/choices/">Opt Out</a>.',
                                    col5: 'marketing'
                                }
                            ]
                        }, {
                            title: 'Other informations',
                            description: 'For any information on cookie policies and our choices, you can contact us at the references indicated on the page <a class="cc-link" href="' + privacypage + '">Privacy Policy</a>.',
                        }
                    ]
                }
            }



        }
    });

    function update_registro() {
        var datatoreg = {};
        //dati per il log registro preferenze
        //contenuto -  cookie GetCookie('cc_cookie')
        //page url  - window.location.toString()
        //ip address client - // meglio se lo aggiungi lato server fai prima
        //user agent   - navigator.userAgent;
        // fare chiamata api o handler per salnvare nel registro prefeferenze .....
        datatoreg.cc_cookie = GetCookie('cc_cookie');
        datatoreg.pageurl = window.location.toString();
        datatoreg.clientip = ''; // meglio se lo aggiungi lato server fai prima
        datatoreg.useragent = navigator.userAgent;
        var serializeddata = JSON.stringify(datatoreg);//  -> passare con chiamata asinrona
        //fare ajax call per memorizzare nel registro
        var lng = lng || "I";
        $.ajax({
            url: pathAbs + commonhandlerpath,
            contentType: "application/json; charset=utf-8",
            global: false,
            cache: false,
            dataType: "text",
            type: "POST",
            //async: false,
            data: { 'q': 'updateregistrocookie', 'data': JSON.stringify(datatoreg), 'lng': lng },
            success: function (result) {
                console.log(result);
            },
            error: function (result) {
                console.log(result);
            }
        });

    }

});



function GetCookie(name) {

    var start = document.cookie.indexOf(name + "=");
    var len = start + name.length + 1;
    if ((!start) && (name != document.cookie.substring(0, name.length))) {
        return null;
    }
    if (start == -1) return null;
    var end = document.cookie.indexOf(";", len);
    if (end == -1) end = document.cookie.length;
    return unescape(document.cookie.substring(len, end));
};

function SetCookie(name, value, expires, path, domain, secure) {

    // set time, it's in milliseconds
    var today = new Date();
    today.setTime(today.getTime());

    /*
    if the expires variable is set, make the correct
    expires time, the current script below will set
    it for x number of days, to make it for hours,
    delete * 24, for minutes, delete * 60 * 24
    */
    if (expires) {
        expires = expires * 1000 * 60 * 60 * 24;
    }
    var expires_date = new Date(today.getTime() + (expires));

    document.cookie = name + "=" + escape(value) +
        ((expires) ? ";expires=" + expires_date.toGMTString() : "") +
        ((path) ? ";path=" + path : "") +
        ((domain) ? ";domain=" + domain : "") +
        ((secure) ? ";secure" : "");
};


