using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;

namespace HandyCrypto.DataHelper
{
    public class SQLiteClient<T> where T : new()
    {
        private SQLiteAsyncConnection connection;
        readonly string basePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        private readonly string path;
        public string DbName { get; set; }

        public SQLiteClient()
        {
            DbName = typeof(T).Name;
            path = Path.Combine(basePath, DbName);
            connection = new SQLiteAsyncConnection(path);

        }

        public async Task<bool> CreateTableAsync()
        {

            try
            {
                await connection.CreateTableAsync<T>();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

       
        public async System.Threading.Tasks.Task<int> InsertAsync(T obj)
        {

            return await connection.InsertAsync(obj);
        }

        public async System.Threading.Tasks.Task<int> InsertAllAsnyc(IEnumerable<T> objects)
        {
            return await connection.InsertAllAsync(objects);
        }

        public Task<List<T>> SelectItems()
        {
            using (var db = new SQLiteConnection(path))
            {
                var s = connection.Table<T>().ToListAsync();
                return s;

            }
        }

        public async Task<T> GetFirst()
        {
            var result = new T();
            try
            {
                using (var db = new SQLiteConnection(path))
                {
                    AsyncTableQuery<T> asyncTableQuery = connection.Table<T>();
                    if (asyncTableQuery != null)
                    {
                        result = await asyncTableQuery.FirstOrDefaultAsync();
                        return result;
                    }
                }
            }
            catch
            {

            }
            return result;
        }

       

        

        public async Task DeleteAll()
        {
            using (var db = new SQLiteConnection(path))
            {
                await connection.DeleteAllAsync<T>();

            }
        }

        public async Task<int> DeleteAsync(T favObj)
        {
            using (var db = new SQLiteConnection(path))
            {
                return await connection.DeleteAsync(favObj);

            }
        }

        public async Task<T> GetById(string pk)
        {
            using (var db = new SQLiteConnection(path))
            {
                return await connection.GetAsync<T>(pk);
            }
        }

      
    }
}