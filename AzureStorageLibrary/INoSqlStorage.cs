using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary
{
    public interface INoSqlStorage<TEntity>//ve generic yaziriq ki istenilen tip'le (class/entity) ile rahat ishleyek.
    {
        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Update(TEntity entity);
        Task Delete(string rowkey, string partitonKey);//sql'de yalniz primary key(yeni ID) gonderirdik
                                            //amma NoSQL'de mutleq hemishe rowKey'in yaninda partitionKey gonderilmelidir.
        Task<TEntity?> Get(string rowKey, string partitionKey);
        Task<IQueryable<TEntity>> All();// IQueryable uzerinde tetbiq etdiyimiz sorgunu
                                        // bir basha database'in uzerinde tetbiq edir, daha INumarable kimi RAM'a kocurub orda elemir.
        Task<IQueryable<TEntity>> Query(Expression<Func<TEntity, bool>> query);// meselen : x=> x>5 bu kimi
                                                                               // anonim expression'lari promoy icinde yaza bilmek uchun
    }
}
