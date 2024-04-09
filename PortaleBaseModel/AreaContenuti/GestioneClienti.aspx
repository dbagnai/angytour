<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="GestioneClienti.aspx.cs" Inherits="AreaContenuti_GestioneClienti" %>

<%@ MasterType VirtualPath="~/AreaContenuti/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Anagrafica Clienti</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%=  InjectedStartPageScripts() %>
    <script type="text/javascript" src="<%= CommonPage.ReplaceAbsoluteLinks("~/lib/js/gestioneclienticontroller.js")+ CommonPage.AppendModTime(Server,"~/lib/js/gestioneclienticontroller.js")%>"></script>
    <script>
        $(document).ready(function () {
            ClientiManager.initVUE($("#idclienteHidden").val());
        });
    </script>
    <asp:Panel runat="server" ID="pnlClienti" Visible="true">
        <div id="vueContainer">
                        <h3>Filtri selezione</h3>
                       <asp:HiddenField ID="idclienteHidden" runat="server" Value="" ClientIDMode="Static" />
                    <div style="border:1px solid black;padding:10px;margin-bottom:15px;background-color:#ddd">
                <div class="row">
                 <div class="col-xs-12 col-sm-3" > 

                        <strong>Ricerca clienti per email o nome: </strong><br />
                        <div class="autocomplete">
                      <%-- <input type="text" id="inpCoordinatore" class="form-control autocompleteinput" v-model="vm.filterparams.Id_cliente"
                            @keypress="isNumber($event)" @focus="onChangeAutocomplete()"  />--%>
                     <%-- <input class="form-control autocompleteinput" placeholder="cerca .." type="text" v-model="im.filterkey" @input="onChangeAutocomplete()" /> --%>
                       <%--   <input class="form-control autocompleteinput" placeholder="cerca .." type="text"  @input="onChangeAutocomplete($event)" /> --%>
                        <input type="text" class="form-control autocompleteinput" v-model="vm.filterparams.Id_cliente"   @input="onChangeAutocomplete($event)"  />

                        <ul class="autocomplete-results" v-show="im.isOpen"  >
                       <%-- <li><input class="autocompleteinput" placeholder="cerca .." type="text" v-model="im.filterkey" @input="onChangeAutocomplete()" /></li>--%>
                        <li v-for="(value, key) in im.filteredautocomplete"  @click="setResultAutocomplete(key)" class="autocomplete-result">
                                    {{ value }}
                            </li>
                        </ul>
                        </div>
                </div>
                <div class="col-xs-12 col-sm-3" > Fitro nazione:<br />  <select v-model="vm.filterparams.CodiceNAZIONE">
                                <option v-for="(value,key) in im.nazionilist" :value="key">{{ value }}</option>
                        </select></div>
                <div class="col-xs-12 col-sm-3" >Filtro Tipologia:<br /><select v-model="vm.filterparams.id_tipi_clienti">
                                <option v-for="(value,key) in im.tipiclientilist" :value="key">{{ value }}</option>
                        </select></div>
                <div class="col-xs-12 col-sm-3" ><input type="button" class="btn btn-info" value="Filtra clienti"   v-on:click="filtraDati()"/> </div>

                </div>
                </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12">
                    <span style="font-size: 1.4rem; color: crimson">
                        <span>{{vm.message}}</span>
                    </span>
                </div>
                      <div class="col-xs-12 col-sm-6">
                       <h2> Lista Clienti</h2>
                    <table class="table table-fixed"   style="border:1px solid black;padding:10px;background-color:#eee">
                        <thead>
                            <tr>
                                <th class="col-xs-1">#</th>
                                <th class="col-xs-1">Id</th>
                                <th class="col-xs-3">Email</th>
                                <th class="col-xs-2">Cognome</th>
                                <th class="col-xs-2">Tipo</th>
                                <th class="col-xs-1">Lingua</th>
                                <th class="col-xs-1">Validato</th>
                                <th class="col-xs-1"></th>
                            </tr>
                        </thead>
                        <tbody style="height:650px">
                            <tr v-for="row in vm.list">
                                <td class="col-xs-1">
                                    <button type="button" v-bind:class="{ selected : isSelected(row.Id_cliente) }" v-on:click="caricaDettaglio(row.Id_cliente)"><i class="fa fa-search fa-2x"></i></button>
                                </td>
                                <td class="col-xs-1">
                                    <div style="height: 50px; overflow-y: auto">
                                        {{row.Id_cliente}} 
                                    </div>
                                </td>
                                <td class="col-xs-3">
                                    <div style="height: 50px; overflow-y: auto;">
                                        {{row.Email}}
                                    </div>
                                </td>
                                <td class="col-xs-2">
                                    <div style="height: 50px; overflow-y: auto;">
                                        {{row.Cognome}}
                                    </div>
                                </td>
                                <td class="col-xs-2">
                                    <div style="height: 50px; overflow-y: auto">
                                        <span v-html="tipotext(row.id_tipi_clienti)"></span>
                                    </div>
                                </td>
                                <td class="col-xs-1">
                                    <div style="height: 50px; overflow-y: auto">
                                        <span v-html="linguatext(row.Lingua)"></span>
                                    </div>
                                </td>
                                <td class="col-xs-1">
                                    <div style="height: 50px; overflow-y: auto">
                                        <input v-model:checked="row.Validato" type="checkbox" disabled />
                                    </div>
                                </td>
                                <td class="col-xs-1">
                                    <div style="height: 50px; overflow: hidden">
                                        <button type="button" class="btn btn-primary btn-sm" v-on:click="eliminaDettaglio(row.Id_cliente)">Del</button>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div id="pager" style="display: block; width: 100%">
                        <div id="listdivPager" style="text-align: center; margin-top: 10px; margin-bottom: 10px; display: block">
                            <div class="paginationcontainer text-center" style="width: 100%; display: block">
                                <div style="display: inline-flex; text-align: center; margin-top: 5px">
                                    <div id="portlist1divactpage">{{vm.Pager.CurrentPage}}/{{vm.Pager.TotalPages}}</div>
                                    <div>&nbsp;-&nbsp;</div>
                                    <div id="portlist1spantotals">
                                        Elementi trovati {{vm.Pager.TotalRecords}}<br/>
                                    </div>
                                </div>
                                <ul class="pagination pagination-sm d-flex flex-wrap justify-content-center py-1" style="display: block" id="portlist1pagenumbers">
                                    <li v-for="row in vm.Pager.Pages">
                                        <button type="button" class="page-link" v-on:click="caricapaginadati(row)">{{row}}</button>
                                    </li>
                                    <%--   <li class="page-item active pt-1"><span class="page-link" style="cursor:default">1</span></li>
                                    <li class="page-item  pt-1"><a class="page-link" href="/?page=2">2</a></li>
                                    <li class="page-item  pt-1"><a class="page-link" href="/?page=3">3</a></li>
                                    <li class="page-item  pt-1"><a class="page-link" href="/?page=4">4</a></li>
                                    <li class="page-item  pt-1"><a class="page-link" href="/?page=2">&gt;</a></li>
                                    <li class="page-item  pt-1"><a class="page-link" href="/?page=4">&gt;&gt;</a></li> --%>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6">
                       <h2> Dettaglio Cliente</h2>

                    <div style="background: #ccc; padding: 10px 30px 30px 30px;">
                        
                        <div class="row" style="padding-bottom: 10px">
                             <input type="button" class="btn btn-primary btn-sm"  v-on:click="svuotaDettaglio()" value="Nuovo" />
                             <input type="button" class="btn btn-primary btn-sm"  v-on:click="inserisciAggiornaDettaglio()" value="Aggiorna/Inserisci" />
                        </div>
                        <div class="row" style="padding-bottom: 10px">
                            <div class="col-sm-3 item-text text-left">
                                <strong>Id Cliente</strong><br />
                                <span v-html="vm.itemselected.Id_cliente"></span>
                            </div>
                              <div class="col-sm-2 item-text  text-left">
                                <strong>Privacy</strong><br /> 
                                <input type="checkbox" v-model:checked="vm.itemselected.ConsensoPrivacy"    />
                            </div> 
                            <div class="col-sm-2 item-text  text-left">
                                 <strong>Mailing</strong><br />
                                <input type="checkbox" v-model:checked="vm.itemselected.Consenso1"    />
                            </div>
                            <div class="col-sm-2 item-text  text-left">
                                <strong>Validato</strong><br /> 
                                <input type="checkbox" v-model:checked="vm.itemselected.Validato"    />
                            </div>
                             <div class="col-sm-3 item-text  text-left">
                                 <strong>Tipologia</strong><br /> 
                                <select v-model="vm.itemselected.id_tipi_clienti">
                                     <option v-for="(value,key) in im.tipiclientilist" :value="key">{{ value }}</option>
                                </select>
                            </div>
                        </div>
                         <div class="row" style="padding-bottom: 10px">
                            <div class="col-sm-3 item-text text-left">
                                <strong>Telefono</strong><br />
                                <input type="text" class="form-control" v-model="vm.itemselected.Telefono"   />
                            </div>
                            <div class="col-sm-3 item-text  text-left">
                                  <strong>Cellulare</strong><br />
                                <input type="text" class="form-control" v-model="vm.itemselected.Cellulare"   />
                            </div>
                             <div class="col-sm-3 item-text  text-left">
                               <strong>Email</strong><br /> 
                                <input type="text" class="form-control" v-model="vm.itemselected.Email"   />
                            </div>
                            <div class="col-sm-3 item-text  text-left">
                                <strong>Lingua</strong>
                                <br />
                                <select  class="form-control"  v-model="vm.itemselected.Lingua">
                                         <option v-for="(value,key) in im.languageslist" :value="key">{{ value }}</option>
                                </select>
                            </div>
                        </div>
                         <div class="row" style="background: #ddd;border: 1px solid black;padding:10px">
                                <div class="col-sm-12 item-text text-left">
                                   <h4><strong>Indirizzo principale ( fatturazione )</strong></h4>
                                </div>
                                 <div class="row" style="padding-bottom: 10px">
                            <div class="col-sm-3 item-text text-left">
                               <strong>Cognome</strong><br />
                                <input type="text" class="form-control" v-model="vm.itemselected.Cognome"   />
                            </div>
                             <div class="col-sm-3 item-text  text-left">
                                   <strong>Nome</strong><br /> 
                                <input type="text" class="form-control" v-model="vm.itemselected.Nome"   />
                            </div>
                            <div class="col-sm-3 item-text  text-left">
                                   <strong>Azienda</strong><br /> 
                                <input type="text" class="form-control" v-model="vm.itemselected.Ragsoc"   />
                            </div>
                            <div class="col-sm-3 item-text  text-left">
                                
                            </div>
                        </div>
                                 <div class="row" style="padding-bottom: 10px">
                                    <div class="col-sm-3 item-text text-left">
                                        <strong>Cap</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.Cap"    />
                                    </div>
                                     <div class="col-sm-3 item-text  text-left">
                                           <strong>Indirizzo/Via</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.Indirizzo"    />
                                    </div>
                                    <div class="col-sm-3 item-text  text-left">
                                          <strong>P.Iva/CF</strong><br /> 
                                        <input type="text" class="form-control" v-model="vm.itemselected.Pivacf"   />
                                    </div>
                                    <div class="col-sm-3 item-text  text-left">
                                        <strong>SDI/PEC</strong><br /> 
                                        <input type="text" class="form-control" v-model="vm.itemselected.Emailpec"   />
                                    </div>
                                </div>
                                 <div class="row" style="padding-bottom: 10px">
                                      <div class="col-sm-3 item-text  text-left">
                                              <strong>Nazione: </strong>
                                              <br />
                                            <select class="form-control" v-model="vm.itemselected.CodiceNAZIONE"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListRegione', 'caricaddlregione','vm.itemselected.CodiceREGIONE');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListProvincia', 'caricaddlprovincia','vm.itemselected.CodicePROVINCIA');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListComune', 'caricaddlcomune','vm.itemselected.CodiceCOMUNE')">
                                                <option v-for="(value,key) in vm.geolist1.ListNazione" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.CodiceNAZIONE"  style="display:none"  />
                                    </div>
                                      <div class="col-sm-3 item-text  text-left">
                                           <strong>Regione: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.CodiceREGIONE"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListProvincia', 'caricaddlprovincia','vm.itemselected.CodicePROVINCIA');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListComune', 'caricaddlcomune','vm.itemselected.CodiceCOMUNE')" >
                                                <option v-for="(value,key) in vm.geolist1.ListRegione" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.CodiceREGIONE"    />

                                    </div>
                                       <div class="col-sm-3 item-text  text-left">
                                            <strong>Provincia: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.CodicePROVINCIA"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist1.ListComune', 'caricaddlcomune','vm.itemselected.CodiceCOMUNE')">
                                                <option v-for="(value,key) in vm.geolist1.ListProvincia" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.CodicePROVINCIA"    />
                                    </div>
                                       <div class="col-sm-3 item-text  text-left">
                                            <strong>Comune: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.CodiceCOMUNE"  >
                                                <option v-for="(value,key) in vm.geolist1.ListComune" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.CodiceCOMUNE"    />
                                    </div>
                                </div>
                        </div>
                         <div class="row" style="background: #e0e0e0;border: 1px solid black;padding:10px">
                                <div class="col-sm-12 item-text text-left">
                                   <h4><strong>Indirizzo spedizione ( opzionale )</strong></h4>
                                </div>
                               
                             
                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-sm-3 item-text text-left">
                                        <strong>Cognome</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.addvalues.Cognome"    />
                                    </div>
                                     <div class="col-sm-3 item-text  text-left">
                                           <strong>Nome</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.addvalues.Nome"    />
                                    </div>
                                    <div class="col-sm-3 item-text  text-left">
                                    </div>
                                    <div class="col-sm-3 item-text  text-left">
                                    </div>
                                </div>


                                <div class="row" style="padding-bottom: 10px">
                                    <div class="col-sm-3 item-text text-left">
                                        <strong>Cap</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.addvalues.Cap"    />
                                    </div>
                                     <div class="col-sm-3 item-text  text-left">
                                           <strong>Indirizzo/Via</strong><br />
                                        <input class="form-control" type="text" v-model="vm.itemselected.addvalues.Indirizzo"    />
                                    </div>
                                    <div class="col-sm-3 item-text  text-left">
                                          <strong>Telefono</strong><br /> 
                                        <input type="text" class="form-control" v-model="vm.itemselected.addvalues.Telefono"   />
                                    </div>
                                    <div class="col-sm-3 item-text  text-left">
                                    </div>
                                </div>
                               <div class="row" style="padding-bottom: 10px">
                                      <div class="col-sm-3 item-text  text-left">
                                              <strong>Nazione: </strong>
                                              <br />
                                            <select class="form-control" v-model="vm.itemselected.addvalues.CodiceNAZIONE"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListRegione', 'caricaddlregione','vm.itemselected.addvalues.CodiceREGIONE');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListProvincia', 'caricaddlprovincia','vm.itemselected.addvalues.CodicePROVINCIA');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListComune', 'caricaddlcomune','vm.itemselected.addvalues.CodiceCOMUNE')">
                                                <option v-for="(value,key) in vm.geolist2.ListNazione" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.addvalues.CodiceNAZIONE"  style="display:none"  />
                                    </div>
                                      <div class="col-sm-3 item-text  text-left">
                                            <strong>Regione: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.addvalues.CodiceREGIONE"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListProvincia', 'caricaddlprovincia','vm.itemselected.addvalues.CodicePROVINCIA');(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListComune', 'caricaddlcomune','vm.itemselected.addvalues.CodiceCOMUNE')" >
                                                <option v-for="(value,key) in vm.geolist2.ListRegione" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.addvalues.CodiceREGIONE"    />

                                    </div>
                                       <div class="col-sm-3 item-text  text-left">
                                            <strong>Provincia: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.addvalues.CodicePROVINCIA"  onchange="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).ddl_Change(event, 'vm.geolist2.ListComune', 'caricaddlcomune','vm.itemselected.addvalues.CodiceCOMUNE')">
                                                <option v-for="(value,key) in vm.geolist2.ListProvincia" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.addvalues.CodicePROVINCIA"    />
                                    </div>
                                       <div class="col-sm-3 item-text  text-left">
                                            <strong>Comune: </strong>
                                              <br />
                                            <select class="form-control"  v-model="vm.itemselected.addvalues.CodiceCOMUNE"  >
                                                <option v-for="(value,key) in vm.geolist2.ListComune" :value="key">{{ value }}</option>
                                            </select>
                                             <input type="text" class="form-control" v-model="vm.itemselected.addvalues.CodiceCOMUNE"    />
                                    </div>
                                </div>
                        </div>
                        <div class="row" style="padding-bottom: 10px">
                             <div class="col-sm-3 item-text  text-left">
                                  <strong>Documento</strong><br /> 
                                <input type="text" class="form-control" v-model="vm.itemselected.Professione"   />
                            </div>
                             <div class="col-sm-3 item-text  text-left">
                                   <strong>Sesso </strong><br />
                                 <select class="form-control" v-model="vm.itemselected.Sesso">
                                <option v-for="(value,key) in im.generelist" :value="key">{{ value }}</option>
                                   </select>
                            </div>
                            <div class="col-sm-3 item-text  text-left">
                                  <strong>Data Nascita</strong><br /> 
                                  <input type="text" class="form-control"
                                    v-bind:value="vm.itemselected.DataNascita | formatshortDate"
                                    v-on:blur="vm.itemselected.DataNascita = formatDateforvue($event.target.value)"
                                    />
                            </div>
                            <div class="col-sm-3 item-text text-left">
                                <strong>Website</strong><br />
                                <input class="form-control" type="text" v-model="vm.itemselected.Spare1"    />
                            </div>
                        </div>
                        <div class="row" style="padding-bottom: 10px">
                             <div class="col-sm-3 item-text  text-left">
                                   <strong>Data Inserimento</strong><br /> 
                                   <input type="text" class="form-control"
                                     v-bind:value="vm.itemselected.DataInserimento | formatshortDate"
                                     v-on:blur="vm.itemselected.DataInserimento = formatDateforvue($event.target.value)"
                                     />
                             </div>
                            <div class="col-sm-3 item-text  text-left">
                                 <strong>Codici Sconto</strong><br />
                                <input class="form-control" type="text" v-model="vm.itemselected.Codicisconto"    />
                            </div>
                        </div>
                            <div class="row" style="background: #ccc;border: 1px solid black;padding:10px">
                            <div class="col-sm-12 item-text text-left">
                                <h4><strong>Accesso utente</strong></h4>
                            </div>
                          <div class="row" style="padding-bottom: 10px">
                            <div class="col-sm-12 item-text  text-left">
                               <b>User:</b>   <span v-html="vm.utente.Campo1"></span><br />
                               <b>Pass:</b>   <input class="form-control" type="text" v-model="vm.utente.Campo2"    /> <br />
                                        <%--<input type="button" class="btn btn-primary btn-sm" style="margin:5px;width:150px"   value="Cambio Password" onclick="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).CallAPIFunc('cambiopassword', JSON.stringify(vuecontroller.vm.utente), 'vm.utente.Campo2',function(data){ vuecontroller.vm.message  },function(data){ vuecontroller.vm.message  },'text')" />--%>
                                 <input type="button" class="btn btn-primary btn-sm" style="margin:5px;width:150px"   value="Cambio Password" onclick="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).CallAPIFunc('cambiopassword', JSON.stringify(vuecontroller.vm.utente), 'vm.utente',function(data){   },function(data){  })" />
                                   <input type="button" class="btn btn-primary btn-sm" style="margin:5px;width:150px"   value="Genera Utente"  onclick="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).CallAPIFunc('generautente', JSON.stringify(vuecontroller.vm.utente), 'vm.utente',function(data){   },function(data){  })" />
    	                          <input type="button" class="btn btn-primary btn-sm" style="margin:5px;width:150px"    value="Elimina Utente"  onclick="(new comServ('vuecontroller', '/lib/hnd/HandlerGestioneclienti.ashx')).CallAPIFunc('eliminautente', JSON.stringify(vuecontroller.vm.utente), 'vm.utente',function(data){   },function(data){  })" />
                        <%--     <input type="button" class="btn btn-primary btn-sm"  v-on:click="inserisciAggiornaDettaglio()" value="Aggiorna/Inserisci" />--%>
                                 <span style="font-size: 1.4rem; color: crimson">
                                   <br /> <span>{{vm.utente.Campo3}}</span>
                                </span>
                            </div>
                        </div>
                        </div>
                        </div>
                    </div>
          
               </div>
            </div>
    </asp:Panel>

   
    <asp:Panel runat="server" ID="pnlImport">
    <div style="background-color: #eee; margin-top:40px; position: relative; border: 1px solid black; padding: 10px;">
          <h2>Sezione Importazione liste clienti</h2>
        <hr />
         <div class="row">
                <div class="col-sm-12">
                        <div style="background-color: #cacaca;  border: 1px solid black; padding: 10px;margin:10px">
                        <h3>Gestione tipologie di clienti</h3>
                        <asp:DropDownList runat="server" Width="310px" ID="ddlTipiClientiUpdate" AutoPostBack="true"
                            AppendDataBoundItems="true" OnSelectedIndexChanged="TipoClienteUpdate">
                        </asp:DropDownList>
                        <asp:TextBox runat="server" ID="txtTipoClienteUpdate" Text="" Width="200" placeholter="inserire nuovo tipo cliente" />
           
                        <asp:Button Text="Aggiorna" ID="btnTipiClienti" runat="server" OnClick="btnTipiClienti_Click" />
                        <br />
                        <br />
                        </div>
                </div>
                <div class="col-sm-12">
                        <div class="pull-left">
                            <asp:Label Text="<h1>Importazione liste clienti TIPO 1</h1><br/>(FORMATO ELENCO : nome1,email1,nome2,email2 .... usando come separatore la , o il ;)"
                                runat="server" />
                            <br />
                            <asp:TextBox runat="server" TextMode="MultiLine" Height="200" Width="600" ID="txtImporta" />
                        </div>
                        <div class="pull-left">
                            <asp:Label Text="<h1>Importazione liste clienti TIPO 2</h1><br/>(FORMATO ELENCO : nome1,cognome1,email1,nome2,cognome2,email2 .... usando come separatore la , o il ;)"
                                runat="server" />
                            <br />
                            <asp:TextBox runat="server" TextMode="MultiLine" Height="200" Width="600" ID="txtImporta1" /> 
                        </div>
                        <div class="clearfix"></div>
                         <asp:Button Text="Importa da testo" runat="server" OnClick="btnImporta_onclick" ID="btnImporta" />
                        <br />
                        <br />
                      <div style="background-color: #dedede;  border: 1px solid black; padding: 10px;margin:10px">
                            Importazione clienti da file Excel:<br />
                            <asp:FileUpload runat="server" ID="uplFile" />
                            <br />
                            <asp:Button Text="2. Carica file importazione" runat="server" ID="Button1" OnClick="btnUploadFile_Click" CausesValidation="false" />
                            <br />
                            <br />
                            <asp:Button Text="3. Importa Clienti da file" runat="server" ID="btnParse" OnClick="btnParse_Click" CausesValidation="false" />
                            <br />
                            <br />
                        </div>
                         <div style="background-color: #eee;  border: 1px solid black; padding: 10px;margin:10px">
                                <div style="width: 200px">
                                   <b> <asp:Label ID="Label1" Text="Selezionare Lingua Clienti per importazione" runat="server" /></b>
                                </div>
                                <asp:DropDownList runat="server" ID="ddlLinguaImporta">
                                    <asp:ListItem Text="Italiano" Value="I" Selected="True" />
                                    <asp:ListItem Text="Inglese" Value="GB" />
                                    <asp:ListItem Text="Russo" Value="RU" />
                                    <asp:ListItem Text="Francese" Value="FR" />
                                    <asp:ListItem Text="Tedesco" Value="DE" />
                                    <asp:ListItem Text="Spagnolo" Value="ES" />
                                </asp:DropDownList>
                                    <br />
                                 <br />
                                <div>
                                    <asp:CheckBox ID="chkCommercialeImporta" Text="Consenso Commerciale" runat="server"
                                        Checked="true" /><br />
                                    <asp:CheckBox ID="chkPrivacyImporta" Text="Consenso Privacy" runat="server" Checked="true" />
                                </div>
                                <br />
                                <div style="width: 200px">
                                    <b><asp:Label ID="Label2" Text="Selezionare Tipo Clienti per importazione" runat="server" /></b>
                                </div>
                                <asp:DropDownList runat="server" ID="ddlTipiClientiImporta" AppendDataBoundItems="true" OnInit="ddlTipiClientiImporta_OnInit" />
                                <br />
                                <br />
                                <input type="button" Value="Cancella Tutti Clienti della Tipologia" onclick="javascript:ConfermaCancellaClienti(this)" id="cancellaClientiTipologia" />
                        </div>
                       
                        <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Esporta Archivio Clienti" />
                        <br />
                        <br />
                        <span style="font-size:1.6rem;color:red">
                          <asp:Literal Text="" runat="server" ID="outputimporta" />
                        </span>
                         <br />
                        
                        <script type="text/javascript">
                            function eseguieliminazioneclienti() {
                                __doPostBack('cancellaclientidatipologia', '');
                            }
                            function ConfermaCancellaClienti() {
                                if (confirm('Vuoi cancellare tutti i clienti per la tipologia selezionata?')) {
                                    eseguieliminazioneclienti()
                                }
                            }

                        </script>
                 </div>
            </div>
    </div>
    </asp:Panel>
</asp:Content>
