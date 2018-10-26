using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(s_grid))]
[CanEditMultipleObjects]
public class edit_struct : Editor{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        s_grid st = (s_grid)target;
        if (GUILayout.Button("Create Prefabs"))
        {
            st.Initialize();
        }

        if (st.ObjectPool != null)
            EditorGUILayout.LabelField("Object pooler present.");
        else
            EditorGUILayout.LabelField("Object pooler not present.");
    }
    private void OnSceneGUI()
    {
        
    }
}
