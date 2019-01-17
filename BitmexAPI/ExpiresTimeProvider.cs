using System;
using System.Collections.Generic;
using System.Text;

namespace BitmexAPI
{
    public class ExpiresTimeProvider
    {
        private const int LifetimeSeconds = 30;

        private static readonly DateTime EpochTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public long Get()
        {
            return (long)(DateTime.UtcNow - EpochTime).TotalSeconds + LifetimeSeconds;
        }
    }
}
