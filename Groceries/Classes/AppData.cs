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
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;

namespace Groceries.Classes
{
    public class AppData
    {
        private static AppData instance; //singleton instance of AppData
        public static List<GroceryListClass> currentLists;
        public static UserClass currentUser;

        public static List<GroceryListClass> offlineLists;

        public static List<GroceryListClass> onlineLists;

        public static List<GroceryListClass> invitationLists;
        public static List<InvitationClass> invitationData;

        public static ChildQuery dataNode;
        public static ChildQuery usersNode;
        static FirebaseApp fireApp;
        public static FirebaseAuth auth;


        public static AppData GetInstance(Context withContext)
        {
            if (instance == null)
            {
                instance = new AppData(withContext);
            }
            return instance;
        }

        //initializer
        private AppData(Context thisContext)
        {
            currentLists = new List<GroceryListClass>();

            var options = new Firebase.FirebaseOptions.Builder()
                                      .SetApplicationId("1:269645497682:android:41aae10df759a3e12da68b")
                                      .SetApiKey("AIzaSyDrsPpcJX-o6wnrhbfY5PbOW5frpoNE2MA")
                                      .Build();

            if (fireApp == null)
            {
                fireApp = FirebaseApp.InitializeApp(thisContext, options);
            }

            auth = FirebaseAuth.GetInstance(fireApp);

            string FirebaseURL = "https://groceries-project.firebaseio.com";
            FirebaseClient rootNode = new FirebaseClient(FirebaseURL);
            dataNode = rootNode.Child("data");
            usersNode = rootNode.Child("users");

        }

    }
}