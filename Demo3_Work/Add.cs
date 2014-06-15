using System;
using System.Xml.Serialization;

namespace Demo3_Work
{
    [Serializable]
    [XmlRoot(Namespace = "hiperos.com", ElementName = "root")]
    public class Add : CalculateBase
    {
        public override string Name
        { 
            get { return "Add"; }
            set { }
        }

        public float X { get; set; }
        public float Y { get; set; }

        public Add()
        { }

        public Add(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public override float Calculate()
        {
            float result = this.X + this.Y;

            Console.WriteLine("{0} + {1} = {2}", this.X, this.Y, result);

            return result;
        }
    }
}