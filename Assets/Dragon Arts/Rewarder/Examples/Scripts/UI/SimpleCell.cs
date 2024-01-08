using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace DragonArts.Rewarder.Example {
    
    public class SimpleCell { // Representation of one Cell (Visualisation Purpose)

        public Transform transform;
        public Image image;
        public TextMeshProUGUI label;
        public bool empty;
        public bool available;

        public SimpleCell (Transform transform, Image image, TextMeshProUGUI label, bool reset = true) {
            this.transform = transform;
            this.image = image;
            this.label = label;
            if (reset) Reset();
        }

        // Setup Cell
        public void Setup (Sprite sprite, int quantity) {
            this.image.sprite = sprite;
            Setup(quantity);
        }

        public void Setup (int quantity) {
            this.label.text = $"{quantity}x";
            empty = false;
        }

        // Reset (Be Empty)
        public void Reset () {
            this.image.sprite = null;
            this.label.text = null;
            empty = true;
        }
    }
}
