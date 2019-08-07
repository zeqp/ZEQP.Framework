using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZEQP.Framework
{
    /// <summary>
    /// 进程缓存
    /// </summary>
    public sealed class SingletonCache
    {
        private static readonly Lazy<SingletonCache> SingletonInstance = new Lazy<SingletonCache>(() => new SingletonCache());

        private SingletonCache<string> StringInstance { get; set; }
        private SingletonCache()
        {
            this.StringInstance = SingletonCache<string>.Instance;
        }

        /// <summary>
        /// 得到实例化对象
        /// </summary>
        public static SingletonCache Instance { get { return SingletonInstance.Value; } }
        /// <summary>
        /// 增加缓存
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="expiresAt">过期时间</param>
        public void Add(string key, string value, DateTime? expiresAt = null)
        {
            this.StringInstance.Add(key, value, expiresAt);
        }
        /// <summary>
        /// 拿到缓存值
        /// 如果不存再，就返回null
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public string Get(string key)
        {
            return this.StringInstance.Get(key);
        }
        public Dictionary<string, string> GetAll()
        {
            return this.StringInstance.GetAll();
        }
        /// <summary>
        /// 拿到当前值列表
        /// </summary>
        /// <param name="listKey">Key集合</param>
        /// <returns></returns>
        public Dictionary<string, string> Get(List<string> listKey)
        {
            return this.StringInstance.Get(listKey);
        }
        /// <summary>
        /// Linq查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public Dictionary<string, string> Get(Func<KeyValuePair<string, string>, bool> predicate)
        {
            return this.StringInstance.Get(predicate);
        }
        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            this.StringInstance.Remove(key);
        }
    }
    /// <summary>
    /// 进程数据缓存
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public sealed class SingletonCache<T>
    {
        private static readonly Lazy<SingletonCache<T>> SingletonInstance = new Lazy<SingletonCache<T>>(() => new SingletonCache<T>());
        private ConcurrentDictionary<string, T> Cache { get; set; }
        private ConcurrentDictionary<string, DateTime> CacheTime { get; set; }
        private SingletonCache()
        {
            this.Cache = new ConcurrentDictionary<string, T>();
            this.CacheTime = new ConcurrentDictionary<string, DateTime>();
        }

        /// <summary>
        /// 得到实例化对象
        /// </summary>
        public static SingletonCache<T> Instance { get { return SingletonInstance.Value; } }

        /// <summary>
        /// 增加缓存
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="expiresAt">过期时间</param>
        public void Add(string key, T value, DateTime? expiresAt = null)
        {
            if (String.IsNullOrEmpty(key)) return;
            if (expiresAt.HasValue && expiresAt.Value <= DateTime.Now) return;

            this.Cache.AddOrUpdate(key, value, (k, v) => { return value; });

            DateTime outTime;
            if (this.CacheTime.ContainsKey(key))
                this.CacheTime.TryRemove(key, out outTime);
            if (expiresAt.HasValue)
                this.CacheTime.AddOrUpdate(key, expiresAt.Value, (k, v) => { return expiresAt.Value; });
        }
        /// <summary>
        /// 拿到缓存值
        /// 如果不存再，就返回null
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public T Get(string key)
        {
            if (String.IsNullOrEmpty(key)) return default(T);

            if (this.Cache.ContainsKey(key))
            {
                if (this.CacheTime.ContainsKey(key))
                {
                    DateTime outTime;
                    this.CacheTime.TryGetValue(key, out outTime);
                    if (outTime > DateTime.Now)
                    {
                        T outVal;
                        this.Cache.TryGetValue(key, out outVal);
                        return outVal;
                    }
                    else
                    {
                        this.CacheTime.TryRemove(key, out outTime);
                        T outVal;
                        this.Cache.TryRemove(key, out outVal);
                        return default(T);
                    }
                }
                else
                {
                    T outVal;
                    this.Cache.TryGetValue(key, out outVal);
                    return outVal;
                }
            }
            else return default(T);
        }

        public Dictionary<string, T> GetAll()
        {
            return this.Cache.ToDictionary(k => k.Key, v => v.Value);
        }

        /// <summary>
        /// 拿到当前值列表
        /// </summary>
        /// <param name="listKey">Key集合</param>
        /// <returns></returns>
        public Dictionary<string, T> Get(List<string> listKey)
        {
            return this.Get(w => listKey.Contains(w.Key));
        }
        /// <summary>
        /// Linq查询
        /// </summary>
        /// <param name="predicate">查询条件</param>
        /// <returns></returns>
        public Dictionary<string, T> Get(Func<KeyValuePair<string, T>, bool> predicate)
        {
            var dic = this.Cache.Where(predicate).ToDictionary(k => k.Key, v => v.Value);
            var listKeys = dic.Keys.ToList();
            var now = DateTime.Now;
            var listExpiredKey = this.CacheTime.Where(w => listKeys.Contains(w.Key) && w.Value < now).Select(s => s.Key).ToList();
            var result = dic.Where(w => !listExpiredKey.Contains(w.Key)).ToDictionary(k => k.Key, v => v.Value);
            return result;
        }

        /// <summary>
        /// 删除Key
        /// </summary>
        /// <param name="key"></param>
        public void Remove(string key)
        {
            if (this.CacheTime.ContainsKey(key))
            {
                DateTime outTime;
                this.CacheTime.TryRemove(key, out outTime);
            }
            if (this.Cache.ContainsKey(key))
            {
                T outVal;
                this.Cache.TryRemove(key, out outVal);
            }
        }
    }
}
