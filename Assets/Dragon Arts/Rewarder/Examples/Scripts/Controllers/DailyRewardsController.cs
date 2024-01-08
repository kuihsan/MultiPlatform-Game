using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DragonArts.Rewarder;
using DragonArts.Common;
using System;
using TMPro;
using DG.Tweening;

namespace DragonArts.Rewarder.Example {

    /***
        Example: Daily Rewards

        This example shows, how to implement a simple Daily Reward.

        Daily Rewards Timer:
        - Lasts 1 Day (Relative - Ends after 24 Hours)
        - Not Influenced by Time.timeScale
        - Updates every Second
    ***/

    public class DailyRewardsController : MonoBehaviour {
        
        [SerializeField] private Timer dailyRewardsTimer; // The Timer
        [SerializeField] private ScriptableGroup dailyReward; // Usage of ScriptableGroup for representing a Pack of Coins
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private Transform rewardCell;
        [SerializeField] private Button claimButton;
        [SerializeField] private Button timeLeapButton;
        [SerializeField] private ToggleButton animateToggle;
        private SimpleCell cell;

        private void Awake () {
            animateToggle.SetToggleValue(true);
            // Initialize Cell
            cell = new SimpleCell(rewardCell, rewardCell.GetChild(0).GetComponent<Image>(), rewardCell.GetChild(1).GetComponent<TextMeshProUGUI>());
            claimButton.interactable = false;
            RegisterDailyTimer();
        }

        private void OnTimerComplete () {
            UpdateCell(true);
        }

        private void OnTimerUpdate (TimeSpan timeLeft) {
            // Update UI Timer
            timerText.text = $"<size=65>Next Reward in</size>\n{timeLeft.Hours.ToString("00")}:{timeLeft.Minutes.ToString("00")}:{timeLeft.Seconds.ToString("00")}";
            timeLeapButton.interactable = timeLeft.Hours > 0;
        }

        private void RegisterDailyTimer () {
            // Register the Timer
            RewardManager.Self.RegisterTimer(dailyRewardsTimer, new TimerOptions {
                onComplete = OnTimerComplete,
                onUpdate = OnTimerUpdate
            });
        }

        public void Claim () {
            UpdateCell(false);
            RewardManager.Self.SetGlobalTime(DateTime.Now); // Synchronize Time
            RegisterDailyTimer();
        }

        public void TimeLeap () {
            // Perform a Time Leap
            RewardManager.Self.SetGlobalTime(RewardManager.Self.GetGlobalTime().AddHours(23).AddMinutes(59).AddSeconds(50));
        }

        private void UpdateCell (bool claimable) {
            claimButton.interactable = false;

            if (animateToggle.IsOn()) {
                // Animate Reward Cell
                cell.transform.DOScale(new Vector3(-1f, 1f, 1f), .3f).SetEase(Ease.Linear).OnUpdate(() => {
                    if (cell.transform.localScale.x > -.1f && cell.transform.localScale.x < .1f) {
                        if (claimable) {
                            cell.Setup((dailyReward.list[0].item as CurrencyItem).image, dailyReward.list[0].quantity);
                        } else {
                            cell.Reset();
                        }
                    }
                }).OnComplete(() => {
                    cell.transform.DOScale(Vector3.one, .3f).SetEase(Ease.Linear).OnComplete(() => {
                        if (claimable) claimButton.interactable = true;
                    });
                });
            } else {
                if (claimable) {
                    cell.Setup((dailyReward.list[0].item as CurrencyItem).image, dailyReward.list[0].quantity);
                    claimButton.interactable = true;
                } else {
                    cell.Reset();
                }
            }
        }
    }
}
