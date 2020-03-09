using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

[CustomEditor(typeof(MapManager))]
public class MapEditor : Editor
{  
    private MapManager m_CurrentMapManager;
    private bool m_ShowMapView = false;
    //Edit des Square
    private bool m_IsInSquareEditMode = false;
    private SquareState m_SquareStateModeEditMode = SquareState.Normal;
    //Edit des Brush
    private bool m_IsInEditBrushMode = false;
    private BrushState m_BrushStateModeEditMode = BrushState.One;

    //Edit des Edges
    private bool m_IsInEdgesEditMode = false;
    private bool m_IsEdgesModeActivate = false;
    //Edit des EdgesHorizontal
    private bool m_EditEdgesHori = false;
    //Edit des Edges Verticale
    private bool m_EditEdgesVerti = false;


  
    public void OnEnable()
    {
        //m_CurrentMapManager = target as MapManager;
        m_CurrentMapManager = (MapManager)target;
        LoadEditorState();
    }
    public void OnDisable()
    {
        SaveEditorState();
    }
    private void SaveEditorState()
    {
        //Sauvegarder la position du boolen des edit des squares
        EditorPrefs.SetBool(nameof(m_IsInSquareEditMode), m_IsInSquareEditMode);
        EditorPrefs.SetInt(nameof(m_SquareStateModeEditMode),(int)m_SquareStateModeEditMode);
        //Sauvegarder la position du boolen des edit des Brush
        EditorPrefs.SetBool(nameof(m_IsInEditBrushMode), m_IsInEditBrushMode);
        EditorPrefs.SetInt(nameof(m_BrushStateModeEditMode), (int)m_BrushStateModeEditMode);
        //Sauvegarder la position du boolen des edit des edges
        EditorPrefs.SetBool(nameof(m_IsInEdgesEditMode), m_IsInEdgesEditMode);
        //Sauvegarder la position du boolen de la vue de la map
        EditorPrefs.SetBool(nameof(m_ShowMapView), m_ShowMapView);
    }
    void LoadEditorState()
    {
        //charger le booleen des edit des Squares a la derière venu dans l'inspector
        m_IsInSquareEditMode = EditorPrefs.GetBool(nameof(m_IsInSquareEditMode));
        m_SquareStateModeEditMode = (SquareState)EditorPrefs.GetInt(nameof(m_SquareStateModeEditMode));
        //charger le booleen des edit des Brush a la derière venu dans l'inspector
        m_IsInEditBrushMode = EditorPrefs.GetBool(nameof(m_IsInEditBrushMode));
        m_BrushStateModeEditMode = (BrushState)EditorPrefs.GetInt(nameof(m_BrushStateModeEditMode));
        //charger le booleen des edit des Edges a la derière venu dans l'inspector
        m_IsInEdgesEditMode = EditorPrefs.GetBool(nameof(m_IsInEdgesEditMode));
        //charger le booleen de la vue de la map a la derière venu dans l'inspector
        m_ShowMapView = EditorPrefs.GetBool(nameof(m_ShowMapView));
       
    }
    public override void OnInspectorGUI()
    {
        GUILayout.Label("=========== Map Editor =========",EditorStyles.boldLabel);
        if(GUILayout.Button("Initialize Map Randomly"))
        {
            m_CurrentMapManager.InitializeMapRandomly();
        }

        GUILayout.Space(10);
        m_ShowMapView = GUILayout.Toggle(m_ShowMapView, "Show View");
        m_CurrentMapManager.navContainer.SetActive(m_ShowMapView);

        if (GUILayout.Button("Initialize Map Empty"))
        {
            m_CurrentMapManager.InitializeEmptyMap();
        }
        
        GUILayout.Label("=========== Edit Square =========", EditorStyles.boldLabel);
        m_IsInSquareEditMode = GUILayout.Toggle(m_IsInSquareEditMode, " Edit Mode");
        if((m_IsInSquareEditMode) && (!m_IsInEdgesEditMode))
        {
            m_SquareStateModeEditMode =  (SquareState)EditorGUILayout.EnumPopup(m_SquareStateModeEditMode);
        }
        else
        {
            m_IsInSquareEditMode = false;
        }
        m_IsInEditBrushMode = GUILayout.Toggle(m_IsInEditBrushMode, "Edit Brush");
        if(m_IsInEditBrushMode)
        {
            m_BrushStateModeEditMode = (BrushState)EditorGUILayout.EnumPopup(m_BrushStateModeEditMode);
        }

        GUILayout.Label("=========== Edit Edges =========", EditorStyles.boldLabel);
        m_IsInEdgesEditMode = GUILayout.Toggle(m_IsInEdgesEditMode, "Edit Modes");
        
        
        if(m_IsInEdgesEditMode)
        {
            m_IsEdgesModeActivate = GUILayout.Toggle(m_IsEdgesModeActivate, "Edit Edges Statu");
        }

        if((m_IsInEdgesEditMode) && (m_IsInEdgesEditMode))
        {
            m_EditEdgesHori = GUILayout.Toggle(m_EditEdgesHori, "Edit Edges Hori");
            m_EditEdgesVerti = GUILayout.Toggle(m_EditEdgesVerti, "Edit Edges Verti");
        }
        
        GUILayout.Label("=========== Map Properties =========");
        base.OnInspectorGUI();
        
    }
    public void OnSceneGUI()
    {
        //Edit des squares
        if(m_IsInSquareEditMode)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Tools.current = Tool.None;

            //calcule de la position de la sourie sur le plen en 3D.
            Vector3 posIntersection = CalculateInteractPositionPlan();

            //on arronide la position pour la snapper sur la grille
            Vector3 posInt = new Vector3((int)posIntersection.x, 0, (int)posIntersection.z);

           
            if (posIntersection.x >= 0 && posIntersection.x < m_CurrentMapManager.mapData.width
                && posIntersection.z >= 0 && posIntersection.z < m_CurrentMapManager.mapData.height)
            {
                //on affiche le gizmos du square.
                DisplayGizmosSquareEdit(posInt);
                //Edit des square
                EditSquareState(posInt);
               
            }
            
            //Si notre mode de selction des Squares est le lock
            if(m_SquareStateModeEditMode == SquareState.Lock)
            {
                EditSquareState(posInt);
            }
            if (m_BrushStateModeEditMode == BrushState.Line)
            {
                DisplayGizmosSquareEdit(posInt);
                DisplayGizmosSquareEdit(posInt + new Vector3(1, 0, 0));
                EditSquareState(posInt);
                EditSquareState(posInt + new Vector3(1,0,0));
                
                
            }
            if (m_BrushStateModeEditMode == BrushState.Square)
            {
                DisplayGizmosSquareEdit(posInt);
                DisplayGizmosSquareEdit(posInt + new Vector3(1, 0, 0));
                DisplayGizmosSquareEdit(posInt + new Vector3(0, 0, 1));
                DisplayGizmosSquareEdit(posInt + new Vector3(1, 0, 1));
                EditSquareState(posInt);
                EditSquareState(posInt + new Vector3(1, 0, 0));
                EditSquareState(posInt + new Vector3(0, 0, 1));
                EditSquareState(posInt + new Vector3(1, 0, 1));
               

            }
            if (m_BrushStateModeEditMode == BrushState.Cross)

            {
                DisplayGizmosSquareEdit(posInt);
                DisplayGizmosSquareEdit(posInt + new Vector3(1, 0, 0));
                DisplayGizmosSquareEdit(posInt + new Vector3(-1, 0, 0));
                DisplayGizmosSquareEdit(posInt + new Vector3(0, 0, 1));
                DisplayGizmosSquareEdit(posInt + new Vector3(0, 0, -1));
                EditSquareState(posInt);
                EditSquareState(posInt + new Vector3(1, 0, 0));
                EditSquareState(posInt + new Vector3(-1, 0, 0));
                EditSquareState(posInt + new Vector3(0, 0, 1));           
                EditSquareState(posInt + new Vector3(0, 0, -1));
               
            }
            if (m_BrushStateModeEditMode == BrushState.L)
            {
                DisplayGizmosSquareEdit(posInt);
                DisplayGizmosSquareEdit(posInt + new Vector3(0, 0, 1));
                DisplayGizmosSquareEdit(posInt + new Vector3(1, 0, 0));
                EditSquareState(posInt);
                EditSquareState(posInt + new Vector3(0, 0, 1));
                EditSquareState(posInt + new Vector3(1, 0, 0));
               
            }
            if (m_BrushStateModeEditMode == BrushState.T)
            {
                DisplayGizmosSquareEdit(posInt);
                DisplayGizmosSquareEdit(posInt + new Vector3(0, 0, 1));
                DisplayGizmosSquareEdit(posInt + new Vector3(-1, 0, 1));
                DisplayGizmosSquareEdit(posInt + new Vector3(1, 0, 1));
                EditSquareState(posInt);
                EditSquareState(posInt + new Vector3(0, 0, 1));
                EditSquareState(posInt + new Vector3(0, 0, 1));                    
                EditSquareState(posInt + new Vector3(-1, 0, 1));                    
                EditSquareState(posInt + new Vector3(1, 0, 1));
                
            }
            if (m_BrushStateModeEditMode == BrushState.Heart)
            {
                DisplayGizmosSquareEdit(posInt);
                DisplayGizmosSquareEdit(posInt + new Vector3(1, 0, 1));
                DisplayGizmosSquareEdit(posInt + new Vector3(-1, 0, 1));
                DisplayGizmosSquareEdit(posInt + new Vector3(2, 0, 2));
                DisplayGizmosSquareEdit(posInt + new Vector3(-2, 0, 2));
                DisplayGizmosSquareEdit(posInt + new Vector3(2, 0, 3));
                DisplayGizmosSquareEdit(posInt + new Vector3(-2, 0, 3));
                DisplayGizmosSquareEdit(posInt + new Vector3(1, 0, 4));
                DisplayGizmosSquareEdit(posInt + new Vector3(-1, 0, 4));
                DisplayGizmosSquareEdit(posInt + new Vector3(0, 0, 3));
                EditSquareState(posInt);
                EditSquareState(posInt + new Vector3(1,0,1));
                EditSquareState(posInt + new Vector3(-1,0,1));
                EditSquareState(posInt + new Vector3(2,0,2));
                EditSquareState(posInt + new Vector3(-2,0,2));
                EditSquareState(posInt + new Vector3(2,0,3));
                EditSquareState(posInt + new Vector3(-2,0,3));
                EditSquareState(posInt + new Vector3(1,0,4));
                EditSquareState(posInt + new Vector3(-1,0,4));
                EditSquareState(posInt + new Vector3(0,0,3));
               
            }
                
        }

