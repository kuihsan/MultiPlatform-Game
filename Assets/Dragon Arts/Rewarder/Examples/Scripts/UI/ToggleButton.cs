using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace DragonArts.Rewarder.Example {

    public class ToggleButton : MonoBehaviour { // a Toggle Button

        [SerializeField] private RectTransform handle;
        [SerializeField] private Toggle toggle;
        [SerializeField] private Image background;
        [SerializeField] private List<Color> backgroundColors;
        [SerializeField] private List<AudioClip> bells;

        private void Awake () {
            toggle.onValueChanged.AddListener(OnSwitch);
        }

        private void OnSwitch (bool on) {
            PlayBell(on);
            UpdateVisual(on);
        }

        private void PlayBell (bool on) {
            Camera.main.GetComponent<AudioSource>().PlayOneShot(bells[on ? 1 : 0], .25f);
        }

        private void UpdateVisual (bool on) {
            handle.DOAnchorPosX(on ? 60f : -60f, .25f).SetEase(Ease.Linear);
            background.color = backgroundColors[on ? 1 : 0];
        }

        private void OnDestroy () {
            toggle.onValueChanged.RemoveListener(OnSwitch);
        }

        public bool IsOn () {
            if (toggle != null)
                return toggle.isOn;
            return false;
        }

        public void SetToggleValue (bool on) {
            toggle.isOn = on;
            UpdateVisual(on);
        }
    }
}
