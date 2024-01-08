#if UNITY_EDITOR

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace DragonArts.Rewarder {

    public class EditorExtender : EditorWindow {
        
        [MenuItem("Assets/Create/Dragon Arts/Drop Table", false, 0)]
        private static void CreateDropTable() {
            DropTable asset = ScriptableObject.CreateInstance<DropTable> ();
            asset.uid = DragonArts.Common.EditorUtilities.GenerateUid("DT");
            DragonArts.Common.EditorUtilities.CreateScriptableObject<DropTable>(asset, "New Drop Table");
        }

        [MenuItem("Assets/Create/Dragon Arts/Timer", false, 1)]
        private static void CreateTimer() {
            Timer asset = ScriptableObject.CreateInstance<Timer> ();
            asset.uid = DragonArts.Common.EditorUtilities.GenerateUid("T");
            DragonArts.Common.EditorUtilities.CreateScriptableObject<Timer>(asset, "New Timer");
        }

        [MenuItem("GameObject/Create Other/Dragon Arts/Reward Manager", false, 0)]
        private static void CreateManager (MenuCommand menuCommand) {
            DragonArts.Common.EditorUtilities.InstantiatePrefab(menuCommand, "Assets/Dragon Arts/Rewarder", "RewardManager", "DA Reward Manager");
        }
    }
}

#endif