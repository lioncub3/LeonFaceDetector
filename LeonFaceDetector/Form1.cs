using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeonFaceDetector
{
    public partial class Form1 : Form
    {
        const string subscriptionKey = "89bff76a0ede4c09b233d76050aaac77";
        const string uriBase =
            "https://facescanner.cognitiveservices.azure.com/face/v1.0/detect";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void Button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;

            string filepath = openFileDialog1.FileName;

            string imageFilePath = filepath;

            pictureBox1.Image = new Bitmap(imageFilePath);

            if (File.Exists(imageFilePath))
            {
                try
                {
                    List<ScanResult> results = await MakeAnalysisRequest(imageFilePath);
                    label11.Text = results[0].faceAttributes.gender;
                    label12.Text = results[0].faceAttributes.age.ToString();
                    label13.Text = results[0].faceAttributes.glasses;
                    label14.Text = results[0].faceAttributes.smile.ToString();
                    label16.Text = results[0].faceAttributes.emotion.anger.ToString();
                    label17.Text = results[0].faceAttributes.emotion.happiness.ToString();
                    label18.Text = results[0].faceAttributes.emotion.sadness.ToString();
                    label19.Text = results[0].faceAttributes.hair.hairColor.First(
                        h => h.confidence == results[0].faceAttributes.hair.hairColor.Max(c => c.confidence)
                        ).color;
                    label20.Text = results[0].faceAttributes.makeUp.lipMakeup.ToString();
                }
                catch (Exception exeption)
                {
                    MessageBox.Show("На фото немає обличчя");
                }
            }
            else
            {
                MessageBox.Show("\nInvalid file path.\nPress Enter to exit...\n");
            }
        }

        private static async Task<List<ScanResult>> MakeAnalysisRequest(string imageFilePath)
        {
            List<ScanResult> results = new List<ScanResult>();

            HttpClient client = new HttpClient();

            // Request headers.
            client.DefaultRequestHeaders.Add(
                "Ocp-Apim-Subscription-Key", subscriptionKey);

            // Request parameters. A third optional parameter is "details".
            string requestParameters = "returnFaceId=true&returnFaceLandmarks=false" +
                "&returnFaceAttributes=age,gender,headPose,smile,facialHair,glasses," +
                "emotion,hair,makeup,occlusion,accessories,blur,exposure,noise";

            // Assemble the URI for the REST API Call.
            string uri = uriBase + "?" + requestParameters;

            HttpResponseMessage response;

            // Request body. Posts a locally stored JPEG image.
            byte[] byteData = GetImageAsByteArray(imageFilePath);

            using (ByteArrayContent content = new ByteArrayContent(byteData))
            {
                // This example uses content type "application/octet-stream".
                // The other content types you can use are "application/json"
                // and "multipart/form-data".
                content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");

                // Execute the REST API call.
                response = await client.PostAsync(uri, content);

                // Get the JSON response.
                string contentString = await response.Content.ReadAsStringAsync();

                results = JsonConvert.DeserializeObject<List<ScanResult>>(contentString);

                if (results.Count > 0)
                {
                    return results;
                }
            }

            return results;
        }

        // Returns the contents of the specified file as a byte array.
        static byte[] GetImageAsByteArray(string imageFilePath)
        {
            using (FileStream fileStream =
                new FileStream(imageFilePath, FileMode.Open, FileAccess.Read))
            {
                BinaryReader binaryReader = new BinaryReader(fileStream);
                return binaryReader.ReadBytes((int)fileStream.Length);
            }
        }

        static string JsonPrettyPrint(string json)
        {
            if (string.IsNullOrEmpty(json))
                return string.Empty;

            json = json.Replace(Environment.NewLine, "").Replace("\t", "");

            StringBuilder sb = new StringBuilder();
            bool quote = false;
            bool ignore = false;
            int offset = 0;
            int indentLength = 3;

            foreach (char ch in json)
            {
                switch (ch)
                {
                    case '"':
                        if (!ignore) quote = !quote;
                        break;
                    case '\'':
                        if (quote) ignore = !ignore;
                        break;
                }

                if (quote)
                    sb.Append(ch);
                else
                {
                    switch (ch)
                    {
                        case '{':
                        case '[':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', ++offset * indentLength));
                            break;
                        case '}':
                        case ']':
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', --offset * indentLength));
                            sb.Append(ch);
                            break;
                        case ',':
                            sb.Append(ch);
                            sb.Append(Environment.NewLine);
                            sb.Append(new string(' ', offset * indentLength));
                            break;
                        case ':':
                            sb.Append(ch);
                            sb.Append(' ');
                            break;
                        default:
                            if (ch != ' ') sb.Append(ch);
                            break;
                    }
                }
            }

            return sb.ToString().Trim();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt32(label12.Text) > 20 && Convert.ToInt32(label12.Text) < 25)
            {
                MessageBox.Show("Ви нам підходите");
            }
            else
            {
                MessageBox.Show("Ви нам не підходите");
            }
        }
    }
}
