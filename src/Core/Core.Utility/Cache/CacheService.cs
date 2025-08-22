//using Core.Domain;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Core.Utility
//{
//    public class CacheService : ICacheService
//    {
//        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(typeof(CacheService));
//        private ICacheService internalService;
//        private CacheConfigInfo cacheInfo;
//        private const string BuildInCacheKey = "CacheHealthKey";

//        private DateTime lastChangeTime = DateTime.UtcNow;
//        public CacheService()
//        {
//            try
//            {
//                cacheInfo = RuntimeContext.Config.CacheServer;
//                internalService = new CSRedisCacheService(cacheInfo);
//                internalService.Set(BuildInCacheKey, "ready");
//            }
//            catch (Exception ex)
//            {
//                logger.Error($"Failed to init cache server information. Missing cache server configuration.", ex);
//                throw;
//            }
//        }

//        #region Object
//        public bool SetObject<T>(string key, T value)
//        {
//            return Invoke(() => internalService.SetObject(key, value));
//        }
//        public bool SetObject<T>(string key, T value, string region = null)
//        {
//            return Invoke(() => internalService.SetObject(key, value, region));
//        }
//        public bool SetObject<T>(string key, T value, TimeSpan expiredTime)
//        {
//            return Invoke(() => internalService.SetObject(key, value, expiredTime));
//        }
//        public bool SetObject<T>(string key, T value, TimeSpan expiredTime, string region)
//        {
//            return Invoke(() => internalService.SetObject(key, value, expiredTime, region));
//        }

//        public async Task<bool> SetObjectAsync<T>(string key, T value)
//        {
//            return await InvokeAsync(() => internalService.SetObjectAsync(key, value));
//        }
//        public async Task<bool> SetObjectAsync<T>(string key, T value, string region = null)
//        {
//            return await InvokeAsync(() => internalService.SetObjectAsync(key, value, region));
//        }
//        public async Task<bool> SetObjectAsync<T>(string key, T value, TimeSpan expiredTime)
//        {
//            return await InvokeAsync(() => internalService.SetObjectAsync(key, value, expiredTime));
//        }
//        public async Task<bool> SetObjectAsync<T>(string key, T value, TimeSpan expiredTime, string region)
//        {
//            return await InvokeAsync(() => internalService.SetObjectAsync(key, value, expiredTime, region));
//        }

//        public T GetObject<T>(string key)
//        {
//            return Invoke(() => internalService.GetObject<T>(key));
//        }
//        public async Task<T> GetObjectAsync<T>(string key)
//        {
//            return await InvokeAsync(() => internalService.GetObjectAsync<T>(key));
//        }

//        public void CheckOrAddRegion(string key, TimeSpan expiredAfter, string region)
//        {
//            Invoke(() => internalService.CheckOrAddRegion(key, expiredAfter, region));
//        }

//        public void RemoveByRegion(string region)
//        {
//            Invoke(() => internalService.RemoveByRegion(region));
//        }

//        public T GetWithAdd<T>(string key, Func<T> AddCacheFunc) where T : class
//        {
//            return Invoke(() => internalService.GetWithAdd<T>(key, AddCacheFunc));
//        }
//        public T GetWithAdd<T>(string key, Func<T> AddCacheFunc, TimeSpan expiresIn) where T : class
//        {
//            return Invoke(() => internalService.GetWithAdd<T>(key, AddCacheFunc, expiresIn));
//        }

//        public async Task<T> GetWithAddAsync<T>(string key, Func<T> AddCacheFunc) where T : class
//        {
//            return await InvokeAsync(() => internalService.GetWithAddAsync<T>(key, AddCacheFunc));
//        }
//        public async Task<T> GetWithAddAsync<T>(string key, Func<T> AddCacheFunc, TimeSpan expiresIn) where T : class
//        {
//            return await InvokeAsync(() => internalService.GetWithAddAsync<T>(key, AddCacheFunc, expiresIn));
//        }
//        #endregion

//        #region Common
//        public static bool IsRedisCacheReady(CacheConfigInfo serverInfo, int maxRetryCount = 100)
//        {
//            var isRedisReady = false;
//            if (!string.IsNullOrWhiteSpace(serverInfo.ConnectionString))
//            {
//                while (!isRedisReady && maxRetryCount >= 0)
//                {
//                    try
//                    {
//                        var cacheService = new CSRedisCacheService(serverInfo);
//                        cacheService.Get("CacheHealthKey");
//                        isRedisReady = true;
//                    }
//                    catch (Exception ex)
//                    {
//                        logger.Warn($"Check redis ready status failed. Connection:{serverInfo.ConnectionString}, current retry count:{maxRetryCount}.", ex);
//                        Thread.Sleep(1000 * 5);
//                    }
//                    finally
//                    {
//                        maxRetryCount--;
//                    }
//                }
//            }
//            else
//            {
//                logger.Error($"Check redis ready status failed. Connection string is empty");
//            }
//            return isRedisReady;
//        }

