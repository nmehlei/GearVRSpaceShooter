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
            return string.Format("{0}:{1}", 
                timeSpan.Minutes < 10 ? "0" + timeSpan.Minutes : timeSpan.Minutes.ToString(),
                timeSpan.Seconds < 10 ? "0" + timeSpan.Seconds : timeSpan.Seconds.ToString());
        }
    }
}