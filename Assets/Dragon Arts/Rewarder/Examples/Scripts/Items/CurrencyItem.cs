using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DragonArts.Common;
using UnityEditor;

namespace DragonArts.Rewarder.Example {

    #if UNITY_EDITOR

    [CustomEditor(typeof(CurrencyItem))]
    public class CurrencyItemEditor : ScriptableItemEditor {

        [MenuItem("Assets/Create/Dragon Arts/Custom/Currency", false, 50)]
        private static void CreateCurrencyItem() {
            CurrencyItem asset = ScriptableObject.CreateInstance<CurrencyItem> ();
            asset.uid = EditorUtilities.GenerateUid();
            EditorUtilities.CreateScriptableObject<CurrencyItem>(asset, "New Currency");
        }
    }

    #endif

    public enum Currency { COIN, SHARD_RED, SHARD_GREEN, SHARD_BLUE, GEM_RED, GEM_GREEN, GEM_BLUE, GEM_WHITE }

    public class CurrencyItem : ScriptableItem {
        public Currency type;
        public Sprite image;
    }
}
