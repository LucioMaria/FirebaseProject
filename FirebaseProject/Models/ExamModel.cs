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

namespace FirebaseProject.Models
{
    public class ExamModel
    {
        public string examId { get; set; }
        public string examName { get; set; }
        public int cfu { get; set; }
        public Timestamp date { get; set; }    
        public Timestamp get_exam_date() { return date; }
        public string get_exam_name() { return examName; }
        public string get_exam_id() { return examId; }
        public int get_exam_cfu() { return cfu; }
    }
}