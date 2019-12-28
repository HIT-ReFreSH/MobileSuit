#nullable enable

namespace MobileSuit.ObjectModel
{
    public interface IDynamicParameter
    {
        bool Parse(string[]? options = null);
    }
}
