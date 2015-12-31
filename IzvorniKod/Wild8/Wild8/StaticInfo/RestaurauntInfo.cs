using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Wild8.StaticInfo
{
    public class RestaurauntInfo
    {
        private static readonly string DataFilePath = "C:\\test.txt";
        private static readonly RestaurauntInfo instance;
        private Object lockObject = new Object();


        private volatile string ownerHomepageInfo;
        public string OwnerHomepageInfo { get { return ownerHomepageInfo; } set { ownerHomepageInfo = value; SaveData(); } }

        private volatile string ownerHomepagePhotoPath;
        public string OwnerHomepagePhotoPath { get { return ownerHomepagePhotoPath; } set { ownerHomepagePhotoPath = value; SaveData(); } }

        private volatile string ownerContactInfo;
        public string OwnerContactInfo { get { return ownerContactInfo; } set { ownerContactInfo = value; SaveData(); } }

        private volatile string ownerContactPhotoPath;
        public string OwnerContactPhotoPath { get { return ownerContactPhotoPath; } set { ownerContactPhotoPath = value; SaveData(); } }

        private volatile string ownerCity;
        public string OwnerCity { get { return ownerCity; } set { ownerCity = value; SaveData(); } }

        private volatile string ownerAddress;
        public string OwnerAddress { get { return ownerAddress; } set { ownerAddress = value; SaveData(); } }


        private volatile string firstHomepagePhotoPath;
        public string FirstHomepagePhotoPath { get { return firstHomepagePhotoPath; } set { firstHomepagePhotoPath = value; SaveData(); } }

        private volatile string secondHomepagePhotoPath;
        public string SecondHomepagePhotoPath { get { return secondHomepagePhotoPath; } set { secondHomepagePhotoPath = value; SaveData(); } }

        private volatile string thirdHomepagePhotoPath;
        public string ThirdHomepagePhotoPath { get { return thirdHomepagePhotoPath; } set { thirdHomepagePhotoPath = value; SaveData(); } }


        private volatile string restaurauntHomepagePhotoPath;
        public string RestaurauntHomepagePhotoPath { get { return restaurauntHomepagePhotoPath; } set { restaurauntHomepagePhotoPath = value; SaveData(); } }

        private volatile string restauranutHomepageInfo;
        public string RestauranutHomepageInfo { get { return restauranutHomepageInfo; } set { restauranutHomepageInfo = value; SaveData(); } }

        static RestaurauntInfo()
        {
            instance = LoadJSONData();
        }

        private RestaurauntInfo()
        {
        }

        private void Initialize()
        {
            ownerHomepageInfo = "Odsgdsfdsg";
            ownerHomepagePhotoPath = "dfdffsdsd";
            ownerContactInfo = "dfdsfds";
            ownerContactPhotoPath = "fddsfsdfsdfsdf";
            ownerCity = ";fdfsfs";
            ownerAddress = "dfdsfsd";
            firstHomepagePhotoPath = "dfsfsf";
            secondHomepagePhotoPath = "fdsfdsfsd";
            thirdHomepagePhotoPath = "dfdsfd";
            restauranutHomepageInfo = "dgfdsfds";
            restaurauntHomepagePhotoPath = "sdgdsdsgdsg";
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

        private void SaveData()
        {
            lock (lockObject)
            {
                string JSON = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(DataFilePath, JSON);
            }
        }

        private static RestaurauntInfo LoadJSONData()
        {
            if (!File.Exists(DataFilePath))
            {
                using (File.Create(DataFilePath)) { };
                RestaurauntInfo info = new RestaurauntInfo();
                info.Initialize();
                return info;
            }
            else
            {
                string[] lines = File.ReadAllLines(DataFilePath);
                string oneLiner = LinesToString(lines);
                RestaurauntInfo obj = JsonConvert.DeserializeObject<RestaurauntInfo>(oneLiner);
                return obj;
            }
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