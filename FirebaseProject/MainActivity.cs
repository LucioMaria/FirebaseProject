using Android.Util;
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
using Android.Views;
using Google.Android.Material.AppBar;
using System.Threading.Tasks;
using Firebase.Database;
using System.IO;
using Plugin.CloudFirestore;
using Google.Android.Material.TextField;

namespace FirebaseProject
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private ImageView imageView;
        ImageView addButton;
        RecyclerView rv;
        ExamAdapter adapter;
        // FirestoreDb database;
        MaterialToolbar topAppBar;
        IOnCompleteListener OnCompleteListener;




        List<ExamModel> ExamList = new List<ExamModel>();




        AddExamFragment addExamFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            string path = Path.Combine(Xamarin.Essentials.FileSystem.CacheDirectory, "db.json");
            System.Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            Forms.SetFlags("SwipeView_Experimental");
            rv = (RecyclerView)FindViewById(Resource.Id.recicler_view_exams);
            imageView = (ImageView)FindViewById(Resource.Id.empty_recycler_image);
            addButton = (ImageView)FindViewById(Resource.Id.add_button_exam);
            // addButton = (ImageView)FindViewById(Resource.Id.addButton)
            Android.Support.V7.Widget.Toolbar topAppBar = this.FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.topAppBar);
            SetSupportActionBar(topAppBar);
            topAppBar.Click += TopAppBar_Click;
            addButton.Click += AddButton_Click;
            // database = FirestoreDb.Create("fir-project-16446");
            FetchandListen();
            SetupRecyclerView();
            /* ItemTouchHelper.Callback callback = new MyItemTouchHelper(database.Collection("exams"));
            ItemTouchHelper itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(rv); */
        }

        private void TopAppBar_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            return base.OnCreateOptionsMenu(menu);
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            addExamFragment = new AddExamFragment();
            var transaction = SupportFragmentManager.BeginTransaction();
            addExamFragment.Show(transaction, "add exam");
        }

       /* public FirestoreDb GetDatabase()
        {
            return FirestoreDb.Create("fir-project-16446");
        } */

        void FetchandListen()
        {
            /* Google.Cloud.Firestore.CollectionReference exams = database.Collection("exams");
            // Google.Cloud.Firestore.Query query = database.Collection("exams");

            FirestoreChangeListener listener = exams.Listen(snapshot =>
            {
                foreach (Google.Cloud.Firestore.DocumentSnapshot documentSnapshot in snapshot.Documents)
                {
                    ExamList.Add(documentSnapshot.ConvertTo<ExamModel>());
                }
            }); */
            CrossCloudFirestore.Current
                   .Instance
                   .Collection("exams")
                   .AddSnapshotListener((snapshot, error) =>
                   {
                       if (snapshot != null)
                       {
                           foreach (var documentChange in snapshot.DocumentChanges)
                           {
                               switch (documentChange.Type)
                               {
                                   case DocumentChangeType.Added:
                                       ExamList.Add(documentChange.Document.ToObject<ExamModel>());
                                       break;
                                   case DocumentChangeType.Removed:
                                       ExamList.Remove(documentChange.Document.ToObject<ExamModel>());
                                       break;
                               }
                           }
                       }
                   });
        }

        private void SetupRecyclerView()
        {
            rv.SetLayoutManager(new Android.Support.V7.Widget.LinearLayoutManager(this));
            adapter = new ExamAdapter(this.ApplicationContext, rv, ExamList);
            adapter.Exam_NameClick += ExamNameTV_Click;
            rv.SetAdapter(adapter);
        }

        private void ExamNameTV_Click(object sender, ExamAdapterClickEventArgs e)
        {
            ExamModel examname_clicked = this.ExamList[e.Position];
            string examname = examname_clicked.get_exam_name();
            Dialog nameDialog = new Dialog(this);
            nameDialog.SetContentView(Resource.Id.dialog_name_update);
            EditText editText = (EditText)nameDialog.FindViewById(Resource.Id.dialog_name_editText);
            editText.Text = examname;
            /* Non riconosce più android.support.design e quindi ho messo il design che sta in google.design*/
            TextInputLayout textInputLayout = (TextInputLayout)nameDialog.FindViewById(Resource.Id.dialog_name_input_layout);
            Android.Widget.Button okButton = (Android.Widget.Button)nameDialog.FindViewById(Resource.Id.name_ok);
            okButton.Click += async delegate
            {
                string examNameNew = editText.Text.ToUpper();
                if (examNameNew.Trim().Equals(""))
                {
                    textInputLayout.SetErrorTextAppearance(Resource.String.empty_name_field);
                    textInputLayout.RequestFocus();
                }
                else if (examNameNew.Length > 15)
                {
                    textInputLayout.SetErrorTextAppearance(Resource.String.overflow_name_field);
                    textInputLayout.RequestFocus();
                }
                else if (IsSameName(examNameNew))
                {
                    textInputLayout.SetErrorTextAppearance(Resource.String.used_name);
                    textInputLayout.RequestFocus();
                }
                else
                { /*
                         Google.Cloud.Firestore.CollectionReference exams = database.Collection("exams");
                        Google.Cloud.Firestore.DocumentReference examDoc = exams.Document(examname);
                        Google.Cloud.Firestore.DocumentSnapshot snapshot = await examDoc.GetSnapshotAsync();
                        if (snapshot.Exists)
                        {
                            await exams.Document(examNameNew).SetAsync(snapshot);
                            await examDoc.DeleteAsync();
                        }
                   */
                }

                    
                    bool IsSameName(string examNewName)
                {
                    for (int j = 0; j < adapter.ItemCount; j++)
                    {
                        if (ExamList[j].examName.Equals(examNewName))
                            return true;
                    }
                    return false;
                }
            };
        }
    }
}
        
