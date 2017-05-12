using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class UC_PagerEx : System.Web.UI.UserControl
{
    public delegate void OnPageCommand(object sender, string PageNum);
    public event OnPageCommand PageCommand;

    public delegate void OnPageGroupClickNext(object sender, string spare);
    public event OnPageGroupClickNext PageGroupClickNext;

    public delegate void OnPageGroupClickPrev(object sender, string spare);
    public event OnPageGroupClickPrev PageGroupClickPrev;

    //public event CommandEventHandler PageGroupClickPrev;
    //public event CommandEventHandler PageGroupClickNext;
    //public void inizialize_handlers()
    //{
    //    prevBtn.Command += new CommandEventHandler(PageGroupClickPrev);
    //    nextBtn.Command += new CommandEventHandler(PageGroupClickNext);
    //    // PrevGruppo.Command += new CommandEventHandler(PageGroupClickPrev);
    //    // NextGruppo.Command += new CommandEventHandler(PageGroupClickNext);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        //List<PageItem> sl = new List<PageItem>();
        //PageItem pi = new PageItem();
        //pi.PageNumber = "1";
        //sl.Add(pi);
        //pi.PageNumber = "2";
        //sl.Add(pi);
        //pi.PageNumber = "3";
        //sl.Add(pi);
        //pi.PageNumber = "4";
        //sl.Add(pi);
        //MyRepeater.DataSource = sl;
        //MyRepeater.DataBind();

    }

    public void Page_PreRender(object sender, EventArgs e)
    {
        if ((int)(TotalRecords / PageSize) > 0)
            totalPages = (int)System.Math.Ceiling(((Double)TotalRecords / (Double)PageSize));
        else
            totalPages = 1;
        // TotalPagesLiteral.Text = totalPages.ToString() + " :";
        //TotalsLiteral.Text = TotalRecords.ToString();
        //CurrentPageLiteral.Text = CurrentPage.ToString();

        //Raggruppo le pagine in gruppi
        int startpage = ((nGruppoPagine - 1) * dimensioneGruppo) + 1;
        int endpage = (nGruppoPagine * dimensioneGruppo);
        if (endpage >= totalPages)
        {
            endpage = totalPages;
            liNext.Visible = false;
        }
        else liNext.Visible = true;
        //Se primo gruppo pagine -> faccio sparire il controllo -> precedente
        if (startpage == 1) liPrev.Visible = false;
        else
            liPrev.Visible = true;

        //NUOVA VERSIONE GRAFICA
        // HtmlTable pagerOuterTable = (HtmlTable)divFullGridPagerNVS.FindControl("pagerOuterTable");

        //pagerInnerTable.Rows[0].Cells.Clear();//Rimuovo le celle presenti

        //Inseriamo il bottone di scorrimento pagine

        List<PageItem> sl = new List<PageItem>();
        if (startpage >= 0 && endpage > 0)
            for (int i = startpage, pos = 0; i <= endpage; i++, pos++)
            {
                PageItem item = new PageItem();
                item.PageNumber = i.ToString();
                if (CurrentPage == i)
                    item.CssClass = "current"; //active
                else
                    item.CssClass = "";
                sl.Add(item);
            }
        RepeaterPages.DataSource = sl;
        RepeaterPages.DataBind();

    }
    //protected bool SettaSeparatore(object numeropag)
    //{

    //    int _p = 0;
    //    int.TryParse(numeropag.ToString(), out _p);
    //    if (_p >= totalPages)
    //        return false;
    //    else
    //        return true;

    //}
    protected class PageItem
    {
        string _PageNumber;
        string _CssClass;

        public string PageNumber
        {
            get { return _PageNumber; }
            set { _PageNumber = value; }
        }

        public string CssClass
        {
            get { return _CssClass; }
            set { _CssClass = value; }
        }


        public PageItem()
        {
            _PageNumber = "0";
            _CssClass = "";
        }
    }

    //private void CreatePageGroupPrev(HtmlTable pagerInnerTable)
    //{
    //    // Create the First and the Previous buttons.
    //    HtmlTableCell tableCell = new HtmlTableCell();
    //    tableCell.Attributes.Add("class", "pageFirstLast");
    //    HtmlImage img = new HtmlImage();
    //    img.Src = "~/Immagini/firstpage.gif";

    //    LinkButton lnk = new LinkButton();
    //    lnk.ID = "lnkPrevGroup";
    //    lnk.CssClass = "pagerLink";
    //    lnk.Text = "Indietro";
    //    lnk.CommandName = "Page";
    //    lnk.CommandArgument = "0";
    //    if (PageGroupClickPrev != null)
    //        lnk.Command += new CommandEventHandler(PageGroupClickPrev);
    //    tableCell.Controls.Add(img);
    //    tableCell.Controls.Add(new LiteralControl("&nbsp;"));
    //    tableCell.Controls.Add(lnk);
    //    pagerInnerTable.Rows[0].Cells.Insert(0, tableCell);
    //}

    //private void CreatePageGroupNext(HtmlTable pagerInnerTable, int insertCellPosition)
    //{
    //    // Create the First and the Previous buttons.
    //    HtmlTableCell tableCell = new HtmlTableCell();
    //    tableCell.Attributes.Add("class", "pageFirstLast");
    //    HtmlImage img = new HtmlImage();
    //    img.Src = "~/Immagini/lastpage.gif";
    //    LinkButton lnk = new LinkButton();
    //    lnk.ID = "lnkNextGroup";
    //    lnk.CssClass = "pagerLink";
    //    lnk.Text = "Avanti";
    //    lnk.CommandName = "Page";
    //    lnk.CommandArgument = "0";
    //    if (PageGroupClickNext != null)
    //        lnk.Command += new CommandEventHandler(PageGroupClickNext);
    //    tableCell.Controls.Add(img);
    //    tableCell.Controls.Add(new LiteralControl("&nbsp;"));
    //    tableCell.Controls.Add(lnk);
    //    pagerInnerTable.Rows[0].Cells.Insert(insertCellPosition, tableCell);
    //}

    public int TotalRecords
    {
        get { return ViewState[this.ID + "totalRecords"] != null ? Convert.ToInt32(ViewState[this.ID + "totalRecords"].ToString()) : 0; }
        set { ViewState[this.ID + "totalRecords"] = value; }
    }
    public int PageSize
    {
        get { return ViewState[this.ID + "pageSize"] != null ? Convert.ToInt32(ViewState[this.ID + "pageSize"].ToString()) : 0; }
        set { ViewState[this.ID + "pageSize"] = value; }
    }

    private string _navigateUrl;
    public string NavigateUrl
    {
        get { return _navigateUrl; }
        set { _navigateUrl = value; }
    }

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

    protected void lbPage_Command(object sender, CommandEventArgs e)
    {
        if (PageCommand != null)
            PageCommand(this, e.CommandArgument.ToString());
    }
    protected void nextBtn_Command(object sender, CommandEventArgs e)
    {
        int ntotalepagine = this.totalPages;
        int totalegruppi = 0;
        if ((int)(ntotalepagine / this.dimensioneGruppo) > 0)
            totalegruppi = (int)System.Math.Ceiling(((Double)ntotalepagine / (Double)this.dimensioneGruppo));
        else
            totalegruppi = 1;
        if (this.nGruppoPagine < totalegruppi)
            this.nGruppoPagine += 1;

        if (PageGroupClickNext != null)
            PageGroupClickNext(this, e.CommandArgument.ToString());
    }

    protected void prevBtn_Command(object sender, CommandEventArgs e)
    {
        int ntotalepagine = this.totalPages;
        int totalegruppi = 0;
        if ((int)(ntotalepagine / this.dimensioneGruppo) > 0)
            totalegruppi = (int)System.Math.Ceiling(((Double)ntotalepagine / (Double)this.dimensioneGruppo));
        else
            totalegruppi = 1;
        if (this.nGruppoPagine > 1)
            this.nGruppoPagine -= 1;

        if (PageGroupClickPrev != null)
            PageGroupClickPrev(this, e.CommandArgument.ToString());
    }

}