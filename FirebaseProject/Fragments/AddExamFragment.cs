using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.Util;
using Xamarin.Forms.PlatformConfiguration;
using SupportV7 = Android.Support.V7.App;
using Firebase.Firestore;
using Firebase;
using FirebaseProject.Models;
using Plugin.CloudFirestore;
using System.Threading.Tasks;
using Google.Android.Material.TextField;

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

            submitButton.Click += async (sender, e) =>
            {
                ExamModel model = new ExamModel();
                model.examName = addexamnameText.EditText.Text;
                model.date = (Firebase.Timestamp)addexamdateText.EditText.Text;
                await CrossCloudFirestore.Current
                             .Instance
                             .Collection("exams")
                             .Document(addexamnameText.EditText.Text)
                             .SetAsync(model);
                this.Dismiss();
            };
            return view;
        }


        /* public FirebaseFirestore GetDatabase()
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
        } */

        /* private async System.Threading.Tasks.Task SubmitButton_ClickAsync(object sender, EventArgs e)
        {
            ExamModel model = new ExamModel();
            model.examName = addexamnameText.EditText.Text;
            model.date = (Firebase.Timestamp)addexamdateText.EditText.Text;
            string addexamname = addexamnameText.EditText.Text;
            string addexamdate = addexamdateText.EditText.Text;
            HashMap examInfo = new HashMap();
            examInfo.Put("examname", addexamname);
            examInfo.Put("examdate", addexamdate);
            DocumentReference docRef = GetDatabase().Collection("exams").Document(addexamname);
            docRef.Set(examInfo); 
            await CrossCloudFirestore.Current
                         .Instance
                         .Collection("exams")
                         .Document(addexamnameText.EditText.Text)
                         .SetAsync(model);
            this.Dismiss(); 
        } */ 
    }  
}