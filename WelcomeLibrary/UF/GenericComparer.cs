using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Configuration;

namespace WelcomeLibrary.UF
{

    /// <summary>
    /// Classe GenericComparer che permette l'ordinamento di una lista generica in base a un campo
    /// </summary>
    public sealed class GenericComparer<T> : IComparer<T>
    {

        private string _sortColumn;
        private ListSortDirection _sortDirection;

        public GenericComparer(string sortColumn, ListSortDirection sortDirection)
        {
            SortColumn = sortColumn;
            SortDirection = sortDirection;
        }

        public string SortColumn
        {
            get { return _sortColumn; }
            private set { _sortColumn = value; }
        }

        public ListSortDirection SortDirection
        {
            get { return _sortDirection; }
            set { _sortDirection = value; }
        }

        /// <summary>
        /// Resituisce int > 0 se x>y (ascending), 0 se sono uguali
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(T x, T y)
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(SortColumn);
            IComparable obj1 = (IComparable)propertyInfo.GetValue(x, null);
            IComparable obj2 = (IComparable)propertyInfo.GetValue(y, null);
            if (SortDirection == ListSortDirection.Ascending)
                return (obj1.CompareTo(obj2));
            else
                return (obj2.CompareTo(obj1));
        }
    }


    /// <summary>
    /// Camparer generico che usa due proprietà per l'ordinamento
    /// sortColumn1 -> colonna primaria di ordinamento
    /// sortColumn2 -> colonna secondaria di ordinamento
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class GenericComparer2<T> : IComparer<T>
    {

        private string _sortColumn1;
        private string _sortColumn2;
        private ListSortDirection _sortDirection1;
        private ListSortDirection _sortDirection2;

        public GenericComparer2(string sortColumn1, ListSortDirection sortDirection1, string sortColumn2, ListSortDirection sortDirection2)
        {
            SortColumn1 = sortColumn1;
            SortDirection1 = sortDirection1;
            SortColumn2 = sortColumn2;
            SortDirection2 = sortDirection2;
        }

        public string SortColumn1
        {
            get { return _sortColumn1; }
            private set { _sortColumn1 = value; }
        }

        public ListSortDirection SortDirection1
        {
            get { return _sortDirection1; }
            set { _sortDirection1 = value; }
        }
        public string SortColumn2
        {
            get { return _sortColumn2; }
            private set { _sortColumn2 = value; }
        }

        public ListSortDirection SortDirection2
        {
            get { return _sortDirection2; }
            set { _sortDirection2 = value; }
        }

        public int Compare(T x, T y)
        {
            PropertyInfo propertyInfo1 = typeof(T).GetProperty(SortColumn1);
            PropertyInfo propertyInfo2 = typeof(T).GetProperty(SortColumn2);

            IComparable obj1_col1 = (IComparable)propertyInfo1.GetValue(x, null);
            IComparable obj2_col1 = (IComparable)propertyInfo1.GetValue(y, null);

            IComparable obj1_col2 = (IComparable)propertyInfo2.GetValue(x, null);
            IComparable obj2_col2 = (IComparable)propertyInfo2.GetValue(y, null);

            //Controllo per i valori nulli
            if (obj1_col1 != null && obj2_col1 == null)
            {
                if (SortDirection1 == ListSortDirection.Ascending)
                    return -1;
                else
                    return 1;
            }
            else if (obj1_col1 == null && obj2_col1 != null)
            {
                if (SortDirection1 == ListSortDirection.Ascending)
                    return 1;
                else
                    return -1;
            }


            if (SortDirection1 == ListSortDirection.Ascending)
            {
                int retval = 0;
                if ((obj1_col1 == null && obj2_col1 == null) || obj1_col1.Equals(obj2_col1))
                {
                    //Controllo per i valori nulli sull'elemento secondario
                    if (obj1_col2 == null && obj2_col2 == null)
                        return 0;
                    if (obj1_col2 != null && obj2_col2 == null)
                    {
                        if (SortDirection2 == ListSortDirection.Ascending)
                            return -1;
                        else
                            return 1;
                    }
                    else if (obj1_col2 == null && obj2_col2 != null)
                    {
                        if (SortDirection2 == ListSortDirection.Ascending)
                            return 1;
                        else
                            return -1;
                    }

                    //Comparazione dei valori
                    if (SortDirection2 == ListSortDirection.Ascending)
                    {
                        retval = obj1_col2.CompareTo(obj2_col2);
                    }
                    else
                    {
                        retval = obj2_col2.CompareTo(obj1_col2);
                    }
                }
                else
                {
                    retval = obj1_col1.CompareTo(obj2_col1);
                }
                return retval;
            }
            else //Comparazione 1 discendente
            {
                int retval = 0;
                if ((obj1_col1 == null && obj2_col1 == null) || obj2_col1.Equals(obj1_col1))
                {

                    //Controllo per i valori nulli sull'elemento secondario
                    if (obj1_col2 == null && obj2_col2 == null)
                        return 0;
                    if (obj1_col2 != null && obj2_col2 == null)
                    {
                        if (SortDirection2 == ListSortDirection.Ascending)
                            return -1;
                        else
                            return 1;
                    }
                    else if (obj1_col2 == null && obj2_col2 != null)
                    {
                        if (SortDirection2 == ListSortDirection.Ascending)
                            return 1;
                        else
                            return -1;
                    }

                    //Comparazione dei valori
                    if (SortDirection2 == ListSortDirection.Ascending)
                    {
                        retval = obj1_col2.CompareTo(obj2_col2);
                    }
                    else
                    {
                        retval = obj2_col2.CompareTo(obj1_col2);
                    }
                }
                else
                { retval = obj2_col1.CompareTo(obj1_col1); }
                return retval;
            }

        }
    }


}