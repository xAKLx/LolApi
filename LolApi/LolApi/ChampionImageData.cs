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

namespace LolApi
{
    public class ChampionImageData
    {
        public int id { get; set; }
        public string title { get; set; }
        public string name { get; set; }
        public ChampionImage image { get; set; }
        public string key { get; set; }
    }
}