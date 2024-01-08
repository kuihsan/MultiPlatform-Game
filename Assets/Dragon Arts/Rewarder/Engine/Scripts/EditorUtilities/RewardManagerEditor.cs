
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

namespace DragonArts.Rewarder {

    [CustomEditor(typeof(RewardManager))]
    public class RewardManagerEditor : Editor {
        
        protected RewardManager manager;

        protected virtual void OnEnable() {
            manager = (RewardManager)target;
        }

        public override void OnInspectorGUI() {
            DrawHeader("REWARD MANAGER");
            DrawInspector();
            SaveInspector();
        }

        private void DrawHeader (string header) {
            DragonArts.Common.EditorUtilities.DrawHeader(header);
        }

        private void DrawInspector (bool drawDefaults = true) {

            manager.dontDestroyOnLoad = EditorGUILayout.Toggle("Don't Destroy On Load", manager.dontDestroyOnLoad);
            
            EditorGUILayout.Space();
            
            if (drawDefaults)
                DrawDefaultInspector();
        }

        private void SaveInspector () {
            // Save the changes back to the object
            if (GUI.changed) {
                EditorUtility.SetDirty(target);
            }
        }
    }
}

#endif