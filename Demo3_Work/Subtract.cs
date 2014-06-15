using System;
using System.Xml.Serialization;

namespace Demo3_Work
{
    [Serializable]
    [XmlRoot(Namespace = "hiperos.com", ElementName = "root")]
    public class Subtract : CalculateBase
    {
        public override string Name
        {
            get { return "Subtract"; }
            set { }
        }

        public float X { get; set; }
        public float Y { get; set; }

        public Subtract() { }

        public Subtract(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public override float Calculate()
        {
            float result = this.X - this.Y;

            Console.WriteLine("{0} - {1} = {2}", this.X, this.Y, result);

            return result;
        }
    }
}