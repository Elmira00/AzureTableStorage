using Azure;
using Azure.Data.Tables;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AzureStorageLibrary.Services
{
    public class TableStorageService<TEntity> : INoSqlStorage<TEntity> where TEntity : class,ITableEntity, new()
    {
        private readonly TableClient _tableClient;
        public TableStorageService()
        {
            var connectionString = ConnectionString.AzureStorageConnectionString;
            var tableName=typeof(TEntity).Name; //hemin o tip'in adi ne olsa avtomatik table'in adini tip adi  edecek.
            var serviceCLient = new TableServiceClient(connectionString);
            _tableClient = serviceCLient.GetTableClient(tableName);
            _tableClient.CreateIfNotExistsAsync();//eger ilk defe olsa yaradacaq 2-ci defe olsa yaratmir.
        }
        public async Task<TEntity> Add(TEntity entity)
        {
            //bir de hele yazilmayib amma gelecekde
            //burda add edende GUID yaz ki unikal bir rowkey versin
            //vunki ozu MongoDb kimi avtomatik vermir , gerek elnen yazasan.
           await _tableClient.AddEntityAsync(entity);
            return await Get(entity.RowKey, entity.PartitionKey);
        }

        public async Task<IQueryable<TEntity>> All()
        {
            var entities = new List<TEntity>();
            var result=  _tableClient.QueryAsync<TEntity>();//baxmayaraq ki async qaytarir amma bunu await etmek olmur.
                                                            //✅ you consume it by iterating
                                                            //await works only on:Task & Task<T> 
                                                            //await doesnt work on : AsyncPageable<T>
                                                            //            If it’s async AND you loop it → await foreach
                                                            //             If it’s async AND returns one value → await
            await foreach (var entity in result) 
            {
                entities.Add(entity);
            }
            return entities.AsQueryable();
        }

        public async Task Delete(string rowkey, string partitonKey)
        {
            await _tableClient.DeleteEntityAsync(rowkey, partitonKey);  
        }

        public async Task<TEntity?> Get(string rowKey, string partitionKey)
        {
            try
            {
                var response = await _tableClient.GetEntityAsync<TEntity>(partitionKey,rowKey);
                return response.Value;
            }
            catch (RequestFailedException e) when (e.Status==404)
            {

                return default(TEntity);// return null yazmaqla eyni sheydir.
            }
        }

        public async Task<IQueryable<TEntity>> Query(Expression<Func<TEntity, bool>> query)
        {
            var entities = new List<TEntity>();
            var result = _tableClient.QueryAsync<TEntity>(query);
            await foreach (var entity in result)
            {
                entities.Add(entity);
            }
            return entities.AsQueryable();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            await _tableClient.UpdateEntityAsync(entity,entity.ETag,TableUpdateMode.Replace);
            return await Get(entity.RowKey, entity.PartitionKey);
            //ETag.All olsa  ve timestamp'ler ferqli olsa onda error vermeden avtomatik overwrite edecek.
            //ETag.All olmasa exception atacaq.

            //Eger entity'nin hansisa propety'si upadte olunursa onda TableUpdateMode.Merge istifade edilir
            //Merge = safe, partial update
            //amma butun entity update olunursa onda TableUpdateMode.Replace istifade edilir.
            //Replace = “delete old entity, then insert the new one”, but  PartitionKey,RowKey are guaranteed to remain.
        }
    }
}
