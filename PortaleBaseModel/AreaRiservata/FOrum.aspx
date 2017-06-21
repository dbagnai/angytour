<%@ Page Language="C#" MasterPageFile="~/AspNetPages/MasterPage.master" AutoEventWireup="true" CodeFile="forum.aspx.cs"
    Inherits="AreaRiservata_Default" Title="Pagina senza titolo" MaintainScrollPositionOnPostback="true" %>

<%@ MasterType VirtualPath="~/AspNetPages/MasterPage.master" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="~/AspNetPages/UC/PagerEx.ascx" TagName="PagerEx" TagPrefix="UC" %>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolderSubhead" runat="Server">

    <script type="text/javascript">

        //var prm = Sys.WebForms.PageRequestManager.getInstance();
        //var stopPost = false;
        //function pageLoad() {
        //    prm.add_initializeRequest(initializeRequest);
        //}
        //function initializeRequest(sender, args) {
        //    if (stopPost == true) {
        //        args.set_cancel(true);
        //        stopPost = false;
        //    }

        //    if (prm.get_isInAsyncPostBack()) //Se già in postback asincrono non ne permetto due insieme
        //    {
        //        alert('Richiesta già in corso attendere il termine.');
        //        args.set_cancel(true);
        //    }
        //}

    </script>
    <div class="row">
        <div class="section-content top-body">
            <div class="container">
                <div class="col-md-3 col-sm-3">
                    <img runat="server" src="~/AreaRiservata/media/gist-symbol.png" style="max-width: 100px" />
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="content-box content-style3">
                        <%--<div class="content-style3-icon fa fa-quote-right"></div>--%>
                        <div class="content-style3-title">
                            <h2 class="h1-body-title" style="color: #5c5c5c">
                                <asp:Literal Text="" runat="server" ID="litNomePagina" />
                            </h2>
                        </div>
                        <div class="content-style3-text">
                            <asp:Literal Text="" runat="server" ID="litTextHeadPage" />
                        </div>
                    </div>
                </div>
                <div class="col-sm-3">
                </div>
            </div>
        </div>
    </div>

    <div style="background-color: rgba(0, 0, 0, 0.2);">
        <div class="container" style="padding-top: 20px; padding-bottom: 20px">

            <div class="row" style="padding: 10px">
                <div class="col-sm-offset-1 col-sm-10">
                    <div class="accordion" data-toggle="on" data-active-index="">
                        <div class="accordion-row">
                            <div class="title">
                                <div class="open-icon"></div>
                                <h3>Clicca qui per Cercare un argomento di discussione o Inserirne uno nuovo</h3>
                            </div>
                            <div class="desc">

                                <h3>Cerca un argomento di discussione:</h3>
                                <div class="sidebar-block" style="display: block">
                                    <div class="sidebar-content tags blog-search">
                                        <div class="input-group">
                                            <input class="form-control blog-search-input text-input" name="q" type="text" placeholder='<%# references.ResMan("Common", Lingua,"TestoCercaBlog") %>' runat="server" id="inputCerca" />
                                            <span class="input-group-addon">
                                                <button onserverclick="Cerca_Click" id="BtnCerca" class="blog-search-button icon-search" runat="server" clientidmode="Static" />
                                            </span>
                                        </div>
                                    </div>
                                </div>

                                <h3>Inserisci un nuovo argomento di discussione:</h3>
                                <asp:TextBox CssClass="form-control" ID="txtDenominazioneI" placeholder="Titolo" Style="background-color: #fafafa" Width="100%" runat="server" />
                                <br />
                                <asp:TextBox CssClass="form-control" ID="txtDescrizioneI" Width="100%" placeholder="Testo"
                                    TextMode="MultiLine" Font-Names="Lato"
                                    Height="200px" Style="background-color: #fafafa" runat="server" /><br />
                                <div class="row">
                                    <div class="col-sm-offset-10 col-sm-2">
                                        <asp:Button OnClick="Inseriscibtn_Click" OnClientClick="javascript:disablebutton(this)" UseSubmitBehavior="false" class="btn btn-block btn-primary pull-right" Text="Inserisci" runat="server" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>

                    <asp:Literal Text="" ID="output" runat="server" />
                </div>

            </div>
            <div class="row">
                <div class="col-sm-offset-1 col-sm-10">
                    <div class="divider stripe-3" style="margin-bottom: 5px"></div>
                    <div class="pull-right">
                        <a class="buttonstyle" style="margin: 20px" href="<%= CommonPage.CreaLinkRoutes(null,false,Lingua,"-","",Tipologia)  %>">Vedi tutti gli argomenti</a>
                    </div>
                    <h3 style="color: #fff; margin-bottom: 5px; padding-bottom: 0px;">Lista argomenti di discussione:</h3>
                    <script type="text/javascript">
                        /* FUNZIONI DI GESTIONE DEL FORUM VIA AJAX CALL */

                        function disablebutton(buttonelem) {
                            $(buttonelem).attr('disabled', true);
                        }

                        function jsinseriscirepost(eleminsrepost) {

                            $(eleminsrepost).css("visibility", "hidden")

                            var idvalue = $(eleminsrepost).parent().find("[id*='divinsertrepostlocalid']")[0].innerHTML;
                            var denominazione = $(eleminsrepost).parent().find("[id*='txtDenominazioneI']")[0].value;
                            var descrizione = $(eleminsrepost).parent().find("[id*='txtDescrizioneI']")[0].value;
                            var userattuale = $(eleminsrepost).parent().find("[id*='divinsertrepostcuruser']")[0].innerHTML;

                            var contenitoredestinazione = $(eleminsrepost).parent().find("[id*='responseContainerInsRepost']");
                            InserisciRePost(contenitoredestinazione, idvalue, denominazione, descrizione, userattuale);
                        }
                        function InserisciRePost(contenitoredestinazione, idvalue, denominazione, descrizione, userattuale) {
                            $.ajax({
                                destinationControl: contenitoredestinazione,
                                type: "POST",
                                url: "<%= PercorsoAssolutoApplicazione %>" + "/AreaRiservata/forum.aspx/InserisciRePost",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: '{id: "' + idvalue + '" , denominazione: "' + denominazione + '" , descrizione:  "' + descrizione + '" , userattuale : "' + userattuale + '"  }',
                                success: function (data) {
                                    OnSuccessInseriscirepost(data, this.destinationControl);
                                },
                                failure: function (response) {
                                    alert(response.d);
                                    //this.destinationControl.empty();
                                    //this.destinationControl.append(response.d);
                                }
                            });
                        }
                        function OnSuccessInseriscirepost(response, destination) {
                            // alert(response.d);
                            // alert(destination[0].id);//Controllo destinazione html
                            destination.empty();
                            destination.append(response.d);
                            __doPostBack('', 'aggiornavisualizzazioneconmail'); //QUesto ricarica tutti i post ( da fare se necessario )
                        }

                        /* ----------------- GESTIONE TASTO CANCELLAZIONE POST ----------------- */
                        function jscancellapost(elemcancella) {
                            var conferma = confirm('Sei sicuro di voler cancellare questo argomento ?');
                            if (conferma) {
                                /*Cerchiamo i valori per l'aggiornamento risalendo al parent del chiamante nella struttura dell'html */
                                // var id = $(elem).parent().find("[id*='Identifier']").attr("class");
                                var idvalue = $(elemcancella).parent().find("[id*='Identifier']")[0].innerHTML;
                                var contenitoredestinazione = $(elemcancella).parent().find("[id*='responseContainer']");
                                CancellaPost(contenitoredestinazione, idvalue);
                            }
                        }
                        function CancellaPost(contenitoredestinazione, idvalue) {
                            $.ajax({
                                destinationControl: contenitoredestinazione,
                                type: "POST",
                                url: "<%= PercorsoAssolutoApplicazione %>" + "/AreaRiservata/forum.aspx/CancellaPost",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: '{id: "' + idvalue + '"  }',
                                success: function (data) {
                                    OnSuccessDelete(data, this.destinationControl);
                                },
                                failure: function (response) {
                                    alert(response.d);
                                    //this.destinationControl.empty();
                                    //this.destinationControl.append(response.d);
                                }
                            });
                        }
                        function OnSuccessDelete(response, destination) {
                            // alert(response.d);
                            // alert(destination[0].id);//Controllo destinazione html
                            destination.empty();
                            destination.append(response.d);
                            __doPostBack('', 'aggiornavisualizzazione'); //QUesto ricarica tutti i post se necessario
                        }
                        /* ----------------- GESTIONE TASTO AGGIORNAMENTO POST ----------------- */
                        function jsaggiornapost(elemaggiorna) {

                            /*Cerchiamo i valori per l'aggiornamento risalendo al parent del chiamante nella struttura dell'html */

                            // var id = $(elem).parent().find("[id*='Identifier']").attr("class");
                            var idvalue = $(elemaggiorna).parent().find("[id*='Identifier']")[0].innerHTML;
                            var denominazione = $(elemaggiorna).parent().find("[id*='txtDenominazioneI_post']")[0].value;
                            var descrizione = $(elemaggiorna).parent().parent().find("[id*='txtDescrizioneI_post']")[0].value;
                            var contenitoredestinazione = $(elemaggiorna).parent().find("[id*='responseContainer']");

                            //  $(elemaggiorna).attr('disabled', true);//SPengo il bottone
                            contenitoredestinazione.append("Attendi per aggiornamento del post ..");

                            // alert(idvalue);
                            AggiornaPost(contenitoredestinazione, idvalue, denominazione, descrizione);
                        }
                        function AggiornaPost(contenitoredestinazione, idvalue, denominazione, descrizione) {
                            $.ajax({
                                destinationControl: contenitoredestinazione,
                                type: "POST",
                                url: "<%= PercorsoAssolutoApplicazione %>" + "/AreaRiservata/forum.aspx/AggiornaPost",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: '{id: "' + idvalue + '" , denominazione: "' + denominazione + '" , descrizione:  "' + descrizione + '"  }',
                                success: function (data) {
                                    OnSuccessAggiornaPost(data, this.destinationControl);
                                },
                                failure: function (response) {
                                    alert(response.d);
                                    //this.destinationControl.empty();
                                    //this.destinationControl.append(response.d);
                                }
                            });
                        }
                        function OnSuccessAggiornaPost(response, destination) {
                            // alert(response.d);
                            // alert(destination[0].id);//Controllo destinazione html
                            destination.empty();
                            destination.append(response.d);
                            //__doPostBack('', 'aggiornavisualizzazioneconmail'); //QUesto ricarica tutti i post se necessario
                        }

                        /* ----------------- GESTIONE TASTO CANCELLAZIONE FILE PER IL POST PRINCIPALE ----------------- */
                        function fileDeletePost(idclicked, nomefile) {
                            var conferma = confirm('Sei sicuro di voler cancellare questo file ?');
                            if (conferma) {



                                DeleteFilePost("", idclicked, nomefile)
                            }
                        }
                        function DeleteFilePost(contenitoredestinazione, idvalue, nomefile) {

                            $.ajax({
                                destinationControl: contenitoredestinazione,
                                type: "POST",
                                url: "<%= PercorsoAssolutoApplicazione %>" + "/AreaRiservata/Handlers/FileUploadHandler.ashx?id=" + idvalue + "&Tipologia=" + "<%= Tipologia %>&Azione=Cancella&nomefile=" + nomefile,
                                data: "",
                                contentType: false,
                                processData: false,
                                success: function (result) {
                                    OnSuccessDeleteFilePost(result, this.destinationControl);
                                },
                                failure: function (response) {
                                    alert(response);
                                    //this.destinationControl.empty();
                                    //this.destinationControl.append(response.d);
                                }
                            });
                        }
                        function OnSuccessDeleteFilePost(response, destination) {
                            alert(response);
                            // alert(destination[0].id);//Controllo destinazione html
                            //destination.empty();
                            //destination.append(response);
                            __doPostBack('', 'aggiornavisualizzazione'); //QUesto ricarica tutti i post se necessario
                        }

                        /* ----------------- GESTIONE TASTO CARICAMENTO FILE PER IL POST PRINCIPALE ----------------- */
                        function fileUpload(elemupload) {



                            //var id = $(elem).parent().find("[id*='Identifier']").attr("class");
                            var idvalue = $(elemupload).parent().find("[id*='divcaricafilepostlocalid']")[0].innerHTML;;
                            var uploadControl = $(elemupload).parent().find("[id*='UploadPost']");
                            var descrizione = $(elemupload).parent().find("[id*='txtDescrizioneFile']")[0].value;;
                            var contenitoredestinazione = $(elemupload).parent().find("[id*='responsepostUpload']");

                            $(elemupload).css("visibility", "hidden");//SPengo il bottone
                            contenitoredestinazione.append("Attendi il caricamento del file..");

                            //var fileUpload = uploadControl.get(0);
                            var fileUpload = uploadControl;
                            //var files = fileUpload.files;
                            var files = fileUpload[0].files;

                            var data = new FormData();
                            for (var i = 0; i < files.length; i++) {
                                (files[i].name);
                                data.append(files[i].name, files[i]);
                            }

                            UploadFilePost(contenitoredestinazione, data, idvalue, descrizione)
                        }
                        function UploadFilePost(contenitoredestinazione, data, idvalue, descrizione) {

                            $.ajax({
                                destinationControl: contenitoredestinazione,
                                type: "POST",
                                url: "<%= PercorsoAssolutoApplicazione %>" + "/AreaRiservata/Handlers/FileUploadHandler.ashx?id=" + idvalue + "&Tipologia=" + "<%= Tipologia %>&Azione=Inserisci&Descrizione=" + descrizione,
                                data: data,
                                contentType: false,
                                processData: false,
                                success: function (result) {
                                    OnSuccessUploadFilePost(result, this.destinationControl);
                                },
                                failure: function (response) {
                                    alert(response);
                                    //this.destinationControl.empty();
                                    //this.destinationControl.append(response.d);
                                }
                            });
                        }
                        function OnSuccessUploadFilePost(response, destination) {
                            alert(response);
                            // alert(destination[0].id);//Controllo destinazione html
                            destination.empty();
                            destination.append(response);
                            __doPostBack('', 'aggiornavisualizzazione'); //QUesto ricarica tutti i post se necessario
                        }

                        function setaccordionpostopened(elemtitle) {
                            var idvalue = $(elemtitle).find("[id*='Identifier']")[0].innerHTML;
                            var oldvalue = $get('<%= hidCurrentPostActive.ClientID %>').value;
                            $get('<%= hidCurrentPostActive.ClientID %>').value = idvalue;
                        }
                        function OpenAccordionRowbyId(idselected) {
                            if (idselected != "") {
                                var idtoselectforopen = "titlepost-" + idselected;

                                /*APRIAMO IL CORRETTO ELEMENTO DELL'ACCORDION*/
                                var title = ($("#accordionForum").find("[id*='" + idtoselectforopen + "']"));
                                var icon = title.find('.open-icon');
                                var desc = title.parent().find('.desc');
                                desc.slideDown('fast');
                                icon.addClass('close-icon');
                                title.addClass('active');
                            }

                        }

                        jQuery(document).ready(function ($) {

                            OpenAccordionRowbyId($get('<%= hidCurrentPostActive.ClientID %>').value);

                        });
                        /*FINE FUNZIONI DI GESTIONE DEL FORUM VIA AJAX CALL */

                    </script>
                    <asp:HiddenField runat="server" ID="hidCurrentPostActive" />
                    <%-- SEZIONE POST DEL FORUM--%>
                    <div class="accordion" data-toggle="off" id="accordionForum">
                        <asp:Repeater runat="server" ID="rptPosts">
                            <ItemTemplate>
                                <div class="accordion-row">
                                    <div id="<%# "titlepost-" + Eval("Id").ToString() %>" class="title" onclick="javascript:setaccordionpostopened(this)">
                                        <%--SISTEMA I LINKBUTTON  PER LA VISUALIZZAZIONE IN BASE AI DIRITTI UTENTE SUL POST!!!! --%>
                                        <%--<asp:LinkButton Visible='<%# ControllaVisibilitaAutore(Eval("Autore").ToString()) %>' runat="server" OnClick="lnkCancellaPost_Click" ID="LinkButton1" CommandArgument='<%# Eval("Id") %>'>  <div class="fa fa-times-circle" style="color:#153556">Cancella</div></asp:LinkButton>--%>
                                        <a visible='<%# ControllaVisibilitaAutore(Eval("Autore").ToString()) %>' runat="server" onclick="javascript:jscancellapost(this)" id="aCancellaPost" onmouseover="this.style.cursor='pointer'">
                                            <div class="fa fa-times-circle" style="color: #153556">Cancella</div>
                                        </a>
                                        &nbsp;
                                <%--<asp:LinkButton Visible='<%# ControllaVisibilitaAutore(Eval("Autore").ToString()) %>' runat="server" OnClick="lnkModificaPost_Click" ID="lnkModificaPost" CommandArgument='<%# Eval("Id") %>'>  <div class="fa fa-pencil" style="color:#153556">Aggiorna</div></asp:LinkButton>--%>
                                        <a visible='<%# ControllaVisibilitaAutore(Eval("Autore").ToString()) %>' runat="server" onclick="javascript:jsaggiornapost(this)" id="aModificaPost" onmouseover="this.style.cursor='pointer'">
                                            <div class="fa fa-pencil" style="color: #153556">Aggiorna</div>
                                        </a>
                                        &nbsp;

                                        <div class="pull-right"><%# CalcolaNumeroCommenti((int)Eval("Id")) %></div>

                                        <div class="clearfix"></div>
                                        <div id="responseContainer"></div>
                                        <div id="Identifier" style="display: none"><%#Eval("Id").ToString() %></div>
                                        <div class="open-icon"></div>
                                        <div class="testimonial">
                                            <div class="testimonial-content">
                                                <div class="testimonial-item" style="display: block;">
                                                    <div class="testimonial-person-pic">
                                                        <img runat="server" class="img-responsive" alt="Gist Forum" src='<%# VisualizzaImmagineSocio(Eval("Autore").ToString()) %>' /><%-- SISTEMA L'IMMAGINE DELL'AUTORE --%>
                                                    </div>
                                                    <div class="testimonial-text-title">
                                                        <textarea id="txtDenominazioneI_post" rows="2" runat="server" style="width: 100%" type="text" class="inputgray" value='<%#  Eval("Denominazione" + Lingua).ToString()  %>' />
                                                    </div>
                                                    <div class="testimonial-by">
                                                        <span class="testimonial-by-name"><%# CercaNomeSociobyUsername( Eval("Autore").ToString() ) %>,</span>
                                                        <span class="testimonial-by-position">Pubblicato il : <%# string.Format("{0:dd/MM/yyyy HH:mm:ss}", Eval("DataInserimento")) %></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="desc" style="display: none;">
                                        <div class="row">
                                            <div class="col-md-12 col-sm-12">
                                                <textarea id="txtDescrizioneI_post" runat="server" class="inputnoback" rows="10"> <%#  Eval("Descrizione" + Lingua).ToString()  %> </textarea>
                                            </div>
                                        </div>
                                        <div style="width: 100%; border-bottom: 1px solid #ccc; margin-top: 5px; margin-bottom: 10px"></div>
                                        <div class="row clearfix">
                                            <div class="col-md-6 col-sm-6">
                                                <h3 style="margin-bottom: 5px; padding-bottom: 0px;">Allegati alla discussione</h3>
                                                <div class="divider stripe-3" style="margin-top: 5px; margin-bottom: 5px"></div>
                                                <asp:Panel Style="background-color: #f0f0f0; padding: 10px" runat="server" Visible='<%# ControllaVisibilitaAutore(Eval("Autore").ToString()) %>'>
                                                    <div id="divcaricafilepostlocalid" style="display: none"><%# Eval("Id").ToString() %></div>
                                                    <input type="file" name="UploadPost" class="pull-left" id="UploadPost" multiple="multiple" />
                                                    &nbsp;&nbsp;
                                            <br />
                                                    <br />
                                                    <div class="pull-left">
                                                        Descrizione File :
                                            <br />
                                                        <input type="text" name="txtDescrizioneFile" id="txtDescrizioneFile" value="" style="max-width: 200px; background-color: #ccc" />
                                                    </div>
                                                    <a class="pull-left buttonstyle" onclick="javascript:fileUpload(this)" title="allega" target="_blank" onmouseover="this.style.cursor='pointer'">
                                                        <div style="font-size: 1em" class="fa fa-save">CARICA ALLEGATO</div>
                                                    </a>
                                                    <br />
                                                    <div class="pull-left" id="responsepostUpload"></div>
                                                    <br />
                                                </asp:Panel>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-6 col-sm-6">
                                                <%--SISTEMA QUI LA LISTA FILES ALLEGATI COMPLETA--%>
                                                <div id="divcancellafilepostlocalid" style="display: none"><%# Eval("Id").ToString() %></div>
                                                <div class="pull-left" id="responsepostDelete"></div>
                                                FILE ALLEGATI:<br />
                                                <ul class="lateralbar" style="list-style: none">
                                                    <%#  CrealistaFiles(Eval("Id"),  Eval("FotoCollection_M"),Eval("Autore").ToString()) %>
                                                </ul>
                                            </div>
                                            <div class="col-md-6 col-sm-6">
                                                <div class="cycle-slideshow" data-cycle-timeout="0" data-cycle-overlay-fx-in="slideDown" data-cycle-overlay-fx-out="slideUp" data-cycle-next=">.cycle-next" data-cycle-prev=">.cycle-prev" data-cycle-swipe="true" data-cycle-slides="> .slider-img">
                                                    <div class="fa fa-chevron-right cycle-next"></div>
                                                    <div class="fa fa-chevron-left cycle-prev"></div>
                                                    <%#  CrealistaImages(Eval("Id"),  Eval("FotoCollection_M")) %>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="divider stripe-4" style="margin-bottom: 10px"></div>
                                        <%--MAIN POST END--%>
                                        <%-- SEZIONE INSERIMENTO REPOST/COMMENTI--%>
                                        <div class="row" id="divRepostInsert" style="padding: 15px">
                                            <div class="col-sm-8" style="background-color: #f0f0f0;">
                                                <div id="divinsertrepostlocalid" style="display: none"><%# Eval("Id").ToString() %></div>
                                                <div id="divinsertrepostcuruser" style="display: none"><%# User.Identity.Name %></div>
                                                <h3 style="margin-top: 5px; margin-bottom: 5px">Inserisci un nuovo contributo alla discussione:</h3>
                                                <asp:TextBox CssClass="form-control" ID="txtDenominazioneI" placeholder="Titolo" Style="background-color: #fafafa" Width="100%" runat="server" Text="" />
                                                <br />
                                                <asp:TextBox CssClass="form-control" ID="txtDescrizioneI" Width="100%" placeholder="Testo" Text=""
                                                    TextMode="MultiLine" Font-Names="Lato"
                                                    Height="150px" Style="background-color: #fafafa" runat="server" /><br />
                                                <a class="buttonstyle" runat="server" onclick="javascript:jsinseriscirepost(this)" id="aInsertRepost" onmouseover="this.style.cursor='pointer'">
                                                    <div class="fa fa-file-o" style="color: #fff">INSERICI NUOVO COMMENTO</div>
                                                </a>
                                                <div id="responseContainerInsRepost"></div>
                                            </div>
                                        </div>
                                        <%--  FINE SEZIONE INSERIMENTO REPOST/COMMENTI--%>
                                        <%--SUBPOST REPEATER--%>
                                        <%--QUI SONO DA INSERIRE I REPOST COLLEGATI AL POST PRINCIPALE--%>
                                        <asp:Repeater runat="server" ID="rptRepost" DataSource='<%# dictRepost[(int)Eval("Id")] %>'>
                                            <ItemTemplate>
                                                <%-- QUI DEVI METTERE I TASTI AGGIORNA CANCELLA PER IL REPOST--%>
                                                <div class="divider stripe-4"></div>
                                                <div class="row">
                                                    <div class=" col-xs-offset-2 col-xs-10 col-md-10 col-sm-10 lateralbar">
                                                        <div id="responseContainer"></div>
                                                        <div id="Identifier" style="display: none"><%#Eval("Id").ToString() %></div>
                                                        <a visible='<%# ControllaVisibilitaAutore(Eval("Autore").ToString()) %>' runat="server" onclick="javascript:jscancellapost(this)" id="aCancellaPost" onmouseover="this.style.cursor='pointer'">
                                                            <div class="fa fa-times-circle" style="color: #153556">Cancella</div>
                                                        </a>
                                                        <a visible='<%# ControllaVisibilitaAutore(Eval("Autore").ToString()) %>' runat="server" onclick="javascript:jsaggiornapost(this)" id="aModificaPost" onmouseover="this.style.cursor='pointer'">
                                                            <div class="fa fa-pencil" style="color: #153556">Aggiorna</div>
                                                        </a>
                                                        <div class="clearfix"></div>
                                                        <div class="testimonial">
                                                            <div class="testimonial-content">
                                                                <div class="testimonial-item" style="display: block">
                                                                    <div class="testimonial-person-pic">
                                                                        <img runat="server" class="img-responsive" alt="Gist Forum" src='<%# VisualizzaImmagineSocio(Eval("Autore").ToString()) %>' /><%-- SISTEMA DINAMICAMENTE L'IMMAGINE DELL'AUTORE --%>
                                                                    </div>
                                                                    <div class="testimonial-text-title">
                                                                        <textarea id="txtDenominazioneI_post" rows="2" runat="server" type="text" class="inputgray" value='<%#  Eval("Denominazione" + Lingua).ToString()  %>' />
                                                                    </div>
                                                                    <div class="testimonial-by">
                                                                        <span class="testimonial-by-name"><%# CercaNomeSociobyUsername( Eval("Autore").ToString() ) %>,</span>
                                                                        <span class="testimonial-by-position">Pubblicato il : <%# string.Format("{0:dd/MM/yyyy HH:mm:ss}", Eval("DataInserimento")) %></span>
                                                                    </div>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-12 col-sm-12">
                                                                <textarea id="txtDescrizioneI_post" runat="server" class="inputnoback" rows="10"> <%#  Eval("Descrizione" + Lingua).ToString()  %> </textarea>
                                                            </div>
                                                        </div>
                                                        <div style="width: 100%; border-bottom: 1px solid #ccc; margin-top: 5px; margin-bottom: 10px"></div>
                                                        <div class="row clearfix">
                                                            <div class="col-md-6 col-sm-6">
                                                                <h3 style="margin-bottom: 5px; padding-bottom: 0px;">Allegati alla discussione</h3>
                                                                <div class="divider stripe-3" style="margin-top: 5px; margin-bottom: 5px"></div>
                                                                <asp:Panel Style="background-color: #f0f0f0; padding: 10px" runat="server" Visible='<%# ControllaVisibilitaAutore(Eval("Autore").ToString()) %>'>
                                                                    <div id="divcaricafilepostlocalid" style="display: none"><%# Eval("Id").ToString() %></div>
                                                                    <input type="file" name="UploadPost" class="pull-left" id="UploadPost" multiple="multiple" />
                                                                    &nbsp;&nbsp;
                                                            <br />
                                                                    <br />
                                                                    <div class="pull-left">
                                                                        Descrizione File :
                                                            <br />
                                                                        <input type="text" name="txtDescrizioneFile" id="txtDescrizioneFile" value="" style="max-width: 200px; background-color: #ccc" />
                                                                    </div>
                                                                    <a class="pull-left buttonstyle" onclick="javascript:fileUpload(this)" title="allega" target="_blank" onmouseover="this.style.cursor='pointer'">
                                                                        <div style="font-size: 1em" class="fa fa-save">CARICA ALLEGATO</div>
                                                                    </a>
                                                                    <br />
                                                                    <div class="pull-left" id="responsepostUpload"></div>
                                                                    <br />
                                                                </asp:Panel>
                                                            </div>
                                                        </div>
                                                        <div class="row">
                                                            <div class="col-md-6 col-sm-6">
                                                                <%--SISTEMA QUI LA LISTA FILES ALLEGATI COMPLETA--%>
                                                                <div id="divcancellafilepostlocalid" style="display: none"><%# Eval("Id").ToString() %></div>
                                                                <div class="pull-left" id="responsepostDelete"></div>
                                                                FILE ALLEGATI:<br />
                                                                <ul class="lateralbar" style="list-style: none">
                                                                    <%#  CrealistaFiles(Eval("Id"),  Eval("FotoCollection_M"),Eval("Autore").ToString()) %>
                                                                </ul>
                                                            </div>
                                                            <div class="col-md-6 col-sm-6">
                                                                <div class="cycle-slideshow" data-cycle-timeout="0" data-cycle-overlay-fx-in="slideDown" data-cycle-overlay-fx-out="slideUp" data-cycle-next=">.cycle-next" data-cycle-prev=">.cycle-prev" data-cycle-swipe="true" data-cycle-slides="> .slider-img">
                                                                    <div class="fa fa-chevron-right cycle-next"></div>
                                                                    <div class="fa fa-chevron-left cycle-prev"></div>
                                                                    <%#  CrealistaImages(Eval("Id"),  Eval("FotoCollection_M")) %>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="divider stripe-4"></div>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                        <%--SUBPOST REPEATER--%>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <%-- FINE SEZIONE POST DEL FORUM--%>

                        <br />
                        <br />
                        <br />
                        <br />

                        <asp:Panel runat="server" ID="pnlPager">
                            <div class="row" style="padding: 0px">
                                <div class="col-sm-3">
                                    <asp:Button ID="btnNext" class="button btn-flat" Text='<%# references.ResMan("Common", Lingua,"txtTastoNext") %>'
                                        runat="server" OnClick="btnNext_click" />
                                </div>
                                <div class="col-sm-6">

                                    <div id="pager" class="text-center">
                                        <UC:PagerEx ID="PagerRisultati" runat="server" NavigateUrl="" PageSize="8" CurrentPage="1"
                                            TotalRecords="0" dimensioneGruppo="10" nGruppoPagine="1" OnPageCommand="PagerRisultati_PageCommand"
                                            OnPageGroupClickNext="PagerRisultati_PageGroupClickNext" OnPageGroupClickPrev="PagerRisultati_PageGroupClickPrev" />
                                    </div>

                                </div>
                                <div class="col-sm-3">
                                    <asp:Button ID="btnPrev" class="button btn-flat" Text='<%# references.ResMan("Common", Lingua,"txtTastoPrev") %>'
                                        runat="server" OnClick="btnPrev_click" />
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>



