using System;
using System.Collections.Generic;
using System.Text;
using WelcomeLibrary.DOM;
using WelcomeLibrary.DAL;
using System.Data.SQLite;
using System.Net.Mail;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using System.Configuration;
using System.Net.Configuration;
using System.Xml;
using ActiveUp.Net;
using ActiveUp.Net.Mail;
using ActiveUp.Net.Security;
using Newtonsoft.Json;
using System.Collections.Specialized;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace WelcomeLibrary.UF
{

    public class listegeografiche
    {
        public OrderedDictionary ListNazione = new OrderedDictionary();
        public OrderedDictionary ListRegione = new OrderedDictionary();
        public OrderedDictionary ListProvincia = new OrderedDictionary();
        public OrderedDictionary ListComune = new OrderedDictionary();
        //public string Lingua = "";

    }

    public class ResultAutocomplete
    {
        public string id { get; set; }
        public string label { get; set; }
        public string value { get; set; }
        public string codice { get; set; }
        public string link { get; set; }
        public string linktext { get; set; }
        public string email { get; set; }
        public string nome { get; set; }
        public string cognome { get; set; }
        public string price { get; set; }
        public string iva { get; set; }
        public string um { get; set; }
        public string qta { get; set; }
        public string image { get; set; }
    }
    public class ModelCarCombinate
    {
        public string id { get; set; }
        public ResultAutocomplete caratteristica1 { get; set; }
        public ResultAutocomplete caratteristica2 { get; set; }
        public string qta { get; set; }
    }
    public static class Csv
    {
        public static string Escape(string s)
        {
            if (s == null) return s;

            s = s.Replace("\r\n", "\n");
            s = QUOTE + s + QUOTE;
            //foreach (string c in CHARACTERS_THAT_MUST_BE_QUOTED)
            //{
            //    s = s.Replace(c, "\\" + c);
            //}

            return s;
        }

        private const string QUOTE = "\"";
        private const string ESCAPED_QUOTE = "\"\"";
        private static string[] CHARACTERS_THAT_MUST_BE_QUOTED = { ",", "\r\n", "\r", "\n" };


        public static void Csv_to_xlsx(string csvfilepath)
        {
            System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
            Messaggi.Add("Messaggio", "");
            Messaggi["Messaggio"] = "Conversione a csv file " + System.DateTime.Now.ToString() + " \r\n";

            const int MAX_NUM_ROW = 33; // num of days in month + id + label =33
            const int MAX_NUM_COLUMN = 16;// num of labels =16
            try
            {
                string myFnameCsv = csvfilepath;
                string myFnameXlsx = myFnameCsv.Replace(".csv", ".xlsx");
                //string mySheetName = myFnameCsv.Replace(".csv", "");
                string mySheetName = Path.GetFileName(myFnameCsv).Replace(".csv", "");

                //System.Text.Encoding myEncSjis = System.Text.Encoding.GetEncoding("Shift_JIS");
                System.Text.Encoding myEncoding = System.Text.Encoding.GetEncoding(1252);//ISO-8859-1 ANSI
                                                                                         //System.Text.Encoding.UTF8

                string[,] myAryStr = new string[MAX_NUM_ROW, MAX_NUM_COLUMN];
                Dictionary<UInt32, Dictionary<UInt32, string>> myAryDict = new Dictionary<UInt32, Dictionary<UInt32, string>>();
                UInt32 uiMaxRow = 0;
                UInt32 uiMaxColumn = 0;

                // read csv file
                using (StreamReader myrd = new StreamReader(myFnameCsv, myEncoding))
                {
                    UInt32 uiR = 0;
                    while (!myrd.EndOfStream)
                    {
                        string linecvs = myrd.ReadLine();
                        string[] tmpStAry = linecvs.Split(';');
                        int i = 0;
                        while (tmpStAry.Length < 85)
                        {
                            linecvs += myrd.ReadLine();
                            tmpStAry = linecvs.Split(';');
                            i++;
                            if (i > 50)
                            {
                                throw new ApplicationException("Errore conversione da csv  TOO MANY carriage return");
                            }
                        }

                        UInt32 uiC = 0;
                        foreach (string tmpstr in tmpStAry)
                        {
                            string tmpstr2 = tmpstr.Replace("\"", "").Replace("\0", "").Trim();

                            //myAryStr[uiR, uiC] = tmpstr2;

                            if (!myAryDict.ContainsKey(uiR))
                                myAryDict.Add(uiR, new Dictionary<UInt32, string>());
                            myAryDict[uiR].Add(uiC, tmpstr2);

                            //Console.Write(tmpstr2 + "\t");
                            uiC += 1;
                        }//end of foreach
                         //uiMaxColumn = uiMaxColumn < uiC ? uiC : uiMaxColumn;
                        uiR += 1;
                        //Console.WriteLine("");
                    }// end while
                    uiMaxRow = uiR;
                }//end using csv file

                // write xlsx file
                using (var myBook = new ClosedXML.Excel.XLWorkbook(ClosedXML.Excel.XLEventTracking.Disabled))
                {
                    var mySheet1 = myBook.AddWorksheet(mySheetName);

                    //for (int uiR = 0; uiR < uiMaxRow; uiR++)
                    //{
                    //    for (int uiC = 0; uiC < uiMaxColumn; uiC++)
                    //    {
                    //        mySheet1.Cell(uiR + 1, uiC + 1).Value = myAryStr[uiR, uiC];
                    //        //Console.Write(myAryStr[uiR, uiC] + "\t");
                    //    }//end for uiC
                    //     //Console.WriteLine("");
                    //}//end for uiR

                    // mySheet1.Column(3).CellsUsed().Style.NumberFormat.Format = "$ ###0,00";

                    foreach (UInt32 riga in myAryDict.Keys)
                    {
                        foreach (KeyValuePair<UInt32, string> colonna in myAryDict[riga])
                        {
                            string tmpcolval = colonna.Value;
                            if (colonna.Key == 2 || colonna.Key == 3 || colonna.Key >= 25) tmpcolval = tmpcolval.Replace(".", ",");
                            mySheet1.Cell((int)(riga + 1), (int)(colonna.Key + 1)).Value = tmpcolval;
                            //setta a testo le colonne che non voglio convertire a numeri
                            //if (!(colonna.Key == 1 ))
                            //    mySheet1.Cell((int)(riga + 1), (int)(colonna.Key + 1)).SetDataType(ClosedXML.Excel.XLCellValues.Text);
                        }
                    }

                    myBook.SaveAs(myFnameXlsx);
                }//end of using xlsx file
            }//end of try
            catch (Exception e)
            {
                //Devi scrivere l'errore in un file di log (per gli errori) sennò nessuno lo vede!!!!
                Messaggi["Messaggio"] += " Errore coversione file csv: " + e.Message + " " + System.DateTime.Now.ToString();
                if (e.InnerException != null)
                    Messaggi["Messaggio"] += " Errore interno conversione csv : " + e.InnerException.Message.ToString() + " " + System.DateTime.Now.ToString();
                WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune);
                throw new ApplicationException("Errore conversione da csv  :" + e.Message, e);

            }//end of catch
        }//end of Main

        /// <summary>
        /// Conversione da csv a xls
        /// </summary>
        /// <param name="csvfilepath"></param>
        /// <exception cref="ApplicationException"></exception>
        public static void Csv_to_xlsx_lowmem(string csvfilepath)
        {
            System.Collections.Generic.Dictionary<string, string> Messaggi = new System.Collections.Generic.Dictionary<string, string>();
            Messaggi.Add("Messaggio", "");
            Messaggi["Messaggio"] = "Conversione a csv file " + System.DateTime.Now.ToString() + " \r\n";

            const int MAX_NUM_ROW = 33; // num of days in month + id + label =33
            const int MAX_NUM_COLUMN = 16;// num of labels =16
            try
            {
                string myFnameCsv = csvfilepath;
                string myFnameXlsx = myFnameCsv.Replace(".csv", ".xlsx");
                //string mySheetName = myFnameCsv.Replace(".csv", "");
                string mySheetName = Path.GetFileName(myFnameCsv).Replace(".csv", "");

                //System.Text.Encoding myEncoding = System.Text.Encoding.GetEncoding("Shift_JIS");
                System.Text.Encoding myEncoding = System.Text.Encoding.GetEncoding(1252);//ISO-8859-1 ANSI
                                                                                         //System.Text.Encoding.UTF8

                // string[,] myAryStr = new string[MAX_NUM_ROW, MAX_NUM_COLUMN];
                //Dictionary<UInt32, Dictionary<UInt32, string>> myAryDict = new Dictionary<UInt32, Dictionary<UInt32, string>>();
                Dictionary<UInt32, Dictionary<UInt32, string>> myAryDictSingleRow = new Dictionary<UInt32, Dictionary<UInt32, string>>();
                UInt32 uiMaxRow = 0;
                UInt32 uiMaxColumn = 0;

                // write xlsx file ogni tot righe per allocare meno menoria
                long maxrowinmemory = 500;
                long blockwrite = 1;

                // write xlsx file
                using (var myBook = new ClosedXML.Excel.XLWorkbook(ClosedXML.Excel.XLEventTracking.Disabled))
                {
                    var mySheet1 = myBook.AddWorksheet(mySheetName);
                    myBook.SaveAs(myFnameXlsx);
                }//end of using xlsx file
             
                using (var myBook = new ClosedXML.Excel.XLWorkbook(myFnameXlsx))
                {
                    var mySheet1 = myBook.Worksheet(mySheetName);

                    // read csv file
                    using (StreamReader myrd = new StreamReader(myFnameCsv, myEncoding))
                    {
                        UInt32 uiR = 0;
                        while (!myrd.EndOfStream)
                        {
                            string linecvs = myrd.ReadLine();
                            string[] tmpStAry = linecvs.Split(';');
                            int i = 0;
                            while (tmpStAry.Length < 85)
                            {
                                linecvs += myrd.ReadLine();
                                tmpStAry = linecvs.Split(';');
                                i++;
                                if (i > 50)
                                {
                                    throw new ApplicationException("Errore conversione da csv  TOO MANY carriage return");
                                }
                            }

                            //carico le colonne della riga attuale nel dictionary temporaneo
                            UInt32 uiC = 0;
                            foreach (string tmpstr in tmpStAry)
                            {
                                string tmpstr2 = tmpstr.Replace("\"", "").Replace("\0", "").Trim();

                                //if (!myAryDict.ContainsKey(uiR))
                                //    myAryDict.Add(uiR, new Dictionary<UInt32, string>());
                                //myAryDict[uiR].Add(uiC, tmpstr2);

                                //alternativa a sigola riga
                                if (!myAryDictSingleRow.ContainsKey(0))
                                    myAryDictSingleRow.Add(0, new Dictionary<UInt32, string>());
                                myAryDictSingleRow[0].Add(uiC, tmpstr2);

                                //Console.Write(tmpstr2 + "\t");
                                uiC += 1;
                            }//end of foreach


                            //scriviamo su excel la riga attuale da myAryDictSingleRow scorrendo le colonne
                            foreach (KeyValuePair<UInt32, string> colonna in myAryDictSingleRow[0])
                            {
                                string tmpcolval = colonna.Value;
                                if (colonna.Key == 2 || colonna.Key == 3 || colonna.Key >= 25) tmpcolval = tmpcolval.Replace(".", ",");
                                mySheet1.Cell((int)(uiR + 1), (int)(colonna.Key + 1)).Value = tmpcolval;
                                if (colonna.Key == 1)
                                    mySheet1.Cell((int)(uiR + 1), (int)(colonna.Key + 1)).SetDataType(ClosedXML.Excel.XLCellValues.Text);
                            }
                            if (uiR > maxrowinmemory * blockwrite)
                            {
                                myBook.SaveAs(myFnameXlsx); //salvo il file excel
                                blockwrite += 1;
                                System.Threading.Thread.Sleep(5);
                                System.GC.Collect();
                                //try
                                //{
                                //    string tmp = WelcomeLibrary.UF.SharedStatic.MakeHttpHtmlGet(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/keepalive.aspx", 1252);
                                //    Messaggi["Messaggio"] += "CAlled page  (" + WelcomeLibrary.STATIC.Global.percorsobaseapplicazione + "/keepalive.aspx" + ")  : " + System.DateTime.Now.ToString() + " \r\n";

                                //}
                                //catch { };
                            }


                            //resetto la tabella a sigola riga
                            myAryDictSingleRow = new Dictionary<UInt32, Dictionary<UInt32, string>>();

                            //uiMaxColumn = uiMaxColumn < uiC ? uiC : uiMaxColumn;
                            uiR += 1;
                            //Console.WriteLine("");
                        }// end while
                        uiMaxRow = uiR;
                    }//end using csv file



                    myBook.SaveAs(myFnameXlsx);

                }//end of using xlsx file

            }//end of try
            catch (Exception e)
            {
                //Devi scrivere l'errore in un file di log (per gli errori) sennò nessuno lo vede!!!!
                Messaggi["Messaggio"] += " Errore conversione file csv: " + e.Message + " " + System.DateTime.Now.ToString();
                if (e.InnerException != null)
                    Messaggi["Messaggio"] += " Errore interno conversione csv : " + e.InnerException.Message.ToString() + " " + System.DateTime.Now.ToString();
                WelcomeLibrary.UF.MemoriaDisco.scriviFileLog(Messaggi, WelcomeLibrary.STATIC.Global.percorsoFisicoComune);
                throw new ApplicationException("Errore conversione da csv  :" + e.Message, e);

            }//end of catch
        }//end of Main


    }




    public static class Utility
    {
        public static string reformatdatetimestring(string inputdatetext, string inputformat = "yyyy-MM-dd HH:mm:ss", string outformat = "{0:dd/MM/yyyy}")
        {
            string ret = "";
            DateTime _dt;
            if (DateTime.TryParseExact(inputdatetext, inputformat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _dt))
                ret = string.Format(outformat, _dt);
            return ret;
        }

        public static string waitwrappercall(string functionname, string functioncall, string mseconds = "300", bool waitconsent = false)
        {
            StringBuilder sb = new StringBuilder();
            string addedcondition = "";
            if (waitconsent)
            {
                addedcondition = " &&  typeof cookieconsentinitialized !== 'undefined' && cookieconsentinitialized == true  ";
            }

            sb.Append("(function wait() {");
            if (!functionname.Contains("."))
                sb.Append("  if (typeof " + functionname + " === \"function\" " + addedcondition + ")");
            else
            {
                string functionitem1 = functionname;
                string functionitem2 = functionname;
                string[] listaarray = functionname.Trim().Split('.');
                if (listaarray != null && listaarray.Length > 1)
                {
                    functionitem1 = listaarray[0];
                    functionitem2 = listaarray[1];
                }
                sb.Append("  if ( typeof " + functionitem1 + " !== 'undefined' &&  " + functionitem1 + " != null  &&  typeof " + functionname + " === \"function\" " + addedcondition + ")");

            }
            sb.Append("    {");
            sb.Append(functioncall + ";");
            sb.Append(" }");
            sb.Append("   else  {");
            sb.Append("  setTimeout(wait, " + mseconds + ");");
            sb.Append("  }  })();");


            return sb.ToString();

        }
        public static void ViewportwManagerSet(string sessionid, string Viewportwidth)
        {
            try
            {
                if (string.IsNullOrEmpty(sessionid)) return;
                if (!WelcomeLibrary.STATIC.Global.Viewportw.ContainsKey(sessionid))
                    WelcomeLibrary.STATIC.Global.Viewportw.Add(sessionid, new System.Collections.Generic.Dictionary<string, string>());

                if (!WelcomeLibrary.STATIC.Global.Viewportw[sessionid].ContainsKey("width"))
                    WelcomeLibrary.STATIC.Global.Viewportw[sessionid].Add("width", Viewportwidth);
                else
                    WelcomeLibrary.STATIC.Global.Viewportw[sessionid]["width"] = Viewportwidth;

                if (!WelcomeLibrary.STATIC.Global.Viewportw[sessionid].ContainsKey("timeadded"))
                    WelcomeLibrary.STATIC.Global.Viewportw[sessionid].Add("timeadded", System.DateTime.Now.ToBinary().ToString());
                else
                    WelcomeLibrary.STATIC.Global.Viewportw[sessionid]["timeadded"] = System.DateTime.Now.ToBinary().ToString();


                //rimozione chiavi scadute
                List<string> keytoremove = new List<string>();
                long minuteslease = 5;
                //Svuotiamo le chiavi scadute da un certo lasso di tempo per ripulirle
                foreach (KeyValuePair<string, Dictionary<string, string>> kv in WelcomeLibrary.STATIC.Global.Viewportw)
                {
                    if (kv.Value.ContainsKey("timeadded"))
                    {
                        DateTime timestored = DateTime.FromBinary(long.Parse(kv.Value["timeadded"]));
                        TimeSpan ts = (DateTime.Now - timestored);
                        if (ts.Minutes > minuteslease) keytoremove.Add(kv.Key);
                    }
                }
                keytoremove.ForEach(k => WelcomeLibrary.STATIC.Global.Viewportw.Remove(k));
            }
            catch
            { }
        }
        public static string ViewportwManagerGet(string sessionid)
        {
            string ret = "";
            try
            {
                if (WelcomeLibrary.STATIC.Global.Viewportw.ContainsKey(sessionid))
                    if (WelcomeLibrary.STATIC.Global.Viewportw[sessionid].ContainsKey("width"))
                        ret = WelcomeLibrary.STATIC.Global.Viewportw[sessionid]["width"];
            }
            catch
            { }
            return ret;
        }

        // EDIT: one of many possible improved versions
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            // initialized to default value (such as 0 or null depending upon type of TValue)
            TValue value;

            // attempt to get the value of the key from the dictionary
            dictionary.TryGetValue(key, out value);
            return value;
        }

        public static System.Globalization.CultureInfo setCulture(string lng)
        {
            string culturename = "";
            switch (lng)
            {
                case "I":
                    culturename = "it";
                    break;
                case "GB":
                    culturename = "en";
                    break;
                case "RU":
                    culturename = "ru";
                    break;
                case "FR":
                    culturename = "fr";
                    break;
                case "DE":
                    culturename = "de";
                    break;
                case "ES":
                    culturename = "es";
                    break;
                default:
                    culturename = "it";
                    break;
            }
            System.Globalization.CultureInfo ci = new System.Globalization.CultureInfo(culturename);
            return ci;
        }




        public static string CompressString(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            var memoryStream = new MemoryStream();
            using (var gZipStream = new System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionMode.Compress, true))
            {
                gZipStream.Write(buffer, 0, buffer.Length);
            }

            memoryStream.Position = 0;

            var compressedData = new byte[memoryStream.Length];
            memoryStream.Read(compressedData, 0, compressedData.Length);

            var gZipBuffer = new byte[compressedData.Length + 4];
            Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
            return Convert.ToBase64String(gZipBuffer);
        }

        public static string DecompressString(string compressedText)
        {
            byte[] gZipBuffer = Convert.FromBase64String(compressedText);
            using (var memoryStream = new MemoryStream())
            {
                int dataLength = BitConverter.ToInt32(gZipBuffer, 0);
                memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

                var buffer = new byte[dataLength];

                memoryStream.Position = 0;
                using (var gZipStream = new System.IO.Compression.GZipStream(memoryStream, System.IO.Compression.CompressionMode.Decompress))
                {
                    gZipStream.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }
        // Compresses the files in the nominated folder, and creates a zip file on disk named as outPathname.
        //
        public static void ZipCompletefolder(string outFilePathname, string password, string sourcefolderPath)
        {

            FileStream fsOut = File.Create(outFilePathname);
            using (ZipOutputStream zipStream = new ZipOutputStream(fsOut))
            {
                zipStream.SetLevel(3); //0-9, 9 being the highest level of compression
                zipStream.Password = password;  // optional. Null is the same as not setting. Required if using AES.
                // This setting will strip the leading part of the folder path in the entries, to
                // make the entries relative to the starting folder.
                // To include the full path for each entry up to the drive root, assign folderOffset = 0.
                int folderOffset = sourcefolderPath.Length + (sourcefolderPath.EndsWith("\\") ? 0 : 1);
                if (Directory.Exists(sourcefolderPath))
                    CompressFolder(sourcefolderPath, zipStream, folderOffset);
                zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
                zipStream.Close();
            }
        }



        //https://github.com/icsharpcode/SharpZipLib/wiki/Zip-Samples
        // Recurses down the folder structure
        //
        private static void CompressFolder(string sourcefilespath, ZipOutputStream zipStream, int folderOffset)
        {

            string[] files = Directory.GetFiles(sourcefilespath);

            foreach (string filename in files)
            {

                FileInfo fi = new FileInfo(filename);

                string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                ZipEntry newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                //   newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;
                newEntry.Size = fi.Length;

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            string[] folders = Directory.GetDirectories(sourcefilespath);
            foreach (string folder in folders)
            {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }



        /// <summary>
        /// Scompatta un file Zip
        /// </summary>
        /// <param name="ZipFilePath"></param>
        /// <param name="DestinationPath"></param>
        public static void UnZip(string ZipFilePath, string DestinationPath, string filter = "")
        {
            try
            {
                string dp = (DestinationPath.EndsWith("\\")) ? DestinationPath : DestinationPath + @"\";
                if (!Directory.Exists(dp)) Directory.CreateDirectory(dp);
                if (File.Exists(ZipFilePath))
                {
                    using (ZipInputStream zip = new ZipInputStream(File.OpenRead(ZipFilePath)))
                    {
                        ZipEntry entry;
                        while ((entry = zip.GetNextEntry()) != null)
                        {
                            if (entry.IsDirectory)
                                Directory.CreateDirectory(dp + entry.Name);
                            else
                            {
                                if (filter == "" || filter.ToLower() == entry.Name.ToLower())
                                {
                                    FileStream streamWriter = File.Create(dp + entry.Name);
                                    using (streamWriter)
                                    {
                                        int size = 2048;
                                        byte[] data = new byte[2048];
                                        while (true)
                                        {
                                            size = zip.Read(data, 0, data.Length);
                                            if (size > 0)
                                                streamWriter.Write(data, 0, size);
                                            else
                                                break;
                                        }
                                        streamWriter.Close();
                                    }
                                }
                            }
                        }
                        zip.Close();
                    }
                }
            }
            catch (Exception error)
            {
                throw new Exception("Errore durante il processo UnZip, l'errore è: " + error.Message);

            }
        }



        private static TipologiaContenutiCollection _TipologieContenuti = new TipologiaContenutiCollection();
        public static TipologiaContenutiCollection TipologieContenuti
        {
            get
            {
                return _TipologieContenuti;
            }
            set { _TipologieContenuti = value; }
        }
        private static TipologiaOfferteCollection _TipologieOfferte = new TipologiaOfferteCollection();
        public static TipologiaOfferteCollection TipologieOfferte
        {
            get
            {
                return _TipologieOfferte;
            }
            set { _TipologieOfferte = value; }
        }
        private static TipologiaAnnunciCollection _TipologieAnnunci = new TipologiaAnnunciCollection();
        public static TipologiaAnnunciCollection TipologieAnnunci
        {
            get
            {
                return _TipologieAnnunci;
            }
            set { _TipologieAnnunci = value; }
        }
        private static FascediprezzoCollection _Fascediprezzo = new FascediprezzoCollection();
        public static FascediprezzoCollection Fascediprezzo
        {
            get
            {
                return _Fascediprezzo;
            }
            set { _Fascediprezzo = value; }
        }

        private static ParametroGenericoCollection _ParametroGenerico1 = new ParametroGenericoCollection();
        public static ParametroGenericoCollection ParametroGenerico1
        {
            get
            {
                return _ParametroGenerico1;
            }
            set { _ParametroGenerico1 = value; }
        }
        private static ParametroGenericoCollection _ParametroGenerico2 = new ParametroGenericoCollection();
        public static ParametroGenericoCollection ParametroGenerico2
        {
            get
            {
                return _ParametroGenerico2;
            }
            set { _ParametroGenerico2 = value; }
        }
        private static CategoriaOfferteLiv1Collection _CategoriaLiv1Offerte = new CategoriaOfferteLiv1Collection();
        public static CategoriaOfferteLiv1Collection CategoriaLiv1Offerte
        {
            get
            {
                return _CategoriaLiv1Offerte;
            }
            set { _CategoriaLiv1Offerte = value; }
        }

        private static Offerte_cat1_linkCollection _LinkOfferteCategoriaLiv1 = new Offerte_cat1_linkCollection();
        public static Offerte_cat1_linkCollection LinkOfferteCategoriaLiv1
        {
            get
            {
                return _LinkOfferteCategoriaLiv1;
            }
            set { _LinkOfferteCategoriaLiv1 = value; }
        }

        private static OfferteCollection _ListAttivita = new OfferteCollection();
        public static OfferteCollection ListAttivita
        {
            get
            {
                return _ListAttivita;
            }
            set { _ListAttivita = value; }
        }
        private static TabrifCollection _TipiClienti = new TabrifCollection();
        public static TabrifCollection TipiClienti
        {
            get
            {
                return _TipiClienti;
            }
            set { _TipiClienti = value; }
        }



        //private static Dictionary<string, TabrifCollection> _Caratteristiche;
        //public static Dictionary<string, TabrifCollection> Caratteristiche
        //{
        //    get { return Utility._Caratteristiche; }
        //    set { Utility._Caratteristiche = value; }
        //}
        private static List<TabrifCollection> _Caratteristiche = new List<TabrifCollection>();
        public static List<TabrifCollection> Caratteristiche
        {
            get { return Utility._Caratteristiche; }
            set { Utility._Caratteristiche = value; }
        }


        //private static TabrifCollection _caratteristica1 = new TabrifCollection();
        //public static TabrifCollection Caratteristica1
        //{
        //    get
        //    {
        //        return _caratteristica1;
        //    }
        //    set { _caratteristica1 = value; }
        //}
        //private static TabrifCollection _caratteristica2 = new TabrifCollection();
        //public static TabrifCollection Caratteristica2
        //{
        //    get
        //    {
        //        return _caratteristica2;
        //    }
        //    set { _caratteristica2 = value; }
        //}

        private static TabrifCollection _Nazioni = new TabrifCollection();
        public static TabrifCollection Nazioni
        {
            get
            {
                return _Nazioni;
            }
            set { _Nazioni = value; }
        }
        private static ProvinceCollection _ElencoProvinceCompleto = new ProvinceCollection();
        public static ProvinceCollection ElencoProvinceCompleto
        {
            get
            {
                return _ElencoProvinceCompleto;
            }
            set { _ElencoProvinceCompleto = value; }
        }
        private static ProvinceCollection _ElencoProvince = new ProvinceCollection();
        public static ProvinceCollection ElencoProvince
        {
            get
            {
                return _ElencoProvince;
            }
            set { _ElencoProvince = value; }
        }
        private static ComuneCollection _ElencoComuni = new ComuneCollection();
        public static ComuneCollection ElencoComuni
        {
            get
            {
                return _ElencoComuni;
            }
            set { _ElencoComuni = value; }
        }

        private static ProdottoCollection _ElencoProdotti = new ProdottoCollection();
        public static ProdottoCollection ElencoProdotti
        {
            get
            {
                return _ElencoProdotti;
            }
            set { _ElencoProdotti = value; }
        }

        private static SProdottoCollection _ElencoSottoProdotti = new SProdottoCollection();
        public static SProdottoCollection ElencoSottoProdotti
        {
            get
            {
                return _ElencoSottoProdotti;
            }
            set { _ElencoSottoProdotti = value; }
        }
        /// <summary>
        /// Converte un testo html in un testo stringa plain text
        /// </summary>
        /// <param name="htmlstring"></param>
        /// <returns></returns>
        public static string ConvertHtmlToPlainText(string htmlstring)
        {
            string plaintext = "";
            Regex RegEx = new Regex("<[^>]*>", RegexOptions.Multiline | RegexOptions.IgnoreCase);
            htmlstring = htmlstring.Replace("<br/>", "\r\n");
            plaintext = RegEx.Replace(htmlstring, "");
            return plaintext;
        }


#if fase
        public static Dictionary<string, string> ReadXmlAttributesOfElement(ref Exception errorret, string element = "mailSettings", string elementliv1 = "smtpbase", string elementliv2 = "network", string elementliv3 = "")
      {
         Dictionary<string, string> values = new Dictionary<string, string>();
         try
         {
            string webConfigFile = System.IO.Path.Combine(WelcomeLibrary.STATIC.Global.percorsofisicoapplicazione, "web.config");
            System.Xml.XmlTextReader webConfigReader = new System.Xml.XmlTextReader(new System.IO.StreamReader(webConfigFile));
            webConfigReader.WhitespaceHandling = WhitespaceHandling.None;
            //if ((webConfigReader.ReadToFollowing("configuration/mailSettings/" + "mailingbase" + "/network")))
            //    Label1.Text += webConfigReader.GetAttribute("host");

            while (webConfigReader.ReadToFollowing(element))
            {
               //if (webConfigReader.Depth == 1) //Carico solo gli elementi di primo livello
               if (elementliv1 != "")
                  webConfigReader.ReadToDescendant(elementliv1);

               if (elementliv1 != "" && elementliv2 != "")
                  webConfigReader.ReadToDescendant(elementliv2);

               if (elementliv1 != "" && elementliv2 != "" && elementliv3 != "")
                  webConfigReader.ReadToDescendant(elementliv3);
               //string host = webConfigReader.GetAttribute("host");
               //string username = webConfigReader.GetAttribute("userName");
               //string password = webConfigReader.GetAttribute("password");
               //string attributes = "";

               //ALTRA LETTURA ATTRIBUTI
               if (webConfigReader.HasAttributes)
               {
                  for (int i = 0; i < webConfigReader.AttributeCount; i++)
                  {
                     webConfigReader.MoveToAttribute(i);
                     string n = webConfigReader.Name;
                     string v = webConfigReader.Value;
                     if (!values.ContainsKey(n))
                     {
                        values.Add(n, v);
                        //  attributes += ("Nam: " + webConfigReader.Name + ", Value: " + webConfigReader.Value);
                     }
                  }
               }

               //PER GLI ATTRIBUTI  DEL NODO LI LEGGO TUTTI E LI CONCATENO
               //int numAttributes = webConfigReader.AttributeCount;
               //for (int i = 0; i < numAttributes; i++)
               //{
               //    attributes += (webConfigReader.GetAttribute(i)) + "|";

               //}
               //attributes = attributes.TrimEnd('|');
               string valore = webConfigReader.ReadString();

               webConfigReader.Close();
            }
         }
         catch (Exception error)
         {
            errorret = error;
         }
         return values;
      }
      
#endif

        public static bool invioMailGenerico(string mittenteNome, string mittenteMail, string SoggettoMail, string Descrizione,
         string destinatarioMail1, string destinatarioNome, List<string> foto = null, string percorsofoto = "", bool incorporaimmagini = false, System.Web.HttpServerUtility server = null, bool plaintext = false, List<string> emaildestbcc = null, string sectionName = "smtpbase", string libtype = "mimekit", string replytoemail = "", string replytoname = "")
        {
            bool ret = false;
            try
            {
                switch (libtype)
                {
                    case "systemweb":
                        ret = invioMailGenericosystemweb(mittenteNome, mittenteMail, SoggettoMail, Descrizione,
                  destinatarioMail1, destinatarioNome, foto, percorsofoto, incorporaimmagini, server, plaintext, emaildestbcc, sectionName);
                        break;

                    case "systemnet":
                        ret = invioMailGenericosystemnet(mittenteNome, mittenteMail, SoggettoMail, Descrizione,
                  destinatarioMail1, destinatarioNome, foto, percorsofoto, incorporaimmagini, server, plaintext, emaildestbcc, sectionName);
                        break;

                    case "mailsystem":
                        ret = invioMailGenericomailsystem(mittenteNome, mittenteMail, SoggettoMail, Descrizione,
                 destinatarioMail1, destinatarioNome, foto, percorsofoto, incorporaimmagini, server, plaintext, emaildestbcc, sectionName);
                        break;
                    case "mimekit":
                        ret = invioMailGenericomimekit(mittenteNome, mittenteMail, SoggettoMail, Descrizione,
                 destinatarioMail1, destinatarioNome, foto, percorsofoto, incorporaimmagini, server, plaintext, emaildestbcc, sectionName);
                        break;
                    default:
                        ret = invioMailGenericosystemnet(mittenteNome, mittenteMail, SoggettoMail, Descrizione,
                  destinatarioMail1, destinatarioNome, foto, percorsofoto, incorporaimmagini, server, plaintext, emaildestbcc, sectionName);
                        break;
                }
            }
            catch (Exception error)
            {
                throw new ApplicationException(error.Message);
            }
            return ret;
        }

        /// <summary>
        /// Implic metho con depracated system.web
        /// </summary>
        /// <param name="mittenteNome"></param>
        /// <param name="mittenteMail"></param>
        /// <param name="SoggettoMail"></param>
        /// <param name="Descrizione"></param>
        /// <param name="destinatarioMail1"></param>
        /// <param name="destinatarioNome"></param>
        /// <param name="foto"></param>
        /// <param name="percorsofoto"></param>
        /// <param name="incorporaimmagini"></param>
        /// <param name="server"></param>
        /// <param name="plaintext"></param>
        /// <param name="emaildestbcc"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static bool invioMailGenericosystemweb(string mittenteNome, string mittenteMail, string SoggettoMail, string Descrizione,
          string destinatarioMail1, string destinatarioNome, List<string> foto = null, string percorsofoto = "", bool incorporaimmagini = false, System.Web.HttpServerUtility server = null, bool plaintext = false, List<string> emaildestbcc = null, string sectionName = "smtpbase")
        {
            bool ret = false;
            System.Web.Mail.MailMessage objMail = new System.Web.Mail.MailMessage();

            try
            {
                //MailAddress mitt = new MailAddress(mittenteMail, mittenteNome);
                objMail.From = mittenteMail;
                objMail.Headers.Add("Message-ID", "<" + Guid.NewGuid().ToString() + "." + mittenteMail + ">"); //header messaggio
                if (destinatarioMail1 != "")
                {
                    //MailAddress dest = new MailAddress(destinatarioMail1, destinatarioNome);
                    objMail.To = (destinatarioMail1);
                }
                if (emaildestbcc != null)
                {
                    string List = "";
                    foreach (string s in emaildestbcc)
                    {
                        //MailAddress dest = new MailAddress(s, "");
                        List += s + ";";
                    }
                    List = List.TrimEnd(';');
                    objMail.Bcc = (List);
                }
                //INSERIAMO GLI ATTACHMENT
                try
                {
                    System.Web.Mail.MailAttachment Att;
                    if (foto != null)
                        foreach (string attach in foto)
                        {
                            Att = new System.Web.Mail.MailAttachment(percorsofoto + attach);
                            objMail.Attachments.Add(Att);
                        }
                    Att = null;
                }
                catch
                { }

                //Corpo e soggetto del messaggio 
                string soggetto = SoggettoMail.Replace("\r\n", "");
                soggetto = soggetto.Replace("\r", "");
                soggetto = soggetto.Replace("\n", "");
                objMail.Subject = soggetto;

                //METODO INCLUSIONE RISORSE INTEGRATE -------------------------------------------------------------



                List<LinkedResource> listaimmagini = new List<LinkedResource>();
                List<string> listapathfileimmagini = new List<string>();
                //LinkedResource img2 = null;
                //string pathfisicoimg2 = "";
                //if (!string.IsNullOrEmpty(pathfisicoimg2))
                //{
                //    StringBuilder sb1 = new StringBuilder("<img border=\"none\" src=\"cid:IMG2\" alt=\"\" />");
                //    img2 = new LinkedResource(pathfisicoimg2);
                //    img2.ContentId = "IMG2";
                //    img2.ContentType = new System.Net.Mime.ContentType("image/jpeg");
                //    listaimmagini.Add(img2);
                //}
                int imgidx = 1;
                int posimmagine = 0;
                if (incorporaimmagini && server != null)
                {
                    posimmagine = Descrizione.ToLower().IndexOf("src=\"", posimmagine);
                    while (posimmagine != -1)
                    {
                        int endposimmagine = -1;
                        string stringtoreplace = "";
                        if (posimmagine + 5 < Descrizione.Length)
                            endposimmagine = Descrizione.ToLower().IndexOf("\"", posimmagine + 5);
                        if (posimmagine != -1 && endposimmagine != -1 && endposimmagine > posimmagine)
                        {
                            stringtoreplace = Descrizione.Substring(posimmagine, endposimmagine - posimmagine + 1);
                            string linkimmagine = stringtoreplace.Substring(5, stringtoreplace.Length - 6);
                            if (!string.IsNullOrEmpty(linkimmagine))
                            {
                                try
                                {

#if false
                                    string percorsoassoluto = linkimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                                    string percorsovirtuale = linkimmagine.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "~");
                                    string percorsofisico = server.MapPath(percorsovirtuale);
                                    listapathfileimmagini.Add(percorsofisico);
                                    string replacetext = "src=\"" + percorsoassoluto + "\"";
                                    Descrizione = Descrizione.Replace(stringtoreplace, replacetext);

#endif
#if true   //VErsione per risorse
                                    string percorsoassoluto = linkimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                                    string percorsovirtuale = linkimmagine.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "~");
                                    string percorsofisico = server.MapPath(percorsovirtuale);

                                    listapathfileimmagini.Add(percorsofisico);
                                    if (System.IO.File.Exists(percorsofisico))
                                    {
                                        string nomefile = percorsovirtuale.Substring(percorsovirtuale.LastIndexOf("/") + 1);
                                        string replacetext = "src=\"cid:" + nomefile + "\"";

                                        // LinkedResource img = new LinkedResource(percorsofisico);
                                        // img.ContentId = "IMG" + imgidx.ToString();
                                        //img.ContentType = new System.Net.Mime.ContentType("image/jpeg");
                                        // listaimmagini.Add(img);

                                        Descrizione = Descrizione.Replace(stringtoreplace, replacetext);
                                        imgidx += 1;//incremento l'indice immagini
                                    }

#endif

                                }
                                catch { }
                            }
                        }
                        posimmagine = posimmagine + 5;
                        if (posimmagine + 5 < Descrizione.Length)
                            posimmagine = Descrizione.ToLower().IndexOf("src=\"", posimmagine);
                        else posimmagine = -1;
                    }
                }


                //INSERIAMO LE IMMAGINI COME  ATTACHMENT
                try
                {
                    System.Web.Mail.MailAttachment Att;

                    foreach (string attach in listapathfileimmagini)
                    {
                        Att = new System.Web.Mail.MailAttachment(attach, System.Web.Mail.MailEncoding.Base64);
                        objMail.Attachments.Add(Att);
                    }
                    Att = null;
                }
                catch
                { }

                //METODO INCLUSIONE RISORSE INTEGRATE ---------------------------------------------------------------
                objMail.BodyFormat = System.Web.Mail.MailFormat.Html;
                objMail.BodyEncoding = System.Text.Encoding.UTF8; ;


                //objMail.BodyEncoding.IsBrowserDisplay = true;
                //   objMail.Priority = System.Web.Mail.MailPriority.High;

                //Tolgo interrruzioni di linea non html
                Descrizione = Descrizione.Replace("\r\n", "");
                Descrizione = Descrizione.Replace("\r", "");
                Descrizione = Descrizione.Replace("\n", "");
                objMail.Body = Descrizione;


                //SISTEMA CREAZIONE MAIL MULTIVIEW PER FILTRO ANTISPAM E LINKEDRESOURCES
#if false
                if (!plaintext)
                {
                    objMail.IsBodyHtml = true;
                    objMail.SubjectEncoding = System.Text.Encoding.UTF8;
                    objMail.BodyEncoding = System.Text.Encoding.UTF8;
                    //objMail.BodyEncoding.IsBrowserDisplay = true;

                    //Tolgo interrruzioni di linea non html
                    Descrizione = Descrizione.Replace("\r\n", "");
                    Descrizione = Descrizione.Replace("\r", "");
                    Descrizione = Descrizione.Replace("\n", "");

                    //CREAZIONE DELLE VIEW DELLA MAIL
                    AlternateView htmlView;
                    htmlView = AlternateView.CreateAlternateViewFromString(
    Descrizione, Encoding.UTF8, "text/html");//.Replace("\r\n", "<br/>")

                    //htmlView.ContentType = new System.Net.Mime.ContentType("text/html"); //Encoding.UTF8,
                    //htmlView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                    //if (img1 != null)
                    //    htmlView.LinkedResources.Add(img1);
                    //if (img2 != null)
                    //    htmlView.LinkedResources.Add(img2);
                    if (listaimmagini != null && listaimmagini.Count > 0)
                        foreach (LinkedResource img in listaimmagini)
                        {
                            htmlView.LinkedResources.Add(img);
                        }
                    objMail.AlternateViews.Add(htmlView);

                }
                else //Versione solo Text!!!
                {
                    objMail.IsBodyHtml = false;
                    objMail.SubjectEncoding = System.Text.Encoding.UTF8;
                    objMail.BodyEncoding = System.Text.Encoding.UTF8;

                    //VERSIONE SENZA VIEW SOLO HTML
                    //objMail.Body = Descrizione;
                    //objMail.Body = objMail.Body.Replace("\r\n", "<br/>");
                    //-------------------------------------------------------------------------------------------
                    //Aggiungo una vista solo testo al messaggio ( per migliorare lo spam rating della mail )
                    //-------------------------------------------------------------------------------------------
                    string simpletext = Descrizione;

                    //Converto a plain text tutto!!!
                    //HtmlToText html = new HtmlToText();   //;
                    //string simpletext = html.Convert(Descrizione);

                    //  string simpletext = ConvertHtmlToPlainText(Descrizione);

                    AlternateView textView =
              System.Net.Mail.AlternateView.CreateAlternateViewFromString(
              simpletext);
                    textView.ContentType = new System.Net.Mime.ContentType("text/plain");
                    textView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;
                    objMail.AlternateViews.Add(textView);
                    //-------------------------------------------------------------------------------------------

                } 
#endif
                //CORREGGE LA STINGA METTENDO LE INIZIALI MAIUSCOLE
                //string myString = "disposition-Notification-To";
                //TextInfo TI = new CultureInfo("it-IT", false).TextInfo;
                //myString = (TI.ToTitleCase(myString));
                //QUESTO AGGIUNGE LA NOTIFICA DI RICEZIONE DEL MESSAGGIO
                //objMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                //objMail.Headers.Add("Disposition-Notification-To", "\"" + mittenteNome + "\"" + " <" + mittenteMail + ">");
                //cli.EnableSsl = true;

                //SEZIONE AGGIUNTA PER SERVER SMTP ALTERNATIVO
                //Dictionary<string, string> attrvalues1 = WelcomeLibrary.UF.Utility.ReadXmlAttributesOfElement(ref errret, "system.net", "mailSettings", "smtp", "network"); //Stringa dedicata
                //Dictionary<string, string> attrvalues2 = WelcomeLibrary.UF.Utility.ReadXmlAttributesOfElement(ref errret, "mailSettings", "mailing", "network"); //Stringa standard
                //Dictionary<string, string> attrvalues3 = WelcomeLibrary.UF.Utility.ReadXmlAttributesOfElement(ref errret, "mailSettings", "smtpbase", "network"); //Stringa standard

                Exception errret = null;
                //Dictionary<string, string> mailvalues = WelcomeLibrary.UF.Utility.ReadXmlAttributesOfElement(ref errret, "mailSettings", sectionName, "network"); //Stringa da web config
                // 2016 06 26 si prende da TBL_Config
                Dictionary<string, string> mailvalues = WelcomeLibrary.UF.ConfigManagement.ReadSection(ref errret, sectionName); //Stringa da web config                
                if (mailvalues.ContainsKey("host"))
                    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", mailvalues["host"]);
                if (mailvalues.ContainsKey("port"))
                    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", mailvalues["port"]);
                if (mailvalues.ContainsKey("enablessl") && mailvalues["enablessl"] == "true")
                    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
                objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
                objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                if (mailvalues.ContainsKey("username"))
                    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", mailvalues["username"]);
                if (mailvalues.ContainsKey("password"))
                    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", mailvalues["password"]);

#if false
                if (sectionName != "")
                {
                    SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("mailSettings/" + sectionName);
                    if (section != null)
                    {
                        if (section.Network != null)
                        {
                            objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", section.Network.Host);
                            //objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", section.Network.Port);
                            objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", "465");
                            objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
                            objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                            objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", section.Network.UserName);
                            objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", section.Network.Password);
                            //if (section.Network.EnableSsl)
                            objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");

                            //if (section.Network.TargetName != null)
                            //    cli.TargetName = section.Network.TargetName;
                        }

                        //cli.DeliveryMethod = section.DeliveryMethod;
                        //if (section.SpecifiedPickupDirectory != null && section.SpecifiedPickupDirectory.PickupDirectoryLocation != null)
                        //cli.PickupDirectoryLocation = section.SpecifiedPickupDirectory.PickupDirectoryLocation;
                    } 
                }
                //else
                //{

                //    SmtpSection sectionbase = (SmtpSection)ConfigurationManager.GetSection("mailSettings/" + sectionName);
                //    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", sectionbase.Network.Host);
                //    //objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", sectionbase.Network.Port);
                //    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", "465");
                //    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
                //    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                //    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", sectionbase.Network.UserName);
                //    objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", sectionbase.Network.Password);
                //    if (sectionbase.Network.EnableSsl)
                //        objMail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
                //}
                //------------------------------

#endif

                int ntentativi = 3;
                int i = 0;
                while (i < ntentativi)
                {
                    //inviamo il mail facendo un certo num di tentativi
                    try
                    {
                        System.Threading.Thread.Sleep(100); //DA TESTARE TOGLIENDOLO
                        System.Web.Mail.SmtpMail.Send(objMail);
                        System.Threading.Thread.Sleep(100); //DA TESTARE TOGLIENDOLO
                        i = ntentativi;
                        ret = true;
                    }
                    catch (Exception errTentativi)
                    {
                        i += 1;
                        if (i == ntentativi) throw new Exception(errTentativi.Message);
                    }
                }

            }
            catch (Exception error)
            {
                string text = "&nbsp <br/> Errore Invio Mail a " + destinatarioNome + ". " + error.Message;
                if (error.InnerException != null)
                    text += error.InnerException.Message;
                throw new ApplicationException(text);
            }

            return ret;
        }

        /// <summary>
        /// Explici methond non permesso aruba
        /// </summary>
        /// <param name="mittenteNome"></param>
        /// <param name="mittenteMail"></param>
        /// <param name="SoggettoMail"></param>
        /// <param name="Descrizione"></param>
        /// <param name="destinatarioMail1"></param>
        /// <param name="destinatarioNome"></param>
        /// <param name="foto"></param>
        /// <param name="percorsofoto"></param>
        /// <param name="incorporaimmagini"></param>
        /// <param name="server"></param>
        /// <param name="plaintext"></param>
        /// <param name="emaildestbcc"></param>
        /// <param name="sectionName"></param>
        /// <returns></returns>
        public static bool invioMailGenericosystemnet(string mittenteNome, string mittenteMail, string SoggettoMail, string Descrizione,
            string destinatarioMail1, string destinatarioNome, List<string> foto = null, string percorsofoto = "", bool incorporaimmagini = false, System.Web.HttpServerUtility server = null, bool plaintext = false, List<string> emaildestbcc = null, string sectionName = "")
        {
            bool ret = false;
            MailMessage objMail = new MailMessage();
            using (objMail)
            {
                System.Net.Mail.SmtpClient cli = new System.Net.Mail.SmtpClient();
                try
                {

                    MailAddress mitt = new MailAddress(mittenteMail, mittenteNome);
                    objMail.From = mitt;
                    //objMail.ReplyTo = mitt;
                    //MailAddress toforantispam = new MailAddress("address", "nome"); //Aggiunto per filtro antispam per evitare 1.0 MISSING_HEADERS  Missing To: header
                    //objMail.To.Add(toforantispam);
                    objMail.Sender = mitt;
                    objMail.Headers.Add("Message-ID", "<" + Guid.NewGuid().ToString() + "." + mittenteMail + ">"); //header messaggio

                    if (destinatarioMail1 != "")
                    {
                        MailAddress dest = new MailAddress(destinatarioMail1, destinatarioNome);
                        objMail.To.Add(dest);

                    }

                    if (emaildestbcc != null)
                    {
                        foreach (string s in emaildestbcc)
                        {
                            MailAddress dest = new MailAddress(s, "");
                            objMail.Bcc.Add(dest);

                        }
                    }

                    //INSERIAMO GLI ATTACHMENT
                    try
                    {
                        Attachment Att;
                        if (foto != null)
                            foreach (string attach in foto)
                            {
                                Att = new Attachment(percorsofoto + attach);
                                objMail.Attachments.Add(Att);
                            }
                        Att = null;
                    }
                    catch
                    { }

                    //Corpo e soggetto del messaggio 
                    string soggetto = SoggettoMail.Replace("\r\n", "");
                    soggetto = soggetto.Replace("\r", "");
                    soggetto = soggetto.Replace("\n", "");
                    objMail.Subject = soggetto;

                    //METODO INCLUSIONE RISORSE INTEGRATE ---------------------------------------------------------------
                    List<LinkedResource> listaimmagini = new List<LinkedResource>();
                    //LinkedResource img1 = null;
                    LinkedResource img2 = null;
                    string pathfisicoimg2 = "";
                    if (!string.IsNullOrEmpty(pathfisicoimg2))
                    {
                        StringBuilder sb1 = new StringBuilder("<img border=\"none\" src=\"cid:IMG2\" alt=\"\" />");
                        img2 = new LinkedResource(pathfisicoimg2);
                        img2.ContentId = "IMG2";
                        img2.ContentType = new System.Net.Mime.ContentType("image/jpeg");
                        listaimmagini.Add(img2);
                    }
                    int imgidx = 1;
                    int posimmagine = 0;
                    if (incorporaimmagini && server != null)
                    {
                        posimmagine = Descrizione.ToLower().IndexOf("src=\"", posimmagine);
                        while (posimmagine != -1)
                        {
                            int endposimmagine = -1;
                            string stringtoreplace = "";
                            if (posimmagine + 5 < Descrizione.Length)
                                endposimmagine = Descrizione.ToLower().IndexOf("\"", posimmagine + 5);
                            if (posimmagine != -1 && endposimmagine != -1 && endposimmagine > posimmagine)
                            {
                                stringtoreplace = Descrizione.Substring(posimmagine, endposimmagine - posimmagine + 1);
                                string linkimmagine = stringtoreplace.Substring(5, stringtoreplace.Length - 6);
                                if (!string.IsNullOrEmpty(linkimmagine))
                                {
                                    try
                                    {
                                        string percorsovirtuale = linkimmagine.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "~");
                                        string percorsofisico = server.MapPath(percorsovirtuale);
                                        if (System.IO.File.Exists(percorsofisico))
                                        {
                                            string replacetext = "src=\"cid:IMG" + imgidx.ToString() + "\"";
                                            LinkedResource img = new LinkedResource(percorsofisico);
                                            img.ContentId = "IMG" + imgidx.ToString();
                                            img.ContentType = new System.Net.Mime.ContentType("image/jpeg");
                                            listaimmagini.Add(img);
                                            Descrizione = Descrizione.Replace(stringtoreplace, replacetext);
                                            imgidx += 1;//incremento l'indice immagini
                                        }
                                    }
                                    catch { }
                                }
                            }
                            posimmagine = posimmagine + 5;
                            if (posimmagine + 5 < Descrizione.Length)
                                posimmagine = Descrizione.ToLower().IndexOf("src=\"", posimmagine);
                            else posimmagine = -1;
                        }
                    }
                    //METODO INCLUSIONE RISORSE INTEGRATE ---------------------------------------------------------------

                    //SISTEMA CREAZIONE MAIL MULTIVIEW PER FILTRO ANTISPAM

                    if (!plaintext)
                    {
                        objMail.IsBodyHtml = true;
                        objMail.SubjectEncoding = System.Text.Encoding.UTF8;
                        objMail.BodyEncoding = System.Text.Encoding.UTF8;
                        //objMail.BodyEncoding.IsBrowserDisplay = true;

                        //Tolgo interrruzioni di linea non html
                        Descrizione = Descrizione.Replace("\r\n", "");
                        Descrizione = Descrizione.Replace("\r", "");
                        Descrizione = Descrizione.Replace("\n", "");

                        //CREAZIONE DELLE VIEW DELLA MAIL
                        AlternateView htmlView;
                        htmlView = AlternateView.CreateAlternateViewFromString(
                           Descrizione, Encoding.UTF8, "text/html");//.Replace("\r\n", "<br/>")

                        //htmlView.ContentType = new System.Net.Mime.ContentType("text/html"); //Encoding.UTF8,
                        //htmlView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;

                        //if (img1 != null)
                        //    htmlView.LinkedResources.Add(img1);
                        //if (img2 != null)
                        //    htmlView.LinkedResources.Add(img2);
                        if (listaimmagini != null && listaimmagini.Count > 0)
                            foreach (LinkedResource img in listaimmagini)
                            {
                                htmlView.LinkedResources.Add(img);
                            }
                        objMail.AlternateViews.Add(htmlView);
                    }
                    else //Versione solo Text!!!
                    {
                        objMail.IsBodyHtml = false;
                        objMail.SubjectEncoding = System.Text.Encoding.UTF8;
                        objMail.BodyEncoding = System.Text.Encoding.UTF8;

                        //VERSIONE SENZA VIEW SOLO HTML
                        //objMail.Body = Descrizione;
                        //objMail.Body = objMail.Body.Replace("\r\n", "<br/>");
                        //-------------------------------------------------------------------------------------------
                        //Aggiungo una vista solo testo al messaggio ( per migliorare lo spam rating della mail )
                        //-------------------------------------------------------------------------------------------
                        string simpletext = Descrizione;

                        //Converto a plain text tutto!!!
                        //HtmlToText html = new HtmlToText();   //;
                        //string simpletext = html.Convert(Descrizione);

                        //  string simpletext = ConvertHtmlToPlainText(Descrizione);

                        AlternateView textView =
                           System.Net.Mail.AlternateView.CreateAlternateViewFromString(
                           simpletext);
                        textView.ContentType = new System.Net.Mime.ContentType("text/plain");
                        textView.TransferEncoding = System.Net.Mime.TransferEncoding.SevenBit;
                        objMail.AlternateViews.Add(textView);
                        //-------------------------------------------------------------------------------------------

                    }


                    //CORREGGE LA STINGA METTENDO LE INIZIALI MAIUSCOLE
                    //string myString = "disposition-Notification-To";
                    //TextInfo TI = new CultureInfo("it-IT", false).TextInfo;
                    //myString = (TI.ToTitleCase(myString));
                    //QUESTO AGGIUNGE LA NOTIFICA DI RICEZIONE DEL MESSAGGIO
                    //objMail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
                    //objMail.Headers.Add("Disposition-Notification-To", "\"" + mittenteNome + "\"" + " <" + mittenteMail + ">");
                    //cli.EnableSsl = true;


                    //SEZIONE AGGIUNTA PER SERVER SMTP ALTERNATIVO
                    if (sectionName != "")
                    {
                        Exception errret = null;
                        //Dictionary<string, string> mailvalues = WelcomeLibrary.UF.Utility.ReadXmlAttributesOfElement(ref errret, "mailSettings", sectionName, "network"); //Stringa da web config
                        // 2016 06 26 si prende da TBL_Config
                        Dictionary<string, string> mailvalues = WelcomeLibrary.UF.ConfigManagement.ReadSection(ref errret, sectionName); //Stringa da web config

                        //    SmtpSection section = (SmtpSection)ConfigurationManager.GetSection("mailSettings/" + sectionName);//Protetto in medium trust
                        if (mailvalues != null)
                        {

                            cli.Host = mailvalues["host"];
                            int port = 25;
                            int.TryParse(mailvalues["port"], out port);
                            cli.Port = port;
                            //mailvalues["defaultCredentials"];
                            //  cli.UseDefaultCredentials = section.Network.DefaultCredentials;
                            cli.Credentials = new System.Net.NetworkCredential(mailvalues["username"], mailvalues["password"]);
                            if (mailvalues.ContainsKey("enablessl") && mailvalues["enablessl"] == "true")
                                cli.EnableSsl = true;
                            //if (section.Network.TargetName != null)
                            //    cli.TargetName = section.Network.TargetName;

                            //cli.DeliveryMethod = section.DeliveryMethod;
                            //if (section.SpecifiedPickupDirectory != null && section.SpecifiedPickupDirectory.PickupDirectoryLocation != null)
                            //    cli.PickupDirectoryLocation = section.SpecifiedPickupDirectory.PickupDirectoryLocation;
                        }
                    }
                    //------------------------------

                    int ntentativi = 3;
                    int i = 0;
                    while (i < ntentativi)
                    {
                        //inviamo il mail facendo un certo num di tentativi
                        try
                        {
                            System.Threading.Thread.Sleep(100); //DA TESTARE TOGLIENDOLO
                            cli.Send(objMail);
                            System.Threading.Thread.Sleep(100); //DA TESTARE TOGLIENDOLO
                            i = ntentativi;
                            ret = true;
                        }
                        catch (Exception errTentativi)
                        {
                            i += 1;
                            if (i == ntentativi) throw new Exception(errTentativi.Message);
                        }
                    }
                    cli = null;

                }
                catch (Exception error)
                {
                    throw new ApplicationException("&nbsp <br/> Errore Invio Mail a " + destinatarioNome + ". " + error.Message);
                }
            }
            return ret;
        }

        public static bool invioMailGenericomailsystem(string mittenteNome, string mittenteMail, string SoggettoMail, string Descrizione,
       string destinatarioMail1, string destinatarioNome, List<string> foto = null, string percorsofoto = "", bool incorporaimmagini = false, System.Web.HttpServerUtility server = null, bool plaintext = false, List<string> emaildestbcc = null, string sectionName = "smtpbase")
        {
            bool ret = false;
            ActiveUp.Net.Mail.Message mailMessage = new ActiveUp.Net.Mail.Message();
            try
            {

                //MimeMailAddress mittente = new MimeMailAddress(mittenteMail); // se metti il nome aegis non funziona
                //MimeMailAddress destinatario = new MimeMailAddress(destinatarioMail1); // se metti il nome aegis non funziona

                mailMessage.From.Email = mittenteMail;
                mailMessage.From.Name = mittenteNome;
                //Set del reply to in base al mittente passato se questo non è quello del sito necessario per non fare relaying
                if (mittenteMail.ToLower().Trim() != ConfigManagement.ReadKey("Email").ToLower().Trim())
                {
                    mailMessage.From.Email = ConfigManagement.ReadKey("Email");
                    mailMessage.From.Name = ConfigManagement.ReadKey("Nome"); ;
                    mailMessage.ReplyTo.Email = mittenteMail;
                    mailMessage.ReplyTo.Name = mittenteNome;
                }
                //if (!string.IsNullOrEmpty(replytoemail))
                //mailMessage.ReplyTo.Email = replytoemail;//indirizzo per risposta
                //if (!string.IsNullOrEmpty(replytoname))
                //    mailMessage.ReplyTo.Name = replytoname;//nome per risposta

                mailMessage.AddHeaderField("Message-ID", "<" + Guid.NewGuid().ToString() + "." + mittenteMail + ">"); //header messaggio

                if (destinatarioMail1 != "")
                {
                    //MailAddress dest = new MailAddress(destinatarioMail1, destinatarioNome);
                    mailMessage.To.Add(destinatarioMail1, destinatarioNome);
                }
                if (emaildestbcc != null)
                {
                    foreach (string s in emaildestbcc)
                    {
                        mailMessage.Bcc.Add(s, "");

                    }
                }
                //INSERIAMO GLI ATTACHMENT

                try
                {
                    //System.Web.Mail.MailAttachment Att;

                    if (foto != null)
                        foreach (string attach in foto)
                        {
                            mailMessage.Attachments.Add(percorsofoto + attach, false);
                        }
                }
                catch
                { }
                //Corpo e soggetto del messaggio 
                string soggetto = SoggettoMail.Replace("\r\n", "");
                soggetto = soggetto.Replace("\r", "");
                soggetto = soggetto.Replace("\n", "");
                mailMessage.Subject = soggetto;
                //METODO INCLUSIONE RISORSE INTEGRATE -------------------------------------------------------------


                List<LinkedResource> listaimmagini = new List<LinkedResource>();
                List<string> listapathfileimmagini = new List<string>();
                //LinkedResource img2 = null;
                //string pathfisicoimg2 = "";
                //if (!string.IsNullOrEmpty(pathfisicoimg2))
                //{
                //    StringBuilder sb1 = new StringBuilder("<img border=\"none\" src=\"cid:IMG2\" alt=\"\" />");
                //    img2 = new LinkedResource(pathfisicoimg2);
                //    img2.ContentId = "IMG2";
                //    img2.ContentType = new System.Net.Mime.ContentType("image/jpeg");
                //    listaimmagini.Add(img2);
                //}
                int imgidx = 1;
                int posimmagine = 0;
                if (incorporaimmagini && server != null)
                {
                    posimmagine = Descrizione.ToLower().IndexOf("src=\"", posimmagine);
                    while (posimmagine != -1)
                    {
                        int endposimmagine = -1;
                        string stringtoreplace = "";
                        if (posimmagine + 5 < Descrizione.Length)
                            endposimmagine = Descrizione.ToLower().IndexOf("\"", posimmagine + 5);
                        if (posimmagine != -1 && endposimmagine != -1 && endposimmagine > posimmagine)
                        {
                            stringtoreplace = Descrizione.Substring(posimmagine, endposimmagine - posimmagine + 1);
                            string linkimmagine = stringtoreplace.Substring(5, stringtoreplace.Length - 6);
                            if (!string.IsNullOrEmpty(linkimmagine))
                            {
                                try
                                {

#if true   //VErsione per risorse
                                    string percorsoassoluto = linkimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                                    string percorsovirtuale = linkimmagine.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "~");
                                    string percorsofisico = server.MapPath(percorsovirtuale);

                                    listapathfileimmagini.Add(percorsofisico);
                                    if (System.IO.File.Exists(percorsofisico))
                                    {
                                        string nomefile = percorsovirtuale.Substring(percorsovirtuale.LastIndexOf("/") + 1);
                                        string replacetext = "src=\"cid:" + nomefile + "\"";

                                        // LinkedResource img = new LinkedResource(percorsofisico);
                                        // img.ContentId = "IMG" + imgidx.ToString();
                                        //img.ContentType = new System.Net.Mime.ContentType("image/jpeg");
                                        // listaimmagini.Add(img);

                                        Descrizione = Descrizione.Replace(stringtoreplace, replacetext);
                                        imgidx += 1;//incremento l'indice immagini
                                    }

#endif

                                }
                                catch { }
                            }
                        }
                        posimmagine = posimmagine + 5;
                        if (posimmagine + 5 < Descrizione.Length)
                            posimmagine = Descrizione.ToLower().IndexOf("src=\"", posimmagine);
                        else posimmagine = -1;
                    }
                }


                //INSERIAMO LE IMMAGINI COME  ATTACHMENT
                try
                {

                    if (foto != null)
                        foreach (string attach in listapathfileimmagini)
                        {
                            mailMessage.Attachments.Add(percorsofoto + attach, false);
                        }
                    //System.Web.Mail.MailAttachment Att;

                    //foreach (string attach in listapathfileimmagini)
                    //{
                    //    Att = new System.Web.Mail.MailAttachment(attach, System.Web.Mail.MailEncoding.Base64);
                    //    objMail.Attachments.Add(Att);
                    //}
                    //Att = null;
                }
                catch
                { }

                //METODO INCLUSIONE RISORSE INTEGRATE ---------------------------------------------------------------
                mailMessage.ContentType.MimeType = "text/html";
                //mailMessage.ContentTransferEncoding = ContentTransferEncoding.EightBits;
                mailMessage.ContentTransferEncoding = ContentTransferEncoding.Base64;
                //mailMessage.Charset = "utf-8";
                mailMessage.BodyHtml.Charset = "utf-8";
                //mailMessage.BodyEncoding = System.Text.Encoding.UTF8; ;
                //mailMessage.SubjectEncoding = Encoding.UTF8; 
                //inseriamo MIME-Version: 1.0 per il filtro antispam
                mailMessage.HeaderFields.Add("MIME-Version", "1.0"); //test
                mailMessage.HeaderFieldNames.Add("mime-version", "MIME-Version"); //test

                //Tolgo interrruzioni di linea non html
                Descrizione = Descrizione.Replace("\r\n", "");
                Descrizione = Descrizione.Replace("\r", "");
                Descrizione = Descrizione.Replace("\n", "");
                mailMessage.BodyHtml.Text = Descrizione;
                //mailMessage.BodyText.Text = Descrizione;


                //mailMessage.BuildMimePartTree();

                Exception errret = null;
                //Server serversmtp = new Server("",2,"","",true,EncryptionType.SSL)
                Server serversmtp = new Server();
                //Dictionary<string, string> mailvalues = WelcomeLibrary.UF.Utility.ReadXmlAttributesOfElement(ref errret, "mailSettings", sectionName, "network"); //Stringa da web config
                // 2016 06 26 si prende da TBL_Config
                Dictionary<string, string> mailvalues = WelcomeLibrary.UF.ConfigManagement.ReadSection(ref errret, sectionName); //Stringa da web config                
                if (mailvalues.ContainsKey("host"))
                    serversmtp.Host = mailvalues["host"];
                if (mailvalues.ContainsKey("port"))
                {
                    int port = 25;
                    int.TryParse(mailvalues["port"], out port);
                    serversmtp.Port = port;
                }
                serversmtp.ServerEncryptionType = EncryptionType.None;

                if (mailvalues.ContainsKey("enablessl") && mailvalues["enablessl"] == "true")
                {
                    //SSL IMLICIT MODE (465)
                    serversmtp.ServerEncryptionType = EncryptionType.SSL;
                    //TLS EXPLICT (587)
                    //serversmtp.ServerEncryptionType = EncryptionType.TLS;
                }
                if (mailvalues.ContainsKey("username"))
                    serversmtp.Username = mailvalues["username"];
                if (mailvalues.ContainsKey("password"))
                    serversmtp.Password = mailvalues["password"];

                //SaslMechanism.Login
                ServerCollection servers = new ServerCollection();
                servers.Add(serversmtp);

                // bool isSpam = BayesianFilter.AnalyzeMessage(string.Empty, this.messageTextbox.Text, "../../spam.txt",
                //"../../ham.txt", "../../ignore.txt");


                //int ntentativi = 3;
                //int i = 0;
                //while (i < ntentativi)
                //{
                //    //inviamo il mail facendo un certo num di tentativi
                //    try
                //    {
                //        System.Threading.Thread.Sleep(50); //DA TESTARE TOGLIENDOLO
                //                                           //System.Web.Mail.SmtpMail.Send(objMail);
                bool validemail = Validator.ValidateSyntax(mailMessage.From.Email);
                if (!validemail) throw new ApplicationException("Email errata|Invalid Email");

                string servermessage = "";
                ActiveUp.Net.Mail.SmtpClient.Send(mailMessage, servers, out servermessage);

                if (!servermessage.StartsWith("250")) throw new ApplicationException("Mail Server Error sending mail " + servermessage);
                //    System.Threading.Thread.Sleep(50); //DA TESTARE TOGLIENDOLO
                //    i = ntentativi;
                //    ret = true;
                //}
                //catch (Exception errTentativi)
                //{
                //    i += 1;
                //    if (i == ntentativi) throw new Exception(errTentativi.Message);
                //}
                //}

            }
            catch (Exception error)
            {
                string text = "&nbsp <br/> Errore Invio Mail a " + destinatarioNome + ". " + error.Message;
                if (error.InnerException != null)
                    text += error.InnerException.Message;
                throw new ApplicationException(text);
            }

            return ret;
        }


        public static bool invioMailGenericomimekit(string mittenteNome, string mittenteMail, string SoggettoMail, string Descrizione,
      string destinatarioMail1, string destinatarioNome, List<string> foto = null, string percorsofoto = "", bool incorporaimmagini = false, System.Web.HttpServerUtility server = null, bool plaintext = false, List<string> emaildestbcc = null, string sectionName = "smtpbase")
        {
            bool ret = false;
            var mimeMessage = new MimeKit.MimeMessage(); //http://www.mimekit.net/docs/html/Creating-Messages.htm

            ActiveUp.Net.Mail.Message mailMessage = new ActiveUp.Net.Mail.Message();
            try
            {
                mimeMessage.From.Add(new MailboxAddress(mittenteNome, mittenteMail));
                //Set del reply to in base al mittente passato se questo non è quello del sito necessario per non fare relaying
                if (mittenteMail.ToLower().Trim() != ConfigManagement.ReadKey("Email").ToLower().Trim())
                {
                    mimeMessage.From.Clear();
                    mimeMessage.From.Add(new MailboxAddress(ConfigManagement.ReadKey("Nome"), ConfigManagement.ReadKey("Email")));
                    //Reply-To: nome <email>
                    int lrply = mimeMessage.Headers.LastIndexOf("Reply-To");
                    if (lrply != -1)
                        mimeMessage.Headers[lrply].Value = (new MailboxAddress(mittenteNome, mittenteMail)).ToString();
                    else
                        mimeMessage.Headers.Add("Reply-To", (new MailboxAddress(mittenteNome, mittenteMail)).ToString());
                }
                mimeMessage.MessageId = Guid.NewGuid().ToString() + "." + mittenteMail;


                if (destinatarioMail1 != "")
                {
                    mimeMessage.To.Add(new MailboxAddress(destinatarioNome, destinatarioMail1));
                }
                if (emaildestbcc != null)
                {
                    foreach (string s in emaildestbcc)
                    {
                        mimeMessage.Bcc.Add(new MailboxAddress("", s));
                    }
                }

                var builder = new BodyBuilder();
                ////INSERIAMO GLI ATTACHMENT //http://www.mimekit.net/docs/html/Frequently-Asked-Questions.htm#CreateAttachments
                try
                {
                    if (foto != null)
                        foreach (string attach in foto)
                        {
                            builder.Attachments.Add(@percorsofoto + attach);
                            // mailMessage.Attachments.Add(percorsofoto + attach, false);
                        }
                }
                catch
                { }
                // da finire migrazione   ->>>>>
                //Corpo e soggetto del messaggio 
                string soggetto = SoggettoMail.Replace("\r\n", "");
                soggetto = soggetto.Replace("\r", "");
                soggetto = soggetto.Replace("\n", "");
                mimeMessage.Subject = soggetto;


#if false   // da concludere integrando questa modalità PER GLI ALLEGATI .....
                //METODO INCLUSIONE RISORSE INTEGRATE -------------------------------------------------------------
                List<LinkedResource> listaimmagini = new List<LinkedResource>();
                List<string> listapathfileimmagini = new List<string>();
                //LinkedResource img2 = null;
                //string pathfisicoimg2 = "";
                //if (!string.IsNullOrEmpty(pathfisicoimg2))
                //{
                //    StringBuilder sb1 = new StringBuilder("<img border=\"none\" src=\"cid:IMG2\" alt=\"\" />");
                //    img2 = new LinkedResource(pathfisicoimg2);
                //    img2.ContentId = "IMG2";
                //    img2.ContentType = new System.Net.Mime.ContentType("image/jpeg");
                //    listaimmagini.Add(img2);
                //}
                int imgidx = 1;
                int posimmagine = 0;
                if (incorporaimmagini && server != null)
                {
                    posimmagine = Descrizione.ToLower().IndexOf("src=\"", posimmagine);
                    while (posimmagine != -1)
                    {
                        int endposimmagine = -1;
                        string stringtoreplace = "";
                        if (posimmagine + 5 < Descrizione.Length)
                            endposimmagine = Descrizione.ToLower().IndexOf("\"", posimmagine + 5);
                        if (posimmagine != -1 && endposimmagine != -1 && endposimmagine > posimmagine)
                        {
                            stringtoreplace = Descrizione.Substring(posimmagine, endposimmagine - posimmagine + 1);
                            string linkimmagine = stringtoreplace.Substring(5, stringtoreplace.Length - 6);
                            if (!string.IsNullOrEmpty(linkimmagine))
                            {
                                try
                                {

                                    string percorsoassoluto = linkimmagine.Replace("~", WelcomeLibrary.STATIC.Global.percorsobaseapplicazione);
                                    string percorsovirtuale = linkimmagine.Replace(WelcomeLibrary.STATIC.Global.percorsobaseapplicazione, "~");
                                    string percorsofisico = server.MapPath(percorsovirtuale);

                                    listapathfileimmagini.Add(percorsofisico);
                                    if (System.IO.File.Exists(percorsofisico))
                                    {
                                        string nomefile = percorsovirtuale.Substring(percorsovirtuale.LastIndexOf("/") + 1);
                                        string replacetext = "src=\"cid:" + nomefile + "\"";

                                        // LinkedResource img = new LinkedResource(percorsofisico);
                                        // img.ContentId = "IMG" + imgidx.ToString();
                                        //img.ContentType = new System.Net.Mime.ContentType("image/jpeg");
                                        // listaimmagini.Add(img);

                                        Descrizione = Descrizione.Replace(stringtoreplace, replacetext);
                                        imgidx += 1;//incremento l'indice immagini
                                    }
                                }
                                catch { }
                            }
                        }
                        posimmagine = posimmagine + 5;
                        if (posimmagine + 5 < Descrizione.Length)
                            posimmagine = Descrizione.ToLower().IndexOf("src=\"", posimmagine);
                        else posimmagine = -1;
                    }
                } 

                 //INSERIAMO LE IMMAGINI COME  ATTACHMENT ESTRATTI DAL TESTO (verificare questa cosa )
                try
                {

                    if (foto == null)
                        foreach (string attach in listapathfileimmagini)
                        {
                            mailMessage.Attachments.Add(percorsofoto + attach, false);
                        }
                    //System.Web.Mail.MailAttachment Att;

                    //foreach (string attach in listapathfileimmagini)
                    //{
                    //    Att = new System.Web.Mail.MailAttachment(attach, System.Web.Mail.MailEncoding.Base64);
                    //    objMail.Attachments.Add(Att);
                    //}
                    //Att = null;
                }
                catch
                { }
#endif

                // da comopletare i 3 settign sotto commentati !!!!
                //mailMessage.ContentType.MimeType = "text/html"; //???
                //mailMessage.ContentTransferEncoding = ContentTransferEncoding.Base64;
                //mailMessage.BodyHtml.Charset = "utf-8";
                mimeMessage.MimeVersion = new Version("1.0");
                //Tolgo interrruzioni di linea non html
                Descrizione = Descrizione.Replace("\r\n", "");
                Descrizione = Descrizione.Replace("\r", "");
                Descrizione = Descrizione.Replace("\n", "");
                builder.HtmlBody = Descrizione;

                mimeMessage.Body = builder.ToMessageBody();

                //mimeMessage.From.ForEach(a => ((MailboxAddress)a).)

                foreach (MailboxAddress a in mimeMessage.From)
                {
                    bool validemail = Validator.ValidateSyntax(a.Address);
                    if (!validemail) throw new ApplicationException("Email errata|Invalid Email");
                }


                Exception errret = null;
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {

                    Dictionary<string, string> mailvalues = WelcomeLibrary.UF.ConfigManagement.ReadSection(ref errret, sectionName); //Stringa da web config
                    string host = "";
                    int port = 25;
                    string usenamemail = "";
                    string passmail = "";
                    if (mailvalues.ContainsKey("username"))
                        usenamemail = mailvalues["username"];
                    if (mailvalues.ContainsKey("password"))
                        passmail = mailvalues["password"];

                    if (mailvalues.ContainsKey("host"))
                        host = mailvalues["host"];
                    if (mailvalues.ContainsKey("port"))
                    {
                        int.TryParse(mailvalues["port"], out port);
                    }

                    //serversmtp.ServerEncryptionType = EncryptionType.None;
                    //if (mailvalues.ContainsKey("enablessl") && mailvalues["enablessl"] == "true")
                    //{
                    //    //SSL IMLICIT MODE (465)
                    //    serversmtp.ServerEncryptionType = EncryptionType.SSL;

                    //}
                    if (port == 25)
                        client.Connect(host, port, SecureSocketOptions.None);
                    else if (port == 465)
                        client.Connect(host, port, SecureSocketOptions.SslOnConnect);
                    else
                        client.Connect(host, port, SecureSocketOptions.Auto);



                    client.Authenticate(usenamemail, passmail);
                    var options = FormatOptions.Default.Clone();

                    if (client.Capabilities.HasFlag(SmtpCapabilities.UTF8))
                        options.International = true;

                    client.Send(options, mimeMessage);

                    // if (!servermessage.StartsWith("250")) throw new ApplicationException("Mail Server Error sending mail " + servermessage);

                    client.Disconnect(true);
                }

            }
            catch (Exception error)
            {
                string text = "&nbsp <br/> Errore Invio Mail a " + destinatarioNome + ". " + error.Message;
                if (error.InnerException != null)
                    text += error.InnerException.Message;
                throw new ApplicationException(text);
            }

            return ret;
        }


        public static string HtmlEncode(string text)
        {
            string result;
            using (StringWriter sw = new StringWriter())
            {
                var x = new System.Web.UI.HtmlTextWriter(sw);
                x.WriteEncodedText(text);
                result = sw.ToString();
            }
            return result;

        }

        ///// <summary>
        ///// Send an electronic message using the Collaboration Data Objects (CDO).
        ///// Do not forget to browse through your COM references and add the "Microsoft CDO for Windows 200 Library" which should add two references: ADODB, and CDO.
        ///// </summary>
        ///// <remarks>http://support.microsoft.com/kb/310212</remarks>
        //private void SendTestCDOMessage()
        //{
        //    try
        //    {
        //        string yourEmail = "YourUserName@gmail.com";

        //        CDO.Message message = new CDO.Message();
        //        CDO.IConfiguration configuration = message.Configuration;
        //        ADODB.Fields fields = configuration.Fields;

        //        Console.WriteLine(String.Format("Configuring CDO settings..."));

        //        // Set configuration.
        //        // sendusing:               cdoSendUsingPort, value 2, for sending the message using the network.
        //        // smtpauthenticate:     Specifies the mechanism used when authenticating to an SMTP service over the network.
        //        //                                  Possible values are:
        //        //                                  - cdoAnonymous, value 0. Do not authenticate.
        //        //                                  - cdoBasic, value 1. Use basic clear-text authentication. (Hint: This requires the use of "sendusername" and "sendpassword" fields)
        //        //                                  - cdoNTLM, value 2. The current process security context is used to authenticate with the service.

        //        ADODB.Field field = fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"];
        //        field.Value = "smtp.gmail.com";

        //        field = fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"];
        //        field.Value = 465;

        //        field = fields["http://schemas.microsoft.com/cdo/configuration/sendusing"];
        //        field.Value = CDO.CdoSendUsing.cdoSendUsingPort;

        //        field = fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"];
        //        field.Value = CDO.CdoProtocolsAuthentication.cdoBasic;

        //        field = fields["http://schemas.microsoft.com/cdo/configuration/sendusername"];
        //        field.Value = yourEmail;

        //        field = fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"];
        //        field.Value = "YourPassword";

        //        field = fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"];
        //        field.Value = "true";

        //        fields.Update();

        //        Console.WriteLine(String.Format("Building CDO Message..."));
        //        message.
        //        message.From = yourEmail;
        //        message.To = yourEmail;
        //        message.Subject = "Test message.";
        //        message.TextBody = "This is a test message. Please disregard.";

        //        Console.WriteLine(String.Format("Attempting to connect to remote server..."));

        //        // Send message.
        //        message.Send();

        //        Console.WriteLine("Message sent.");
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}


        #region GESTIONE CARICAMENTO CONTENENTI DATI RELATIVI ALLE VARIE TABELLE DI RIFERIMTNTO

        public static void CaricaMemoriaStaticaCaratteristiche(string codicetipologia, bool removenotpresent = false)
        {
            #region CARICAMENTO DELLE TABELLE DI RIFERIMENTO PER LE CARATTERISTICHE!!
            Caratteristiche = new List<WelcomeLibrary.DOM.TabrifCollection>();
            //Caratteristica1
            Caratteristiche.Add(CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica1"));
            //Caratteristica2
            Caratteristiche.Add(CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica2"));
            //Caratteristica3
            Caratteristiche.Add(CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica3"));
            //Caratteristica4
            Caratteristiche.Add(CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica4"));
            //Caratteristica5
            Caratteristiche.Add(CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica5"));
            //Caratteristica6
            Caratteristiche.Add(CaricaListaStaticaCaratteristica(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, "dbo_TBLRIF_Caratteristica6"));

            //Caratteristiche[2].Sort(new WelcomeLibrary.UF.GenericComparer<WelcomeLibrary.DOM.Tabrif>("Double1", System.ComponentModel.ListSortDirection.Ascending));
            Caratteristiche[3].Sort(new WelcomeLibrary.UF.GenericComparer<WelcomeLibrary.DOM.Tabrif>("Double1", System.ComponentModel.ListSortDirection.Ascending));
            Caratteristiche[4].Sort(new WelcomeLibrary.UF.GenericComparer<WelcomeLibrary.DOM.Tabrif>("Double1", System.ComponentModel.ListSortDirection.Ascending));

            if (removenotpresent)
            {
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //ELIMINO LE CARATTERISTICHE NON PRESENTI NELLA LISTA VEICOLI DEL DB TRAMITE FUNZIONE CaricaListaIdCaratteristiche che rileva gli id presenti nel db
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                List<string> idtoremove = new List<string>();
                WelcomeLibrary.DAL.offerteDM offDM = new WelcomeLibrary.DAL.offerteDM();
                List<string> idpresenti = offDM.CaricaListaIdCaratteristiche(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicetipologia, "Caratteristica1");
                foreach (WelcomeLibrary.DOM.Tabrif t in Caratteristiche[0])
                    if (!idpresenti.Exists(i => i == t.Codice))
                        idtoremove.Add(t.Codice);
                foreach (string id in idtoremove) Caratteristiche[0].RemoveAll(c => c.Codice == id);

                idtoremove = new List<string>();
                idpresenti = offDM.CaricaListaIdCaratteristiche(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicetipologia, "Caratteristica2");
                foreach (WelcomeLibrary.DOM.Tabrif t in Caratteristiche[1])
                    if (!idpresenti.Exists(i => i == t.Codice))
                        idtoremove.Add(t.Codice);
                foreach (string id in idtoremove) Caratteristiche[1].RemoveAll(c => c.Codice == id);

                idtoremove = new List<string>();
                idpresenti = offDM.CaricaListaIdCaratteristiche(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicetipologia, "Caratteristica3");
                foreach (WelcomeLibrary.DOM.Tabrif t in Caratteristiche[2])
                    if (!idpresenti.Exists(i => i == t.Codice))
                        idtoremove.Add(t.Codice);
                foreach (string id in idtoremove) Caratteristiche[2].RemoveAll(c => c.Codice == id);

                idtoremove = new List<string>();
                idpresenti = offDM.CaricaListaIdCaratteristiche(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicetipologia, "Caratteristica4");
                foreach (WelcomeLibrary.DOM.Tabrif t in Caratteristiche[3])
                    if (!idpresenti.Exists(i => i == t.Codice))
                        idtoremove.Add(t.Codice);
                foreach (string id in idtoremove) Caratteristiche[3].RemoveAll(c => c.Codice == id);

                idtoremove = new List<string>();
                idpresenti = offDM.CaricaListaIdCaratteristiche(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicetipologia, "Caratteristica5");
                foreach (WelcomeLibrary.DOM.Tabrif t in Caratteristiche[4])
                    if (!idpresenti.Exists(i => i == t.Codice))
                        idtoremove.Add(t.Codice);
                foreach (string id in idtoremove) Caratteristiche[4].RemoveAll(c => c.Codice == id);

                idtoremove = new List<string>();
                idpresenti = offDM.CaricaListaIdCaratteristiche(WelcomeLibrary.STATIC.Global.NomeConnessioneDb, codicetipologia, "Caratteristica6");
                foreach (WelcomeLibrary.DOM.Tabrif t in Caratteristiche[5])
                    if (!idpresenti.Exists(i => i == t.Codice))
                        idtoremove.Add(t.Codice);
                foreach (string id in idtoremove) Caratteristiche[5].RemoveAll(c => c.Codice == id);
            }
            #endregion

        }

        public static TabrifCollection CaricaListaStaticaCaratteristica(string connection, string tablename)
        {
            TabrifCollection _list = new TabrifCollection();

            if (connection == null || connection == "") return _list;
            Tabrif _item = new Tabrif();

            try
            {
                string query = "SELECT * FROM " + tablename + " order BY Lingua COLLATE NOCASE asc,Descrizione COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return _list; };
                    if (reader.HasRows == false)
                        return _list;

                    while (reader.Read())
                    {
                        _item = new Tabrif();
                        _item.Id = reader.GetInt64(reader.GetOrdinal("ID")).ToString();
                        _item.Codice = reader.GetInt64(reader.GetOrdinal("CodiceTipo")).ToString().Trim();
                        _item.Campo1 = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();

                        if (!reader["RelatedCodiceTipo"].Equals(DBNull.Value))
                            _item.Campo2 = reader.GetString(reader.GetOrdinal("RelatedCodiceTipo")).Trim();

                        if (!reader["Spare1"].Equals(DBNull.Value))
                            _item.Campo3 = reader.GetString(reader.GetOrdinal("Spare1")).Trim();
                        if (!reader["Spare2"].Equals(DBNull.Value))
                            _item.Campo4 = reader.GetString(reader.GetOrdinal("Spare2")).Trim();
                        if (!reader["Spare3"].Equals(DBNull.Value))
                            _item.Campo5 = reader.GetString(reader.GetOrdinal("Spare3")).Trim();
                        if (!reader["Spare4"].Equals(DBNull.Value))
                            _item.Campo6 = reader.GetString(reader.GetOrdinal("Spare4")).Trim();
                        if (!reader["Spare5"].Equals(DBNull.Value))
                            _item.Campo7 = reader.GetString(reader.GetOrdinal("Spare5")).Trim();

                        double d = 0;
                        if (double.TryParse(_item.Campo1, out d))
                            _item.Double1 = d;

                        _item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        _list.Add(_item);
                    }
                }


            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica " + tablename + " :" + error.Message, error);
            }
            return _list;
        }


        public static void CaricaListaStaticaTipiClienti(string connection)
        {
            if (connection == null || connection == "") return;


            TabrifCollection _list = new TabrifCollection();

            Tabrif _item = new Tabrif();

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_TIPI_CLIENTI order BY Lingua COLLATE NOCASE asc,TipoCliente COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return; };
                    if (reader.HasRows == false)
                        return;

                    while (reader.Read())
                    {
                        _item = new Tabrif();
                        _item.Id = reader.GetInt64(reader.GetOrdinal("ID")).ToString();
                        _item.Codice = reader.GetInt64(reader.GetOrdinal("CodiceTipo")).ToString().Trim();
                        _item.Campo1 = reader.GetString(reader.GetOrdinal("TipoCliente")).Trim();
                        _item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        _list.Add(_item);
                    }
                }

                TipiClienti = _list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica Tipi clienti :" + error.Message, error);
            }
            return;
        }

        /// <summary>
        /// Carica la memoria delle nazioni
        /// </summary>
        /// <returns></returns>
        public static void CaricaListaStaticaNazioni(string connection, bool listacompleta = true)
        {
            if (connection == null || connection == "") return;

            TabrifCollection _list = new TabrifCollection();
            Nazioni = _list;

            //string jserialized = Serializzafascedipeso();
            //System.Diagnostics.Debug.WriteLine(jserialized);

            Tabrif _naz = new Tabrif();

            //CREO UNA NAZIONE FITTIZIA CHE RAGGRUPPA TUTTE QUELLE FUORI ITALIA
            _naz.Codice = "XX";
            _naz.Lingua = "I";
            _naz.Campo1 = "Estero";
            _naz.Double1 = 0;
            _list.Add(_naz);
            _naz = new Tabrif();
            _naz.Codice = "XX";
            _naz.Lingua = "GB";
            _naz.Campo1 = "International";
            _naz.Double1 = 0;
            _list.Add(_naz);
            _naz = new Tabrif();
            _naz.Codice = "XX";
            _naz.Lingua = "RU";
            _naz.Campo1 = "International";
            _naz.Double1 = 0;
            _list.Add(_naz);

            _naz = new Tabrif();
            _naz.Codice = "XX";
            _naz.Lingua = "FR";
            _naz.Campo1 = "International";
            _naz.Double1 = 0;
            _list.Add(_naz);



            _naz = new Tabrif();
            _naz.Codice = "XX";
            _naz.Lingua = "DE";
            _naz.Campo1 = "International";
            _naz.Double1 = 0;
            _list.Add(_naz);



            _naz = new Tabrif();
            _naz.Codice = "XX";
            _naz.Lingua = "ES";
            _naz.Campo1 = "International";
            _naz.Double1 = 0;
            _list.Add(_naz);

            try
            {
                if (listacompleta)
                {
                    string query = "SELECT * FROM dbo_TBLRIF_NAZIONI order BY Lingua COLLATE NOCASE asc,CodiceNazione COLLATE NOCASE asc,Descrizione COLLATE NOCASE asc";
                    SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                    using (reader)
                    {
                        if (reader == null) { return; };
                        if (reader.HasRows == false)
                            return;

                        while (reader.Read())
                        {
                            _naz = new Tabrif();

                            _naz.Codice = reader.GetString(reader.GetOrdinal("CodiceNazione")).Trim();
                            _naz.Campo1 = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                            _naz.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                            if (!reader["Double1"].Equals(DBNull.Value))
                                _naz.Double1 = reader.GetDouble(reader.GetOrdinal("Double1"));
                            if (!reader["jsondata"].Equals(DBNull.Value))
                                _naz.Campo2 = reader.GetString(reader.GetOrdinal("jsondata"));

                            _list.Add(_naz);
                        }
                    }
                }

                Nazioni = _list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica nazioni :" + error.Message, error);
            }

            return;
            /*
            
AD	Andorra
AE	Emirati Arabi Uniti
AF	Afghanistan
AG	Antigua e Barbuda
AI	Anguilla
AL	Albania
AM	Armenia
AO	Angola
AQ	Antartide
AR	Argentina
AS	Samoa Americane
AT	Austria
AU	Australia
AW	Aruba
AX	Isole Åland
AZ	Azerbaigian
BA	Bosnia-Erzegovina
BB	Barbados
BD	Bangladesh
BE	Belgio
BF	Burkina Faso
BG	Bulgaria
BH	Bahrain
BI	Burundi
BJ	Benin
BL	Saint-Barthélemy
BM	Bermuda
BN	Brunei
BO	Bolivia
BQ	Isole BES
BR	Brasile
BS	Bahamas
BT	Bhutan
BV	Isola Bouvet
BW	Botswana
BY	Bielorussia
BZ	Belize
CA	Canada
CC	Isole Cocos e Keeling
CD	Repubblica Democratica del Congo
CF	Repubblica Centrafricana
CG	Repubblica del Congo
CH	Svizzera
CI	Costa d'Avorio
CK	Isole Cook
CL	Cile
CM	Camerun
CN	Cina
CO	Colombia
CR	Costa Rica
CU	Cuba
CV	Capo Verde
CW	Curaçao
CX	Isola del Natale
CY	Cipro
CZ	Repubblica Ceca
DE	Germania
DJ	Gibuti
DK	Danimarca
DM	Dominica
DO	Repubblica Dominicana
DZ	Algeria
EC	Ecuador
EE	Estonia
EG	Egitto
EH	Sahara Occidentale
ER	Eritrea
ES	Spagna
ET	Etiopia
FI	Finlandia
FJ	Figi
FK	Isole Falkland
FM	Stati Federati di Micronesia
FO	Isole Fær Øer
FR	Francia
GA	Gabon
GB	Regno Unito
GD	Grenada
GE	Georgia
GF	Guyana Francese
GG	Guernsey
GH	Ghana
GI	Gibilterra
GL	Groenlandia
GM	Gambia
GN	Guinea
GP	Guadalupa
GQ	Guinea Equatoriale
GR	Grecia
GS	Georgia del Sud e isole Sandwich meridionali
GT	Guatemala
GU	Guam
GW	Guinea-Bissau
GY	Guyana
HK	Hong Kong
HM	Isole Heard e McDonald
HN	Honduras
HR	Croazia
HT	Haiti
HU	Ungheria
ID	Indonesia
IE	Irlanda
IL	Israele
IM	Isola di Man
IN	India
IO	Territori Britannici dell'Oceano Indiano
IQ	Iraq
IR	Iran
IS	Islanda
IT	Italia
JE	Jersey
JM	Giamaica
JO	Giordania
JP	Giappone
KE	Kenya
KG	Kirghizistan
KH	Cambogia
KI	Kiribati
KM	Comore
KN	Saint Kitts e Nevis
KP	Corea del Nord
KR	Corea del Sud
KW	Kuwait
KY	Isole Cayman
KZ	Kazakistan
LA	Laos
LB	Libano
LC	Santa Lucia
LI	Liechtenstein
LK	Sri Lanka
LR	Liberia
LS	Lesotho
LT	Lituania
LU	Lussemburgo
LV	Lettonia
LY	Libia
MA	Marocco
MC	Monaco
MD	Moldavia
ME	Montenegro
MF	Saint-Martin
MG	Madagascar
MH	Isole Marshall
MK	Macedonia
ML	Mali
MM	Birmania
MN	Mongolia
MO	Macao
MP	Isole Marianne Settentrionali
MQ	Martinica
MR	Mauritania
MS	Oswald
MT	Malta
MU	Mauritius
MV	Maldive
MW	Malawi
MX	Messico
MY	Malesia
MZ	Mozambico
NA	Namibia
NC	Nuova Caledonia
NE	Niger
NF	Isola Norfolk
NG	Nigeria
NI	Nicaragua
NL	Olanda
NO	Norvegia
NP	Nepal
NR	Nauru
NU	Niue
NZ	Nuova Zelanda
OM	Oman
PA	Panamá
PE	Perù
PF	Polinesia Francese
PG	Papua Nuova Guinea
PH	Filippine
PK	Pakistan
PL	Polonia
PM	Saint Pierre e Miquelon
PN	Isole Pitcairn
PR	Porto Rico
PS	Territori Palestinesi Occupati
PT	Portogallo
PW	Palau
PY	Paraguay
QA	Qatar
RE	Réunion
RO	Romania
RU	Russia
RS	Serbia
RW	Ruanda
SA	Arabia Saudita
SB	Isole Salomone
SC	Seychelles
SD	Sudan
SE	Svezia
SG	Singapore
SH	Sant'Elena, Isola di Ascensione e Tristan da Cunha
SI	Slovenia
SJ	Svalbard e Jan Mayen
SK	Slovacchia
SL	Sierra Leone
SM	San Marino
SN	Senegal
SO	Somalia
SR	Suriname
ST	São Tomé e Príncipe
SV	El Salvador
SX	Sint Maarten
SY	Siria
SZ	Swaziland
TC	Isole Turks e Caicos
TD	Ciad
TF	Territori Francesi del Sud
TG	Togo
TH	Thailandia
TJ	Tagikistan
TK	Tokelau
TL	Timor Est
TM	Turkmenistan
TN	Tunisia
TO	Tonga
TR	Turchia
TT	Trinidad e Tobago
TV	Tuvalu
TW	Repubblica di Cina
TZ	Tanzania
UA	Ucraina
UG	Uganda
UM	Isole minori esterne degli Stati Uniti
US	Stati Uniti d'America
UY	Uruguay
UZ	Uzbekistan
VA	Città del Vaticano
VC	Saint Vincent e Grenadine
VE	Venezuela
VG	Isole Vergini Britanniche
VI	Isole Vergini Americane
VN	Vietnam
VU	Vanuatu
WF	Wallis e Futuna
WS	Samoa
YE	Yemen
YT	Mayotte
ZA	Sudafrica
ZM	Zambia
ZW	Zimbabwe

             */


        }

        /// <summary>
        /// Funzione di test per generazione stringa serializzata dati nazioni
        /// </summary>
        /// <returns></returns>
        private static string Serializzafascedipeso()
        {
            jsonspedizioni js = new jsonspedizioni();

            List<fascespedizioni> fasce = new List<fascespedizioni>();
            //fascedi
            fascespedizioni fs = new fascespedizioni();
            fs.PesoMin = 0;
            fs.PesoMax = 5;
            fs.Costo = 10;
            fasce.Add(fs);
            fs = new fascespedizioni();
            fs.PesoMin = 5;
            fs.PesoMax = 10;
            fs.Costo = 20;
            fasce.Add(fs);
            fs = new fascespedizioni();
            fs.PesoMin = 10;
            fs.PesoMax = 20;
            fs.Costo = 30;
            fasce.Add(fs);
            fs = new fascespedizioni();
            fs.PesoMin = 20;
            fs.PesoMax = 9999999;
            fs.Costo = 40;
            fasce.Add(fs);
            js.fascespedizioni = fasce;

            js.keyValuePairs = new Dictionary<string, string>();

            js.keyValuePairs.Add("sogliaazzeramento", "0");
            js.keyValuePairs.Add("supplementosp", "0");
            js.keyValuePairs.Add("sogliapeso", "0");


            return Newtonsoft.Json.JsonConvert.SerializeObject(js, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.None,
            });
        }


        public static jsonspedizioni Deserializzafascedipeso(string jsonstr)
        {
            jsonspedizioni returncoll = Newtonsoft.Json.JsonConvert.DeserializeObject<jsonspedizioni>(jsonstr);
            return returncoll;
        }


        /// <summary>
        /// Carica la lista delle provincie in tutte le lingue in una memoria statica apposita per
        /// riutilizzo nell'applicazione
        /// </summary>
        /// <param name="NomeFileXml"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaProvince(string connection)
        {
            if (connection == null || connection == "") return false;
            ProvinceCollection list = new ProvinceCollection();
            Province item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_PROVINCE order BY Lingua COLLATE NOCASE asc,CodiceRegione COLLATE NOCASE asc,CodiceProvincia COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new Province();

                        item.Codice = reader.GetString(reader.GetOrdinal("Codice")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.SiglaNazione = reader.GetString(reader.GetOrdinal("AreaGeo")).Trim();

                        item.Regione = reader.GetString(reader.GetOrdinal("Regione")).Trim();
                        item.Regione =
                            Regex.Replace(item.Regione.ToLower(), @"\b[a-z]", delegate (Match m)
                            {
                                return m.Value.ToUpper();
                            });



                        item.Provincia = reader.GetString(reader.GetOrdinal("Provincia")).Trim();
                        item.CodiceProvincia = reader.GetString(reader.GetOrdinal("CodiceProvincia")).Trim();
                        item.CodiceRegione = reader.GetString(reader.GetOrdinal("CodiceRegione")).Trim();
                        item.SiglaProvincia = reader.GetString(reader.GetOrdinal("SiglaProv")).Trim();
                        list.Add(item);
                    }
                }
                list.Sort(new GenericComparer2<Province>("Regione", System.ComponentModel.ListSortDirection.Ascending, "Codice", System.ComponentModel.ListSortDirection.Ascending));

                ElencoProvince = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica Province :" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la lista dei comuni in tutte le lingue in una memoria statica per il riuso dell'applicazione
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaComuni(string connection)
        {
            if (connection == null || connection == "") return false;
            ComuneCollection list = new ComuneCollection();
            Comune item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_COMUNI order BY Lingua COLLATE NOCASE asc,CodiceIncrocio COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new Comune();

                        item.CodiceIncrocio = reader.GetString(reader.GetOrdinal("CodiceIncrocio")).Trim();
                        //item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        //item.CodiceComune = reader.GetString(reader.GetOrdinal("Codice"));
                        item.Nome = reader.GetString(reader.GetOrdinal("Comune")).Trim();
                        list.Add(item);
                    }
                }
                ElencoComuni = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica Comuni :" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la memoria statica dei comuni in base al codice provincia passato
        /// come codice uso quello di incrocio tra le tabelle province - comuni
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codiceprovincia"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaComuni(string connection, string codiceprovincia)
        {
            if (connection == null || connection == "") return false;
            ComuneCollection list = new ComuneCollection();
            Comune item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_COMUNI where CodiceIncrocio = @codiceprovincia order BY Lingua COLLATE NOCASE asc,CodiceIncrocio COLLATE NOCASE asc";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@codiceprovincia", codiceprovincia);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new Comune();

                        item.CodiceIncrocio = reader.GetString(reader.GetOrdinal("CodiceIncrocio")).Trim();
                        //item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        //item.CodiceComune = reader.GetString(reader.GetOrdinal("Codice"));
                        item.Nome = reader.GetString(reader.GetOrdinal("Comune")).Trim();
                        list.Add(item);
                    }
                }
                ElencoComuni = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica Comuni :" + error.Message, error);
            }

            return true;
        }


        /// <summary>
        /// Carica la lista delle TIPOLOGIE CONTENUTI in tutte le lingue in una memoria statica apposita per
        /// riutilizzo nell'applicazione
        /// </summary>
        /// <param name="NomeFileXml"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaTipologieContenuti(string connection)
        {
            if (connection == null || connection == "") return false;
            TipologiaContenutiCollection list = new TipologiaContenutiCollection();
            TipologiaContenuti item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_CONTENUTI order BY Lingua COLLATE NOCASE asc,CodiceContenuto COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new TipologiaContenuti();
                        item.Codice = reader.GetString(reader.GetOrdinal("CodiceContenuto")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        list.Add(item);
                    }
                }
                TipologieContenuti = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica tipologie contenuti :" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la lista delle TIPOLOGIE CONTENUTI nella lingua indicata in una memoria statica apposita per
        /// riutilizzo nell'applicazione
        /// </summary>
        /// <param name="NomeFileXml"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaTipologieContenuti(string connection, string Lingua)
        {
            if (connection == null || connection == "") return false;
            TipologiaContenutiCollection list = new TipologiaContenutiCollection();
            TipologiaContenuti item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_CONTENUTI where Lingua=@Lingua order BY Lingua COLLATE NOCASE asc,CodiceContenuto COLLATE NOCASE asc";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@Lingua", Lingua);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new TipologiaContenuti();
                        item.Codice = reader.GetString(reader.GetOrdinal("CodiceContenuto")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        list.Add(item);
                    }
                }
                TipologieContenuti = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica tipologie contenuti :" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la memoria statica delle tipologie Annunci in tutte le lingue
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaTipologieAnnunci(string connection)
        {
            if (connection == null || connection == "") return false;
            TipologiaAnnunciCollection list = new TipologiaAnnunciCollection();
            TipologiaAnnunci item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_ANNUNCI order BY  Lingua COLLATE NOCASE asc,CodiceTipologia COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new TipologiaAnnunci();
                        item.Codice = reader.GetString(reader.GetOrdinal("CodiceTipologia")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        list.Add(item);
                    }
                }
                TipologieAnnunci = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica tipologie annunci :" + error.Message, error);
            }

            return true;
        }


        /// <summary>
        /// Carica la memoria statica delle tipologie Offerte in tutte le lingue
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaTipologieOfferte(string connection)
        {
            if (connection == null || connection == "") return false;
            TipologiaOfferteCollection list = new TipologiaOfferteCollection();
            TipologiaOfferte item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_ATTIVITA order BY  Lingua COLLATE NOCASE asc,CodiceTipologia COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new TipologiaOfferte();
                        item.Codice = reader.GetString(reader.GetOrdinal("CodiceTipologia")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        list.Add(item);
                    }
                }
                TipologieOfferte = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica tipologie contenuti :" + error.Message, error);
            }

            return true;
        }

        public static string CaricaListaStaticaTipologieOfferte(string connection, bool debug)
        {
            if (connection == null || connection == "") return "nessuna connessione specificata";
            TipologiaOfferteCollection list = new TipologiaOfferteCollection();
            TipologiaOfferte item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_ATTIVITA order BY  Lingua COLLATE NOCASE asc,CodiceTipologia COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return "reader nullo"; };
                    if (reader.HasRows == false)
                        return "nessuna riga trovata";

                    while (reader.Read())
                    {
                        item = new TipologiaOfferte();
                        item.Codice = reader.GetString(reader.GetOrdinal("CodiceTipologia")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        list.Add(item);
                    }
                }
                TipologieOfferte = list;
            }
            catch (Exception error)
            {
                // throw new ApplicationException("Errore Caricamento Memoria Statica tipologie contenuti :" + error.Message, error);
                return ("Errore Caricamento Memoria Statica tipologie contenuti :" + error.Message);
            }

            return "Caricato correttamente da db";
        }


        /// <summary>
        /// Carica la memoria statica delle fasce di prezzo per la ricerca  
        /// raggruppabili anche per codicetipologiacollegata ( per differenziare i parametri a seconda della tipologia)
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaFascediprezzo(string connection)
        {
            if (connection == null || connection == "") return false;
            FascediprezzoCollection list = new FascediprezzoCollection();
            Fascediprezzo item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_PREZZI order BY  CodiceTipologiaCollegata COLLATE NOCASE asc,Lingua COLLATE NOCASE asc,CodiceFascia COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new Fascediprezzo();
                        item.Codice = reader.GetString(reader.GetOrdinal("CodiceFascia")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        item.PrezzoMin = reader.GetDouble(reader.GetOrdinal("PrezzoMin"));
                        item.PrezzoMax = reader.GetDouble(reader.GetOrdinal("PrezzoMax"));
                        item.CodiceTipologiaCollegata = reader.GetString(reader.GetOrdinal("CodiceTipologiaCollegata")).Trim();

                        list.Add(item);
                    }
                }
                Fascediprezzo = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica fasce di prezzo ricerca contenuti :" + error.Message, error);
            }

            return true;
        }


        /// <summary>
        /// Carica la memoria statica PARAMETRO1 per la ricerca  
        /// raggruppabili anche per codicetipologiacollegata ( per differenziare i parametri a seconda della tipologia)
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaParametro1(string connection)
        {
            if (connection == null || connection == "") return false;
            ParametroGenericoCollection list = new ParametroGenericoCollection();
            ParametroGenerico item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_PARAMETRO1 order BY  Descrizione COLLATE NOCASE asc,Lingua COLLATE NOCASE asc,CodiceTipologia COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new ParametroGenerico();
                        item.Codice = reader.GetString(reader.GetOrdinal("CodiceTipologia")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        item.CodiceTipologiaCollegata = reader.GetString(reader.GetOrdinal("CodiceTipologiaCollegata")).Trim();

                        list.Add(item);
                    }
                }
                ParametroGenerico1 = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica ParametroGenerico1 ricerca contenuti :" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la memoria statica PARAMETRO2 per la ricerca  
        /// raggruppabili anche per codicetipologiacollegata ( per differenziare i parametri a seconda della tipologia)
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaParametro2(string connection)
        {
            if (connection == null || connection == "") return false;
            ParametroGenericoCollection list = new ParametroGenericoCollection();
            ParametroGenerico item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_PARAMETRO2 order BY  Descrizione COLLATE NOCASE asc,Lingua COLLATE NOCASE asc,CodiceTipologia COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new ParametroGenerico();
                        item.Codice = reader.GetString(reader.GetOrdinal("CodiceTipologia")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        item.CodiceTipologiaCollegata = reader.GetString(reader.GetOrdinal("CodiceTipologiaCollegata")).Trim();

                        list.Add(item);
                    }
                }
                ParametroGenerico2 = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica ParametroGenerico2 ricerca contenuti :" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la lista delle categorie di primo livello per le offerte
        /// </summary>
        /// <param name="connection"></param>
        public static bool CaricaListaStaticaCategorieOfferteLiv1(string connection)
        {

            if (connection == null || connection == "") return false;
            CategoriaOfferteLiv1Collection list = new CategoriaOfferteLiv1Collection();
            CategoriaOfferteLiv1 item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_ATTIVITACAT1 order BY  Lingua COLLATE NOCASE asc,codcat1 COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new CategoriaOfferteLiv1();
                        item.Codice = reader.GetString(reader.GetOrdinal("codcat1")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        list.Add(item);
                    }
                }
                _CategoriaLiv1Offerte = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica categorie contenuti offerta :" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la memoria statica delle tipologie Offerte in tutte le lingue
        /// includendo il campo categoria di primo livello PER il filtraggio su due livelli
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool CaricaRelazioniOfferteCategorieLiv1(string connection)
        {
            if (connection == null || connection == "") return false;
            Offerte_cat1_linkCollection list = new Offerte_cat1_linkCollection();
            Offerte_cat1_link item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_ATTIVITA_LINK_LIV1 order BY  CodiceTipologia COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new Offerte_cat1_link();
                        item.Codcat1 = reader.GetString(reader.GetOrdinal("codcat1")).Trim();
                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTipologia")).Trim();
                        list.Add(item);
                    }
                }
                LinkOfferteCategoriaLiv1 = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica relazioni Tipologie Offerte/Categorie Liv1 contenuti :" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la memoria statica delle tipologie di attività presenti in tutte le lingue, 
        /// includendo il prio e il seconsìdo livello per i filtraggi
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaAttivitaCodiciRapporto(string connection)
        {

            if (connection == null || connection == "") return false;
            OfferteCollection list = new OfferteCollection();
            ListAttivita = list;

            Offerte item;

            try
            {
                string query = "SELECT * FROM dbo_TBL_ATTIVITA order BY  CodiceTipologia COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new Offerte();
                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTipologia")).Trim();

                        item.CodiceCategoria = reader.GetString(reader.GetOrdinal("CodiceCategoria")).Trim();
                        item.CodiceCategoria2Liv = reader.GetString(reader.GetOrdinal("CodiceCategoria2Liv")).Trim();

                        list.Add(item);
                    }
                }
                ListAttivita = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica relazioni Attivita - Categoria - Sottocategoria :" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la memoria statica dei prodotti in base al codice attivita passato
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codiceprovincia"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaProdottoPerCodice(string connection, string codiceattivita)
        {
            if (connection == null || connection == "") return false;
            ProdottoCollection list = new ProdottoCollection();
            Prodotto item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_PRODOTTO where CodiceTipologia = @codiceattivita order BY Lingua COLLATE NOCASE asc,CodiceTipologia COLLATE NOCASE asc";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@codiceattivita", codiceattivita);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new Prodotto();

                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTipologia")).Trim();
                        //item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        //item.CodiceComune = reader.GetString(reader.GetOrdinal("Codice"));
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto")).Trim();
                        list.Add(item);
                    }
                }
                ElencoProdotti = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica Prodotti :" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la memoria statica dei prodotti in tutte le lingue
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaProdotto(string connection)
        {
            if (connection == null || connection == "") return false;
            ProdottoCollection list = new ProdottoCollection();
            Prodotto item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_PRODOTTO order BY  Lingua COLLATE NOCASE asc,CodiceProdotto COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                    { ElencoProdotti = list; return false; }

                    while (reader.Read())
                    {
                        item = new Prodotto();
                        item.CodiceTipologia = reader.GetString(reader.GetOrdinal("CodiceTipologia")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto")).Trim();
                        list.Add(item);
                    }
                }
                ElencoProdotti = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica Prodotti:" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la memoria statica dei sotto prodotti in tutte le lingue
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaSottoProdotto(string connection)
        {
            if (connection == null || connection == "") return false;
            SProdottoCollection list = new SProdottoCollection();
            SProdotto item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_SOTTOPRODOTTO order BY  Lingua COLLATE NOCASE asc,CodiceSottoprodotto COLLATE NOCASE asc";
                SQLiteDataReader reader = dbDataAccess.GetReaderOle(query, null, connection);
                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                    { ElencoSottoProdotti = list; return false; }

                    while (reader.Read())
                    {
                        item = new SProdotto();
                        item.CodiceSProdotto = reader.GetString(reader.GetOrdinal("CodiceSottoprodotto")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto")).Trim();
                        list.Add(item);
                    }
                }
                ElencoSottoProdotti = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica sottoprodotto :" + error.Message, error);
            }

            return true;
        }

        /// <summary>
        /// Carica la lista statica dei sottoprodotti in base al codice prodotto passato
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="codiceprodotto"></param>
        /// <returns></returns>
        public static bool CaricaListaStaticaSottoProdottoPerCodice(string connection, string codiceprodotto)
        {
            if (connection == null || connection == "") return false;
            SProdottoCollection list = new SProdottoCollection();
            SProdotto item;

            try
            {
                string query = "SELECT * FROM dbo_TBLRIF_SOTTOPRODOTTO where CodiceProdotto = @codiceprodotto order BY Lingua COLLATE NOCASE asc,CodiceProdotto COLLATE NOCASE asc";
                List<SQLiteParameter> parColl = new List<SQLiteParameter>();
                SQLiteParameter p1 = new SQLiteParameter("@codiceprodotto", codiceprodotto);//OleDbType.VarChar
                parColl.Add(p1);
                SQLiteDataReader reader = dbDataAccess.GetReaderListOle(query, parColl, connection);

                using (reader)
                {
                    if (reader == null) { return false; };
                    if (reader.HasRows == false)
                        return false;

                    while (reader.Read())
                    {
                        item = new SProdotto();

                        item.CodiceProdotto = reader.GetString(reader.GetOrdinal("CodiceProdotto")).Trim();
                        //item.Lingua = reader.GetString(reader.GetOrdinal("Lingua"));
                        //item.CodiceComune = reader.GetString(reader.GetOrdinal("Codice"));
                        item.Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")).Trim();
                        item.CodiceSProdotto = reader.GetString(reader.GetOrdinal("CodiceSProdotto")).Trim();
                        item.Lingua = reader.GetString(reader.GetOrdinal("Lingua")).Trim();
                        list.Add(item);
                    }
                }
                ElencoSottoProdotti = list;
            }
            catch (Exception error)
            {
                throw new ApplicationException("Errore Caricamento Memoria Statica Prodotti :" + error.Message, error);
            }

            return true;
        }


        #endregion



        public static string SostituisciTestoACapo(string testo)
        {

            testo = testo.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", " <br/> ");
            return testo;

        }

    }
}
