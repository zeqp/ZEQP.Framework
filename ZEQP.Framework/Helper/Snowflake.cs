using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace ZEQP.Framework
{
    /// <summary>
    /// 分布式ID生成器
    /// </summary>
    public class Snowflake
    {
        private static readonly Lazy<Snowflake> SingletonInstance = new Lazy<Snowflake>(() => new Snowflake());
        public SnowflakeConfig Config { get; set; }
        private static readonly Stopwatch _sw = Stopwatch.StartNew();

        private int _sequence = 0;
        private long _lastgen = -1;
        private readonly long _generatorId;

        private readonly long MASK_SEQUENCE;
        private readonly long MASK_TIME;
        private readonly long MASK_GENERATOR;
        private readonly int SHIFT_TIME;
        private readonly int SHIFT_GENERATOR;

        private readonly object _genlock = new object();
        private Snowflake()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            //this.Config = new SnowflakeConfig();// SnowflakeConfig.Default;
            this.Config = builder.GetSection("Snowflake").Get<SnowflakeConfig>();
            if (this.Config == null) this.Config = SnowflakeConfig.Default;

            if (this.Config.IdBits + this.Config.TimeBits + this.Config.SeqBits != 63)
                throw new Exception("数据位必需加起来等于63");
            if (this.Config.IdBits > 31)
                throw new Exception("ID位不能大于31");
            if (this.Config.SeqBits > 31)
                throw new Exception("序号位不能大于31");
            MASK_TIME = this.GetMask(this.Config.TimeBits);
            MASK_GENERATOR = this.GetMask(this.Config.IdBits);
            MASK_SEQUENCE = this.GetMask(this.Config.SeqBits);

            if (this.Config.AppId < 0 || this.Config.AppId > MASK_GENERATOR)
                throw new Exception("APPID不能小于0,或者超过最大位数值");

            SHIFT_TIME = this.Config.IdBits + this.Config.SeqBits;
            SHIFT_GENERATOR = this.Config.SeqBits;
            _generatorId = this.Config.AppId;
        }
        /// <summary>
        /// 得到实例化对象
        /// </summary>
        public static Snowflake Instance { get { return SingletonInstance.Value; } }
        private long GetMask(byte bits)
        {
            return (1L << bits) - 1;
        }
        private long GetTicks()
        {
            return ((DateTimeOffset.UtcNow - this.Config.Epoch).Ticks + _sw.Elapsed.Ticks) / this.Config.Duration.Ticks;
        }
        public long CreateId()
        {
            lock (_genlock)
            {
                var ticks = this.GetTicks();
                var timestamp = ticks & MASK_TIME;
                if (timestamp < _lastgen || ticks < 0)
                    throw new Exception("时钟回拨");
                if (timestamp == _lastgen)
                {
                    if (_sequence < MASK_SEQUENCE)
                        _sequence++;
                    else
                        throw new Exception("生成达到最大值，等下一个时间才能生成");
                }
                else
                {
                    _sequence = 0;
                    _lastgen = timestamp;
                }
                unchecked
                {
                    return (timestamp << SHIFT_TIME)
                        + (_generatorId << SHIFT_GENERATOR)
                        + _sequence;
                }
            }
        }
    }
    public class SnowflakeConfig
    {
        /// <summary>
        /// 应用程序ID
        /// </summary>
        public int AppId { get; set; }
        /// <summary>
        /// 生成ID的开始时间
        /// </summary>
        public DateTimeOffset Epoch { get; set; }
        /// <summary>
        /// 时间位数
        /// </summary>
        public byte TimeBits { get; set; }
        /// <summary>
        /// 应用程序ID位数
        /// </summary>
        public byte IdBits { get; set; }
        /// <summary>
        /// 序号位数
        /// </summary>
        public byte SeqBits { get; set; }
        /// <summary>
        /// 时间间隔
        /// </summary>
        public TimeSpan Duration { get; set; }
        public static SnowflakeConfig Default
        {
            get
            {
                return new SnowflakeConfig()
                {
                    AppId = 0,
                    Epoch = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    TimeBits = 41,
                    IdBits = 10,
                    SeqBits = 12,
                    Duration = TimeSpan.FromMilliseconds(1)
                };
            }
        }
    }
}
