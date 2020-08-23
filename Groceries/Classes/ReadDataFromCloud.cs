using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Firebase.Database.Query;

namespace Groceries.Classes
{

    public class TempGroceryListClass
    {
        public string Name { get; set; }
        public UserClass Owner { get; set; }
    }


    public static class ReadDataFromCloud
    {
        public static async Task Read()
        {
            AppData.onlineLists = new List<GroceryListClass>();

            if (AppData.auth.CurrentUser == null)
            {
                return;
            }


            ChildQuery listsNode = AppData.dataNode.Child(AppData.currentUser.Uid);

            var allListsData = await listsNode.OnceAsync<TempGroceryListClass>();

            foreach (FirebaseObject<TempGroceryListClass> any in allListsData)
            {
                List<ItemClass> itemsOfList = new List<ItemClass>();

                ChildQuery thisListNode = listsNode.Child(any.Object.Name);

                var readItems = await thisListNode.Child("Items").OnceAsync<ItemClass>();

                foreach (FirebaseObject<ItemClass> anyItem in readItems)
                {
                    itemsOfList.Add(anyItem.Object);
                }

                GroceryListClass thisList = new GroceryListClass
                {
                    Name = any.Object.Name,
                    Items = itemsOfList,
                    Owner = any.Object.Owner
                };

                AppData.onlineLists.Add(thisList);
            }
        }
    }
}