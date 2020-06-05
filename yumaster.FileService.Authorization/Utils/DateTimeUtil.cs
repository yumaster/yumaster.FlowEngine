using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace yumaster.FileService.Authorization.Utils
{
    /// <summary>
    /// DateTime工具类
    /// </summary>
    [Obfuscation(Feature = "stringencryption", Exclude = false)]
    public class DateTimeUtil
    {
        private static readonly DateTime DateTime1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static DateTime _baseTime = DateTime.MinValue;

        private static int _timeDifference = 0;

        //
        // 摘要:
        //     返回网络上的当前时间，失败则返回DateTime.MinValue
        [Obsolete("已过时，请使用GetNetNowAsync")]
        public static DateTime NetNow => GetNetNowAsync().Result;

        //
        // 摘要:
        //     时间戳(精确到毫秒)转为本地DateTime
        public static DateTime TimestampToDateTime(long timestamp)
        {
            return DateTime1970.ToLocalTime().AddMilliseconds(timestamp);
        }

        //
        // 摘要:
        //     本地DateTime转为时间戳(精确到毫秒)
        public static long DateTimeToTimestamp(DateTime dt)
        {
            return (long)dt.ToUniversalTime().Subtract(DateTime1970).TotalMilliseconds;
        }

        //
        // 摘要:
        //     当前DateTime转为时间戳(精确到毫秒)
        public static long DateTimeToTimestamp()
        {
            return DateTimeToTimestamp(DateTime.Now);
        }

        //
        // 摘要:
        //     unix时间戳(精确到秒)转为本地DateTime
        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            return DateTime1970.AddSeconds(timestamp).ToLocalTime();
        }

        //
        // 摘要:
        //     本地DateTime转为unix时间戳(精确到秒)
        public static long DateTimeToUnixTimestamp(DateTime dt)
        {
            return (long)dt.ToUniversalTime().Subtract(DateTime1970).TotalSeconds;
        }

        //
        // 摘要:
        //     当前DateTime转为unix时间戳(精确到秒)
        public static long DateTimeToUnixTimestamp()
        {
            return DateTimeToUnixTimestamp(DateTime.Now);
        }

        //
        // 摘要:
        //     返回网络上的当前时间，失败则返回DateTime.MinValue
        public static async Task<DateTime> GetNetNowAsync()
        {
            if (_baseTime == DateTime.MinValue)
            {
                _timeDifference = System.Environment.TickCount;
                _baseTime = await GetNowTimeByNTPAsync(CrackProtection.Xor("\u000e\u0001@\u0014\0\u0003\u0001A\0\u0010\u001fB\u0002\u001d\t"));
                if (_baseTime == DateTime.MinValue)
                {
                    _baseTime = await GetNowTimeByNTPAsync(CrackProtection.Xor("\u0019\u0018@\u0014\0\u0003\u0001A\0\u0010\u001fB\u0002\u001d\t"));
                }
                else if (_baseTime == DateTime.MinValue)
                {
                    _baseTime = await GetNowTimeByDateHeaderAsync(new Uri(CrackProtection.Xor("\u0005\u001b\u001a\u0014UCB\u0018\u0019\u0013A\u000e\f\u0006\n\u0011A\u000f\u0002\u0002")));
                }
            }
            return (_baseTime == DateTime.MinValue) ? DateTime.MinValue : _baseTime.AddMilliseconds(System.Environment.TickCount - _timeDifference);
        }

        //
        // 摘要:
        //     重置NetNow
        public static void ResetNewNow()
        {
            _baseTime = DateTime.MinValue;
        }

        //
        // 摘要:
        //     返回当前时间是否在有效期内
        public static bool NowIsValid(DateTime beginTime, DateTime endTime)
        {
            DateTime now = DateTime.Now;
            if (now >= beginTime)
            {
                return now <= endTime;
            }
            return false;
        }

        private static Task<DateTime> GetNowTimeByNTPAsync(string ntpServer)
        {
            return Task.Factory.StartNew(delegate
            {
                try
                {
                    byte[] array = new byte[48];
                    array[0] = 27;
                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                    {
                        socket.ReceiveTimeout = 5000;
                        socket.Connect(ntpServer, 123);
                        socket.Send(array);
                        socket.Receive(array);
                    }
                    ulong arg = BitConverter.ToUInt32(array, 40);
                    ulong arg2 = BitConverter.ToUInt32(array, 44);
                    Func<ulong, uint> obj = (ulong x) => (uint)(((x & 0xFF) << 24) + ((x & 0xFF00) << 8) + ((x & 0xFF0000) >> 8) + ((x & 4278190080u) >> 24));
                    arg = obj(arg);
                    arg2 = obj(arg2);
                    ulong num = arg * 1000 + arg2 * 1000 / 4294967296uL;
                    return new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)num).ToLocalTime();
                }
                catch
                {
                    return DateTime.MinValue;
                }
            });
        }

        private static async Task<DateTime> GetNowTimeByDateHeaderAsync(Uri pageUri)
        {
            DateTime dtRetn = DateTime.MinValue;
            DateTime dtTemp = DateTime.Now;
            try
            {
                using (HttpClient hc = new HttpClient())
                {
                    using (HttpResponseMessage httpResponseMessage = await hc.GetAsync(pageUri))
                    {
                        HttpResponseHeaders headers = httpResponseMessage.Headers;
                        if (headers.Date.HasValue)
                        {
                            dtTemp = headers.Date.Value.DateTime;
                        }
                        if (headers.Age.HasValue)
                        {
                            dtTemp = dtTemp.AddSeconds(headers.Age.Value.TotalSeconds);
                        }
                        dtRetn = dtTemp;
                    }
                }
                return dtRetn;
            }
            catch
            {
                return dtRetn;
            }
        }
    }
}
