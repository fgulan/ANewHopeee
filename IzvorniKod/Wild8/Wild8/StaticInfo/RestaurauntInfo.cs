using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Wild8.StaticInfo
{
    public class RestaurauntInfo
    {
        private static CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
        private static readonly string DataFilePath = "info.dat";
        private static readonly RestaurauntInfo instance;
        private Object lockObject = new Object();

        public string RestaurantEmail { get; set; }
        public string OwnerName { get; set; }
        public string RestaurauntName { get; set; }
        public string OwnerHomepageInfo { get; set; }
        public string OwnerContactInfo { get; set; }
        public string OwnerCity { get; set; }
        public string OwnerAddress { get; set; }
        public string OwnerPhone { get; set; }
        public string OwnerEMail { get; set; }
        public string RestStartH { get; set; }
        public string RestStartM { get; set; }
        public string RestEndH { get; set; }
        public string RestEndM { get; set; }
        public string RestauranutHomepageInfo { get; set; }
        public decimal MinimalOrderPrice { get; set; }

        static RestaurauntInfo()
        {
            instance = LoadJSONData();
        }

        private RestaurauntInfo()
        {
        }

        private void Initialize()
        {
            RestaurantEmail = "wild8@fer.hr";
            OwnerName = "Mateo Marcelic";
            RestaurauntName = "Wild 8";
            OwnerHomepageInfo = "Rođen kao 3. član obitelji Marcelić, u Zadru, 30. prosinca 1994. godine." + " Školovan u gimnaziji Jurja Barakovića u Zadru, prirodoslovno-matematički smjer."
                +" Kao neuspješni student FER-a uspješnu karijeru potražuje u ugostiteljstvu. Porijeklo iz ribarske obitelji nudilo je dobro poznavanje ribarstva,"
                +" ribarskih običaja, te ribarskih tehnika koje mu omogućuju da to iskoristi za vrhunsku pripremu jela s ribom i morskim plodovima, osobito na gradelama."
                +" Duboko ukorijenjena tradicija u njegovoj obitelji pogodovala je stvaranju bogate kuhinje pune morskih delikatesa. Svoje kuharske i gurmanske vještine utemeljio je u svom prvom restoranu \"Wild8\" u Zagrebu na ljeto 2014. godine,"
                +" gdje ubrzo ostvaruje visok ugled. Kasnije, svoje vještine proširuje na ostala jela pripremljena na roštilju i uspješno postaje poznat po cijeloj Republici kao vlasnik najboljeg restorana koji priprema jela s roštilja.";
            OwnerCity = "poštanski broj, ime grada";
            OwnerAddress = "nekakva adresa";
			OwnerPhone = "+(385) 1234 1234";
			OwnerEMail = "restaurant@wild8.me";
            RestStartH = "09";
            RestStartM = "00";
            RestEndH = "23";
            RestEndM = "00";
            OwnerContactInfo = "“Kuhanje je moja strast, moja antistres terapija. Moj hobi koji je prerastao u stil života."
				+ " Otkad znam za sebe kuham, a sve se moje slobodne aktivnosti i prijateljska druženja odvijaju upravo u meni najdražoj prostoriji – kuhinji."
				+ " Kuhanje ne doživljavam kao rad, više kao neku vrstu osobnog zadovoljstva. Kroz kuhanje sam upoznao mnoštvo divnih ljudi koji dijele slične interese,"
				+ " te s nekima postao i jako dobar izvan tog kuharskog svijeta, što je jedna od ljepših stvari koje mi je kuhanje donijelo. A kada dobivate pohvale od vjernih gostiju,"
				+ " to zadovoljstvo ne može zamijeniti ništa. Na kraju dana legnete u krevet napunjeni pozitivnom energijom, koja vam daje snage za nove radne pobjede!”";
            RestauranutHomepageInfo = "dgfdsfds";
            RestStartH = "08";
            RestStartM = "00";
            RestEndH = "23";
            RestEndM = "00";
            MinimalOrderPrice = 30M;
            SaveData();
        }

        private static string LinesToString(string[] lines)
        {
            string tempString = "";
            foreach (string line in lines)
            {
                tempString += line + "\n";
            }
            return tempString;
        }

        private void UploadJSON(string JSON)
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("info");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(DataFilePath);
            blockBlob.UploadText(JSON);
        }

        private static string LoadJSON()
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("info");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(DataFilePath);
            if (!blockBlob.Exists())
            {
                return null;
            }
            return blockBlob.DownloadText();
        }


        public void SaveData()
        {
            lock (lockObject)
            {
                string JSON = JsonConvert.SerializeObject(this, Formatting.Indented);
                UploadJSON(JSON);
            }
        }

        private static RestaurauntInfo LoadJSONData()
        {
            string JSON = null;
            RestaurauntInfo obj;
            if (JSON == null)
            {
                obj = new RestaurauntInfo();
                obj.Initialize();
            } else
            {
                obj = JsonConvert.DeserializeObject<RestaurauntInfo>(JSON);
            }
            return obj;
        }

        public static RestaurauntInfo Instance
        {
            get
            {
                return instance;
            }
        }
    }
}