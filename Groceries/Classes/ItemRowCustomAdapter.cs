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
   public class ItemRowListAdapter : BaseAdapter<ItemClass>
    {
		readonly List<ItemClass> receivedItemsLST;
		readonly Activity myContext;

		public override ItemClass this[int position]
		{
			get
			{
				return receivedItemsLST[position];
			}
		}

		public ItemRowListAdapter(Activity context, List<ItemClass> inpItems) : base()
		{
			this.myContext = context;
			this.receivedItemsLST = inpItems;
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override int Count
		{
			get { return receivedItemsLST.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView;
			if (view == null)
			{
				view = myContext.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItemChecked, null); 
			}



			var checkedTextView = view.FindViewById<CheckedTextView>(Android.Resource.Id.Text1);

			ItemClass thisItem = receivedItemsLST[position];

			checkedTextView.Text = thisItem.Name;
			checkedTextView.SetHeight(128);

			bool curStatus = false;
			if (thisItem.Purchased == "True")
				curStatus = true;

			checkedTextView.Checked = curStatus;
			if (curStatus)
			{
				checkedTextView.SetBackgroundColor(Color.DarkGray);
				checkedTextView.PaintFlags = PaintFlags.StrikeThruText | PaintFlags.AntiAlias | PaintFlags.SubpixelText;
				checkedTextView.SetTextColor(Color.LightGray);
			}
			else
			{
				checkedTextView.SetBackgroundColor(Color.LightGray);
				checkedTextView.PaintFlags = PaintFlags.LinearText | PaintFlags.AntiAlias | PaintFlags.SubpixelText; ;
				checkedTextView.SetTextColor(Color.Black);
			}


			return view;
		}

	}
}