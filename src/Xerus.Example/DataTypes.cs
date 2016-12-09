namespace Xerus.Example
{
    using Model.Attributes;
    using Model.DataTypes;
    using Model.IntrinsicTypes;


    [Documentation("Sample containter")]
    public class SampleContainer : XerusStruct
    {
        public XerusUInt8 Version;
        public XerusUInt32 Id;
        public XerusUInt8 PayloadLength;
        public readonly XerusUInt8[] Payload = new XerusUInt8[256];
    }

}