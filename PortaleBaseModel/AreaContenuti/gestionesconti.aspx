<%@ Page Title="" Language="C#" MasterPageFile="~/AreaContenuti/MasterPage.master" AutoEventWireup="true" CodeFile="gestionesconti.aspx.cs" Inherits="AreaContenuti_gestionesconti" %>

<%@ MasterType VirtualPath="~/AreaContenuti/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Gestione Sconti Ecommerce</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%=  InjectedStartPageScripts() %>
    <script type="text/javascript" src="<%= CommonPage.ReplaceAbsoluteLinks("~/lib/js/sconticontroller.js")+ CommonPage.AppendModTime(Server,"~/lib/js/sconticontroller.js")%>"></script>
    <script>
        $(document).ready(function () {
            ScontiManager.initVUE($("#hididselected").val());
        });
    </script>
    <%= WelcomeLibrary.DAL.dbDataAccess.CorrectDatenow(System.DateTime.Now).ToString() %>
    <div id="vueContainer">
        <asp:HiddenField runat="server" ID="hididselected" Value="" ClientIDMode="Static" />
        <div class="row" style="background-color:#ccc;padding:10px">
              <div class="col-xs-12" >  <h3>Filtri Sconti</h3>  </div>
             <div class="col-xs-12 item-text  text-left" style="padding-bottom:10px">
                    <strong>Vedi scaduti</strong> <input type="checkbox" v-model:checked="im.chkvediscaduti"    />
               </div>
              <div class="col-xs-12 col-sm-3" > <input class="form-control" placeholder="filtro codicesconto .." type="text" v-model="vm.filterparams.Testocodicesconto"   />  </div>
              <div class="col-xs-12 col-sm-3" > <input class="form-control" placeholder="filtro prodotti .." type="text" v-model="vm.filterparams.Idprodotto"   />  </div>
              <div class="col-xs-12 col-sm-3" > <input class="form-control" placeholder="filtro clienti .." type="text" v-model="vm.filterparams.Idcliente"   />  </div>
              <div class="col-xs-12 col-sm-3" > <input class="form-control" placeholder="filtro codici categorie .." type="text" v-model="vm.filterparams.Codicifiltro"   />  </div>
              <div class="col-xs-12 col-sm-3" ><input type="button" class="btn btn-info" value="Filtra Sconti"   v-on:click="filtraDati()"/> </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6">
                <h2>Lista Sconti</h2>

                <table class="table table-fixed" style="border: 1px solid black; padding: 10px; background-color: #eee">
                    <thead>
                        <tr>
                            <th class="col-xs-1">#</th>
                            <th class="col-xs-1">Id</th>
                            <th class="col-xs-3">Codice Sconto</th>
                            <th class="col-xs-2">Importo Sconto(€)</th>
                            <th class="col-xs-2">Perc. Sconto(%)</th>
                            <th class="col-xs-1">Singolo Uso</th>
                            <th class="col-xs-1">Data Scadenza</th>
                            <th class="col-xs-1"></th>
                        </tr>
                    </thead>
                    <tbody style="height: 650px">
                        <tr v-for="row in vm.list">
                            <td class="col-xs-1">
                                <button type="button" v-bind:class="{ selected : isSelected(row.Id) }" v-on:click="caricaDettaglio(row.Id)"><i class="fa fa-search fa-2x"></i></button>
                            </td>
                            <td class="col-xs-1">
                                <div style="height: 50px; overflow-y: auto">
                                    {{row.Id}} 
                                </div>
                            </td>
                            <td class="col-xs-3">
                                <div style="height: 50px; overflow-y: auto;">
                                    {{row.Testocodicesconto}}
                                </div>
                            </td>
                            <td class="col-xs-2">
                                <div style="height: 50px; overflow-y: auto;">
                                    <span>{{ row.Scontonum | formatNumber }} €</span>
                                </div>
                            </td>
                            <td class="col-xs-2">
                                <div style="height: 50px; overflow-y: auto;">
                                    <span>{{ row.Scontoperc | formatNumber }} %</span>
                                </div>
                            </td>
                            <td class="col-xs-1">
                                <div style="height: 50px; overflow-y: auto">
                                    <input v-model:checked="row.Usosingolo" type="checkbox" disabled />
                                </div>
                            </td>
                            <td class="col-xs-1">
                                <div style="height: 50px; overflow-y: auto">
                                    <span>{{ row.Datascadenza | formatshortDate }}</span>
                                </div>
                            </td>
                            <td class="col-xs-1">
                                <div style="height: 50px; overflow: hidden">
                                    <button type="button" class="btn btn-primary btn-sm" v-on:click="eliminaDettaglio(row.Id)">Del</button>
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
                                    Elementi trovati {{vm.Pager.TotalRecords}}<br />
                                </div>
                            </div>
                            <ul class="pagination pagination-sm d-flex flex-wrap justify-content-center py-1" style="display: block" id="portlist1pagenumbers">
                                <li v-for="row in vm.Pager.Pages">
                                    <button type="button" class="page-link" v-on:click="caricapaginadati(row)">{{row}}</button>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-xs-12 col-sm-6">
                <div style="font-size: 1.8rem; color: crimson;background-color:#ccc">
                    <span>{{vm.message}}</span>
                </div>
                <h2>Dettaglio Codice Sconto</h2>
                <div style="background: #ccc; padding: 10px 30px 30px 30px;">
                    <div class="row" style="padding-bottom: 10px">
                        <input type="button" class="btn btn-primary btn-sm" v-on:click="svuotaDettaglio()" value="Nuovo" />
                        <input type="button" class="btn btn-primary btn-sm" v-on:click="inserisciAggiornaDettaglio()" value="Aggiorna/Inserisci" />
                    </div>
                    <div class="row" style="padding-bottom: 10px">
                        <div class="col-sm-3 item-text text-left">
                            <strong>Id sconto: </strong>
                            <span v-html="vm.itemselected.Id"></span>
                        </div>


                    </div>
                    <div class="row" style="padding-bottom: 10px">
                        <div class="col-sm-4 item-text text-left">
                            <strong>Codice Sconto<br />(*richiesto)</strong><br />
                            <input type="text" class="form-control" v-model="vm.itemselected.Testocodicesconto" />
                        </div>
                        <div class="col-sm-4 item-text">
                            <strong>Sconto Importo € <br />(valore esclusivo)</strong>
                            <br />
                            <input type="text" class="form-control"
                                @keypress="isNumber($event)"
                                v-bind:value="vm.itemselected.Scontonum | formatNumber"
                                v-on:blur="vm.itemselected.Scontonum = formatNumberforvue($event.target.value)" />
                        </div>
                        <div class="col-sm-4 item-text">
                            <strong>Sconto Percentuale %<br />(valore esclusivo)</strong>
                            <br />
                            <input type="text" class="form-control"
                                @keypress="isNumber($event)"
                                v-bind:value="vm.itemselected.Scontoperc | formatNumber"
                                v-on:blur="vm.itemselected.Scontoperc = formatNumberforvue($event.target.value)" />
                        </div>
                        
                    </div>
                    <div class="row" style="padding-bottom: 10px">

                          <div class="col-sm-4 item-text text-left">
                              <strong>Commerciale associato<br />(facoltativo)</strong>
                        <div class="autocomplete">
                        <input type="text" id="inpCliente" class="form-control autocompleteinput" v-model="vm.itemselected.Idcliente"
                            @keypress="isNumber($event)"   @focus="onChangeAutocomplete()"  />
                        <ul class="autocomplete-results" v-show="im.isOpen"  >
                        <li><input class="autocompleteinput" placeholder="cerca cliente .." type="text" v-model="im.filterkey" @input="onChangeAutocomplete()" /></li>
                        <li v-for="(value, key) in im.filteredautocomplete"
                            @click="setResultAutocomplete(key)"
                            class="autocomplete-result">
                                    {{ value }}
                            </li>
                        </ul>
                        </div>

                        </div>
                        
                           <div class="col-sm-4 item-text">
                            <strong>Id Prodotto Sconto<br />(facoltativo) </strong>
                            <br />
                          <%--  <input type="text" class="form-control"
                                @keypress="isNumber($event)"
                                v-bind:value="vm.itemselected.Idprodotto | formatNumber"
                                v-on:blur="vm.itemselected.Idprodotto = formatNumberforvue($event.target.value)" />--%>

                                <div class="autocomplete">
                        <input type="text" id="inpProdotto" class="form-control autocompleteinput" v-model="vm.itemselected.Idprodotto"
                            @keypress="isNumber($event)"   @focus="onChangeAutocomplete1()"  />
                        <ul class="autocomplete-results" v-show="im.isOpen1"  >
                        <li><input class="autocompleteinput" placeholder="cerca prodotto .." type="text" v-model="im.filterkey1" @input="onChangeAutocomplete1()" /></li>
                        <li v-for="(value, key) in im.filteredautocomplete1"
                            @click="setResultAutocomplete1(key)"
                            class="autocomplete-result">
                                    {{ value }}
                            </li>
                        </ul>
                        </div>


                        </div>

                          <div class="col-sm-4 item-text text-left">
                            <strong>Filtro Categorie prod.<br />(facoltativo)</strong><br />
                            <input type="text" class="form-control" v-model="vm.itemselected.Codicifiltro" />

                              <select class="width-100" id="ddlProdotto" v-model="im.selectedcategoria" @change="updateinitmodel($event.target.value)">
                                <option v-for="(value,key) in im.categoria" :value="key">{{ value }}</option>
                            </select>
                              
                              <select  class="width-100" id="ddlsProdotto" v-model="im.selectedsottocategoria"  @change="updateinitmodel($event.target.value)">
                                <option v-for="(value,key) in im.sottocategoria" :value="key">{{ value }}</option>
                            </select>

                        </div>


                      

                    </div>
                    <div class="row" style="padding-bottom: 10px">
                        <div class="col-sm-6 item-text  text-left">
                            <strong>Singolo Utilizzo</strong><br />
                            <input type="checkbox" v-model:checked="vm.itemselected.Usosingolo" />
                        </div>
                      <div class="col-sm-6 item-text  text-left">
                            <strong>Scadenza codice (facoltativo)</strong><br />
                            <input type="text" class="form-control" id="datascadenza"
                                v-bind:value="vm.itemselected.Datascadenza | formatshortDate"
                                v-on:blur="vm.itemselected.Datascadenza = formatDateforvue($event.target.value)" />
                        </div>
                    
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

