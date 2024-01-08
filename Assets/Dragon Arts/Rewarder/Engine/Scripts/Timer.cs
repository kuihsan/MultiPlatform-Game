using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DragonArts.Common;
using UnityEngine;

namespace DragonArts.Rewarder {

    public enum TimerPatternType { Millisecond, Second, Minute, Hour, Day, Month, Year }

    [Serializable]
    public class TimerPattern {
        public TimerPatternType type;
        public int value;
    }

    [Serializable]
    public class TimerInstance {
        public string uid;
        public bool removed;
        public TimerPattern pattern;
        public DateTime startTime;
        public DateTime endTime;
        public DateTime? updateTime;
        public DateTime time;
        public Action onComplete;
        public Action<TimeSpan> onUpdate;

        public void CalculateEndTime () {
            CalculateEndTime(startTime, pattern);
        }
        
        public void CalculateEndTime (DateTime startTime, TimerPattern pattern) {
            switch (pattern.type) {
                case TimerPatternType.Millisecond:
                    endTime = startTime.AddMilliseconds(pattern.value);
                    break;
                case TimerPatternType.Second:
                    endTime = startTime.AddSeconds(pattern.value);
                    break;
                case TimerPatternType.Minute:
                    endTime = startTime.AddMinutes(pattern.value);
                    break;
                case TimerPatternType.Hour:
                    endTime = startTime.AddHours(pattern.value);
                    break;
                case TimerPatternType.Day:
                    endTime = startTime.AddDays(pattern.value);
                    break;
                case TimerPatternType.Month:
                    endTime = startTime.AddMonths(pattern.value);
                    break;
                case TimerPatternType.Year:
                    endTime = startTime.AddYears(pattern.value);
                    break;
                default: break;
            }
        }

        public TimeSpan GetTimeLeft () {
            if (time >= endTime) return new TimeSpan();
            return endTime.Subtract(time);
        }
    }

    public class TimerOptions {
        public Action onComplete;
        public Action<TimeSpan> onUpdate;
    }

    [Serializable]
    public class TimerConfiguration {
        public TimerPattern pattern;
    }

    public class Timer : ScriptableItem {
        [HideInInspector]
        public TimerConfiguration configuration;
        [HideInInspector]
        public List<TimerInstance> instances;

        public Timer () {
            instances = new List<TimerInstance>();
        }

        public TimerInstance Add (TimerOptions options) {
            System.Random random = new System.Random();
            string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";
            string newUid = DragonArts.Common.Utilities.GenerateUid(chars, random, 5);

            while (instances.FirstOrDefault(i => i.uid == newUid) != null) {
                newUid = DragonArts.Common.Utilities.GenerateUid(chars, random, 5);
            }

            TimerInstance newTimer = new TimerInstance {
                uid = newUid,
                startTime = DateTime.Now,
                pattern = configuration.pattern,
                time = DateTime.Now,
                onComplete = options.onComplete,
                onUpdate = options.onUpdate
            };

            newTimer.CalculateEndTime();

            instances.Add(newTimer);

            return newTimer;
        }

        public void Remove (string uid) {
            TimerInstance instance = instances.FirstOrDefault(i => i.uid == uid);
            instance.removed = true;
        }

        public void RemoveAll () {
            foreach (TimerInstance instance in instances) {
                instance.removed = true;
            }
        }

        public void CleanUp () {
            instances.RemoveAll(i => i.removed);
        }
    }
}