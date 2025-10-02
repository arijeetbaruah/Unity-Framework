using System.IO;
using Baruah.Service;
using SQLite;
using UnityEngine;

namespace Baruah.Database
{
    public class DatabaseService : IService
    {
        private ISQLiteConnection _connection;
        
        public void Initialize()
        {
            string file = Path.Combine(Application.persistentDataPath, "db.sqlite");
            _connection = new SQLiteConnection(file);
            _connection.Execute("PRAGMA foreign_keys = ON;");
            
            Debug.Log($"Database initialized: {file}");
        }

        public void Update()
        {
            
        }

        public void OnDestroy()
        {
            _connection.Close();
        }

        public void Execute(string sql)
        {
            _connection.Execute(sql);
        }

        public void CreateTables<T>()
        {
            _connection.CreateTable<T>();
            
            Debug.Log($"Table created: {typeof(T).Name}");
        }

        public TableQuery<T> GetTable<T>() where T : new()
        {
            return _connection.Table<T>();
        }

        public T FindWithPrimaryKey<T>(object primaryKey) where T : new()
        {
            return _connection.Find<T>(primaryKey);
        }

        public void Insert<T>(T entity) where T : new()
        {
            _connection.Insert(entity);
        }
        
        public void UpdateTable<T>(T entity) where T : new()
        {
            _connection.Update(entity);
        }

        public void Delete<T>(object primaryKey) where T : new()
        {
            _connection.Delete<T>(primaryKey);
        }
    }
}
