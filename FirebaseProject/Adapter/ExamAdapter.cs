using System;

using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using FirebaseProject.Models;
using System.Collections.Generic;
using Firebase.Firestore;

namespace FirebaseProject.Adapter
{
    class ExamAdapter : RecyclerView.Adapter 
    {
        public Android.Support.V7.Widget.RecyclerView.ContextClickEventArgs ContextClickEventArgs;
        public Android.Support.V7.Widget.RecyclerView recyclerView;
        public event EventHandler<ExamAdapterClickEventArgs> ItemClick;
        public event EventHandler<ExamAdapterClickEventArgs> ItemLongClick;
        List<ExamModel> Items;
        CollectionReference colRef;



        public ExamAdapter(Android.Support.V7.Widget.RecyclerView.ContextClickEventArgs ContextClickEventArgs, Android.Support.V7.Widget.RecyclerView recyclerView, List<ExamModel>Exams)
        {
            this.ContextClickEventArgs = ContextClickEventArgs;
            this.recyclerView = recyclerView;
            Items = Exams;
        }

        // Create new views (invoked by the layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {

            //Setup your layout here
            View itemview = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.examrow, parent, false);
            var vh = new ExamAdapterViewHolder(itemview, OnClick, OnLongClick);
            return vh;
        }

        // Replace the contents of a view (invoked by the layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder viewHolder, int position)
        {
            var exam = Items[position];
            var holder = viewHolder as ExamAdapterViewHolder;
            //holder.TextView.Text = items[position];
            holder.examNameText.Text = exam.examName;
            holder.examDateText.Text = exam.examDateText;    
            holder.examMemorizedText.Text = exam.examMemorizedText;
            holder.examNameText.Click += ItemView_Click;
        }

        private void ItemView_Click(object sender, EventArgs e)
        {
            int position = this.recyclerView.GetChildAdapterPosition((View)sender);
            ExamModel examname_clicked = this.Items[position];
            string examname = examname_clicked.get_exam_name();
            DocumentReference docRef = colRef.Document(examname);
            docRef.Update("examname", "Update");
        }

        public override int ItemCount => Items.Count;

        void OnClick(ExamAdapterClickEventArgs args) => ItemClick?.Invoke(this, args);
        void OnLongClick(ExamAdapterClickEventArgs args) => ItemLongClick?.Invoke(this, args);       
    }

    public class ExamAdapterViewHolder : RecyclerView.ViewHolder
    {
        //public TextView TextView { get; set; }
        public TextView examNameText { get; set; }
        public TextView examDateText { get; set; }      
        public TextView examMemorizedText { get; set; }
        public ImageView deleteButton { get; set; }
        public CheckBox checkboxButton { get; set; }

        public ExamAdapterViewHolder(View itemView, Action<ExamAdapterClickEventArgs> clickListener,
                            Action<ExamAdapterClickEventArgs> longClickListener) : base(itemView)
        {
            //TextView = v;
            examNameText = (TextView)itemView.FindViewById(Resource.Id.examnameText);
            examDateText = (TextView)itemView.FindViewById(Resource.Id.examdateTextEntry);
            examMemorizedText = (TextView)ItemView.FindViewById(Resource.Id.exammemorizedText);
            deleteButton = (ImageView)itemView.FindViewById(Resource.Id.deleteButton);
            checkboxButton = (CheckBox)itemView.FindViewById(Resource.Id.checkbox);
            
            itemView.Click += (sender, e) => clickListener(new ExamAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            itemView.LongClick += (sender, e) => longClickListener(new ExamAdapterClickEventArgs { View = itemView, Position = AdapterPosition });
            
        }
    }

    public class ExamAdapterClickEventArgs : EventArgs
    {
        public View View { get; set; }
        public int Position { get; set; }
    }
}