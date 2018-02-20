using System.Xml.Serialization;

namespace EBerzosa.Pundit.Core.Repository.Xml
{
    public enum XmlRepositoryType
    {
        [XmlEnum("pundit")]
        Pundit,

        [XmlEnum("nuget")]
        NuGet
    }
}