using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(GameObjectPool))]
public class ObjectPoolEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("generate template"))
        {
            GameObjectPool pool = (GameObjectPool)target;
            pool.EditorInitPool();
        }
    }
}
