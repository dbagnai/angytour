"use strict";


var vuecontroller = null;
var ProdottiManager = new function () {

    //this.defaultEditModel = {
    //    id: 0,
    //    idattivita=0,
    //    stato: 1
    //};

    this.initVUE = function (idattivita) {
        scaglionivuemodel.idattivita = idattivita || 0;
        Init();
    };


    function Init() {
        vuecontroller = new Vue({
            el: '#vueContainer',
            data: {
                vm: scaglionivuemodel,
                im: initvm
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
                this.datePickerInit("#txtdatapartenza", "vuecontroller.vm.itemselected", "datapartenza");

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
                    (new comServ("vuecontroller", '/lib/hnd/HandlerGestioneprodotti.ashx')).CallAPIFunc('initpage', JSON.stringify(this.im), 'im',
                        function (data) {
                            caricaDati();
                        },
                        function (data) { vuecontroller.vm.message = data.responseText; }
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
                    return index == this.vm.idscaglione;
                },
                handleClickOutsideAutocomplete(evt) {
                    if (!$(evt.target).hasClass("autocompleteinput")) {
                        //if (!this.$el.contains(evt.target)) {
                        this.im.filterkey = "";
                        this.im.selectedkey = "";
                        this.im.isOpen = false;
                    }
                },
                onChangeAutocomplete() {
                    this.im.isOpen = true;
                    //Svuoto i risutalti presenti
                    this.im.filteredautocomplete = {}; 
                    //this.im.filterkey = "";
                    this.im.selectedkey = "";
                    //this.im.filterkey1 = this.im.filterkey1;
                    //qui devi chiamare il server ad ogni chiave per fare il filtro sulla lista che ti interessa e tornarla
                    //da modificare non agendo su lista ma con chiamata all'handler con parametri di filtro ....
                    //inim1.filterkey1  ritorna inim1.filteredautocomplete
                    if (this.im.filterkey != undefined && this.im.filterkey.length > 1)
                        (new comServ("vuecontroller", '/lib/hnd/HandlerGestioneprodotti.ashx')).CallAPIFunc('filtercoordinatori', JSON.stringify(this.im), 'im',
                            function (data) {
                                // tmpautocomplete.results = vuecontroller.im.filteredautocomplete;
                            },
                            function (data) { vuecontroller.vm.message = data.responseText; }
                        );
                },
                setResultAutocomplete(keyselect) {
                    //tmpautocomplete.selected = {};
                    //tmpautocomplete.selected.codice = keyselect;
                    //tmpautocomplete.selected.value = tmpautocomplete.results[keyselect];
                    //tmpautocomplete.search = tmpautocomplete.results[keyselect];
                    //vardestinazione = JSON.parse(JSON.stringify(tmpautocomplete.selected));
                    //this.im.filterkey = this.im.filteredautocomplete[keyselect];
                    this.im.selectedkey = keyselect;
                    this.vm.itemselected.idcoordinatore = keyselect;
                    //$('#' + vardestinazione).val(tmpautocomplete.selected.codice)
                    this.im.isOpen = false;
                },
                statustext(value) {
                    //this function will determine what is displayed in the input
                    if (this.im.statuslist !== undefined && this.im.statuslist.hasOwnProperty(value))
                        return this.im.statuslist[value]
                    else
                        return value;
                },
                caricaDettaglio(id) {
                    this.vm.idscaglione = id;
                    caricaDati();

                },
                svuotaDettaglio() {
                    this.vm.idscaglione = 0;
                    caricaDati();
                },
                eliminaDettaglio(id) {
                    this.vm.idscaglione = id;
                    var conferma = confirm('Sei sicuro di voler cancellare questa voce?');
                    this.vm.message = "";
                    if (conferma) {
                        this.vm.message = "confermato cancellazione elemento";
                        (new comServ("vuecontroller", '/lib/hnd/HandlerGestioneprodotti.ashx')).CallAPIFunc('deletedata', JSON.stringify(vuecontroller.vm), 'vm',
                            function (data) { }, function (data) { vuecontroller.vm.message = data.responseText; }
                        );
                    }
                },
                inserisciAggiornaDettaglio() {
                    (new comServ("vuecontroller", '/lib/hnd/HandlerGestioneprodotti.ashx')).CallAPIFunc('inserisciaggiorna', JSON.stringify(vuecontroller.vm), 'vm',
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
        (new comServ("vuecontroller", '/lib/hnd/HandlerGestioneprodotti.ashx')).CallAPIFunc('loaddata', JSON.stringify(vuecontroller.vm), 'vm',
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