//using Core.Domain;
//using CSRedis;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Core.Utility
//{
//    internal class CSRedisCacheService : ICacheService
//    {
//        private static readonly SiasunLogger logger = SiasunLogger.GetInstance(typeof(CSRedisCacheService));
//        private static CSRedisClient rds;
//        private const string RegionPrefix = "RG_";
//        private TimeSpan DefaultExpirationSpan = TimeSpan.FromMinutes(5);
//        internal CSRedisCacheService(CacheConfigInfo serverInfo)
//        {
//            DefaultExpirationSpan = TimeSpan.FromSeconds(serverInfo.DefaultExpirationTime);
//            if (rds == null)
//            {
//                rds = new CSRedisClient(serverInfo.ConnectionString);
//            }
//        }

//        #region Object

//        public bool SetObject<T>(string key, T value)
//        {
//            return SetObject<T>(key, value, DefaultExpirationSpan);
//        }

//        public bool SetObject<T>(string key, T value, string region = null)
//        {
//            return SetObject<T>(key, value, DefaultExpirationSpan, region);
//        }

//        public bool SetObject<T>(string key, T value, TimeSpan expiredTime)
//        {
//            return SetObject<T>(key, value, expiredTime, "");
//        }

//        public bool SetObject<T>(string key, T value, TimeSpan expiredTime, string region)
//        {
//            CheckOrAddRegion(key, expiredTime, region);
//            return rds.Set(UpdateKeyOrRegionForDevelop(key), value, expiredTime);
//        }

//        public async Task<bool> SetObjectAsync<T>(string key, T value)
//        {
//            return await SetObjectAsync<T>(key, value, DefaultExpirationSpan);
//        }

//        public async Task<bool> SetObjectAsync<T>(string key, T value, string region = null)
//        {
//            return await SetObjectAsync<T>(key, value, DefaultExpirationSpan, region);
//        }

//        public async Task<bool> SetObjectAsync<T>(string key, T value, TimeSpan expiredTime)
//        {
//            return await SetObjectAsync<T>(key, value, expiredTime, "");
//        }

//        public async Task<bool> SetObjectAsync<T>(string key, T item, TimeSpan expiredTime, string region)
//        {
//            CheckOrAddRegion(key, expiredTime, region);
//            return await rds.SetAsync(UpdateKeyOrRegionForDevelop(key), item, expiredTime);
//        }

//        public T GetObject<T>(string key)
//        {
//            return rds.Get<T>(UpdateKeyOrRegionForDevelop(key));
//        }

//        public async Task<T> GetObjectAsync<T>(string key)
//        {
//            return await rds.GetAsync<T>(UpdateKeyOrRegionForDevelop(key));
//        }

//        public void CheckOrAddRegion(string key, TimeSpan expiredAfter, string region)
//        {
//            //warn: here we cache the regionId with never expire, this region must be empty manually
//            if (!string.IsNullOrEmpty(region))
//            {
//                var regionKey = RegionPrefix + region;
//                if (!KeyExists(regionKey))
//                {
//                    logger.Info($"Region {regionKey} is not exist in cache server. Adding the region to cache server.");
//                    HashSet<string> set = new HashSet<string>();
//                    set.Add(UpdateKeyOrRegionForDevelop(key));
//                    SetObject<HashSet<string>>(regionKey, set, expiredAfter);
//                }
//                else
//                {
//                    HashSet<string> set = GetObject<HashSet<string>>(regionKey);
//                    var realKey = UpdateKeyOrRegionForDevelop(key);
//                    if (!set.Contains(realKey))
//                    {
//                        set.Add(realKey);
//                        SetObject<HashSet<string>>(regionKey, set, expiredAfter);
//                    }
//                }
//            }
//        }

//        public void RemoveByRegion(string region)
//        {
//            var regionKey = RegionPrefix + region;
//            var set = GetObject<HashSet<string>>(regionKey);
//            if (set != null)
//            {
//                RemoveByList(set.ToList());
//                Remove(regionKey);
//            }
//        }

//        public T GetWithAdd<T>(string key, Func<T> AddCacheFunc) where T : class
//        {
//            return GetWithAdd(key, AddCacheFunc, DefaultExpirationSpan);
//        }
//        public T GetWithAdd<T>(string key, Func<T> AddCacheFunc, TimeSpan expiresIn) where T : class
//        {
//            try
//            {
//                if (KeyExists(key))
//                {
//                    return GetObject<T>(key);
//                }

//                T value = AddCacheFunc();
//                rds.Set(UpdateKeyOrRegionForDevelop(key), value, expiresIn);
//                return value;
//            }
//            catch (Exception ex)
//            {
//                logger.Warn($"An error occurred while tring to get or add with cache key [{key}].", ex);
//                return AddCacheFunc();
//            }
//        }

