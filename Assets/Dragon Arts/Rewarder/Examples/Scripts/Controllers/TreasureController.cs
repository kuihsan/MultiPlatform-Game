using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DragonArts.Rewarder;
using System;
using TMPro;
using DG.Tweening;

namespace DragonArts.Rewarder.Example {

    /***
        Example: Treasure - Weighted Drop

        This example shows a basic usage of weighted Drop Tables.
        It simulates standard Treasure Chest Opening, from where the player can obtain guaranteed and also optional rewards.

        Guaranteed Rewards:
        - Coins (from 25 to 250 - inclusive)
        - Minimum 5x Gems (with various weights - the most common is Red Gem)
    ***/

    public class TreasureController : MonoBehaviour {
        
        [SerializeField] private DropTable treasureDropTable; // The Drop Table
        [SerializeField] private Transform table;
        [SerializeField] private RectTransform treasure;
        [SerializeField] private Button openButton;
        [SerializeField] private ToggleButton animateToggle;
        private List<SimpleCell> cells;
        private float treasurePosY;

        private void Awake () {
            animateToggle.SetToggleValue(true);
            treasurePosY = treasure.anchoredPosition.y;
            // Initialize Cells
            cells = new List<SimpleCell>();
            foreach (Transform child in table) {
                cells.Add(new SimpleCell(child, child.GetChild(0).GetComponent<Image>(), child.GetChild(1).GetComponent<TextMeshProUGUI>()));
            }
        }

        public void OpenTreasure () {
            openButton.interactable = false;

            // Pick Guaranteed and Optional (from 5 to 8 - inclusive) Rewards
            PickResult result = RewardManager.Self.PickRewards(treasureDropTable, new int[]{ 5, 8 });

            // Update Cells
            for (int i = 0; i < cells.Count; i++) { // Loop through Cells
                SimpleCell c = cells[i];
                Reward r = i < result.rewards.Count ? result.rewards[i] : null; // Get Reward from PickResult

                if (animateToggle.IsOn()) {
                    c.transform.DOScale(new Vector3(-1f, 1f, 1f), .3f).SetEase(Ease.Linear).OnUpdate(() => {
                        if (c.transform.localScale.x > -.1f && c.transform.localScale.x < .1f) {
                            c.Reset();
                        }
                    }).OnComplete(() => {
                        c.transform.DOScale(Vector3.one, .3f).SetEase(Ease.Linear).OnUpdate(() => {
                            if (c.transform.localScale.x > -.1f && c.transform.localScale.x < .1f) {
                                if (r != null && c.empty) {
                                    c.Setup((r.item as CurrencyItem).image, r.quantity); // Setup Cell based on Picked Reward
                                }
                            }
                        });
                    });
                } else {
                    if (r == null) {
                        c.Reset();
                    } else {
                        c.Setup((r.item as CurrencyItem).image, r.quantity); // Setup Cell based on Picked Reward
                    }
                }
            }

            if (animateToggle.IsOn()) {
                // Animate Treasure Chest
                treasure.DOAnchorPosY(-2500f, .35f).SetEase(Ease.Linear).OnComplete(() => {
                    treasure.anchoredPosition = new Vector2(treasure.anchoredPosition.x, 2500f);
                    treasure.DOAnchorPosY(treasurePosY, .5f).SetEase(Ease.Linear).OnComplete(() => {
                        openButton.interactable = true;
                    });
                });
            } else {
                openButton.interactable = true;
            }
        }
    }
}
