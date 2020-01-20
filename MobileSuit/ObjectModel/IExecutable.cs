using System.Reflection;

namespace PlasticMetal.MobileSuit.ObjectModel
{
    public interface IExecutable
    {
        public const BindingFlags Flags = BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
        public TraceBack Execute(string[] args);
    }
}