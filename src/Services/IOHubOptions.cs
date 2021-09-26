using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlasticMetal.MobileSuit.ObjectModel;

namespace PlasticMetal.MobileSuit.Services
{
    /// <summary>
    /// Options of IOHub
    /// </summary>
    public interface IIOHubOptions
    {
        /// <summary>
        ///     ColorSetting of IOHub
        /// </summary>
        public ColorSetting ColorSetting { get;  }

        /// <summary>
        ///     Input Stream of IOHub
        /// </summary>
        public TextReader Input { get;  }

        /// <summary>
        ///     Output Stream of IOHub
        /// </summary>
        public TextWriter Output { get;  }

        /// <summary>
        ///     Error Stream of IOHub
        /// </summary>
        public TextWriter Error { get;}
    }
    /// <summary>
    /// Options of IOHub
    /// </summary>
    public class IOHubOptions:IIOHubOptions
    {
        /// <summary>
        ///     ColorSetting of host's IO
        /// </summary>
        public ColorSetting ColorSetting { get; set; }

        /// <summary>
        ///     Input Stream of host's IO
        /// </summary>
        public TextReader Input { get; set; } = Console.In;

        /// <summary>
        ///     Output Stream of host's IO
        /// </summary>
        public TextWriter Output { get; set; }=Console.Out;

        /// <summary>
        ///     Error Stream of host's IO
        /// </summary>
        public TextWriter Error { get; set; }=Console.Error;
    }
}
