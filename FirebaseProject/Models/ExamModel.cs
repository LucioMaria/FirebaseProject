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

namespace FirebaseProject.Models
{
    public class ExamModel
    {
        public string Id { get; set; }
        public string examName { get; set; }
        public string examDateText { get; set; }
        public string examMemorizedText { get; set; }    
        public string get_exam_date() { return examDateText; }
        public string get_exam_name() { return examName; }
        public string get_exam_id() { return Id; }
    }
}