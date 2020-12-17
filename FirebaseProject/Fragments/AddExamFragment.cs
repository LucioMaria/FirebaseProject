using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Util;
using Xamarin.Forms.PlatformConfiguration;
using SupportV7 = Android.Support.V7.App;
using Firebase.Firestore;
using Firebase;

namespace FirebaseProject.Fragments
{
    public class AddExamFragment : Android.Support.V4.App.DialogFragment
    {
        TextInputLayout addexamnameText;
        TextInputLayout addexamdateText;
        Button submitButton;
        



        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your fragment here
        }

       

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            View view = inflater.Inflate(Resource.Layout.addexam, container, false);
            addexamnameText = (TextInputLayout)view.FindViewById(Resource.Id.addexamnameText);
            addexamdateText = (TextInputLayout)view.FindViewById(Resource.Id.addexamdateText);
            submitButton = (Button)view.FindViewById(Resource.Id.submitButton);

            submitButton.Click += SubmitButton_Click;

            return view;
        }

        public FirebaseFirestore GetDatabase()
        {
            var app = FirebaseApp.InitializeApp(this.Context);
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

                app = FirebaseApp.InitializeApp(this.Context, options);
                database = FirebaseFirestore.GetInstance(app);
            }
            else
            {
                database = FirebaseFirestore.GetInstance(app);
            }
            return database;
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            string addexamname = addexamnameText.EditText.Text;
            string addexamdate = addexamdateText.EditText.Text;
            HashMap examInfo = new HashMap();
            examInfo.Put("examname", addexamname);
            examInfo.Put("examdate", addexamdate);
            DocumentReference docRef = GetDatabase().Collection("exams").Document();
            docRef.Set(examInfo);
            this.Dismiss();
        }
    }
}