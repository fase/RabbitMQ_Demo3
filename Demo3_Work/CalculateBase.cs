using System;
using System.Xml.Serialization;

namespace Demo3_Work
{
    public interface ICalculate
    {
        string Name { get; set; }
        float Calculate();
    }
    
    [Serializable]
    [XmlInclude(typeof(Add))]
    [XmlInclude(typeof(Subtract))]
    [XmlRoot(Namespace = "hiperos.com", ElementName = "root")]
    public abstract class CalculateBase : ICalculate
    {
        public virtual string Name { get; set; }

        public virtual float Calculate()
        {
            throw new NotImplementedException();
        }
    }
}