        //edit des edges 
        if(m_IsInEdgesEditMode)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            Tools.current = Tool.None;
            Vector3 posIntersection = CalculateInteractPositionPlan();
            Vector3 posInt = new Vector3((int)posIntersection.x, 0, (int)posIntersection.z);
            //EdgesVerticale
            bool[] edgesVerti = m_CurrentMapManager.mapData.edgesVert;
            int verti = m_CurrentMapManager.mapData.width +1;
            //edgesHorizontal
            bool[] edgesHori = m_CurrentMapManager.mapData.edgesHori;
            int hori = m_CurrentMapManager.mapData.width;

            //Edit des edges vertical.
            //Positionné les edges vertical, largeur
            if (posIntersection.x >= 0 && posIntersection.x < m_CurrentMapManager.mapData.width + 1
               && posIntersection.z >= 0 && posIntersection.z < m_CurrentMapManager.mapData.height )
            {
                if (m_EditEdgesVerti && m_IsInEditBrushMode && !m_EditEdgesHori && !m_IsEdgesModeActivate)
                {                  
                    //création du gizmos pour les edges Vertical
                    DisplayGizmosEdgesVertiEdit(posInt);
                    //Edit des Edges Verticals
                    EditEdgesVertiState(posInt);
                    //suppression du square.lock 
                    m_CurrentMapManager.ProcessRuleSquareVSEdge(edgesVerti,verti,new Vector3(-1,0,0));
                }
                if ((m_IsEdgesModeActivate) && (m_EditEdgesVerti))
                {
                   //Création des gizmos des edges Verticals
                    DisplayGizmosEdgesVertiEdit(posInt);
                    //Suppression des edges Verticales
                    DeleteEdgesVertiState(posInt);
                }
            }            

