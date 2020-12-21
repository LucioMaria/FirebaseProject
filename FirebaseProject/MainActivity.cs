using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Support.V7.Widget;
using System;
using FirebaseProject.Models;
using System.Collections.Generic;
using FirebaseProject.Adapter;
using FirebaseProject.Fragments;
using Firebase.Firestore;
using Android.Gms.Tasks;
using Firebase;
using Java.Lang;
using Xamarin.Forms;
using Android.Support.V7.Widget.Helper;

namespace FirebaseProject
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnSuccessListener, IEventListener
    {
        ImageView searchButton;
        ImageView addButton;
        EditText searchText;
        RecyclerView rv;
        ExamAdapter adapter;
        FirebaseFirestore database;
        public Android.Support.V7.Widget.RecyclerView.ContextClickEventArgs ContextClickEventArgs;


        List<ExamModel> ExamList = new List<ExamModel>();




        AddExamFragment addExamFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            Forms.SetFlags("SwipeView_Experimental");
            rv = (RecyclerView)FindViewById(Resource.Id.myRecyclerView);
            searchButton = (ImageView)FindViewById(Resource.Id.searchButton);
            addButton = (ImageView)FindViewById(Resource.Id.addButton);
            searchText = (EditText)FindViewById(Resource.Id.search_text);
            searchButton.Click += SearchButton_Click;
            addButton.Click += AddButton_Click;
            database = GetDatabase();
            FetchandListen();
            SetupRecyclerView();
            ItemTouchHelper.Callback callback = new MyItemTouchHelper(database.Collection("exams"));
            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(rv);
        }


        private void AddButton_Click(object sender, EventArgs e)
        {
            addExamFragment = new AddExamFragment();
            var transaction = SupportFragmentManager.BeginTransaction();
            addExamFragment.Show(transaction, "add exam");
        }

        public FirebaseFirestore GetDatabase()
        {
            var app = FirebaseApp.InitializeApp(this);
            FirebaseFirestore database;
            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                .SetProjectId("fir-project-16446")
                .SetApplicationId("fir-project-16446")
                .SetApiKey("AIzaSyB4tFXBV6P6AiHCZmsjNNEWlF_9eXncoQg")
                .SetDatabaseUrl("https://fir-project-16446.firebaseio.com")
                .SetStorageBucket("fir-project-16446.appspot.com")
                .Build();

                app = FirebaseApp.InitializeApp(this, options);
                database = FirebaseFirestore.GetInstance(app);
            }
            else
            {
                database = FirebaseFirestore.GetInstance(app);
            }
            return database;
        }

        void FetchData()
        {
            database.Collection("exams").Get()
                .AddOnSuccessListener(this);
        }

        void FetchandListen()
        {
            database.Collection("exams").AddSnapshotListener(this);
        }

        private void SetupRecyclerView()
        {
            rv.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(rv.Context));
            adapter = new ExamAdapter(ContextClickEventArgs,rv,ExamList);
            adapter.ItemLongClick += Adapter_ItemLongClick;
            rv.SetAdapter(adapter);
        }

        private void Adapter_ItemLongClick(object sender, ExamAdapterClickEventArgs e)
        {
            string examID = ExamList[e.Position].Id;
            DocumentReference docRef = database.Collection("exams").Document(examID);
            docRef.Delete();
        }

        private void SearchButton_Click(object sender, System.EventArgs e)
        {
            if (searchText.Visibility == Android.Views.ViewStates.Gone)
            {
                searchText.Visibility = Android.Views.ViewStates.Visible;
            }
            else
            {
                searchText.ClearFocus();
                searchText.Visibility = Android.Views.ViewStates.Gone;
            }
        }

       

        public void OnEvent(Java.Lang.Object value, FirebaseFirestoreException error)
        {
            var snapshot = (QuerySnapshot)value;
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                ExamList.Clear();
                foreach (DocumentSnapshot item in documents)
                {
                    ExamModel exam = new ExamModel();
                    exam.Id = item.Id;
                    exam.examName = item.Get("examname").ToString();
                    exam.examDateText = item.Get("examdate").ToString();

                    ExamList.Add(exam);
                }
                if (adapter !=null)
                {
                    adapter.NotifyDataSetChanged();
                }
            }
        }
        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;
            if (!snapshot.IsEmpty)
            {
                var documents = snapshot.Documents;
                ExamList.Clear();
                foreach (DocumentSnapshot item in documents)
                {
                    ExamModel exam = new ExamModel();
                    exam.examName = item.Get("examname").ToString();
                    exam.examDateText = item.Get("examdate").ToString();

                    ExamList.Add(exam);
                }
            }
        }      
    }
}