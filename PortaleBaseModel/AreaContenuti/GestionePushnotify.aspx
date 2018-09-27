<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" CodeFile="GestionePushnotify.aspx.cs" Inherits="AreaContenuti_GestionePushnotify" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript" src='<%= "/sw-register.js" + CommonPage.AppendModTime(Server,"~/sw-register.js") %>'></script>
    <script type="text/javascript" src='<%= "/sw.js" + CommonPage.AppendModTime(Server,"~/sw.js") %>'></script>

    <%-- 
        https://developers.google.com/web/fundamentals/push-notifications/sending-messages-with-web-push-libraries
        https://github.com/web-push-libs/web-push-csharp 
    --%>
    <script>    
        var jobj = "";
        $(document).ready(function () {
            InizializzaDati();
        });
        function InizializzaDati(devicesload, callback) {
            var devicesload = devicesload || true;
            $.ajax({
                url: pathAbs + pushhandlerpath,
                contentType: "application/json; charset=utf-8",
                //async: false,
                cache: false,
                data: { 'q': 'inizializzapushserver', 'lng': lng },
                success: function (result) {
                    try {
                        jobj = JSON.parse(result);
                        var PublicKey = jobj["VapidKeys"]["PublicKey"];
                        var PrivateKey = jobj["VapidKeys"]["PrivateKey"];
                        $("#spanPublicKey").text(PublicKey);
                        $("#spanPrivateKey").text(PrivateKey);
                        if (devicesload == true)
                            getdatadevices();
                        if (callback != null)
                            callback();
                    }
                    catch (e) {
                        console.log('fail init push', e);
                        if (callback != null)
                            callback();
                    }
                },
                failure: function (result) {
                    console.log('fail init push', '');
                    if (callback != null)
                        callback();
                }
            })
        };

        function GenerateKeys() {
            // call functon to generate keys and display ( use ajax call)
            $.ajax({
                url: pathAbs + pushhandlerpath,
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                type: "POST",
                cache: false,
                data: { 'q': 'generatekeys', 'lng': lng },
                success: function (result) {
                    try {
                        jobj = JSON.parse(result);
                        var PublicKey = jobj["VapidKeys"]["PublicKey"];
                        var PrivateKey = jobj["VapidKeys"]["PrivateKey"];
                        $("#spanPublicKey").text(PublicKey);
                        $("#spanPrivateKey").text(PrivateKey);
                    }
                    catch (e) {
                        console.log('fail generate keys push', e);
                    }
                },
                failure: function (result) {
                    console.log('fail  generate keys push', '');
                }
            })

            //Alternativa da testare
            //var request = new XMLHttpRequest();
            //var url = pathAbs + pushhandlerpath;
            //request.open("POST", url, true);
            //request.setRequestHeader("Content-Type", "application/json; charset=utf-8");
            //var data = JSON.stringify({ 'q': 'generatekeys', 'lng': lng });
            //request.send(data);
            //request.onreadystatechange = function () {
            //    if (request.readyState === 4 && request.status === 200) {
            //        try {
            //            jobj = JSON.parse(request.response);
            //            var PublicKey = jobj["VapidKeys"]["PublicKey"];
            //            var PrivateKey = jobj["VapidKeys"]["PrivateKey"];
            //            $("#spanPublicKey").text(PublicKey);
            //            $("#spanPrivateKey").text(PrivateKey);
            //        }
            //        catch (e) {
            //            console.log('fail generate keys push', e);
            //        }
            //    }
            //    else {
            //        console.log('fail  generate keys push', '');
            //    }
            //};
        }
    </script>
    <h2>Configure Keys</h2>
    <div class="alert alert-warning alert-dismissable">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <strong>Developer Notes:</strong>
        <ul>
            <li>Web Push notification require a public and private key to send a payload.</li>
            <li>Newly generated keys for you to use are below.</li>
            <li>Clients must generate subscription using public key, and server uses private/public key to send notification</li>
        </ul>
        <div class="form-group">
            <label class="control-label">Public Key / applicationServerPublicKey</label>
            <span id="spanPublicKey">
                <%= WelcomeLibrary.UF.ConfigManagement.ReadKey("PublicKey")  %>
            </span>
        </div>
        <div class="form-group">
            <label class="control-label">Private Key</label>
            <span id="spanPrivateKey">
                <%= WelcomeLibrary.UF.ConfigManagement.ReadKey("PrivateKey")  %>
            </span>
        </div>
        <div class="form-group">
            <input type="button" value="Regenerate Keys" class="btn btn-primary" onclick="javascript: GenerateKeys()" />
        </div>
    </div>

    <script>    
        $(document).ready(function () {
            $("#Title, #Message, #Link, #Tag").keyup(function () {
                var payloadObject = {
                    title: $("#Title").val(),
                    message: $("#Message").val(),
                    link: $("#Link").val(),
                    tag: $("#Tag").val()
                };
                $("#Payload").val(JSON.stringify(payloadObject));
            });
        });
        function SendPush() {
            //va chiamata la funzione serverside che passando il payload $("#Payload").val() ed eventualmente l'id del device (da pescare da una lista di selezione dei device sottoscritti aò servizio ) a cui inviare esegiue il push pushDM.SendNotification
            jobj["payload"] = $("#Payload").val();
            $.ajax({
                url: pathAbs + pushhandlerpath,
                dataType: "text",
                type: "POST",
                //async: false,
                cache: false,
                data: { 'q': 'sendnotification', 'lng': lng, 'pushcontainer': JSON.stringify(jobj) },
                success: function (result) {
                    try {
                        console.log('push Done');
                        // jobj = JSON.parse(result);
                        $("#spanResponse").text('Push Done');
                    }
                    catch (e) {
                        console.log('fail sending push', e);
                        $("#spanResponse").text('fail sending push');
                    }
                },
                failure: function (result) {
                    console.log('fail  sending push', '');
                    $("#spanResponse").text('fail sending push');
                }
            })
        }
    </script>

    <div class="container">
        <div class="row">
            <div class="col-xs-12">
                <h2>Push notifications</h2>
                <span style="font-size: 12px;">Type your Title and Message and your payload to notify to pass to push notificator will be generated below.</span>
                <hr />
                <div class="form-group">
                    <label class="control-label" for="Title">Title</label>
                    <input class="form-control" type="text" id="Title" name="Title" value="" />
                </div>
                <div class="form-group">
                    <label class="control-label" for="Message">Message</label>
                    <input class="form-control" type="text" id="Message" name="Message" value="" />
                </div>
                <div class="form-group">
                    <label class="control-label" for="Link">Link</label>
                    <input class="form-control" type="text" id="Link" name="Link" value="https://www.webmouse.sm" />
                </div>
                <div class="form-group">
                    <label class="control-label" for="Tag">Tag</label>
                    <input class="form-control" type="text" id="Tag" name="Tag" value="notificationtag1" />
                </div>
                <div class="form-group">
                    <label class="control-label" for="Payload">Payload</label>
                    <textarea id="Payload" name="Payload" style="width: 500px; height: 200px;"></textarea>
                </div>
                <div class="form-group">
                    <input type="button" value="Send Push" class="btn btn-primary" onclick="javascript: SendPush()" />
                </div>
                <div class="form-group">
                    <span id="spanResponse"></span>
                </div>
            </div>
        </div>
    </div>
    <script>

        function askPermission() {
            return new Promise(function (resolve, reject) {
                const permissionResult = Notification.requestPermission(function (result) {
                    resolve(result);
                });
                if (permissionResult) {
                    permissionResult.then(resolve, reject);
                }
            }).then(function (permissionResult) {
                if (permissionResult !== 'granted') {
                    throw new Error('We weren\'t granted permission.');
                } else { console.log('Push ntfy permission granted'); }
            })
        }
        function getNotificationPermissionState() {
            if (navigator.permissions) {
                return navigator.permissions.query({ name: 'notifications' })
                    .then((result) => {
                        return result.state;
                    });
            }
            return new Promise((resolve) => {
                resolve(Notification.permission);
            });
        }

        function unSubscribeTonotifications() {
            navigator.serviceWorker.ready.then(function (registration) {
                registration.pushManager.getSubscription()
                    .then(function (subscription) {
                        if (subscription) {
                            return subscription.unsubscribe().then(function (successful) {
                                deleteDatadevice(jobj["Devices"].Id);
                                //removePushSubscription(subscription);
                                jobj["Devices"].Id = 0;
                                jobj["Devices"].Name = "";
                                jobj["Devices"].PushP256DH = "";
                                jobj["Devices"].PushAuth = "";
                                jobj["Devices"].PushEndpoint = "";
                                $('#lblidactual').text('');
                                $('#Name').val('');
                                $('#PushEndpoint').val('');
                                $('#PushP256DH').val('');
                                $('#PushAuth').val('');
                                $("#spanSubscriptionsResponse").text('User unsubscribed');
                                console.log('User is unsubscribed.');
                                pushM.isSubscribed = false;
                                //Rimuovere dalla tabella
                                // da fare
                            }).catch(function (error) {
                                $("#spanSubscriptionsResponse").text('Error unsubscribing');
                                console.log('Error unsubscribing', error);
                            })
                        }
                    })
                    .catch(function (error) {
                        $("#spanSubscriptionsResponse").text('Error unsubscribing');
                        console.log('Error unsubscribing', error);
                    })
            });
        }

        function SubscribeTonotifications() {
            //askPermission();
            pushM.applicationServerPublicKey = jobj["VapidKeys"]["PublicKey"];//Imposto la chiave pubblica corretta per il server attualemente attiva
            navigator.serviceWorker.ready.then(function (registration) {

                // Do we already have a push message subscription?
                registration.pushManager.getSubscription()
                    .then(function (subscription) {

                        //Controllo se sottoscritto e la puclikkey coincide
                        if (subscription) {
                            if (pushM.base64Encode(subscription.options.applicationServerKey) != pushM.applicationServerPublicKey) //sottoscrizione non più valida per cambio keys->unsubscribe
                                subscription.unsubscribe().then(function (successful) { SubscribeTonotifications(); return; });
                        }

                        pushM.isSubscribed = !(subscription === null);
                        if (pushM.isSubscribed) {

                            //pushM.base64Encode(subscription.options.applicationServerKey);
                            //pushM.applicationServerPublicKey
                            //pushM.base64Encode(subscription.getKey('p256dh'))
                            // pushM.base64Encode(subscription.getKey('auth'))
                            //subscription.toJSON().keys.auth

                            console.log('User is already subscribed to push notifications');
                            $("#spanSubscriptionsResponse").text('User is already subscribed to push notifications');
                        } else {
                            $("#spanSubscriptionsResponse").text('User is not yet subscribed to push notifications');
                            console.log('User is not yet subscribed to push notifications');
                            //Facciamo la sottosrizione
                            var subscribeParams = { userVisibleOnly: true };
                            var applicationServerKey = pushM.urlB64ToUint8Array(pushM.applicationServerPublicKey);
                            subscribeParams.applicationServerKey = applicationServerKey;
                            registration.pushManager.subscribe(subscribeParams)
                                .then(function (subscription) {
                                    pushM.isSubscribed = true;
                                    var p256dh = pushM.base64Encode(subscription.getKey('p256dh'));
                                    var auth = pushM.base64Encode(subscription.getKey('auth'));
                                    console.log(subscription);
                                    var Name = $('#Name').val();
                                    $('#PushEndpoint').val(subscription.endpoint);
                                    $('#PushP256DH').val(p256dh);
                                    $('#PushAuth').val(auth);
                                    $('#lblidactual').text('');
                                    jobj["Devices"].Id = 0;
                                    jobj["Devices"].Name = Name;
                                    jobj["Devices"].PushP256DH = p256dh;
                                    jobj["Devices"].PushAuth = auth;
                                    jobj["Devices"].PushEndpoint = subscription.endpoint;
                                    //Qui devo aggiungere a modificare il Device in tabella db
                                    updateDatasubscriptions(); //Memorizzo la sottoscrizione nell'application server

                                    $("#spanSubscriptionsResponse").text('User is subscribed to push notifications');
                                    console.log('User is  subscribed to push notifications');
                                })
                                .catch(function (e) {
                                    errorHandlerDevice('[subscribe] Unable to subscribe to push', e);
                                });
                        }
                    })
                    .catch(function (err) {
                        console.log('[req.pushManager.getSubscription] Unable to get subscription details.', err);
                    });

            });
        };
        function updateDatasubscriptions() {

            $(".loader").fadeIn("slow");
            var pushcontainer = JSON.stringify(jobj);
            if (pushcontainer != '' && pushcontainer != null) {
                $.ajax({
                    type: "POST",
                    url: pathAbs + pushhandlerpath,
                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    data: {
                        'q': 'subscribedevice', 'pushcontainer': pushcontainer
                    },
                    success: OnCompleteupdateDevice,
                    error: errorHandlerDevice
                });
            }
        }
        function OnCompleteupdateDevice(result) {
            try {
                //result contiene l'id dell'elemento appena aggiunto !! da capire se serve
                getdatadevices(); //Ricarico il tutto
                $("#spanSubscriptionsResponse").text('Richiesta Aggiornamento Completata. ' + result);
                $(".loader").fadeOut("slow");
            }
            catch (e) {
                $(".loader").fadeOut("slow");
                $("#spanSubscriptionsResponse").text('Richiesta Aggiornamento error. ' + e);
            }
        }
        function errorHandlerDevice(message, e) {
            if (typeof e == 'undefined') {
                e = null;
            }
            console.error(message, e);
            $("#spanSubscriptionsResponse").text(message);
        }

        function deleteDatadevice(idtodel) {

            var idtodel = idtodel || 0;
            if (idtodel != 0)
                jobj["Devices"].Id = idtodel;
            $(".loader").fadeIn("slow");
            var pushcontainer = JSON.stringify(jobj);
            if (pushcontainer != '' && pushcontainer != null) {
                $.ajax({
                    type: "POST",
                    url: pathAbs + pushhandlerpath,
                    contentType: "application/json; charset=utf-8",
                    cache: false,
                    data: {
                        'q': 'deletesubscribebyid', 'pushcontainer': pushcontainer
                    },
                    success: OnCompletedeleteDevice,
                    error: errorHandlerDevice
                });
            }
        }
        function OnCompletedeleteDevice(result) {
            try {
                //result contiene l'id dell'elemento appena aggiunto !! da capire se serve
                getdatadevices(); //Ricarico il tutto
                jobj["Devices"].Id = 0;
                jobj["Devices"].Name = "";
                jobj["Devices"].PushP256DH = "";
                jobj["Devices"].PushAuth = "";
                jobj["Devices"].PushEndpoint = "";
                $('#lblidactual').text('');
                $('#Name').val('');
                $('#PushEndpoint').val('');
                $('#PushP256DH').val('');
                $('#PushAuth').val('');
                $("#spanSubscriptionsResponse").text('Richiesta Cancellazione Completata. ' + result);
                $(".loader").fadeOut("slow");
            }
            catch (e) {
                $(".loader").fadeOut("slow");
                $("#spanSubscriptionsResponse").text('Richiesta Cancellazione error. ' + e);
            }
        }
        function getdatadevices() {  //caricamento dei devices evices 

            var objfiltro = objfiltro || "";
            var page = page || "";
            var pagesize = pagesize || "";
            var enablepager = enablepager || false;

            $.ajax({
                url: pathAbs + pushhandlerpath,
                contentType: "application/json; charset=utf-8",
                global: false,
                cache: false,
                dataType: "text",
                type: "POST",
                //async: false,
                data: { 'q': 'deviceslist' },
                success: function (result) {
                    if (result != '') {
                        var datarray = JSON.parse(result);
                        jobj = datarray;
                        databind('row1', 'row1', datarray.DevicesList);
                    }

                },
                error: function (e) {
                    //sendmessage('fail creating link');
                    errorHandlerDevice(result.responseText, e);
                }
            });
        }
        var templatehtml;
        function databind(containerid, templateid, data) {
            if (!data.length) {
                // $('#' + el).html(''); 
                //$('#' + objfiltrotmp.containerid).html('');
                $('#' + containerid).html('');
                return;
            }
            //Memorizzo il template per i successivi binding
            var str = $($('#' + templateid)[0]).html();
            if (templatehtml == null) {
                str = $($('#' + templateid)[0]).html();
                templatehtml = str;
            } else str = templatehtml;

            var jquery_obj = $(str);
            var container = $('#' + containerid);
            $(container).html('');
            for (var j = 0; j < data.length; j++) {
                FillBindControls(jquery_obj.wrap('<p>').parent(), data[j], "", "",
                    function (ret) {
                        $(container).append(ret.html()) + "<br/>\r\n";
                    });
            }
            bindclickevent();
        }
        function bindclickevent() {
            $('label').on("click", function () {
                selectdevice((this));
            });

        }
        function selectdevice(el) {
            try {
                /*Qui ho dei sottoelementi nella struttura del json*/
                //Stabiliamo il livello di annidamento del controllo nella struttura in base all'attributo mybind 
                var idbind = $(el).attr("idbind");
                if (idbind != null) {

                    var indexToSelect;
                    $.each(jobj.DevicesList, function (index) {
                        if (this.Id == idbind) {
                            indexToSelect = index;
                            return false;
                        }
                    });
                    if (indexToSelect != null) {
                        jobj["Devices"].Id = idbind;
                        jobj["Devices"].Name = jobj.DevicesList[indexToSelect].Name;
                        jobj["Devices"].PushP256DH = jobj.DevicesList[indexToSelect].PushP256DH;
                        jobj["Devices"].PushAuth = jobj.DevicesList[indexToSelect].PushAuth;
                        jobj["Devices"].PushEndpoint = jobj.DevicesList[indexToSelect].PushEndpoint;

                        $('#lblidactual').text(idbind);
                        $('#Name').val(jobj["Devices"].Name);
                        $('#PushEndpoint').val(jobj["Devices"].PushEndpoint);
                        $('#PushP256DH').val(jobj["Devices"].PushP256DH);
                        $('#PushAuth').val(jobj["Devices"].PushAuth);
                    }
                    //alert(idbind);
                }
            }
            catch (e) { }
        }
        $(document).ready(function () {
            //  getdatadevices();
        });

        function requesclientpermission() {
            Notification.requestPermission(function (result) {
                if (result !== 'granted') {
                    //handle permissions deny
                    console.log('Permission denied');
                } else {
                    console.log('Permission granted');
                }
            });
        }
    </script>
    <div class="container">
        <div class="row">
            <div class="col-xs-12">
                <h2>Push subscriptions ( Devices )</h2>
                <span style="font-size: 12px;">To be DONE, gestione delle subscriptions  (Create, Delete , List)-> vedi devices controller .</span>
                <div class="alert alert-warning alert-dismissable">
                    <div class="form-group">
                        <input type="button" value="Request client permission" class="btn btn-primary" onclick="javascript: requesclientpermission()" />
                        <input type="button" value="Subscribe to Push notifications" class="btn btn-primary" onclick="javascript: SubscribeTonotifications()" />
                        <input type="button" value="UNSubscribe to Push notifications" class="btn btn-primary" onclick="javascript: unSubscribeTonotifications()" />
                        <input type="button" value="Cancella id selezionato" class="btn btn-primary" onclick="javascript: deleteDatadevice()" />
                    </div>

                    <div class="form-group">
                        <label class="control-label" id="lblidactual">Id</label>
                    </div>
                    <div class="form-group">
                        <label class="control-label" for="Name">Name</label>
                        <input class="form-control" type="text" id="Name" name="Name" value="" />
                    </div>
                    <div class="form-group">
                        <label class="control-label" for="PushEndpoint">PushEndpoint</label>
                        <input class="form-control" type="text" id="PushEndpoint" name="PushEndpoint" value="" />
                    </div>
                    <div class="form-group">
                        <label class="control-label" for="PushP256DH">PushP256DH</label>
                        <input class="form-control" type="text" id="PushP256DH" name="PushP256DH" value="" />
                    </div>
                    <div class="form-group">
                        <label class="control-label" for="PushAuth">PushAuth</label>
                        <input class="form-control" type="text" id="PushAuth" name="PushAuth" value="" />
                    </div>
                    <div class="form-group">
                        <span id="spanSubscriptionsResponse"></span>
                    </div>
                </div>
                <p>&nbsp;</p>
                <div style="background-color: #ddd; padding: 20px">
                    <h2>Devices presenti in archivio</h2>
                    <div class="row" id="row1">
                        <div class="col-sm-12">
                            <div>
                                <label class="bind" mybind="Id" idbind="Id" style="font-size: 2rem; color: green; cursor: pointer"></label>
                                <br />
                                <label class="bind" mybind="Name"></label>
                                <br />
                                <label class="bind" mybind="PushEndpoint"></label>
                                <br />
                                <label class="bind" mybind="PushP256DH"></label>
                                <br />
                                <label class="bind" mybind="PushAuth"></label>
                                <br />
                                <br />
                                <hr />
                            </div>
                        </div>
                    </div>
                </div>
                <p>&nbsp;</p>
            </div>
        </div>
    </div>
</asp:Content>