//        public long ClearAllKeys()
//        {
//            return Invoke(() => internalService.ClearAllKeys());
//        }
//        public async Task<long> ClearAllKeysAsync()
//        {
//            return await InvokeAsync(() => internalService.ClearAllKeysAsync());
//        }

//        public bool Remove(string key)
//        {
//            return Invoke(() => internalService.Remove(key));
//        }
//        public async Task<long> RemoveAsync(string key)
//        {
//            return await InvokeAsync(() => internalService.RemoveAsync(key));
//        }

//        public long RemoveByList(IEnumerable<string> keys)
//        {
//            return Invoke(() => internalService.RemoveByList(keys));
//        }
//        public async Task<long> RemoveByListAsync(IEnumerable<string> keys)
//        {
//            return await InvokeAsync(() => internalService.RemoveByListAsync(keys));
//        }

//        public bool SetExpire(string key, TimeSpan expiresIn)
//        {
//            return Invoke(() => internalService.SetExpire(key, expiresIn));
//        }
//        public async Task<bool> SetExpireAsync(string key, TimeSpan expiresIn)
//        {
//            return await InvokeAsync(() => internalService.SetExpireAsync(key, expiresIn));
//        }

//        public bool KeyExpired(string key)
//        {
//            return Invoke(() => internalService.KeyExpired(key));
//        }
//        public async Task<bool> KeyExpiredAsync(string key)
//        {
//            return await InvokeAsync(() => internalService.KeyExpiredAsync(key));
//        }

//        public bool KeyExists(string key)
//        {
//            return Invoke(() => internalService.KeyExists(key));
//        }
//        public async Task<bool> KeyExistsAsync(string key)
//        {
//            return await InvokeAsync(() => internalService.KeyExistsAsync(key));
//        }

//        public IEnumerable<string> SearchKeys(string pattern)
//        {
//            return Invoke(() => internalService.SearchKeys(pattern));
//        }
//        public async Task<IEnumerable<string>> SearchKeysAsync(string pattern)
//        {
//            return await InvokeAsync(() => internalService.SearchKeysAsync(pattern));
//        }

//        public bool LockKey(string key, TimeSpan expiresIn, Action action)
//        {
//            return Invoke(() => internalService.LockKey(key, expiresIn, action));
//        }
//        #endregion

//        #region String
//        public string Get(string key)
//        {
//            return Invoke(() => internalService.Get(key));
//        }
//        public async Task<string> GetAsync(string key)
//        {
//            return await InvokeAsync(() => internalService.GetAsync(key));
//        }

//        public bool Set(string key, string value)
//        {
//            return Invoke(() => internalService.Set(key, value));
//        }

//        public bool Set(string key, string value, TimeSpan expiresIn)
//        {
//            return Invoke(() => internalService.Set(key, value, expiresIn));
//        }

//        public async Task<bool> SetAsync(string key, string value)
//        {
//            return await InvokeAsync(() => internalService.SetAsync(key, value));
//        }

//        public async Task<bool> SetAsync(string key, string value, TimeSpan expiresIn)
//        {
//            return await InvokeAsync(() => internalService.SetAsync(key, value, expiresIn));
//        }
//        #endregion

//        #region DataSet
//        public long RemoveSetItems(string setId, List<string> items)
//        {
//            return Invoke(() => internalService.RemoveSetItems(setId, items));
//        }

//        public async Task<long> RemoveSetItemsAsync(string setId, List<string> items)
//        {
//            return await InvokeAsync(() => internalService.RemoveSetItemsAsync(setId, items));
//        }

//        public List<string> GetAllKeysInSet(string setId)
//        {
//            return Invoke(() => internalService.GetAllKeysInSet(setId));
//        }

//        public async Task<List<string>> GetAllKeysInSetAsync(string setId)
//        {
//            return await InvokeAsync(() => internalService.GetAllKeysInSetAsync(setId));
//        }

//        public long AddItemInSet(string setId, string value)
//        {
//            return Invoke(() => internalService.AddItemInSet(setId, value));
//        }

//        public async Task<long> AddItemInSetAsync(string setId, string value)
//        {
//            return await InvokeAsync(() => internalService.AddItemInSetAsync(setId, value));
//        }

//        public long AddSet(string setId, List<string> items)
//        {
//            return Invoke(() => internalService.AddSet(setId, items));
//        }

