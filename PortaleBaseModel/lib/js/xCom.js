"use strict";

//function ddlProdotto_Change(event) {
//   var value = event.target.selectedOptions["0"].value;
//   (new comx()).CallAPI("cg/metodoapi", value,
//		function (data) {
//		   vueController.vm.ddlSottoProdotto = data;
//		}
//	);

//}

// a partire da un source carica il target

//function ddl_Change(event, targetController, route) {
//   var value = event.target.selectedOptions["0"].value;

//   (new comx()).CallAPIFunc(route, value, targetController);
//   return true;
//}

//var comserv = new comx();

function comServ(controllername, apiPrefix) {
    return new comx(controllername, apiPrefix);
}
function comServPlan(controllername) {
    //return new comx(controllername, '/cg/cg.ashx');
    return new comx(controllername, '/lib/hnd/HandlerGestioneprodotti.ashx');
}
function comx(controllername, apiPrefix) {
    var mainscope = this;
    var comref = null;
    var vuepre = "";
    var vuelast = "";
    controllername = controllername || "vueController";
    apiPrefix = apiPrefix || '/lib/hnd/';
    //if (controllername.length > 0 && !controllername.endsWith('.'))
    //   controllername += ".";


    this.ddl_Change = function (event, targetController, route, tagetvueValue) {
        var value = '';
        //if (!(Object.prototype.toString.call(event) === "[object String]") && event.target.selectedOptions.length > 0)
            if (!(Object.prototype.toString.call(event) === "[object String]") )
            value = event.target.selectedOptions["0"].value;

        //////////////////////////////////////////
        //Controllo valore finale di destinazione per eventuale svuotamento
        //////////////////////////////////////////
        if (!(tagetvueValue === null || tagetvueValue === undefined || tagetvueValue === '')) {
            var c = tagetvueValue.split('.');
            var pre = "";
            var last = "";
            if (c.length > 1) {
                last = c[c.length - 1];
                pre = ".";
                for (var i = 0; i < c.length - 1; i++)
                    pre += c[i] + '.';
                pre = pre.substring(0, pre.length - 1);
            } else last = tagetvueValue;
            try { Vue.set(eval(controllername + pre), last, ''); } catch (e) { console.trace(e.message); } //vuoto il campo destinazione se indicato nei parametri
        }
        ///////////////////////////

        this.CallAPIFunc(route, value, targetController);
        return true;
    };

    this.CallAPI = function (method, Model, Success, Error, datatype) {
        var paramsData = { model: Model };
        if (paramsData === null)
            paramsData = {};

        paramsData["q"] = method;

        var JSONparamsData = JSON.stringify(paramsData);
        datatype = datatype || "json";
        var ret = $.ajax({
            url: apiPrefix, //+ method, //route dedicata alle chiamate api
            type: 'POST',
            data: JSONparamsData,
            dataType: datatype,
            contentType: 'application/json',

            success: function (data) {
                if (Success !== undefined)
                    Success(data);
                return false;
            },
            error: function (data) {
                if (Error !== undefined)
                    Error(data);
                return false;
            }
        });
        return ret;
    };

    /* chiamata per accesso triviale senza modello comp. verso vecchie versioni */
    this.Call = function (method, Model, Success, Error) {
        var paramsData = Model;
        if (paramsData === null)
            paramsData = {};

        paramsData["q"] = method;

        var JSONparamsData = JSON.stringify(paramsData);
        var ret = $.ajax({
            url: apiPrefix, //+ method, //route dedicata alle chiamate api
            type: 'POST',
            data: JSONparamsData,
            dataType: 'json',
            contentType: 'application/json',

            success: function (data) {
                if (Success !== undefined)
                    Success(data);
                return false;
            },
            error: function (data) {
                if (Error !== undefined)
                    Error(data);
                return false;
            }
        });
        return ret;
    };

    this.updatevuemodel = function (vuemodel, data) {
        //Preparo prima gli elementi per reinserire nel modello
        var c = vuemodel.split('.');
        vuepre = "";
        vuelast = "";
        if (c.length > 1) {
            vuelast = c[c.length - 1];
            vuepre = ".";
            for (var i = 0; i < c.length - 1; i++)
                vuepre += c[i] + '.';
            vuepre = vuepre.substring(0, vuepre.length - 1);
        } else vuelast = vuemodel;
        ///////////////////////
        if (Object.prototype.toString.call(data) === "[object String]" && testJSON(data))
            data = JSON.parse(data);
        if (vuelast !== '' && vuelast !== null)
            try { Vue.set(eval(controllername + vuepre), vuelast, data); } catch (e) { console.trace(e.message); }

    };

    this.CallAPIRoute = function (route, vuecontroller, vuemodel, Success, Error) {
        var param = vuecontroller[vuemodel]; //modello del controller vue da riempire
        //if (eval(controllername) != null) // il controller vue è nullo la prima volta fino uscita created
        //{
        //    param =  eval(controllername + "." + vuemodel);
        //}

        //Preparo prima gli elementi per reinserire nel modello
        var c = vuemodel.split('.');
        vuepre = "";
        vuelast = "";
        if (c.length > 1) {
            vuelast = c[c.length - 1];
            vuepre = ".";
            for (var i = 0; i < c.length - 1; i++)
                vuepre += c[i] + '.';
            vuepre = vuepre.substring(0, vuepre.length - 1);
        } else vuelast = vuemodel;
        ///////////////////////

        this.CallAPI(route, param,
            function (data) {
                if (Object.prototype.toString.call(data) === "[object String]" && testJSON(data))
                    data = JSON.parse(data);
                if (vuelast !== '' && vuelast !== null)
                    try { Vue.set(eval(controllername + vuepre), vuelast, data); } catch (e) { console.trace(e.message); }
                //vuecontroller[vuemodel] = JSON.parse(data); //metodo inserimento standard nel modello
                if (Success !== undefined)
                    Success(data);
            },
            function (data) {
                if (Error !== undefined)
                    Error(data);
            }
        );
    };

    // chiama una funzione lato API e ritorna il valore nel modello specificato
    this.CallAPIFunc = function (route, param, targetController, Success, Error, datatype) {
        mainscope.comref = targetController; //riusabile in callback è l'elemnte del modello in cui inserire i dati

        ///////////////////////////////////////
        //Preparo prima gli elementi per reinserire nel modello
        ///////////////////////////////////////
        var c = mainscope.comref.split('.');
        vuepre = "";
        vuelast = "";
        if (c.length > 1) {
            vuelast = c[c.length - 1];
            vuepre = ".";
            for (var i = 0; i < c.length - 1; i++)
                vuepre += c[i] + '.';
            vuepre = vuepre.substring(0, vuepre.length - 1);
        } else vuelast = mainscope.comref;
        ///////////////////////////////////////

        this.CallAPI(route, param,
            function (data) {
                if (Object.prototype.toString.call(data) === "[object String]" && testJSON(data))
                    data = JSON.parse(data);
                if (vuelast !== '' && vuelast !== null) {

                    try { Vue.set(eval(controllername + vuepre), vuelast, data); } catch (e) { console.trace(e.message); }
                }
                if (Success !== undefined)
                    Success(data);
            },
            function (data) {
                if (Error !== undefined)
                    Error(data);
            }, datatype
        );
    };



}
