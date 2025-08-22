//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Core.Utility
//{
//    public interface ICacheService
//    {
//        #region Object

//        bool SetObject<T>(string key, T value);

//        bool SetObject<T>(string key, T value, string region = null);

//        bool SetObject<T>(string key, T value, TimeSpan expiredTime);

//        bool SetObject<T>(string key, T value, TimeSpan expiredTime, string region);

//        Task<bool> SetObjectAsync<T>(string key, T value);

//        Task<bool> SetObjectAsync<T>(string key, T value, string region = null);

//        Task<bool> SetObjectAsync<T>(string key, T value, TimeSpan expiredTime);

//        Task<bool> SetObjectAsync<T>(string key, T item, TimeSpan expiredTime, string region);

//        T GetObject<T>(string key);

//        Task<T> GetObjectAsync<T>(string key);

//        void CheckOrAddRegion(string key, TimeSpan expiredAfter, string region);

//        void RemoveByRegion(string region);

//        T GetWithAdd<T>(string key, Func<T> AddCacheFunc) where T : class;
//        T GetWithAdd<T>(string key, Func<T> AddCacheFunc, TimeSpan expiresIn) where T : class;

//        Task<T> GetWithAddAsync<T>(string key, Func<T> AddCacheFunc) where T : class;
//        Task<T> GetWithAddAsync<T>(string key, Func<T> AddCacheFunc, TimeSpan expiresIn) where T : class;
//        #endregion

//        #region Common
//        long ClearAllKeys();
//        Task<long> ClearAllKeysAsync();
//        bool Remove(string key);
//        Task<long> RemoveAsync(string key);
//        long RemoveByList(IEnumerable<string> keys);
//        Task<long> RemoveByListAsync(IEnumerable<string> keys);

//        bool SetExpire(string key, TimeSpan expiresIn);

//        Task<bool> SetExpireAsync(string key, TimeSpan expiresIn);
//        bool KeyExpired(string key);

//        Task<bool> KeyExpiredAsync(string key);

//        bool KeyExists(string key);

//        Task<bool> KeyExistsAsync(string key);

//        IEnumerable<string> SearchKeys(string pattern);

//        Task<IEnumerable<string>> SearchKeysAsync(string pattern);

//        bool LockKey(string key, TimeSpan expiresIn, Action action);
//        #endregion

//        #region String
//        string Get(string key);

//        Task<string> GetAsync(string key);

//        bool Set(string key, string value);

//        bool Set(string key, string value, TimeSpan expiresIn);

//        Task<bool> SetAsync(string key, string value);

//        Task<bool> SetAsync(string key, string value, TimeSpan expiresIn);
//        #endregion

//        #region DataSet
//        long RemoveSetItems(string setId, List<string> items);

//        Task<long> RemoveSetItemsAsync(string setId, List<string> items);

//        List<string> GetAllKeysInSet(string setId);

//        Task<List<string>> GetAllKeysInSetAsync(string setId);

//        long AddItemInSet(string setId, string value);

//        Task<long> AddItemInSetAsync(string setId, string value);

//        long AddSet(string setId, List<string> items);

//        Task<long> AddSetAsync(string setId, List<string> items);

//        bool IsSetContains(string setId, string value);

//        Task<bool> IsSetContainsAsync(string setId, string value);

//        bool MoveItemToNewSet(string sourceKey, string destinationKey, string value);

//        Task<bool> MoveItemToNewSetAsync(string sourceSetId, string destinationSetId, string value);

//        string[] GetRandomItemsInSet(string setId, int count);

//        Task<string[]> GetRandomItemsInSetAsync(string setId, int count);
//        #endregion

//        #region Blob
//        bool SetBlob(string key, byte[] value);

//        Task<bool> SetBlobAsync(string key, byte[] value);
//        bool SetBlob(string key, byte[] value, TimeSpan expiresIn);

//        Task<bool> SetBlobAsync(string key, byte[] value, TimeSpan expiresIn);
//        byte[] GetBlob(string key);

//        Task<byte[]> GetBlobAsync(string key);
//        #endregion
//    }
//}
