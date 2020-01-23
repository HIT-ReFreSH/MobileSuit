#nullable enable

namespace PlasticMetal.MobileSuit.ObjectModel.Interfaces
{
    public interface IDynamicParameter
    {
        bool Parse(string[]? options = null);
    }
}