using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Firestore;
using FirebaseProject.Adapter;

namespace FirebaseProject
{
    public class MyItemTouchHelper : ItemTouchHelper.Callback
    {
        CollectionReference colRef;
        

        public MyItemTouchHelper(CollectionReference colRef)
        {
            
            this.colRef= colRef;
        }
        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            int dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;
            int swipeFlags = ItemTouchHelper.Start | ItemTouchHelper.End;
            return MakeMovementFlags(dragFlags, swipeFlags);
        }

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return true;
        }
        
        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            var holder = viewHolder as ExamAdapterViewHolder;
            string examname = holder.examNameText.Text;
            DocumentReference docRef = colRef.Document(examname);
            docRef.Delete();
        }
    }
}