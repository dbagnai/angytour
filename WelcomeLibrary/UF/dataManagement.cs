using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Xml;
using System.Security;
using System.Web;
using System.Net;

namespace WelcomeLibrary.UF
{
    public class AssemblyChangeBinderWelcomeLibraryToGESiteFunctions : System.Runtime.Serialization.SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            //String assemVer1 = System.Reflection.Assembly.GetExecutingAssembly().FullName;
            //String originaltypeVer = "Version1Type";
            //if (assemblyName == assemVer1 && typeName == originaltypeVer)
            //{
            //assemblyName = assemblyName.Replace("1.0.0.0", "2.0.0.0");
            //Modifico l'assembly name ovunque secondo i valori che mi servono nella deserializzazione
            assemblyName = assemblyName.Replace("WelcomeLibrary", "GESiteFunctions");
            typeName = typeName.Replace("WelcomeLibrary", "GESiteFunctions");
            //}

            // The following line of code returns the type.
            Type typeToDeserialize = null;
            return typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                 typeName, assemblyName));
        }//method
    }//clas
    public static class dataManagement
    {
        #region HASH METHODS


        //public static string GetUniqueHashFromString(string s)
        //{
        //    //int hashcode = s.GetHashCode();
        //    //return hashcode.ToString();
        //    using (SHA1Managed sha1 = new SHA1Managed())
        //    {
        //        var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(s));
        //        //make sure the hash is only alpha numeric to prevent charecters that may break the url
        //        return string.Concat(Convert.ToBase64String(hash).ToCharArray().Where(x => char.IsLetterOrDigit(x)).Take(10));
        //    }

        //}
        public static string CalculateMD5(string input)
        {
            // Create MD5 Hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            // Convert byte array to string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));
            return sb.ToString();
        }

        public static string CalculateSHA1(string text, Encoding enc)
        {
            byte[] buffer = enc.GetBytes(text);
            SHA1CryptoServiceProvider crypto = new SHA1CryptoServiceProvider();
            byte[] hash = crypto.ComputeHash(buffer);
            // Convert byte array to string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
                sb.Append(hash[i].ToString("x2"));
            return sb.ToString();
        }

        //public static string Stringshortner(string text)
        //{
        //    return System.Text.RegularExpressions.Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
        //}

        public static byte[] CalcMd5HashToByteArray(string filename)
        {
            byte[] md5Hash = new byte[0];
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                if (File.Exists(filename))
                {
                    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    md5Hash = md5.ComputeHash(fs);
                    fs.Close();
                    fs.Dispose();
                }
            }
            catch { }
            return md5Hash;
        }

        public static string CalcMd5FileHashToString(string filename)
        {
            string sz = "";
            try
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                if (File.Exists(filename))
                {
                    FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    byte[] md5Hash = md5.ComputeHash(fs);
                    fs.Close();
                    fs.Dispose();
                    sz = Convert.ToBase64String(md5Hash);
                }
            }
            catch { }
            return sz;
        }

        public static byte[] ConvertStringToByteArray(string str)
        {
            byte[] buf = new byte[0];
            try
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                buf = encoding.GetBytes(str);
            }
            catch { }

            return buf;
        }

        public static string ConvertByteArrayToString(byte[] buf)
        {
            string sz = "";
            try
            {
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                sz = enc.GetString(buf);
            }
            catch { }
            return sz;
        }

        #endregion

        #region SYMETRIC CRYPTOGRAPHY


        static private Byte[] m_Key = new Byte[8];
        static private Byte[] m_IV = new Byte[8];


        public static string EncryptData(string originalString, String strKey)
        {
            if (String.IsNullOrEmpty(originalString))
            {
                return ("The string which needs to be encrypted can not be null.");
            }
            if (!InitKey(strKey))
            {
                return "Error. Fail to generate key for encryption";
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            ICryptoTransform desEncrypt = cryptoProvider.CreateEncryptor(m_Key, m_IV);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, desEncrypt, CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(originalString);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);

        }
        public static string DecryptData(string cryptedString, String strKey)
        {

            if (String.IsNullOrEmpty(cryptedString))
            {
                return ("The string which needs to be decrypted can not be null.");
            }

            //1. Generate the Key used for decrypting
            if (!InitKey(strKey))
            {
                return "Error. Fail to generate key for decryption";
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream
                    (Convert.FromBase64String(cryptedString));
            ICryptoTransform desDecrypt = cryptoProvider.CreateDecryptor(m_Key, m_IV);


            CryptoStream cryptoStream = new CryptoStream(memoryStream, desDecrypt, CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            return reader.ReadToEnd();

        }


        ////////////////////////////
        ////Function to encrypt data
        //public static string EncryptData(String strKey, String strData)
        //{
        //    string strResult;		//Return Result

        //    //1. String Length cannot exceed 90Kb. Otherwise, buffer will overflow. See point 3 for reasons
        //    if (strData.Length > 92160)
        //    {
        //        strResult = "Error. Data String too large. Keep within 90Kb.";
        //        return strResult;
        //    }

        //    //2. Generate the Keys
        //    if (!InitKey(strKey))
        //    {
        //        strResult = "Error. Fail to generate key for encryption";
        //        return strResult;
        //    }

        //    //3. Prepare the String
        //    //	The first 5 character of the string is formatted to store the actual length of the data.
        //    //	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
        //    //	If anyone figure a good way to 'remember' the original length to facilite the decryption without having to use additional function parameters, pls let me know.
        //    strData = String.Format("{0,5:00000}" + strData, strData.Length);


        //    //4. Encrypt the Data
        //    byte[] rbData = new byte[strData.Length];
        //    ASCIIEncoding aEnc = new ASCIIEncoding();
        //    aEnc.GetBytes(strData, 0, strData.Length, rbData, 0);

        //    DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();

        //    ICryptoTransform desEncrypt = descsp.CreateEncryptor(m_Key, m_IV);


        //    //5. Perpare the streams:
        //    //	mOut is the output stream. 
        //    //	mStream is the input stream.
        //    //	cs is the transformation stream.
        //    MemoryStream mStream = new MemoryStream(rbData);
        //    CryptoStream cs = new CryptoStream(mStream, desEncrypt, CryptoStreamMode.Read);
        //    MemoryStream mOut = new MemoryStream();

        //    //6. Start performing the encryption
        //    int bytesRead;
        //    byte[] output = new byte[1024];
        //    do
        //    {
        //        bytesRead = cs.Read(output, 0, 1024);
        //        if (bytesRead != 0)
        //            mOut.Write(output, 0, bytesRead);
        //    } while (bytesRead > 0);

        //    //7. Returns the encrypted result after it is base64 encoded
        //    //	In this case, the actual result is converted to base64 so that it can be transported over the HTTP protocol without deformation.
        //    if (mOut.Length == 0)
        //        strResult = "";
        //    else
        //        strResult = Convert.ToBase64String(mOut.GetBuffer(), 0, (int)mOut.Length);

        //    return strResult;
        //}

        ////////////////////////////
        ////Function to decrypt data
        //public static string DecryptData(String strKey, String strData)
        //{
        //    string strResult;

        //    //1. Generate the Key used for decrypting
        //    if (!InitKey(strKey))
        //    {
        //        strResult = "Error. Fail to generate key for decryption";
        //        return strResult;
        //    }

        //    //2. Initialize the service provider
        //    int nReturn = 0;
        //    DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();
        //    ICryptoTransform desDecrypt = descsp.CreateDecryptor(m_Key, m_IV);

        //    //3. Prepare the streams:
        //    //	mOut is the output stream. 
        //    //	cs is the transformation stream.
        //    MemoryStream mOut = new MemoryStream();
        //    CryptoStream cs = new CryptoStream(mOut, desDecrypt, CryptoStreamMode.Write);

        //    //4. Remember to revert the base64 encoding into a byte array to restore the original encrypted data stream
        //    byte[] bPlain = new byte[strData.Length];
        //    try
        //    {
        //        bPlain = Convert.FromBase64CharArray(strData.ToCharArray(), 0, strData.Length);
        //    }
        //    catch (Exception)
        //    {
        //        strResult = "Error. Input Data is not base64 encoded.";
        //        return strResult;
        //    }

        //    long lRead = 0;
        //    long lTotal = strData.Length;

        //    try
        //    {
        //        //5. Perform the actual decryption
        //        while (lTotal >= lRead)
        //        {
        //            cs.Write(bPlain, 0, (int)bPlain.Length);
        //            //descsp.BlockSize=64
        //            lRead = mOut.Length + Convert.ToUInt32(((bPlain.Length / descsp.BlockSize) * descsp.BlockSize));
        //        };

        //        ASCIIEncoding aEnc = new ASCIIEncoding();
        //        strResult = aEnc.GetString(mOut.GetBuffer(), 0, (int)mOut.Length);

        //        //6. Trim the string to return only the meaningful data
        //        //	Remember that in the encrypt function, the first 5 character holds the length of the actual data
        //        //	This is the simplest way to remember to original length of the data, without resorting to complicated computations.
        //        String strLen = strResult.Substring(0, 5);
        //        int nLen = Convert.ToInt32(strLen);
        //        strResult = strResult.Substring(5, nLen);
        //        nReturn = (int)mOut.Length;

        //        return strResult;
        //    }
        //    catch (Exception)
        //    {
        //        strResult = "Error. Decryption Failed. Possibly due to incorrect Key or corrputed data";
        //        return strResult;
        //    }
        //}

        /////////////////////////////////////////////////////////////
        //Private function to generate the keys into member variables
        static private bool InitKey(String strKey)
        {
            try
            {
                // Convert Key to byte array
                byte[] bp = new byte[strKey.Length];
                ASCIIEncoding aEnc = new ASCIIEncoding();
                aEnc.GetBytes(strKey, 0, strKey.Length, bp, 0);

                //Hash the key using SHA1
                SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
                byte[] bpHash = sha.ComputeHash(bp);

                int i;
                // use the low 64-bits for the key value
                for (i = 0; i < 8; i++)
                    m_Key[i] = bpHash[i];

                for (i = 8; i < 16; i++)
                    m_IV[i - 8] = bpHash[i];

                return true;
            }
            catch (Exception)
            {
                //Error Performing Operations
                return false;
            }
        }



        #endregion

        #region Serializzazione Classi

        /// <summary>
        /// Serializza la classe passata in una stringa di testo usando il binaryformatter
        /// </summary>
        /// <param name="Classe"></param>
        /// <returns></returns>
        public static string SerializzaClasse(object Classe)
        {
            MemoryStream ms = new MemoryStream();
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf1 = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            bf1.Serialize(ms, Classe);
            //return ms.ToArray();

            return Convert.ToBase64String(ms.ToArray());
        }

        public static T DeserializzaClasse<T>(string Base64EncodedString)
        {
            byte[] theByteArray = Convert.FromBase64String(Base64EncodedString);
            MemoryStream ms = new MemoryStream(theByteArray);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf1 = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            ms.Position = 0;
            return (T)bf1.Deserialize(ms);
        }
        /// <summary>
        /// Permette la deserializzane con assembly origine -> destinazione diversi
        /// mantenendo le stesse classi e oggetti
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Base64EncodedString"></param>
        /// <returns></returns>
        public static T DeserializzaClasseConAssemblyChangeWelcomeLibraryToGESiteFunctions<T>(string Base64EncodedString)
        {
            byte[] theByteArray = Convert.FromBase64String(Base64EncodedString);
            MemoryStream ms = new MemoryStream(theByteArray);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf1 = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            bf1.Binder = new AssemblyChangeBinderWelcomeLibraryToGESiteFunctions();
            ms.Position = 0;
            return (T)bf1.Deserialize(ms);
        }
        /// <summary>
        /// Converte una stringa in base64
        /// </summary>
        /// <param name="toEncode"></param>
        /// <returns></returns>
        public static string EncodeToBase64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeFromBase64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);
            return returnValue;
        }


        /// <summary>
        /// Converte una stringa in base64
        /// </summary>
        /// <param name="toEncode"></param>
        /// <returns></returns>
        public static string EncodeUtfToBase64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.UTF8.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);
            return returnValue;
        }

        public static string DecodeUtfFromBase64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.ASCIIEncoding.UTF8.GetString(encodedDataAsBytes);
            return returnValue;
        }

        public static void DecodeBase64String(string inputString, string outputFileName)
        {
            // Convert the Base64 UUEncoded input into binary output.
            byte[] binaryData;
            try
            {
                binaryData =
                    System.Convert.FromBase64String(inputString);
            }
            catch (System.ArgumentNullException)
            {
                System.Console.WriteLine("Base 64 string is null.");
                return;
            }
            catch (System.FormatException)
            {
                System.Console.WriteLine("Base 64 string length is not " +
                    "4 or is not an even multiple of 4.");
                return;
            }

            // Write out the decoded data.
            System.IO.FileStream outFile;
            try
            {
                outFile = new System.IO.FileStream(outputFileName,
                                                   System.IO.FileMode.Create,
                                                   System.IO.FileAccess.Write);
                outFile.Write(binaryData, 0, binaryData.Length);
                outFile.Close();
            }
            catch (System.Exception exp)
            {
                // Error creating stream or writing to it.
                System.Console.WriteLine("{0}", exp.Message);
            }
            //return true;
        }

        /// <summary>
        /// Codifica un file in una stringa Base64 leggendolo dal disco
        /// </summary>
        /// <param name="inputFileName"></param>
        /// <returns></returns>
        public static string EncodeBase64String(string inputFileName)
        {
            System.IO.FileStream inFile;
            byte[] binaryData;

            try
            {
                inFile = new System.IO.FileStream(inputFileName,
                                                  System.IO.FileMode.Open,
                                                  System.IO.FileAccess.Read);
                binaryData = new Byte[inFile.Length];
                long bytesRead = inFile.Read(binaryData, 0,
                                            (int)inFile.Length);
                inFile.Close();
            }
            catch (System.Exception exp)
            {
                // Error creating stream or reading from it.
                System.Console.WriteLine("{0}", exp.Message);
                return "";
            }

            // Convert the binary input into Base64 UUEncoded output.
            string base64String;
            try
            {
                base64String =
                   System.Convert.ToBase64String(binaryData,
                                                 0,
                                                 binaryData.Length);
            }
            catch (System.ArgumentNullException)
            {
                System.Console.WriteLine("Binary data array is null.");
                return "";
            }
            return base64String;
            //// Write the UUEncoded version to the output file.
            //System.IO.StreamWriter outFile;
            //try
            //{
            //   outFile = new System.IO.StreamWriter(outputFileName,
            //                               false,
            //                               System.Text.Encoding.ASCII);
            //   outFile.Write(base64String);
            //   outFile.Close();
            //}
            //catch (System.Exception exp)
            //{
            //   // Error creating stream or writing to it.
            //   System.Console.WriteLine("{0}", exp.Message);
            //}
        }

        #endregion

    }
    public static class SharedStatic
    {
        //public static void MakeHttpPost(string PostURL, string PostContent)
        //{
        //    try
        //    {
        //        //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.PostUrl);
        //        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(PostURL);

        //        req.Method = "POST";

        //        //req.ContentLength = this.RequestLength.Length + 21;
        //        req.ContentLength = PostContent.Length;
        //        req.ContentType = "application/x-www-form-urlencoded";

        //        byte[] param = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);
                

        //        req.ContentLength = PostContent.Length;

        //        StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
        //        streamOut.Write(PostContent);
        //        streamOut.Close();
        //        StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
        //        string response = streamIn.ReadToEnd();
        //        streamIn.Close();
        //        //output.Text = response;

        //    }
        //    catch
        //    {

        //    }

        //}

        public static string MakeHttpHtmlGet(string GetURL,int encodint)
        {
            string rethtml = "";
            try
            {
                //HttpWebRequest req = (HttpWebRequest)WebRequest.Create(this.PostUrl);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GetURL);
                //<meta http-equiv="Content-Type" content="text/html; charset=windows-1252">

                req.Method = "GET";
                System.Net.HttpWebResponse objResponse = (System.Net.HttpWebResponse)req.GetResponse();
              
                //string Charset = objResponse.CharacterSet;
                //Encoding encoding = Encoding.GetEncoding(Charset);
                Encoding encoding = Encoding.GetEncoding(encodint); 

                StreamReader streamIn = new StreamReader(objResponse.GetResponseStream(), encoding);
                string response = streamIn.ReadToEnd();
                streamIn.Close();
                rethtml = response;

            }
            catch 
            {

            }
            return rethtml;
        }
        /// <summary>
        /// Copies the contents of input to output. Doesn't close either stream.
        /// </summary>
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }
        public static void MakeHttpGet(string GetURL, string destPath)
        {
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(GetURL);
                req.Method = "GET";
                byte[] param = HttpContext.Current.Request.BinaryRead(HttpContext.Current.Request.ContentLength);
                // Write out the decoded data.
                using (System.IO.FileStream outFile = new System.IO.FileStream(destPath, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                {
                    try
                    {
                        // req.GetResponse().GetResponseStream().Seek(0, SeekOrigin.Begin);
                        // req.GetResponse().GetResponseStream().CopyTo(outFile);
                        CopyStream(req.GetResponse().GetResponseStream(), outFile);
                    }
                    catch (System.Exception exp)
                    {
                        // Error creating stream or writing to it.
                        System.Console.WriteLine("{0}", exp.Message);
                    }
                }

            }
            catch
            {

            }
        }



        public static string Get(string uri)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch { }
            return "";
        }
        public static async System.Threading.Tasks.Task<string> GetAsync(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }

        /*
         
METODO PER POSTARE DA CODICE I DATI IN JSON (esempio )
#if true
                    //PASSO IL CLIENTE AL SISTEMA DI GESTIONE TRADING SYSTEM
                    if (cliente != null && tipocontenuto1 == "prova gratuita")
                    {
                        string clienteserialized = Newtonsoft.Json.JsonConvert.SerializeObject(cliente, Newtonsoft.Json.Formatting.Indented, new JsonSerializerSettings()
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            MissingMemberHandling = MissingMemberHandling.Ignore,
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                            PreserveReferencesHandling = PreserveReferencesHandling.None,
                        });
                        SharedStatic.Post(ConfigManagement.ReadKey("urlfortrial"), clienteserialized, "application/json");
                    }
#endif
         */
        public static string Post(string uri, string data, string contentType, string method = "POST")
        {
            try
            {
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                request.ContentLength = dataBytes.Length;
                request.ContentType = contentType;
                request.Method = method;

                using (Stream requestBody = request.GetRequestStream())
                {
                    requestBody.Write(dataBytes, 0, dataBytes.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception err) { string message = err.Message; }
            return "";
        }
        public static async System.Threading.Tasks.Task<string> PostAsync(string uri, string data, string contentType, string method = "POST")
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ContentLength = dataBytes.Length;
            request.ContentType = contentType;
            request.Method = method;

            using (Stream requestBody = request.GetRequestStream())
            {
                await requestBody.WriteAsync(dataBytes, 0, dataBytes.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }


        /// <summary>
        /// Prende un file da un indirizzo Url e lo passa in uno stream per riferimento
        /// al chiamante
        /// </summary>
        /// <param name="myUrl"></param>
        /// <param name="DestStream"></param>
        /// <returns></returns>
        public static int GetFile(string myUrl, ref MemoryStream DestStream)
        {
            WebResponse response = null;
            long FileSize = 0;
            long DownloadedData = 0;
            int ret = 0;
            try
            {
                WebRequest request = WebRequest.Create(myUrl);
                request.Timeout = 30000;
                response = request.GetResponse();
                FileSize = response.ContentLength;

                Stream responseStream = response.GetResponseStream();

                MemoryStream ms = new MemoryStream();

                byte[] buffer = new Byte[1024];
                int count = responseStream.Read(buffer, 0, buffer.Length);
                DownloadedData = count;
                while (count > 0)
                {
                    ms.Write(buffer, 0, count);
                    count = responseStream.Read(buffer, 0, buffer.Length);
                    DownloadedData += count;
                }
                ms.Flush();
                ms.Seek(0, SeekOrigin.Begin);
                responseStream.Close();

                if (FileSize == DownloadedData)
                {
                    //Passo il memorystream nello stream dei parametri
                    //per riferimento
                    //ms.WriteTo(retstream);
                    //ms.Close();
                    DestStream = ms;
                    //DestStream = retstream;
                }
                else
                {
                    DestStream = null;
                }

            }
            catch (UriFormatException)
            {
                ret = -1;
            }
            catch (WebException)
            {
                ret = -2;
            }
            catch (IOException)
            {
                ret = -3;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
            return ret;
        }


        public static void DownloadFile(string url)
        {

            string fileName = String.Empty;
            string filePath = String.Empty;


            if (!String.IsNullOrEmpty(url))
            {

                filePath = HttpContext.Current.Server.MapPath(url);
                fileName = System.IO.Path.GetFileName(filePath);
                FileInfo finfo = new FileInfo(filePath);
                if (finfo == null) return;

                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();

                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                // Add the file size into the response header (Devi calcolarti il FileSize!!)
                HttpContext.Current.Response.AddHeader("Content-Length", finfo.Length.ToString());

                HttpContext.Current.Response.TransmitFile(filePath);

                //HttpContext.Current.Response.AddHeader("Content-Disposition",
                //"inline; filename=" + fileName);
                //HttpContext.Current.Response.WriteFile(filePath);
                HttpContext.Current.Response.End();

            }
        }
        public static void DownloadFile(string url, bool cancellafile)
        {

            string fileName = String.Empty;
            string filePath = String.Empty;


            if (!String.IsNullOrEmpty(url))
            {

                filePath = HttpContext.Current.Server.MapPath(url);
                fileName = System.IO.Path.GetFileName(filePath);
                FileInfo finfo = new FileInfo(filePath);
                if (finfo == null) return;

                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();

                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                // Add the file size into the response header (Devi calcolarti il FileSize!!)
                HttpContext.Current.Response.AddHeader("Content-Length", finfo.Length.ToString());

                HttpContext.Current.Response.TransmitFile(filePath);

                //HttpContext.Current.Response.AddHeader("Content-Disposition",
                //"inline; filename=" + fileName);
                //HttpContext.Current.Response.WriteFile(filePath);
                HttpContext.Current.Response.Flush();
                finfo.Delete();
                HttpContext.Current.Response.End();

            }
        }


        public static void WriteToFile(string vFile, string pathFile, string vMessage, bool rigenera = false)
        {
            if (pathFile != null)
            {
                if (pathFile.Substring(pathFile.Length - 1) != "\\" && pathFile.Substring(pathFile.Length - 1) != "/")
                    pathFile += "\\";
            }
            else
                pathFile = "";
            string FileName = vFile;
            string FilePath = pathFile + FileName;

            try
            {
                if (System.IO.File.Exists(FilePath) && rigenera)
                {

                    System.IO.File.Delete(FilePath);
                }

                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(FilePath, true, System.Text.Encoding.UTF8))
                {
                    sw.WriteLine(vMessage);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Source + "\r\n" + ex.StackTrace);
            }
        }

    }
}
