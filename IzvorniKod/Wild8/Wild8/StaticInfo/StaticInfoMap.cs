using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace Wild8.StaticInfo
{
    public class StaticInfoMap
    {
        //Owner info keys
        public static readonly string OwnerName = "own-name";
        public static readonly string OwnEmail = "own-email";
        public static readonly string HomePageOwnInfo = "home-page-own-info";
        public static readonly string ConPageOwnInfo = "con-page-own-info";
        public static readonly string OwnHomePagePic = "own-home-page-pic";
        public static readonly string OwnConPagePic = "own-con-page-pic";

        //Restaurant info keys
        public static readonly string RestName = "Wild8";
        public static readonly string RestAddr = "rest-addr";
        public static readonly string RestCity = "rest-city";
        public static readonly string RestPhone = "rest-phone";
        public static readonly string RestEmail = "wild8opp@gmail.com";
        public static readonly string RestEmailPass = "pass_wild8"; // Cannot be hashed, needs to be raw for gmail
        public static readonly string RestMailSmtpClient = "smtp.gmail.com";
        public static readonly string RestStartH = "rest-start-h";
        public static readonly string RestStartM = "rest-start-m";
        public static readonly string RestEndH = "rest-end-h";
        public static readonly string RestEndM = "rest-end-m";
        
        private static readonly string DataFilePath   = HostingEnvironment.MapPath("/App_Data/info.dat");
        private static readonly string CaroselPicPath = HostingEnvironment.MapPath("/App_Data/pic.dat");

        private static Object lockObject = new object();
        private static StaticInfoMap instance;

        private Dictionary<string, string> infoMap;
        private List<string> picList; 

        private StaticInfoMap()
        {
            instance = new StaticInfoMap();
            LoadFromFile();
        }

        public StaticInfoMap Instance => instance ?? new StaticInfoMap();

        public void Save(string key, string value)
        {
            lock (infoMap)
            {
                infoMap.Add(key, value);
                SaveDataToFile();
            }
        }

        public void MultipleSave(string[] keys, string[] values)
        {
            if (keys.Length != values.Length)
            {
                throw new ArgumentException("Keys and values length are not the same");
            }
            lock (infoMap)
            {
                for (int i = 0; i < keys.Length; i++)
                {
                    infoMap.Add(keys[i], values[i]);
                }
                SaveDataToFile();
            }
        }

        public string GetInfo(string key)   //Get is not a thread safe problem
        {
            var info = infoMap[key];
            return info ?? "";
        }

        public void SaveCaroselPic(string[] picturePaths)
        {
            lock (picList)
            {
                picList.Clear();
                foreach (var picturePath in picturePaths)
                {
                    picList.Add(picturePath);
                }
                SavePicturesToFile();
            }
        }

        public string[] GetCaroselPictures()
        {
            return picList.ToArray();
        }
        
        private void SaveDataToFile()
        {
            lock (infoMap)
            {
                string JSON = JsonConvert.SerializeObject(infoMap, Formatting.Indented);
                File.WriteAllText(DataFilePath, JSON);
            }
        }

        private void SavePicturesToFile()
        {
            lock (picList)
            {
                string JSON = JsonConvert.SerializeObject(picList, Formatting.Indented);
                File.WriteAllText(CaroselPicPath, JSON);
            }
        }

        private void LoadFromFile()
        {
            lock (lockObject) { 
                if(!File.Exists(DataFilePath))
                {
                    infoMap = new Dictionary<string, string>();
                }
                else
                {
                    var file = File.ReadAllText(DataFilePath);
                    infoMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(file);
                }

                if (!File.Exists(CaroselPicPath))
                {
                    picList = new List<string>();
                }
                else
                {
                    var file = File.ReadAllText(CaroselPicPath);
                    picList = JsonConvert.DeserializeObject<List<string>>(file);
                }
            }
        }
    }
}