//        public async Task<T> GetWithAddAsync<T>(string key, Func<T> AddCacheFunc) where T : class
//        {
//            return await GetWithAddAsync(key, AddCacheFunc, DefaultExpirationSpan);
//        }
//        public async Task<T> GetWithAddAsync<T>(string key, Func<T> AddCacheFunc, TimeSpan expiresIn) where T : class
//        {
//            try
//            {
//                if (await KeyExistsAsync(key))
//                {
//                    return await GetObjectAsync<T>(key);
//                }

//                T value = AddCacheFunc();
//                await rds.SetAsync(UpdateKeyOrRegionForDevelop(key), value, expiresIn);
//                return value;
//            }
//            catch (Exception ex)
//            {
//                logger.Warn($"An error occurred while tring to get or add with cache key [{key}].", ex);
//                return AddCacheFunc();
//            }
//        }
//        #endregion

//        #region Common
//        public long ClearAllKeys()
//        {
//            var keys = rds.Keys(UpdateKeyOrRegionForDevelop("*"));
//            return rds.Del(keys);
//        }
//        public async Task<long> ClearAllKeysAsync()
//        {
//            var keys = rds.Keys(UpdateKeyOrRegionForDevelop("*"));
//            return await rds.DelAsync(keys);
//        }
//        public bool Remove(string key)
//        {
//            return rds.Del(UpdateKeyOrRegionForDevelop(key)) > 0 ? true : false;
//        }
//        public async Task<long> RemoveAsync(string key)
//        {
//            return await rds.DelAsync(UpdateKeyOrRegionForDevelop(key));
//        }
//        public long RemoveByList(IEnumerable<string> keys)
//        {
//            long result = 0;
//            if (keys != null && keys.Any())
//            {
//                result = rds.Del(UpdateKeysForDevelop(keys.ToList()).ToArray());
//            }
//            return result;
//        }
//        public async Task<long> RemoveByListAsync(IEnumerable<string> keys)
//        {
//            long result = 0;
//            if (keys != null && keys.Any())
//            {
//                result = await rds.DelAsync(UpdateKeysForDevelop(keys.ToList()).ToArray());
//            }
//            return result;
//        }

//        public bool SetExpire(string key, TimeSpan expiresIn)
//        {
//            return rds.Expire(UpdateKeyOrRegionForDevelop(key), expiresIn);
//        }

//        public async Task<bool> SetExpireAsync(string key, TimeSpan expiresIn)
//        {
//            return await rds.ExpireAsync(UpdateKeyOrRegionForDevelop(key), expiresIn);
//        }
//        public bool KeyExpired(string key)
//        {
//            return rds.PTtl(UpdateKeyOrRegionForDevelop(key)) > 0;
//        }

//        public async Task<bool> KeyExpiredAsync(string key)
//        {
//            return await rds.PTtlAsync(UpdateKeyOrRegionForDevelop(key)) > 0;
//        }

//        public bool KeyExists(string key)
//        {
//            return rds.Exists(UpdateKeyOrRegionForDevelop(key));
//        }

//        public async Task<bool> KeyExistsAsync(string key)
//        {
//            return await rds.ExistsAsync(UpdateKeyOrRegionForDevelop(key));
//        }

//        public IEnumerable<string> SearchKeys(string pattern)
//        {
//            return rds.Keys(UpdateKeyOrRegionForDevelop(pattern)).ToList();
//        }

//        public async Task<IEnumerable<string>> SearchKeysAsync(string pattern)
//        {
//            return await rds.KeysAsync(UpdateKeyOrRegionForDevelop(pattern));
//        }

//        public bool LockKey(string key, TimeSpan expiresIn, Action action)
//        {
//            if (action == null || string.IsNullOrWhiteSpace(key) || expiresIn == TimeSpan.Zero)
//            {
//                return false;
//            }
//            int exp = (int)expiresIn.TotalSeconds;
//            var locker = rds.Lock(UpdateKeyOrRegionForDevelop(key), exp, false);
//            if (locker == null)
//            {
//                return false;
//            }
//            try
//            {
//                action();
//            }
//            finally
//            {
//                locker.Unlock();
//            }
//            return true;
//        }
//        #endregion

//        #region String
//        public string Get(string key)
//        {
//            return rds.Get(UpdateKeyOrRegionForDevelop(key));
//        }
//        public async Task<string> GetAsync(string key)
//        {
//            return await rds.GetAsync(UpdateKeyOrRegionForDevelop(key));
//        }

//        public bool Set(string key, string value)
//        {
//            return Set(key, value, DefaultExpirationSpan);
//        }

//        public bool Set(string key, string value, TimeSpan expiresIn)
//        {
//            return rds.Set(UpdateKeyOrRegionForDevelop(key), value, expiresIn);
//        }

//        public async Task<bool> SetAsync(string key, string value)
//        {
//            return await SetAsync(key, value, DefaultExpirationSpan);
//        }

//        public async Task<bool> SetAsync(string key, string value, TimeSpan expiresIn)
//        {
//            return await rds.SetAsync(UpdateKeyOrRegionForDevelop(key), value, expiresIn);
//        }
//        #endregion

