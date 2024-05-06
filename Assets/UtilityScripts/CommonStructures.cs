using System;
using UnityEngine;

namespace CustomUtilityScripts
{
    [Serializable]
    public struct FloatRange
    {
        [SerializeField]
        public float _min;
        [SerializeField]
        public float _max;

        public float Min => _min;
        public float Max => _max;

        public float Clamp(float value)
        {
            return Mathf.Clamp(value, _min, _max);
        }
    }

    public struct ApproximateInterval
    {
        private const float DAYS_TO_YEARS = 1f / 365;
        private const float DAYS_TO_MONTHS = 1 / 30.5f;

        private TimeSpan _timeSpan;

        public ApproximateInterval(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
        }

        public long TotalYears
        {
            get
            {
                return (long)(TotalDays * DAYS_TO_YEARS);
            }
        }

        public long TotalMonths
        {
            get
            {
                return (long)(TotalDays * DAYS_TO_MONTHS);
            }
        }

        public long TotalDays
        {
            get
            {
                return (long)(_timeSpan.TotalDays);
            }
        }

        public long TotalHours
        {
            get
            {
                return (long)(_timeSpan.TotalHours);
            }
        }

        public long TotalMinutes
        {
            get
            {
                return (long)(_timeSpan.TotalMinutes);
            }
        }

        public long TotalSeconds
        {
            get
            {
                return (long)(_timeSpan.TotalSeconds);
            }
        }

        public override string ToString()
        {
            long years = TotalYears;
            if (years != 0)
            {
                return years + " year(s) ago";
            }

            long months = TotalMonths;
            if (months != 0)
            {
                return months + " month(s) ago";
            }

            long days = TotalDays;
            if (days != 0)
            {
                return days + " day(s) ago";
            }

            long hours = TotalHours;
            if (hours != 0)
            {
                return hours + " hour(s) ago";
            }

            long minutes = TotalMinutes;
            if (minutes != 0)
            {
                return minutes + " minute(s) ago";
            }

            long seconds = TotalSeconds;
            if (seconds != 0)
            {
                return seconds + " second(s) ago";
            }

            return string.Empty;
        }
    }
}
