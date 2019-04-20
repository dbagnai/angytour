using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AreaContenuti_ConfigPage : CommonPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SetCulture("it"); //forzo la cultura italia

    }
    protected void reset_Click(object sender, EventArgs e)
	 {
		 System.Web.HttpRuntime.UnloadAppDomain();
	 }
    protected void redirect_Click(object sender, EventArgs e)
    {
        references.updateredirecttable();
    }



    protected void btnUploadFile_Click(object sender, EventArgs e)
    {
        try
        {
            //-------------------------------------
            //Carichiamoci il file
            //-------------------------------------
            if (uplFile.HasFile)
            {
                if (uplFile.PostedFile.ContentLength > 10000000)
                {
                    output.Text = "Il file non può essere caricato perché supera 10MB!";
                }
                else
                {
                    //CArichiamo il file sul server
                    string pathDestinazione = WelcomeLibrary.STATIC.Global.percorsoFisicoComune + "\\_temp";
                    string virpathDestinazione = WelcomeLibrary.STATIC.Global.PercorsoComune + "/_temp";
                    DirectoryInfo di = new DirectoryInfo(pathDestinazione);
                    if (!di.Exists)
                        di.Create();
                    string NomeCorretto = "redirect.xlsx";
                    uplFile.PostedFile.SaveAs(pathDestinazione + "\\" + NomeCorretto);
                    string returl = ReplaceAbsoluteLinks(virpathDestinazione + "/" + NomeCorretto);
                    output.Text = "Importato file dati correttamente";
                }
            }
        }
        catch (Exception err)
        {
            output.Text = "Errore durante importazione file dati :" + err.Message;
        }
    }
}