            //Edit des edges hrizontal
            //Positionné les edges horizontal, hauteur
            if (posIntersection.x >= 0 && posIntersection.x < m_CurrentMapManager.mapData.width
                && posIntersection.z >= 0 && posIntersection.z < m_CurrentMapManager.mapData.height + 1)
            {
                if (((m_EditEdgesHori) && (!m_EditEdgesVerti) && (!m_IsEdgesModeActivate)))
                {
                    //Création des gizmos pour les Edges Horizontales
                    DisplayGizmosEdgesHoriEdit(posInt);
                    //Edit des Edges Horizontal
                    EditEdgesHoriState(posInt);
                    //suppression du square.lock 
                    m_CurrentMapManager.ProcessRuleSquareVSEdge(edgesHori, hori, new Vector3(0,0,-1));
                }
                if ((m_IsEdgesModeActivate) && (m_EditEdgesHori))
                {
                    //Création des gizmos pour les Edges Horizontales
                    DisplayGizmosEdgesHoriEdit(posInt);
                    //Supression des Edges Horizontal
                    DeleteEdgesHoriState(posInt);
                }
            }
        }

        SceneView.RepaintAll();      
    }
   
   
    private void EditEdgesHoriState(Vector3 posInt)
    {
        if (Event.current.button == 0)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                case EventType.MouseDrag:
                    // On recupères les datas du bon edges en fonction de posInt
                    int index = m_CurrentMapManager.GetIndexEdgesHoriFromPos(posInt);
                    // on set le state du Edges en fct m_squareStateEditMode
                    m_CurrentMapManager.mapData.edgesHori[index] = true;
                    break;
                case EventType.MouseUp:
                    m_CurrentMapManager.CreateMapView();
                    break;
            }
        }       
    }
    private void DeleteEdgesHoriState(Vector3 posInt)
    {
        if (Event.current.button == 0)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                case EventType.MouseDrag:
                   
                    // On recupères les datas du bon edges en fonction de posInt
                    int index = m_CurrentMapManager.GetIndexEdgesHoriFromPos(posInt);
                    // on set le state du Edges en fct m_squareStateEditMode
                    m_CurrentMapManager.mapData.edgesHori[index] = false;
                    break;
                case EventType.MouseUp:
                    m_CurrentMapManager.CreateMapView();
                    break;
            }
        }
    }

    private void EditEdgesVertiState(Vector3 posInt)
    {
        if (Event.current.button == 0)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                case EventType.MouseDrag:
                    // On recupères les datas du bon edges en fonction de posInt                    
                    int index = m_CurrentMapManager.GetIndexEdgesVertiFromPos(posInt);
                    // on set le state du Edges en fct m_squareStateEditMode
                    m_CurrentMapManager.mapData.edgesVert[index] = true;
                    
                    break;
                case EventType.MouseUp:
                    m_CurrentMapManager.CreateMapView();
                    break;
            }
        }
       
    }
    private void DeleteEdgesVertiState(Vector3 posInt)
    {
        if (Event.current.button == 0)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                case EventType.MouseDrag:
                    // On recupères les datas du bon edges en fonction de posInt
                    int index = m_CurrentMapManager.GetIndexEdgesVertiFromPos(posInt);
                    // on set le state du Edges en fct m_squareStateEditMode
                    m_CurrentMapManager.mapData.edgesVert[index] = false;
                    break;
                case EventType.MouseUp:
                    m_CurrentMapManager.CreateMapView();
                    break;
            }
        }

    }
    private void EditSquareState(Vector3 posInt)
    {
        if(Event.current.button == 0)
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                case EventType.MouseDrag:
                    // On recupères les datas du bon square en fonction de posInt
                    int index = m_CurrentMapManager.GetIndexSquareFromPos(posInt);
                    // on set le state du square en fct m_squareStateEditMode
                    m_CurrentMapManager.mapData.grid[index].state = m_SquareStateModeEditMode;
                    //m_CurrentMapManager.mapData.grid[index].brush = m_BrushStateModeEditMode;

                    //on update la vue
                    DeleteEdgesVertiState(posInt);
                    DeleteEdgesVertiState(posInt + new Vector3(1,0,0));
                    DeleteEdgesHoriState(posInt);
                    DeleteEdgesHoriState(posInt + new Vector3(0,0,1));
                    break;
                case EventType.MouseUp:
                    m_CurrentMapManager.CreateMapView();
                    break;
            }
        }
    }

    private Vector3 CalculateInteractPositionPlan()
    {
        //recuperation de la position de la mouse sur le viewport.
        Vector2 pos = Event.current.mousePosition;
        //création d'un rayon dans l'espace 3D par rapport a la camera.
        Ray ray = HandleUtility.GUIPointToWorldRay(pos);

        //création d'un plan temporaire.
        Plane plan = new Plane(Vector3.up, Vector3.zero);
        
        if(plan.Raycast(ray,out float distance))
        {
            return ray.GetPoint(distance);
        }
       
        return (Vector3.zero);
        
    }
    private void DisplayGizmosSquareEdit(Vector3 ponInt)
    {
        Handles.color = m_CurrentMapManager.GetColorFromState (m_SquareStateModeEditMode);
        Vector3 sizeWireSqaure = Vector3.one;
        //Handles.DrawWireDisc(ponInt, Vector3.up, 2);
        ponInt.x += 0.5f;
        ponInt.z += 0.5f;
        sizeWireSqaure.y = 0.25f;
        Handles.DrawWireCube(ponInt,sizeWireSqaure);

    }
    private void DisplayGizmosEdgesVertiEdit(Vector3 ponInt)
    {
        Handles.color = Color.cyan;
        Vector3 sizeWireEdge = Vector3.one;
        ponInt.x += 0f;
        ponInt.z += 0.5f;
        sizeWireEdge.x = 0.25f;
        Handles.DrawWireCube(ponInt, sizeWireEdge);
      
    }
    private void DisplayGizmosEdgesHoriEdit(Vector3 ponInt)
    {
        Handles.color = Color.green;
        Vector3 sizeWireEdge = Vector3.one;
        ponInt.x += 0.5F;
        ponInt.z += 0;
        sizeWireEdge.z = 0.25f;
        Handles.DrawWireCube(ponInt, sizeWireEdge);
       
    }
    private void LoadLastEditorState()
    {
        m_IsInSquareEditMode = EditorPrefs.GetBool(nameof(m_IsInSquareEditMode));
        m_SquareStateModeEditMode = (SquareState)EditorPrefs.GetInt(nameof(m_SquareStateModeEditMode));
    }
    private void SaveCurrentEditorState()
    {
        EditorPrefs.SetBool(nameof(m_IsInSquareEditMode), m_IsInSquareEditMode);
        EditorPrefs.SetInt(nameof(m_SquareStateModeEditMode), (int)m_SquareStateModeEditMode);
    }
}
