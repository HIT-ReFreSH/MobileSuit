using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuit.ObjectModel.Interfaces
{
    public interface IInfoProvider
    {
        [MsIgnorable]
        string Text { get; }
    }
}
