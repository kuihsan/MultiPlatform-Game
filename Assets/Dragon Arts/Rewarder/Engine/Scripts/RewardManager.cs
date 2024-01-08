using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Data;

namespace DragonArts.Rewarder {

    public class RewardManager : MonoBehaviour {

        public static RewardManager Self { get; private set; }

        [HideInInspector]
        public bool dontDestroyOnLoad = true;

        private System.Random random;
        private DateTime time;
        private List<TimerInstance> timers = new List<TimerInstance>();

        private void Awake () {
            if (dontDestroyOnLoad && Self != null) {
                Destroy(gameObject);
                return;
            }
            Self = this;
            if (dontDestroyOnLoad) {
                DontDestroyOnLoad(gameObject);
            }
            
            SetGlobalRandom(Guid.NewGuid().GetHashCode());
            SetGlobalTime(DateTime.Now);

            StartCoroutine(InvokeRealtimeCoroutine(RunGlobalTimeRoutine, 1));
        }

        private void OnDestroy () {
            foreach (TimerInstance t in timers) {
                t.removed = true;
            }
        }

        public void SetGlobalRandom (int seed) {
            random = new System.Random(seed);
        }

        public System.Random GetGlobalRandom (int seed) {
            return random;
        }

        private System.Random GetRandom () {
            return new System.Random(Guid.NewGuid().GetHashCode());
        }

        #region Drop Table

        private Reward PickOneReward (DropTable dropTable, System.Random random) {
            List<DropTableRow> rows = dropTable.rows.FindAll(r => {
                return r.weight > 0;
            });
            
            int total = rows.Sum(r => r.weight);

            int n = random.Next(1, total+1);

            DropTableRow row = null;
            int c = 0;
            for (int i = 0; i < rows.Count; i++) {
                c += rows[i].weight;
                if (n > c) continue;
                row = rows[i]; break;
            }

            if (row != null && row.item != null) {
                return new Reward(row.item, CalculateQuantity(row, random), new List<string>() { row.uid });
            }

            return null;
        }

        private int CalculateQuantity (DropTableRow dropTableRow, System.Random random) {
            DropTableRow row = new DropTableRow(dropTableRow.uid, dropTableRow.weight, dropTableRow.quantity);

            int q = 1;
            switch (row.GetDropQuantity()) {
                case DropQuantity.Multiple:
                    q = row.quantity[0];
                    break;
                case DropQuantity.Random:
                    int rTotal = (row.quantity[1] - row.quantity[0]) / row.quantity[2];
                    q = row.quantity[0] + (random.Next(0, rTotal+1) * row.quantity[2]);
                    break;
                default: break;                    
            }
            return q;
        }

        public PickResult PickRewards (DropTable dropTable, int optionalCount) {
            PickResult result = new PickResult();
            System.Random random = GetRandom();

            List<DropTableRow> guaranteed = dropTable.rows.FindAll(r => r.weight == -1);

            for (int i = 0; i < optionalCount; i++) {
                Reward reward = PickOneReward(dropTable, random);

                if (reward != null) 
                    result.rewards.Add(reward);
            }

            int j = 0;

            List<Reward> guaranteedRewards = new List<Reward>();

            while (j < guaranteed.Count) {
                DropTableRow row = guaranteed[j];

                if (row == null || row.item == null) {
                    j++;
                    continue;
                }

                guaranteedRewards.Add(new Reward(row.item, CalculateQuantity(row, random), new List<string>() { row.uid }));

                j++;
            }

            guaranteedRewards.AddRange(result.rewards);
            result.rewards = guaranteedRewards;

            return result;
        }

        public PickResult PickRewards (DropTable dropTable, int[] optionalRange) {
            return PickRewards(dropTable, this.random.Next(optionalRange[0], optionalRange[1]+1));
        }

        #endregion
    
        #region Timer

        public void SetGlobalTime (DateTime time) {
            this.time = time;
        }

        public DateTime GetGlobalTime () {
            return time;
        }

        public TimerInstance RegisterTimer (Timer timer, TimerOptions options) {
            timer.RemoveAll();
            TimerInstance newTimer = timer.Add(options);
            timers.Add(newTimer);
            timer.CleanUp();
            return newTimer;
        }
 
        private IEnumerator InvokeRealtimeCoroutine (Action action, float seconds) {
            yield return new WaitForSecondsRealtime(seconds);
            action?.Invoke();
            StartCoroutine(InvokeRealtimeCoroutine(action, seconds));
        }

        private void RunGlobalTimeRoutine () {
            time = time.AddSeconds(1);
            RunTimerCallbacks(time, timers, 1000);
        }

        private void RunTimerCallbacks (DateTime currentTime, List<TimerInstance> timers, int updateRateMultiplier = 1) {
            for (int i = 0; i < timers.Count; i++) {
                TimerInstance timer = timers[i];
                if (timer == null || timer.removed) continue;
                timer.time = currentTime;
                
                if (timer.updateTime == null || timer.time >= (timer.updateTime ?? timer.time).AddMilliseconds(updateRateMultiplier)) {
                    timer.updateTime = timer.time;
                    timer.onUpdate?.Invoke(timer.GetTimeLeft());
                }

                if (timer.time >= timer.endTime) {
                    timer.onComplete?.Invoke();
                    timer.removed = true;
                }
            }

            timers.RemoveAll(t => t.removed);
        }

        #endregion
    }
}
