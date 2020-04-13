using PlasticMetal.MobileSuit.ObjectModel.Attributes;

namespace PlasticMetal.MobileSuit.ObjectModel.Interfaces
{
    /// <summary>
    /// Represents an object which can provide information to Mobile Suit.
    /// </summary>
    public interface IInfoProvider
    {
        /// <summary>
        /// The information provided.
        /// </summary>
        [SuitIgnore] string Text { get; }
    }
}