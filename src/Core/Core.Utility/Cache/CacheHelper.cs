//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Core.Utility
//{
//    public static class CacheHelper
//    {
//        private static SiasunLogger logger = SiasunLogger.GetInstance(typeof(CacheHelper));

//        private static ICacheService _cacheService { get; set; }

//        public static void InitCacheHelper()
//        {
//            _cacheService = _cacheService ?? new CacheService();
//        }

//        #region LEO Used Functions
//        public static Result FromCache<Result>(this Func<Result> method, string Key, TimeSpan expiredAfter = default(TimeSpan), string region = null)
//        {
//            return CacheControll<Result>(() => { return method(); }, Key, expiredAfter, region);
//        }

//        public static Result FromCache<Result, T1>(this Func<T1, Result> method, T1 arg1, string Key, TimeSpan expiredAfter = default(TimeSpan), string region = null)
//        {
//            return CacheControll<Result>(() => { return method(arg1); }, Key, expiredAfter, region);
//        }

//        public static Result FromCache<Result, T1, T2>(this Func<T1, T2, Result> method, T1 arg1, T2 arg2, string Key, TimeSpan expiredAfter = default(TimeSpan), string region = null)
//        {
//            return CacheControll<Result>(() => { return method(arg1, arg2); }, Key, expiredAfter, region);
//        }

//        public static Result FromCache<Result, T1, T2, T3>(this Func<T1, T2, T3, Result> method, T1 arg1, T2 arg2, T3 arg3, string Key, TimeSpan expiredAfter = default(TimeSpan), string region = null)
//        {
//            return CacheControll<Result>(() => { return method(arg1, arg2, arg3); }, Key, expiredAfter, region);
//        }

//        public static void RemoveCache(string key)
//        {
//            _cacheService.Remove(key);
//        }

//        public static void RemoveCacheByRegion(string region)
//        {
//            _cacheService.RemoveByRegion(region);
//        }

//        public static void AddCache<T>(string key, T value, TimeSpan timeSpan)
//        {
//            _cacheService.SetObject<T>(key, value, timeSpan);
//        }

//        public static async void AddCacheAsync<T>(string key, T value, TimeSpan timeSpan)
//        {
//            await _cacheService.SetObjectAsync<T>(key, value, timeSpan);
//        }

//        public static bool FetchItem<T>(string key, out T returnValue)
//        {
//            returnValue = default(T);
//            bool fetched = false;
//            try
//            {
//                var result = _cacheService.GetObject<T>(key);
//                if (result != null)
//                {
//                    if (result is T)
//                    {
//                        returnValue = (T)result;
//                        fetched = true;
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                logger.Warn("error occurred when fetch item" + e.ToString());
//            }

//            return fetched;
//        }

//        public static T FetchItem<T>(string key)
//        {
//            T returnValue;
//            FetchItem<T>(key, out returnValue);
//            return returnValue;
//        }

//        public static async Task<bool> KeyExistsAsync(string key)
//        {
//            return await _cacheService.KeyExistsAsync(key);
//        }

//        public static bool CacheItemExist(string key)
//        {
//            bool exist = false;
//            try
//            {
//                exist = _cacheService.KeyExists(key);
//            }
//            catch (Exception e)
//            {
//                logger.Warn("error occurred when fetch item" + e.ToString());
//            }

//            return exist;
//        }

//        #region Private
//        private static Result CacheControll<Result>(Func<Result> func, string Key, TimeSpan expiredAfter = default(TimeSpan), string region = null)
//        {
//            Ensure<Result>(Key);

//            #region process region

//            ProcessRegion(Key, expiredAfter, region);

//            #endregion

//            Result returnValue = default(Result);
//            bool fetched = true;

//            lock (Key)
//            {
//                #region fetch item from cache

//                try
//                {
//                    var result = _cacheService.GetObject<Result>(Key);
//                    if (result != null)
//                    {
//                        returnValue = result;
//                    }
//                    else
//                    {
//                        //we required that null should not be cached. because null value also will be returned if cache expired. it makes logic complicated
//                        //we believe null means items doesn't exist in cache here.
//                        fetched = false;
//                    }
//                }
//                catch (Exception e)
//                {
//                    logger.Warn(string.Format("Error occurred when get object from cache, Key:[{0}], exception:[{1}]", Key, e));
//                    fetched = false;
//                }

//                #endregion

//                if (!fetched)
//                {
//                    //throw orginal exception in delegate; 
//                    returnValue = func();

//                    if (returnValue == null)
//                    {
//                        //todo handle null
//                    }
//                    else
//                    {
//                        try
//                        {
//                            var exist = !_cacheService.SetObject(Key, returnValue, expiredAfter);

//                            if (exist)
//                            {
//                                logger.Info("add item failed, cause might be you are adding a key already exist, please check the logic.");
//                            }
//                        }
//                        catch (Exception e)
//                        {
//                            logger.Warn(string.Format("Error occurred when add object to cache, Key:[{0}], exception:[{1}]", Key, e));
//                        }
//                    }
//                }
//            }
//            return returnValue;
//        }

//        private static void ProcessRegion(string Key, TimeSpan expiredAfter, string region)
//        {
//            //warn: here we cache the regionId with never expire, this region must be empty manually
//            if (!string.IsNullOrEmpty(region))
//            {
//                var set = _cacheService.GetObject<HashSet<string>>("RG_" + region);
//                if (set == null)
//                {
//                    set = new HashSet<string>();
//                }
//                //just ensure we add the key,we don't care whether this key exist or not before
//                set.Add(Key);
//                CacheItem("RG_" + region, set, expiredAfter);
//            }
//        }

//        public static bool CacheItem(string key, object item, TimeSpan expiredAfter = default(TimeSpan), string region = null)
//        {
//            ProcessRegion(key, expiredAfter, region);
//            return _cacheService.SetObject(key, item, expiredAfter);
//        }

//        private static void EnsureSerializable<T>()
//        {
//            if (!typeof(T).IsSerializable)
//            {
//                throw new Exception(string.Format("Type:[{0}] can not be serializable, only serializable type can be cached", typeof(T).ToString()));
//            }
//        }

//        private static void Ensure<T>(string key)
//        {
//            if (string.IsNullOrEmpty(key))
//            {
//                throw new ArgumentException("key should not be null or empty");
//            }

//            EnsureSerializable<T>();
//        }
//        #endregion

//        #endregion
//    }
//}
