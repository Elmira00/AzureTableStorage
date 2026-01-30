using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary
{
    public interface ConnectionString
    {
        public static string? AzureStorageConnectionString { get; set; }
        //bu set olunacaq WEBUI'daki appsetting.json faylinda olan connection string'e
        //(program.cs'de appsettings.json'dan goturub bura yazacayiq)
    }
}
