using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database.Query;

namespace Groceries.Classes
{
    public static class SaveListOnCloud
    {
        public static void Save(GroceryListClass inpList)
        {
            if (AppData.auth.CurrentUser == null)
            {
                return;
            }


            var allItemsDict = new Dictionary<string, object> { };

            foreach (ItemClass anyItem in inpList.Items)
            {
                allItemsDict.Add(anyItem.Name, anyItem);
            }

            var mainDict = new Dictionary<string, object>
            {
                { "Items", allItemsDict},
                { "Name", inpList.Name},
                { "Owner", inpList.Owner}
            };

            if (allItemsDict.Count == 0)
            {
                mainDict.Remove("Items");
            }



            AppData.dataNode
                   .Child(AppData.currentUser.Uid)
                   .Child(inpList.Name)
                   .PutAsync(mainDict);
        }
    }
}