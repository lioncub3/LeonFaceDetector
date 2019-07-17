using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeonFaceDetector
{
    public class FaceRectangle
    {
        public int top { get; set; }
        public int left { get; set; }
        public int width { get; set; }
        public int height { get; set; }

    }
    public class Emotion
    {
        public double anger { get; set; }
        public double contempt { get; set; }
        public double happiness { get; set; }
        public double sadness { get; set; }
    }

    public class Hair
    {
        public List<HairColor> hairColor { get; set; }
    }

    public class HairColor
    {
        public string color { get; set; }
        public double confidence { get; set; }
    }

    public class MakeUp
    {
        public bool eyeMakeup { get; set; }
        public bool lipMakeup { get; set; }
    }

    public class FaceAttributes
    {
        public double smile { get; set; }
        public string gender { get; set; }
        public double age { get; set; }
        public string glasses { get; set; }
        public Emotion emotion { get; set; }
        public Hair hair { get; set; }
        public MakeUp makeUp { get; set; }
    }

    public class ScanResult
    {
        public string faceId { get; set; }
        public FaceRectangle faceRectangle { get; set; }
        public FaceAttributes faceAttributes { get; set; }
    }
}
