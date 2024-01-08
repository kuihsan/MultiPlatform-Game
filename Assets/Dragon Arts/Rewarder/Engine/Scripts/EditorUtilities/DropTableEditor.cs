
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

    [CustomEditor(typeof(DropTable))]
    public class DropTableEditor : DragonArts.Common.ScriptableItemEditor {
        private ReorderableList reorderable;
        private DropTable table;
        private List<string> rowUids = new List<string>();

        protected override void OnEnable() {
            base.OnEnable();
            table = (DropTable)target;

            reorderable = new ReorderableList(table.rows, typeof(DropTableRow));
            reorderable.drawHeaderCallback = OnHeaderCallback;
            reorderable.drawElementCallback = OnElementCallback;
            reorderable.onAddCallback = OnAddCallback;
            reorderable.elementHeightCallback = (index) => {
                return OnElementHeightCallback(index);
            };

            rowUids.Clear();
            table.LoopRecursive((DropTableRow row) => {
                if (String.IsNullOrWhiteSpace(row.uid)) row.uid = GenerateRowUid();
                rowUids.Add(row.uid);
            });
        }

        private string GenerateRowUid () {
            System.Random random = new System.Random();
            string chars = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz0123456789";

            string newUid = DragonArts.Common.Utilities.GenerateUid(chars, random, 5);

            while (rowUids.Contains(newUid)) {
                newUid = DragonArts.Common.Utilities.GenerateUid(chars, random, 5);
            }

            return newUid;
        }

        private void OnHeaderCallback(Rect rect) {
            EditorGUI.LabelField(rect, $"Rows ({table.rows.Count})");
        }

        private int OnElementHeightCallback (int index) {
            switch (table.rows[index].GetDropQuantity()) {
                case DropQuantity.Multiple: return 130;
                case DropQuantity.Random: return 180;
                default: return 105;
            }
        }

        private void OnElementCallback(Rect rect, int index, bool isActive, bool isFocused) {
            if (table.rows.Count <= 0)
                return;

            EditorGUILayout.BeginHorizontal();
            table.rows[index].uid = EditorGUI.TextField(new Rect(rect.x + 25, rect.y + 5 , rect.width - 25, 20), "UID", table.rows[index].uid);
            if (GUI.Button(new Rect(rect.x + 50, rect.y + 5 , 75, 20), DragonArts.Common.EditorUtilities.GetCopyIcon(), new GUIStyle())) {
                GUIUtility.systemCopyBuffer = table.rows[index].uid;
            }
            EditorGUILayout.EndHorizontal();

            table.rows[index].item = EditorGUI.ObjectField(new Rect(rect.x + 25, rect.y + 30 , rect.width - 25, 20), "Item", table.rows[index].item, typeof(ScriptableItem), false) as ScriptableItem;
            int w = table.rows[index].weight;
            table.rows[index].weight = EditorGUI.IntField(new Rect(rect.x + 25, rect.y + 55 , rect.width - 25, 20), "Weight", (w < -1) ? -1 : (w > 10000) ? 10000 : w);
            if (table.rows[index].item is DropTable) {
                EditorGUI.BeginDisabledGroup(true);
                table.rows[index].SetDropQuantity(DropQuantity.One);
                EditorGUI.EnumPopup(new Rect(rect.x + 25, rect.y + 80 , rect.width - 25, 20), "Quantity", DropQuantity.One);
                EditorGUI.EndDisabledGroup();
            } else {
                table.rows[index].SetDropQuantity((DropQuantity)EditorGUI.EnumPopup(new Rect(rect.x + 25, rect.y + 80 , rect.width - 25, 20), "Quantity", table.rows[index].GetDropQuantity()));
                if (table.rows[index].GetDropQuantity() == DropQuantity.Multiple) {
                    EditorGUI.indentLevel++;
                    int v = table.rows[index].quantity[0];
                    table.rows[index].quantity[0] = EditorGUI.IntField(new Rect(rect.x + 25, rect.y + 105 , rect.width - 50, 20), "Value", (v < 2) ? 2 : v);
                    EditorGUI.indentLevel--;
                } else if (table.rows[index].GetDropQuantity() == DropQuantity.Random) {
                    EditorGUI.indentLevel++;
                    int min = table.rows[index].quantity[0];
                    int max = table.rows[index].quantity[1];
                    table.rows[index].quantity[0] = EditorGUI.IntField(new Rect(rect.x + 25, rect.y + 105 , rect.width - 50, 20), "Min", (min < 1) ? 1 : min);                
                    table.rows[index].quantity[1] = EditorGUI.IntField(new Rect(rect.x + 25, rect.y + 130 , rect.width - 50, 20), "Max", max);
                    int d = table.rows[index].quantity[2];
                    table.rows[index].quantity[2] = EditorGUI.IntField(new Rect(rect.x + 25, rect.y + 155 , rect.width - 50, 20), "Divisor", (d < 1) ? 1 : d);
                    EditorGUI.indentLevel--;
                }
            }
        }

        private void OnAddCallback (ReorderableList list) {
            ReorderableList.defaultBehaviours.DoAddButton(list);
            table.rows[table.rows.Count-1].uid = GenerateRowUid();
        }

        public override void OnInspectorGUI() {
            DrawHeader("DROP TABLE");
            DrawInspector();
            SaveInspector();
        }

        protected override void DrawInspector (bool drawDefaults = true) {
            base.DrawInspector(false);
            EditorGUILayout.Space();
            reorderable.DoLayoutList();
            EditorGUILayout.Space();
            if (drawDefaults)
                DrawDefaultInspector();
        }
    }
}

#endif
