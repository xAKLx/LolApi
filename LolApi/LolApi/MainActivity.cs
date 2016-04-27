using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Android.Graphics;

namespace LolApi
{
    [Activity(Label = "LolApi", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate 
            {
                
                //http request
                var apiKey = Personal.Personal.id;
                var region = "lan";
                var champData = "image";
                var request = HttpWebRequest.Create(string.Format(@"https://global.api.pvp.net/api/lol/static-data/{0}/v1.2/champion?champData={1}&api_key={2}", region, champData, apiKey));
                request.ContentType = "application/json";
                request.Method = "GET";
                string json;

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Console.Out.WriteLine("Error fetching data. Server returned status code: {0}", response.StatusCode);
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        json = reader.ReadToEnd();
                        if (string.IsNullOrWhiteSpace(json))
                        {
                            Console.Out.WriteLine("Response contained empty body...");
                        }
                    }
                }

                LinearLayout ly1 = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
                //TextView tv1 = FindViewById<TextView>(Resource.Id.textView1);

                var champList = JsonConvert.DeserializeObject<ChampionList>(json);
                foreach(var champ in champList.data.Values)
                {
                    
                    var tv = new TextView(this);
                    tv.Append(champ.id.ToString()+"\n");
                    tv.Append(champ.name + "\n");
                    tv.Append(champ.title + "\n");
                    tv.Append(champ.image.full + "\n");
                    var img = new ImageView(this);

                    var webClient = new WebClient();

                    string version = "6.8.1";

                    var url = new Uri(string.Format(@"http://ddragon.leagueoflegends.com/cdn/{0}/img/champion/{1}",version,champ.image.full));

                    
                    var bytes = webClient.DownloadData(url); // get the downloaded data
                    img.SetImageBitmap(BitmapFactory.DecodeByteArray(bytes,0,bytes.Length));


                    ly1.AddView(tv);
                    ly1.AddView(img);
                }
            };
        }
    }
}

