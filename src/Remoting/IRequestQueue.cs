// /*
//  * Author: Ferdinand Sukhoi
//  * Email: ${User.Email}
//  * Date: 04 18, 2024
//  *
//  */

using System.Threading.Tasks;

namespace HitRefresh.MobileSuit;

public interface IRequestQueue
{
    bool HasRequest { get; }
    Task<string> GetRequestAsync();
    Task StopAsync();
    int FetchPeriod { get; }
}