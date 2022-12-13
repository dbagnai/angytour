"use strict";


var vuecontroller = null;
var ScontiManager = new function () {

    this.initVUE = function (idselected) {
        scontivuemodel.idselected = idselected || 0;
        //scontivuemodel.filterparams.Id = idselected || 0;
        Init();
    };


    function Init() {
        vuecontroller = new Vue({
            el: '#vueContainer',
            data: {
                vm: scontivuemodel,
                im: initpagemodelsconti
            },
            created: function () {
                this.fetchMessages();
            },
            filters: {
                capitalize: function (value) {
                    if (!value) return ''
                    value = value.toString()
                    return value.charAt(0).toUpperCase() + value.slice(1)
                },
                formatshortDate: function (value) {
                    if (value) {
                        var formatteddate = moment(String(value), ['DD/MM/YYYY', 'YYYY-MM-DD']).format('DD/MM/YYYY');
                        return formatteddate;
                    }
                },
                formatDate: function (value) {
                    if (value) {
                        //var formatteddate = moment(String(value), ['DD/MM/YYYY HH:mm', 'YYYY-MM-DDTHH:mm:ss']).format('DD/MM/YYYY HH:mm');
                        var formatteddate = moment(String(value), ['DD/MM/YYYY HH:mm', 'YYYY-MM-DDTHH:mm:ss']).format('DD/MM/YYYY');
                        return formatteddate;
                    }
                },
                formatNumber: function (value) {
                    if (value) {
                        var formattednumber = (String(value)).replace(/\,/g, "").replace(/\./g, ",")
                        return formattednumber;
                    }
                }
                //,format_number:
                //{
                //     // Model => View
                //      read(value) {
                //         return (String(value)).replace(/\,/g, "").replace(/\./g, ",")
                //      },
                //      // View => Model
                //      write(value, oldVal) {
                //          const number = +(String(value)).replace(/\./g, "").replace(/\,/g, ".");
                //        return isNaN(number) ? 0 : parseFloat(number)
                //      }
                //}
            },
            computed: {
            },
            watch: {
            },
            mounted() {
                document.addEventListener('click', this.handleClickOutsideAutocomplete)
                this.datePickerInit("#datascadenza", "vuecontroller.vm.itemselected", "datascadenza");
            },
            destroyed() {
                document.removeEventListener('click', this.handleClickOutsideAutocomplete)
            },
            methods: {
                //setcorrectdate: function (value) {
                //    if (value) {
                //        var formattednumber = (String(value)).replace(/\./g, "").replace(/\,/g, ".");
                //        return formattednumber;
                //    }
                //},
                formatNumberforvue: function (value) {
                    if (value) {
                        var formattednumber = (String(value)).replace(/\./g, "").replace(/\,/g, ".");
                        return formattednumber;
                    }
                },
                formatDateforvue: function (value) {
                    if (value) {
                        var formatteddate = moment(String(value), ['DD/MM/YYYY HH:mm', 'YYYY-MM-DDTHH:mm:ss']).format('YYYY-MM-DDTHH:mm:ss');
                        return formatteddate;
                    }
                },
                fetchMessages: function () {
                    (new comServ("vuecontroller", '/lib/hnd/HandlerSconti.ashx')).CallAPIFunc('initpage', JSON.stringify(this.im), 'im',
                        function (data) {
                            caricaDati();
                        },
                        function (data) { this.vm.message = data.responseText; }
                    );

                },
                updateinitmodel: function (value) {

                    (new comServ("vuecontroller", '/lib/hnd/HandlerSconti.ashx')).CallAPIFunc('initpage', JSON.stringify(this.im), 'im',
                        function (data) {
                            setcodicefiltro();
                        },
                        function (data) { this.vm.message = data.responseText; }
                    );

                    //console.log(this.im.selectedcategoria);
                },
                isNumber: function (evt) {
                    evt = (evt) ? evt : window.event;
                    var charCode = (evt.which) ? evt.which : evt.keyCode;
                    //if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 46 && charCode !== 44 ) {
                    if ((charCode > 31 && (charCode < 48 || charCode > 57)) && charCode !== 44) { //solo virgola ... ma va'trasformato in . decimale per la notazione vue
                        evt.preventDefault();;
                    } else {
                        return true;
                    }
                },
                isSelected: function (index) {
                    return index == this.vm.idselected;
                },
                handleClickOutsideAutocomplete(evt) {
                    if (!$(evt.target).hasClass("autocompleteinput")) {
                        //if (!this.$el.contains(evt.target)) {
                        //   this.im.selectedkey = ""; //non usato
                        this.im.filterkey = "";
                        this.im.isOpen = false;

                        this.im.filterkey1 = "";
                        this.im.isOpen1 = false;

                    }
                },
                onChangeAutocomplete() {
                    this.im.isOpen = true;
                    //Svuoto i risutalti presenti
                    this.im.filteredautocomplete = {};
                    //this.im.filterkey = "";
                    //   this.im.selectedkey = ""; //non usato
                    //qui devi chiamare il server ad ogni chiave per fare il filtro sulla lista che ti interessa e tornarla
                    //da modificare non agendo su lista ma con chiamata all'handler con parametri di filtro ....
                    //inim1.filterkey1  ritorna inim1.filteredautocomplete
                    if (this.im.filterkey != undefined && this.im.filterkey.length > 1)
                        (new comServ("vuecontroller", '/lib/hnd/HandlerSconti.ashx')).CallAPIFunc('filterclienti', JSON.stringify(this.im), 'im',
                            function (data) {
                                // tmpautocomplete.results = vuecontroller.im.filteredautocomplete;
                            },
                            function (data) { vuecontroller.vm.message = data.responseText; }
                        );
                }, onChangeAutocomplete1() {
                    this.im.isOpen1 = true;
                    //Svuoto i risutalti presenti
                    this.im.filteredautocomplete1 = {};
                    if (this.im.filterkey1 != undefined && this.im.filterkey1.length > 1)
                        (new comServ("vuecontroller", '/lib/hnd/HandlerSconti.ashx')).CallAPIFunc('filterprodotti', JSON.stringify(this.im), 'im',
                            function (data) {
                                // tmpautocomplete.results = vuecontroller.im.filteredautocomplete;
                            },
                            function (data) { vuecontroller.vm.message = data.responseText; }
                        );
                },
                setResultAutocomplete(keyselect) {
                    //this.im.selectedkey = keyselect; //non usato
                    this.vm.itemselected.Idcliente = keyselect;
                    this.im.isOpen = false;
                },
                setResultAutocomplete1(keyselect) {
                    //this.im.selectedkey = keyselect; //non usato
                    this.vm.itemselected.Idprodotto = keyselect;
                    this.im.isOpen1 = false;
                },
                //linguatext(value) {
                //    if (this.im.languageslist !== undefined && this.im.languageslist.hasOwnProperty(value))
                //        return this.im.languageslist[value]
                //    else
                //        return value;
                //},
                caricapaginadati(pagina) {
                    this.vm.Pager.CurrentPage = pagina;
                    caricaDati();
                },
                caricaDettaglio(id) {
                    this.vm.idselected = id;
                    caricaDati();
                },
                svuotaDettaglio() {
                    this.vm.idselected = 0;
                    caricaDati();
                },
                filtraDati() {
                    this.vm.idselected = 0;


                    var d = new Date();
                    if (this.im.chkvediscaduti == 1)
                        this.vm.filterparams.Datascadenza = d.toUTCString();
                    else this.vm.filterparams.Datascadenza = null;
                    if (this.vm.filterparams.Id == '') this.vm.filterparams.Id = 0;
                    caricaDati();
                },
                eliminaDettaglio(id) {
                    this.vm.idselected = id;
                    var conferma = confirm('Sei sicuro di voler cancellare questa voce?');
                    this.vm.message = "";
                    if (conferma) {
                        this.vm.message = "confermato cancellazione elemento";
                        (new comServ("vuecontroller", '/lib/hnd/HandlerSconti.ashx')).CallAPIFunc('deletedata', JSON.stringify(vuecontroller.vm), 'vm',
                            function (data) { }, function (data) { vuecontroller.vm.message = data.responseText; }
                        );
                    }
                },
                inserisciAggiornaDettaglio() {
                    (new comServ("vuecontroller", '/lib/hnd/HandlerSconti.ashx')).CallAPIFunc('inserisciaggiorna', JSON.stringify(vuecontroller.vm), 'vm',
                        function (data) { }, function (data) { vuecontroller.vm.message = data.responseText; }
                    );
                },
                datePickerInit(txtBox, controllername, model) {
                    var d = $(txtBox);
                    var self = d;
                    d.datepicker({
                        //defaultDate: "+1w",
                        changeMonth: true,
                        numberOfMonths: 1,
                        //numberOfMonths: 2,
                        dateFormat: 'dd/mm/yy',
                        prevText: '<i class="fa fa-chevron-left"></i>',
                        nextText: '<i class="fa fa-chevron-right"></i>',
                        /*
                        onClose: function (selectedDate) {
                            //self.datepicker("option", "maxDate", selectedDate);
                        }
                        */
                        onSelect: function (selectedDate, datePicker) {
                            self.date = selectedDate;
                            try { Vue.set(eval(controllername), model, vuecontroller.formatDateforvue(selectedDate)); } catch (e) { console.trace(e.message); }
                        }
                    });
                }
            }
        });

    };

    function caricaDati() {
        (new comServ("vuecontroller", '/lib/hnd/HandlerSconti.ashx')).CallAPIFunc('loaddata', JSON.stringify(vuecontroller.vm), 'vm',
            function (data) {
                ressetcodicefiltro();
            }, function (data) { vuecontroller.vm.message = data.responseText; }
        );
    }

    function setcodicefiltro() {
        if (vuecontroller.im.selectedcategoria != '') vuecontroller.vm.itemselected.Codicifiltro = vuecontroller.im.selectedcategoria;
        else vuecontroller.vm.itemselected.Codicifiltro = "";
        if (vuecontroller.im.selectedsottocategoria != '') vuecontroller.vm.itemselected.Codicifiltro = vuecontroller.im.selectedsottocategoria;

        if (vuecontroller.im.selectedcaratteristica1 != '') vuecontroller.vm.itemselected.caratteristica1filtro = vuecontroller.im.selectedcaratteristica1;
    }
    function ressetcodicefiltro() {
        vuecontroller.im.selectedcategoria = "";
        vuecontroller.im.selectedsottocategoria = "";
        vuecontroller.im.selectedcaratteristica1 = "";
    }
    // Date Range Picker
    // function datePickerInit(txtBox, controllername, model) {
    //     var d = $(txtBox);
    //     var self = d;
    //     d.datepicker({
    //         //defaultDate: "+1w",
    //         changeMonth: true,
    //         numberOfMonths: 1,
    //         //numberOfMonths: 2,
    //         dateFormat: 'dd/mm/yy',
    //         prevText: '<i class="fa fa-chevron-left"></i>',
    //         nextText: '<i class="fa fa-chevron-right"></i>',
    ///*
    //onClose: function (selectedDate) {
    //	//self.datepicker("option", "maxDate", selectedDate);
    //}
    //*/
    //         onSelect: function (selectedDate, datePicker) {
    //             self.date = selectedDate; //self.val(selectedDate);
    //             try { Vue.set(eval(controllername), model, formatDateforvue(selectedDate)); } catch (e) { console.trace(e.message); }
    //         }
    //     });
    // }

}