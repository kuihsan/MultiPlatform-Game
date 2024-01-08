using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using DragonArts.Common;

namespace DragonArts.Rewarder {

    public enum DropQuantity { One, Multiple, Random }

    [Serializable]
    public class DropTableRow {
        public string uid;
        [Range(-1, 10000)]
        public int weight;
        public ScriptableItem item;
        public int[] quantity;

        public DropTableRow () {
            quantity = new int[0];
        }

        public DropTableRow (string uid, int weight, int[] quantity) {
            this.uid = uid;
            this.weight = weight;
            this.quantity = new List<int>(quantity).ToArray();
        }

        public DropQuantity GetDropQuantity () {
            switch (quantity.Length) {
                case 1: return DropQuantity.Multiple;
                case 3: return DropQuantity.Random;
                default: return DropQuantity.One;
            }
        }

        public void SetDropQuantity (DropQuantity dq) {
            int length = 0;
            switch (dq) {
                case DropQuantity.Multiple: length = 1; break;
                case DropQuantity.Random: length = 3; break;
                default: break;
            }
            if (quantity.Length != length) {
                quantity = new int[length];
            }
        }
    }

    public class PickResult {
        public List<Reward> rewards;

        public PickResult () {
            rewards = new List<Reward>();
        }
    }

    [Serializable]
    public class Reward {
        public ScriptableItem item;
        public int quantity;
        public List<List<string>> paths;

        public Reward (ScriptableItem item, int quantity, List<string> path) {
            this.item = item;
            this.quantity = quantity;
            this.paths = new List<List<string>>();
            this.paths.Add(path);
        }

        public List<string> GetPath (int index = 0) {
            return index < paths.Count ? paths[index] : null;
        }
        
        public string GetUid (int index = 0) {
            return index < paths.Count ? String.Join("-", paths[index].ToArray()) : null;
        }
    }

    public class DropTable : ScriptableItem {
        [HideInInspector]
        public List<DropTableRow> rows;
        
        public DropTable () {
            rows = new List<DropTableRow>();
        }

        public void LoopRecursive (Action<DropTableRow> onRow) {
            LoopRecursive(this, onRow);
        }

        private void LoopRecursive (DropTable dropTable, Action<DropTableRow> onRow) {
            foreach (DropTableRow r in dropTable.rows) {
                onRow?.Invoke(r);
                if (r.item is DropTable) {
                    LoopRecursive (r.item as DropTable, onRow);
                }
            }
        }
    }
}