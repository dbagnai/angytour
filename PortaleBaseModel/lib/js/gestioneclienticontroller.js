"use strict";


var vuecontroller = null;
var ClientiManager = new function () {

    //this.defaultEditModel = {
    //    id: 0,
    //    idattivita=0,
    //    stato: 1
    //};

    this.initVUE = function (idcliente) {
        clientivuemodel.idselected = idcliente || 0;
        clientivuemodel.filterparams.Id_cliente = idcliente || 0;
        Init();
    };


    function Init() {
        vuecontroller = new Vue({
            el: '#vueContainer',
            data: {
                vm: clientivuemodel,
                im: initpagemodelclienti
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
                //this.datePickerInit("#txtdatapartenza", "vuecontroller.vm.itemselected", "datapartenza");
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
                    (new comServ("vuecontroller", '/lib/hnd/HandlerGestioneclienti.ashx')).CallAPIFunc('initpage', JSON.stringify(this.im), 'im',
                        function (data) {
                            caricaDati();
                        },
                        function (data) { this.vm.message = data.responseText; }
                    );

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
                        this.im.filterkey = "";
                        this.im.selectedkey = ""; //non usato
                        this.im.isOpen = false;
                    }
                },
                onChangeAutocomplete(evt) {
                    this.im.isOpen = true;
                    //Svuoto i risutalti presenti
                    this.im.filteredautocomplete = {};
                    //this.im.filterkey = "";
                    this.im.selectedkey = ""; //non usato

                    if (evt != null && $(evt.target).length > 0)
                        this.im.filterkey = $(evt.target).val();

                    //qui devi chiamare il server ad ogni chiave per fare il filtro sulla lista che ti interessa e tornarla
                    //da modificare non agendo su lista ma con chiamata all'handler con parametri di filtro ....
                    //inim1.filterkey1  ritorna inim1.filteredautocomplete
                    if (this.im.filterkey != undefined && this.im.filterkey.length > 1)
                        (new comServ("vuecontroller", '/lib/hnd/HandlerGestioneclienti.ashx')).CallAPIFunc('filterclienti', JSON.stringify(this.im), 'im',
                            function (data) {
                                // tmpautocomplete.results = vuecontroller.im.filteredautocomplete;
                            },
                            function (data) { vuecontroller.vm.message = data.responseText; }
                        );
                },
                setResultAutocomplete(keyselect) {
                    this.im.selectedkey = keyselect; //non usato
                    this.vm.filterparams.Id_cliente = keyselect;
                    this.im.isOpen = false;
                },
                tipotext(value) {
                    //this function will determine what is displayed in the input
                    if (this.im.tipiclientilist !== undefined && this.im.tipiclientilist.hasOwnProperty(value))
                        return this.im.tipiclientilist[value]
                    else
                        return value;
                },
                linguatext(value) {
                    if (this.im.languageslist !== undefined && this.im.languageslist.hasOwnProperty(value))
                        return this.im.languageslist[value]
                    else
                        return value;
                },
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
                    if (this.vm.filterparams.Id_cliente == '') this.vm.filterparams.Id_cliente = 0;
                    caricaDati();
                },
                eliminaDettaglio(id) {
                    this.vm.idselected = id;
                    var conferma = confirm('Sei sicuro di voler cancellare questa voce?');
                    this.vm.message = "";
                    if (conferma) {
                        this.vm.message = "confermato cancellazione elemento";
                        (new comServ("vuecontroller", '/lib/hnd/HandlerGestioneclienti.ashx')).CallAPIFunc('deletedata', JSON.stringify(vuecontroller.vm), 'vm',
                            function (data) { }, function (data) { vuecontroller.vm.message = data.responseText; }
                        );
                    }
                },
                inserisciAggiornaDettaglio() {
                    (new comServ("vuecontroller", '/lib/hnd/HandlerGestioneclienti.ashx')).CallAPIFunc('inserisciaggiorna', JSON.stringify(vuecontroller.vm), 'vm',
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
        (new comServ("vuecontroller", '/lib/hnd/HandlerGestioneclienti.ashx')).CallAPIFunc('loaddata', JSON.stringify(vuecontroller.vm), 'vm',
            function (data) {

            }, function (data) { vuecontroller.vm.message = data.responseText; }
        );
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