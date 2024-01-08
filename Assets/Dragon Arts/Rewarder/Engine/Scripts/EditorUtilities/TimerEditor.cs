
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System;
using DragonArts.Common;

namespace DragonArts.Rewarder {
    
    [CustomEditor(typeof(Timer))]
    public class TimerEditor : DragonArts.Common.ScriptableItemEditor {

        private ReorderableList reorderable;
        private Timer timer;
        private bool showInstances;

        protected override void OnEnable() {
            base.OnEnable();
            timer = (Timer)target;
            showInstances = true;
        }

        public override void OnInspectorGUI() {
            DrawHeader("TIMER");
            DrawInspector();
            SaveInspector();
        }

        protected override void DrawInspector (bool drawDefaults = true) {
            base.DrawInspector(false);

            GUILayout.Label("Configuration");
            EditorGUI.indentLevel++;
            EditorGUILayout.LabelField("Pattern");
            EditorGUI.indentLevel++;
            timer.configuration.pattern.type = (TimerPatternType) EditorGUILayout.EnumPopup("Type", timer.configuration.pattern.type);
            if (timer.configuration.pattern.type == TimerPatternType.Millisecond) {
                timer.configuration.pattern.type = TimerPatternType.Second;
            }
            timer.configuration.pattern.value = EditorGUILayout.IntField("Value", timer.configuration.pattern.value);
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            List<TimerInstance> activeInstances = timer.instances.FindAll(i => !i.removed);
            showInstances = EditorGUILayout.Foldout(showInstances, $"Instances ({activeInstances.Count})");

            if (showInstances) {
                EditorGUI.indentLevel++;
                foreach (TimerInstance i in activeInstances) {
                    EditorGUILayout.LabelField($"UID({i.uid})", $"until {i.endTime.ToString("yyyy-MM-dd hh:mm:ss")}");
                }
                EditorGUI.indentLevel--;
            }
            
            EditorGUILayout.Space();
            if (drawDefaults)
                DrawDefaultInspector();
        }
    }
}

#endif
