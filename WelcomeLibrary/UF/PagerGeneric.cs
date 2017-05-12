using System;
using System.Collections.Generic;
using System.Text;

namespace WelcomeLibrary.UF
{
    /// <summary>
    /// Offre un sistema generico per paginare collection di elementi
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Pager<T> : List<T>
    {

        //public string SessionNameExt
        //{
        //    get { return SessionName + page.UniqueID; }
        //}
        ////

        const string SessionName = "PagerG";
        private List<T> _list = new List<T>();
        private string _PropertyName = "";

        private bool _UsaCache = false;
        public bool UsaCache
        {
            get { return _UsaCache; }
            set { _UsaCache = value; }
        }

        /// <summary>
        /// Restituisce il numero massimo di pagine possibili nella collection con il dato numero di records per page
        /// </summary>
        /// <param name="RecordsPerPage"></param>
        /// <returns></returns>
        public int PageCount(int RecordsPerPage)
        {
            if (RecordsPerPage == 0) RecordsPerPage = 1;
            int MaxPage = base.Count / RecordsPerPage;
            return MaxPage++;
        }

        /// <summary>
        ///  Costruttore senza parametri
        /// </summary>
        public Pager()
        {
        }
        /// <summary>
        /// Costruttore con passaggio di dati
        /// </summary>
        /// <param name="oCollection"></param>
        public Pager(List<T> oCollection)
        {
            if (oCollection != null)
                foreach (T item in oCollection) base.Add(item);
        }
        /// <summary>
        /// Costruttore per utilizzo di cache
        /// </summary>
        /// <param name="oCollection"></param>
        /// <param name="UsaCache"></param>
        /// <param name="page"></param>
        #region GESTIONE DELLA CACHE TIPO VECCHIO
        public Pager(List<T> oCollection, bool UsaCache, System.Web.UI.Page page)
        {
            if (oCollection != null)
                foreach (T item in oCollection) base.Add(item);
            _UsaCache = UsaCache;
            if (UsaCache)
                page.Session.Add(SessionName + page.UniqueID, this);
        }

        public bool LoadFromCache(System.Web.UI.Page page)
        {
            bool ret = true;
            base.Clear();
            try
            {
                if (page.Session[SessionName + page.UniqueID] == null)//Sessione scaduta-> devo ricaricare  OCCHIO QUESTO non funziona con più pager nella stessa pagina
                    return false;
                List<T> oCollection = (List<T>)page.Session[SessionName + page.UniqueID];
                if (oCollection != null)
                    foreach (T item in oCollection) base.Add(item);
            }
            catch { }
            return ret;
        }

        public void RemoveCache(System.Web.UI.Page page)
        {
            page.Session.Remove(SessionName + page.UniqueID);
        }
        
        #endregion

        #region GESTIONE DELLA CACHE TIPO NUOVO CON IDENTIFICATORE UNICO PER PAGINA E PAGER
        public Pager(List<T> oCollection, bool UsaCache, System.Web.UI.Page page, string UniqueIdentifierCache)
        {
            if (oCollection != null)
                foreach (T item in oCollection) base.Add(item);
            _UsaCache = UsaCache;
            if (UsaCache)
                page.Session.Add(SessionName + UniqueIdentifierCache, this);
        }

        public bool LoadFromCache(System.Web.UI.Page page, string UniqueIdentifierCache)
        {
            bool ret = true;
            base.Clear();
            try
            {
                if (page.Session[SessionName + UniqueIdentifierCache] == null)//Sessione scaduta-> devo ricaricare i dati nel pager dal db
                    return false;
                List<T> oCollection = (List<T>)page.Session[SessionName + UniqueIdentifierCache];
                if (oCollection != null)
                    foreach (T item in oCollection) base.Add(item);
            }
            catch { }
            return ret;
        }

        public void RemoveCache(System.Web.UI.Page page, string UniqueIdentifierCache)
        {
            page.Session.Remove(SessionName + UniqueIdentifierCache);
        }

        #endregion
     
        
        public void OrderBy(string PropertyName)
        {
            _PropertyName = PropertyName;
            if (base.Count > 0)
            {
                T[] _base = new T[base.Count];
                // faccio un duplicato della classe base
                base.CopyTo(_base);
                // lo ordino secondo la property
                Array.Sort<T>(_base, PropertyComparer);
                // rimetto tutto nel Base
                base.Clear();
                for (int i = 0; i < _base.Length; i++) base.Add(_base[i]);

            }
        }

        public List<T> GetOrderedBy(string PropertyName)
        {
            _list.Clear();
            if (base.Count > 0)
            {
                T[] _base = new T[base.Count];
                // faccio un duplicato della classe base
                base.CopyTo(_base);
                // lo ordino secondo la property
                Array.Sort<T>(_base, PropertyComparer);
                // Copio nella lista da restituire senza ordinare il Base
                base.Clear();
                for (int i = 0; i < _base.Length; i++) _list.Add(_base[i]);
            }
            return _list;
        }

        public List<T> GetPagerList(int PageIndex, int RecordsPerPage, string OrderByNomeCampo)
        {
            OrderBy(OrderByNomeCampo);
            return GetPagerList(PageIndex, RecordsPerPage);
        }

        public List<T> GetPagerList(int PageIndex, int RecordsPerPage)
        {

            List<T> myList = new List<T>();
            if ((base.Count > 0) && (RecordsPerPage > 0))
            {
                int StartIndex = 0;
                int MaxPage = 1;
                if ((int)(base.Count / RecordsPerPage) > 0)
                    MaxPage = (int)System.Math.Ceiling(((Double)base.Count / (Double)RecordsPerPage));
                if (PageIndex > MaxPage) PageIndex = MaxPage;
                StartIndex = (PageIndex - 1) * RecordsPerPage;
                // evitiamo di andare fuori range
                if (StartIndex < 0) StartIndex = 0;
                int EndIndex = StartIndex + RecordsPerPage;
                if (EndIndex > base.Count) EndIndex = base.Count;
                // siccome gli elementi sono già ordinati (si spera) per quello che vogliamo...
                for (int i = StartIndex; i < EndIndex; i++) myList.Add(base[i]);
            }
            return myList;
        }

        private int PropertyComparer(T x, T y)
        {
            int Ret = 0;
            // con la reflection tiro su da T i campi da comparare
            Type objectTypeX = x.GetType();
            System.Reflection.PropertyInfo pix = objectTypeX.GetProperty(_PropertyName);
            Type objectTypeY = y.GetType();
            System.Reflection.PropertyInfo piy = objectTypeY.GetProperty(_PropertyName);

            if (objectTypeX == typeof(string))
            {
                string _sx = Convert.ToString(pix.GetValue(x, null));
                string _sy = Convert.ToString(piy.GetValue(y, null));
                Ret = string.Compare(_sx, _sy);
            }

            else if (objectTypeX == typeof(int))
            {
                int _ix = Convert.ToInt32(pix.GetValue(x, null));
                int _iy = Convert.ToInt32(piy.GetValue(y, null));
                if (_ix > _iy) Ret = 1; else Ret = -1;
            }

            return Ret;

            // pi.GetValue
            //Type objectType = objectToInspect.GetType();
            //System.Reflection.PropertyInfo[] properties =
            //objectType.GetProperties();
        }

    }
}
