using System.Reflection;
using Baruah.Service;
using SQLite;
using UnityEngine;

namespace Baruah.Database
{
    public abstract class BaseTable<TData> where TData : IData, new()
    {
        public BaseTable()
        {
            CreateTable();
        }
        
        public virtual void CreateTable()
        {
            ServiceManager.Get<DatabaseService>().CreateTables<TData>();
        }

        public TData FetchDataByPrimaryKey(object key)
        {
            return ServiceManager.Get<DatabaseService>().FindWithPrimaryKey<TData>(key);
        }
        
        public void InsertData(TData data)
        {
            ServiceManager.Get<DatabaseService>().Insert(data);
        }
        
        public void UpdateData(TData data)
        {
            ServiceManager.Get<DatabaseService>().UpdateTable(data);
        }

        public void DeleteData(object key)
        {
            ServiceManager.Get<DatabaseService>().Delete<TData>(key);
        }

        public TableQuery<TData> GetTable()
        {
            return ServiceManager.Get<DatabaseService>().GetTable<TData>();
        }
    }

    public interface IData
    {
    }
}
