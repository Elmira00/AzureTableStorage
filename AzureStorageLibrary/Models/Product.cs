using Azure;
using Azure.Data.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Models
{
    public class Product : ITableEntity
    {
        string ITableEntity.PartitionKey { get ; set; }
        string ITableEntity.RowKey { get; set; }
        DateTimeOffset? ITableEntity.Timestamp { get ; set ; }
        ETag ITableEntity.ETag { get; set ; }

        public string? Name { get; set; }
        public double Price  { get; set; }
        public int Stock { get; set; }
        public string? Color { get; set; }
    }
}