//        public async Task<long> AddSetAsync(string setId, List<string> items)
//        {
//            return await InvokeAsync(() => internalService.AddSetAsync(setId, items));
//        }

//        public bool IsSetContains(string setId, string value)
//        {
//            return Invoke(() => internalService.IsSetContains(setId, value));
//        }

//        public async Task<bool> IsSetContainsAsync(string setId, string value)
//        {
//            return await InvokeAsync(() => internalService.IsSetContainsAsync(setId, value));
//        }

//        public bool MoveItemToNewSet(string sourceSetId, string destinationSetId, string value)
//        {
//            return Invoke(() => internalService.MoveItemToNewSet(sourceSetId, destinationSetId, value));
//        }

//        public async Task<bool> MoveItemToNewSetAsync(string sourceSetId, string destinationSetId, string value)
//        {
//            return await InvokeAsync(() => internalService.MoveItemToNewSetAsync(sourceSetId, destinationSetId, value));
//        }

//        public string[] GetRandomItemsInSet(string setId, int count)
//        {
//            return Invoke(() => internalService.GetRandomItemsInSet(setId, count));
//        }

//        public async Task<string[]> GetRandomItemsInSetAsync(string setId, int count)
//        {
//            return await InvokeAsync(() => internalService.GetRandomItemsInSetAsync(setId, count));
//        }
//        #endregion

//        #region Blob
//        public bool SetBlob(string key, byte[] value)
//        {
//            return Invoke(() => internalService.SetBlob(key, value));
//        }

//        public async Task<bool> SetBlobAsync(string key, byte[] value)
//        {
//            return await InvokeAsync(() => internalService.SetBlobAsync(key, value));
//        }
//        public bool SetBlob(string key, byte[] value, TimeSpan expiresIn)
//        {
//            return Invoke(() => internalService.SetBlob(key, value, expiresIn));
//        }

//        public async Task<bool> SetBlobAsync(string key, byte[] value, TimeSpan expiresIn)
//        {
//            return await InvokeAsync(() => internalService.SetBlobAsync(key, value, expiresIn));
//        }
//        public byte[] GetBlob(string key)
//        {
//            return Invoke(() => internalService.GetBlob(key));
//        }

//        public async Task<byte[]> GetBlobAsync(string key)
//        {
//            return await InvokeAsync(() => internalService.GetBlobAsync(key));
//        }
//        #endregion

//        #region private Functions
//        private T Invoke<T>(Func<T> func, int retryTimes = 0)
//        {
//            T result = default(T);
//            var isSuccessfully = false;
//            while (!isSuccessfully && retryTimes++ < cacheInfo.RetryCount)
//            {
//                try
//                {
//                    result = func();
//                    isSuccessfully = true;
//                }
//                catch (CustomExceptionBase customEx)
//                {
//                    throw customEx;
//                }
//                catch (Exception ex)
//                {
//                    logger.Error($"Failed to invoke cache function for specified type. Current retry number:{retryTimes}.", ex);
//                }
//            }
//            return result;
//        }

//        private async Task<T> InvokeAsync<T>(Func<Task<T>> func, int retryTimes = 0)
//        {
//            T result = default(T);
//            var isSuccessfully = false;
//            while (!isSuccessfully && retryTimes++ < cacheInfo.RetryCount)
//            {
//                try
//                {
//                    result = await func();
//                    isSuccessfully = true;
//                }
//                catch (CustomExceptionBase customEx)
//                {
//                    throw customEx;
//                }
//                catch (Exception ex)
//                {
//                    logger.Error($"Failed to invoke cache function for specified type. Current retry number:{retryTimes}.", ex);
//                }
//            }
//            return result;
//        }

//        private void Invoke(Action func, int retryTimes = 0)
//        {
//            var isSuccessfully = false;
//            while (!isSuccessfully && retryTimes++ < cacheInfo.RetryCount)
//            {
//                try
//                {
//                    func();
//                    isSuccessfully = true;
//                }
//                catch (Exception ex)
//                {
//                    logger.Error($"Failed to invoke cache function.Current retry number:{retryTimes}.", ex);
//                }
//            }
//        }

//        private async Task InvokeAsync(Func<Task> func, int retryTimes = 0)
//        {
//            var isSuccessfully = false;
//            while (!isSuccessfully && retryTimes++ < cacheInfo.RetryCount)
//            {
//                try
//                {
//                    await func();
//                    isSuccessfully = true;
//                }
//                catch (Exception ex)
//                {
//                    logger.Error($"Failed to invoke cache function.Current retry number:{retryTimes}.", ex);
//                }
//            }
//        }
//        #endregion
//    }
//}
