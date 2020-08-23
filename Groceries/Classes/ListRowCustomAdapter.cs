using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Groceries.Classes
{
    public class ListRowCustomAdapter : BaseAdapter<GroceryListClass>
    {
        readonly List<GroceryListClass> recievedList; //AppData.currentLists
        readonly Activity myContext;




        public ListRowCustomAdapter(Activity inpContext, List<GroceryListClass> inpLists) : base()
        {
            recievedList = inpLists;
            myContext = inpContext;
        }





        public override GroceryListClass this[int position]
        {
            get
            {
                return recievedList[position];
            }
        }




        public override int Count
        {
            get
            {
                return recievedList.Count;
            }
        }




        public override long GetItemId(int position)
        {
            return position;
        }




        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;
            if (view == null)
            {
                view = myContext.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);
            }

            view.SetMinimumHeight(128 * (int) Android.Content.Res.Resources.System.DisplayMetrics.Density);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = recievedList[position].Name;
            view.FindViewById<TextView>(Android.Resource.Id.Text1).TextSize = 22;
            view.FindViewById<TextView>(Android.Resource.Id.Text1).SetTypeface(null, TypefaceStyle.Bold);

            string subStr = recievedList[position].Items.Count.ToString() + " items for " + recievedList[position].Owner.Name;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = subStr;
            view.FindViewById<TextView>(Android.Resource.Id.Text2).SetTextColor(Color.DarkGray);

            if (recievedList[position].Owner.Uid != AppData.currentUser.Uid)
            {
                view.FindViewById<TextView>(Android.Resource.Id.Text2).SetTextColor(Color.Red);
                view.FindViewById<TextView>(Android.Resource.Id.Text2).SetTypeface(null, TypefaceStyle.Bold);
            }

            return view;
        }
    }
}