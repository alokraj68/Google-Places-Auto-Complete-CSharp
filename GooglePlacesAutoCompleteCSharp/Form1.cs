using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace GooglePlacesAutoCompleteCSharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.ResetText();
            string inputstring = textBox1.Text.ToString().Trim();
            //post not working
            //        using (var client = new HttpClient())
            //        {
            //            var values = new Dictionary<string, string>
            //{
            //   { "input", textBox1.Text.Trim() },
            //   { "types", "cities" }
            //};

            //            var content = new FormUrlEncodedContent(values);
            //            textBox2.AppendText(content.ToString());
            //            var response = await client.PostAsync("https://maps.googleapis.com/maps/api/place/autocomplete/json?language=en&key=AIzaSyBcMuqYEpPieu2HNNk0bmbZk-WLvNFNTCE", content);
            //            textBox2.AppendText(response.ToString());
            //            var responseString = await response.Content.ReadAsStringAsync();
            //            textBox2.AppendText(responseString.ToString());

            // get

            //using (var client = new WebClient())
            //{
            //    string input = "https://maps.googleapis.com/maps/api/place/autocomplete/json?input=" + textBox1.Text.ToString().Trim() + "&types=geocode&language=en&key=AIzaSyBcMuqYEpPieu2HNNk0bmbZk-WLvNFNTCE";
            //    textBox2.AppendText(input);
            //    var responseString = client.DownloadString(input);
            //    textBox2.AppendText(responseString.ToString());
            //}

            //post test
            using (WebClient client = new WebClient())
            {

                byte[] response =
                client.UploadValues("https://maps.googleapis.com/maps/api/place/autocomplete/json?types=geocode&language=en&key=AIzaSyBcMuqYEpPieu2HNNk0bmbZk-WLvNFNTCE&libraries=places&input=" + inputstring, new NameValueCollection()
                {
                    { "types", "(cities)" }
                });
                textBox2.AppendText(response.ToString());

                string result = System.Text.Encoding.UTF8.GetString(response);
                try
                {
                    //dynamic stuff = JObject.Parse(result);
                    dynamic jsonObj = JsonConvert.DeserializeObject(result);
                    if (jsonObj.status == "OK")
                    {
                        foreach (var prediction in jsonObj.predictions)
                        {
                            foreach (var type in prediction.types)
                            {
                                if (type == "locality" || type == "sublocality_level_1" || type == "sublocality")
                                {
                                    textBox2.AppendText(prediction.description.ToString() + ", " + prediction.types.ToString());
                                    textBox2.AppendText(Environment.NewLine);
                                }
                            }
                        }
                    }
                    else
                    {
                        textBox2.AppendText("error");
                    }

                }
                catch (Exception ex)
                {
                    textBox2.AppendText(ex.ToString());

                }

            }
        }

    }
}

