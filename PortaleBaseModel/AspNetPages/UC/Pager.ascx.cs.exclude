using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class UC_Pager : System.Web.UI.UserControl
{
    public event CommandEventHandler PageGroupClickPrev;
    public event CommandEventHandler PageGroupClickNext;
    public void inizialize_handlers()
    {
        prevBtn.Command += new CommandEventHandler(PageGroupClickPrev);
        nextBtn.Command += new CommandEventHandler(PageGroupClickNext);
        // PrevGruppo.Command += new CommandEventHandler(PageGroupClickPrev);
        // NextGruppo.Command += new CommandEventHandler(PageGroupClickNext);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void Page_PreRender(object sender, EventArgs e)
    {

        TotalsLiteral.Text = TotalRecords.ToString();
        CurrentPageLiteral.Text = CurrentPage.ToString();
        if ((int)(TotalRecords / PageSize) > 0)
            totalPages = (int)System.Math.Ceiling(((Double)TotalRecords / (Double)PageSize));
        else
            totalPages = 1;
        TotalPagesLiteral.Text = totalPages.ToString();

        //Raggruppo le pagine in gruppi
        int startpage = ((nGruppoPagine - 1) * dimensioneGruppo) + 1;
        int endpage = (nGruppoPagine * dimensioneGruppo);
        if (endpage >= totalPages)
        {
            endpage = totalPages;
            nextBtn.Visible = false;
            tdNext.Visible = true;
        }
        else
        {
            nextBtn.Visible = true;
            tdNext.Visible = true;
        }

        //Se primo gruppo pagine -> faccio sparire il controllo -> precedente
        if (startpage == 1)
        {
            prevBtn.Visible = false;
            tdPrev.Visible = true;
        }
        else
        {
            prevBtn.Visible = true;
            tdPrev.Visible = true;
        }

        LinkButton link;

#if FALSE
        //VECCHIO SISTEMA DI GENERAZIONE PER I LINK
        for (int i = startpage; i <= endpage; i++)
        {
            link = new LinkButton();
            link.PostBackUrl = String.Format(NavigateUrl, "");//Non uso la querystring ma il viewstate
            //link.PostBackUrl = String.Format(NavigateUrl, i.ToString());
            // link.NavigateUrl = String.Format(NavigateUrl, i.ToString());
            link.Text = i.ToString();
            link.CommandArgument = i.ToString();
            link.ID = this.ID + "PagerOldLink_" + i.ToString();
            Links.Controls.Add(new LiteralControl(" | "));
            if (i != CurrentPage)
                Links.Controls.Add(link);
            else
            { Links.Controls.Add(new LiteralControl(i.ToString())); }
            Links.Controls.Add(new LiteralControl(" | "));
        }
        
#endif

        //NUOVA VERSIONE GRAFICA
        HtmlTable pagerInnerTable = (HtmlTable)divFullGridPagerNVS.FindControl("pagerInnerTable");
        HtmlTable pagerOuterTable = (HtmlTable)divFullGridPagerNVS.FindControl("pagerOuterTable");
        
        pagerInnerTable.Rows[0].Cells.Clear();//Rimuovo le celle presenti

        //Inseriamo il bottone di scorrimento pagine
        //CreatePageGroupPrev(pagerInnerTable);
        int insertCellPosition = 0;
        if (pagerInnerTable != null)
        {
            for (int i = startpage, pos = 0; i <= endpage; i++, pos++)
            {
                // Create a new table cell for every data page number.
                HtmlTableCell tableCell = new HtmlTableCell();
                
                link = new LinkButton();
                link.ID = "Page" + i.ToString();
                link.PostBackUrl = String.Format(NavigateUrl, "");//Non uso la querystring ma il viewstate
                link.Text = i.ToString();
                link.CommandArgument = i.ToString();
                link.ID = this.ID + "PagerLink_" + i.ToString();
               
                if (CurrentPage == i)
                {
                    //link.CssClass = "pageCurrentNumber_b";
                    tableCell.Attributes.Add("class", "pageCurrentNumber_b");
                }
                else
                {
                    //link.CssClass = "pagePrevNextNumber_b";
                    tableCell.Attributes.Add("class", "pagePrevNextNumber_b");
                }

                
                //link.CssClass = "pagerLink_b";
                // Place link inside the table cell; Add the cell to the table row.                
                tableCell.Controls.Add(link);
                pagerInnerTable.Rows[0].Cells.Insert(insertCellPosition + pos, tableCell);
            }
        }
        int lastCellPosition = pagerInnerTable.Rows[0].Cells.Count;
        //CreatePageNextGroup(pagerOuterTable, lastCellPosition);
    }

    private void CreatePageGroupPrev(HtmlTable pagerInnerTable)
    {
        ImageButton lnk = prevBtn;
        lnk.CommandName = "Page";
        lnk.CommandArgument = "0";
        lnk.ImageUrl = "~/AspNetPages/Immagini/nav_leftarr.gif";
        if (PageGroupClickPrev != null)
            lnk.Command += new CommandEventHandler(PageGroupClickPrev);
    }
    private void CreatePageGroupNext(HtmlTable pagerInnerTable, int insertCellPosition)
    {
        ImageButton lnk = nextBtn;
        lnk.ImageUrl = "~/AspNetPages/Immagini/nav_rightarr.gif";
        lnk.CommandName = "Page";
        lnk.CommandArgument = "0";
        if (PageGroupClickNext != null)
            lnk.Command += new CommandEventHandler(PageGroupClickNext);   
    }



   
    private int _pageSize;
    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value; }
    }

    private string _navigateUrl;
    public string NavigateUrl
    {
        get { return _navigateUrl; }
        set { _navigateUrl = value; }
    }
    public int TotalRecords
    {
        get { return ViewState[this.ID + "TotalRecords"] != null ? Convert.ToInt32(ViewState[this.ID + "TotalRecords"].ToString()) : 1; }
        set { ViewState[this.ID + "TotalRecords"] = value; }
    }
    //private int _totalRecords;
    //public int TotalRecords
    //{
    //    get { return _totalRecords; }
    //    set { _totalRecords = value; }
    //}
    public int CurrentPage
    {
        get { return ViewState[this.ID + "CurrentPage"] != null ? Convert.ToInt32(ViewState[this.ID + "CurrentPage"].ToString()) : 1; }
        set { ViewState[this.ID + "CurrentPage"] = value; }
    }

    //Raggruppamenti di pagine
    public int nGruppoPagine
    {
        get { return ViewState[this.ID + "nGruppoPagine"] != null ? Convert.ToInt32(ViewState[this.ID + "nGruppoPagine"].ToString()) : 1; }
        set { ViewState[this.ID + "nGruppoPagine"] = value; }
    }
    private int _dimensioneGruppo;
    public int dimensioneGruppo
    {
        get { return _dimensioneGruppo; }
        set { _dimensioneGruppo = value; }
    }
    public int totalPages
    {
        get { return ViewState[this.ID + "totalPages"] != null ? Convert.ToInt32(ViewState[this.ID + "totalPages"].ToString()) : 0; }
        set { ViewState[this.ID + "totalPages"] = value; }
    }

}
