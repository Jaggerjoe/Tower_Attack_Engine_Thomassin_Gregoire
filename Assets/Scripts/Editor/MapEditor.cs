using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(MapManager))]
public class MapEditor : Editor
{
    private MapManager m_CurrentMapManager;
    public void OnEnable()
    {
        //m_CurrentMapManager = target as MapManager;
        m_CurrentMapManager = (MapManager)target;
    }
    public override void OnInspectorGUI()
    {
        GUILayout.Label("=========== Map Editor =========",EditorStyles.boldLabel);
        if(GUILayout.Button("Initialize Map Randomly"))
        {
            m_CurrentMapManager.InitializeMapRandomly();
        }
        if(GUILayout.Button("Initialize Map Empty"))
        {
            m_CurrentMapManager.InitializeEmptyMap();
        }
        GUILayout.Label("=========== Map Properties =========");
        base.OnInspectorGUI();
        
    }
    //public void OnSceneGUI()
    //{
        
    //}
}