//        #region DataSet
//        public long RemoveSetItems(string setId, List<string> items)
//        {
//            return rds.SRem(UpdateKeyOrRegionForDevelop(setId), items);
//        }

//        public async Task<long> RemoveSetItemsAsync(string setId, List<string> items)
//        {
//            return await rds.SRemAsync(UpdateKeyOrRegionForDevelop(setId), items);
//        }

//        public List<string> GetAllKeysInSet(string setId)
//        {
//            return rds.SMembers(UpdateKeyOrRegionForDevelop(setId))?.ToList();
//        }

//        public async Task<List<string>> GetAllKeysInSetAsync(string setId)
//        {
//            var result = new List<string>();
//            var members = await rds.SMembersAsync(UpdateKeyOrRegionForDevelop(setId));
//            if (members != null)
//            {
//                result = members.ToList();
//            }
//            return result;
//        }

//        public long AddItemInSet(string setId, string value)
//        {
//            return rds.SAdd(UpdateKeyOrRegionForDevelop(setId), value);
//        }

//        public async Task<long> AddItemInSetAsync(string setId, string value)
//        {
//            return await rds.SAddAsync(UpdateKeyOrRegionForDevelop(setId), value);
//        }

//        public long AddSet(string setId, List<string> items)
//        {
//            return rds.SAdd<string>(UpdateKeyOrRegionForDevelop(setId), items.ToArray());
//        }

//        public async Task<long> AddSetAsync(string setId, List<string> items)
//        {
//            return await rds.SAddAsync<string>(UpdateKeyOrRegionForDevelop(setId), items.ToArray());
//        }

//        public bool IsSetContains(string setId, string value)
//        {
//            return rds.SIsMember(UpdateKeyOrRegionForDevelop(setId), value);
//        }

//        public async Task<bool> IsSetContainsAsync(string setId, string value)
//        {
//            return await rds.SIsMemberAsync(UpdateKeyOrRegionForDevelop(setId), value);
//        }

//        public bool MoveItemToNewSet(string sourceSetId, string destinationSetId, string value)
//        {
//            return rds.SMove(UpdateKeyOrRegionForDevelop(sourceSetId), UpdateKeyOrRegionForDevelop(destinationSetId), value);
//        }

//        public async Task<bool> MoveItemToNewSetAsync(string sourceSetId, string destinationSetId, string value)
//        {
//            return await rds.SMoveAsync(UpdateKeyOrRegionForDevelop(sourceSetId), UpdateKeyOrRegionForDevelop(destinationSetId), value);
//        }

//        public string[] GetRandomItemsInSet(string setId, int count)
//        {
//            return rds.SRandMembers(UpdateKeyOrRegionForDevelop(setId), count);
//        }

//        public async Task<string[]> GetRandomItemsInSetAsync(string setId, int count)
//        {
//            return await rds.SRandMembersAsync(UpdateKeyOrRegionForDevelop(setId), count);
//        }
//        #endregion

//        #region Blob
//        public bool SetBlob(string key, byte[] value)
//        {
//            return SetBlob(key, value, DefaultExpirationSpan);
//        }

//        public async Task<bool> SetBlobAsync(string key, byte[] value)
//        {
//            return await SetBlobAsync(key, value, DefaultExpirationSpan);
//        }
//        public bool SetBlob(string key, byte[] value, TimeSpan expiresIn)
//        {
//            return rds.Set(UpdateKeyOrRegionForDevelop(key), value, expiresIn);
//        }

//        public async Task<bool> SetBlobAsync(string key, byte[] value, TimeSpan expiresIn)
//        {
//            return await rds.SetAsync(UpdateKeyOrRegionForDevelop(key), value, expiresIn);
//        }
//        public byte[] GetBlob(string key)
//        {
//            return rds.Dump(UpdateKeyOrRegionForDevelop(key));
//        }

//        public async Task<byte[]> GetBlobAsync(string key)
//        {
//            return await rds.DumpAsync(UpdateKeyOrRegionForDevelop(key));
//        }
//        #endregion

//        private string UpdateKeyOrRegionForDevelop(string key)
//        {
//            //if (RuntimeContext.Config.IsDevelopment && !string.IsNullOrEmpty(key))
//            //{
//            //    var ip = NetWork.LocalIPAddress.Replace(".", "_");
//            //    key = $"{ip}_{key}";
//            //}
//            return key;
//        }

//        private List<string> UpdateKeysForDevelop(List<string> keys)
//        {
//            //if (RuntimeContext.Config.IsDevelopment)
//            //{
//            //    var ip = NetWork.LocalIPAddress.Replace(".", "_");
//            //    keys.ForEach(k =>
//            //    {
//            //        if (!string.IsNullOrEmpty(k))
//            //        {
//            //            k = $"{ip}_{k}";
//            //        }
//            //    });
//            //}
//            return keys;
//        }
//    }
//}
