using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    /// <summary>
    /// Extends TimeSpan class for easier time output handling in game classes
    /// </summary>
    public static class TimeSpanExtender
    {
        /// <summary>
        /// Formats given TimeSpan instance to counter-based string
        /// </summary>
        /// <param name="timeSpan">The TimeSpan instance to use for formatting</param>
        /// <returns></returns>
        public static string ToCounterTimeString(this TimeSpan timeSpan)
        {
            var minutes = timeSpan.Minutes;
            var seconds = timeSpan.Seconds;

            minutes += timeSpan.Hours * 60;

            // only return --:-- when otherwise resulting string would be too big
            if (minutes > 99)
                return "--:--";

            return string.Format("{0}:{1}",
                minutes < 10 ? "0" + minutes : minutes.ToString(),
                seconds < 10 ? "0" + seconds : seconds.ToString());
        }
    }
}