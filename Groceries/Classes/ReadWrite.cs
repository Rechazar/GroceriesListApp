using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace Groceries.Classes
{
    public class ReadWrite
    {
        static readonly string mainPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        static readonly string userPath = Path.Combine(mainPath, "user.json");
        static readonly string dataPath = Path.Combine(mainPath, "data.json");

        //write data
        public static void WriteData()
        {
            //lists in current Lists that belongs to us and write them down
            AppData.offlineLists = new List<GroceryListClass>();
            //checks if the current list is not empty
            if (AppData.currentLists != null)
            {
                //goes through all lists in currentLists
                foreach (GroceryListClass any in AppData.currentLists)
                {
                    //checks if any of those lists belong to the current user via id
                    if (any.Owner.Uid == AppData.currentUser.Uid)
                    {
                        //adds those lists to the offline list
                        AppData.offlineLists.Add(any);
                    }
                }
            }
            //convert offline list from currentLists into Json string then write to Json dataPath
            string dataJson = JsonConvert.SerializeObject(AppData.offlineLists, Formatting.Indented);
            File.WriteAllText(dataPath, dataJson);
        }

        //read data
        public static void ReadData()
        {
            //new empty list
            AppData.offlineLists = new List<GroceryListClass>();

            //open dataPath if exists
            if (File.Exists(dataPath))
            {
                using StreamReader file = File.OpenText(dataPath);
                JsonSerializer serializer = new JsonSerializer();
                AppData.offlineLists = (List<GroceryListClass>)serializer.Deserialize(file, typeof(List<GroceryListClass>));
            }
        }

        //write user
        public static void WriteUser()
        {
            string userJson = JsonConvert.SerializeObject(AppData.currentUser, Formatting.Indented);
            File.WriteAllText(userPath, userJson);
        }

        //read user
        public static void ReadUser()
        {
            if (File.Exists(userPath))
            {
                using StreamReader file = File.OpenText(userPath);
                JsonSerializer serializer = new JsonSerializer();
                AppData.currentUser = (UserClass)serializer.Deserialize(file, typeof(UserClass));
            }
        }
    }
}