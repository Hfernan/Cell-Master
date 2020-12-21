using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KT
{

public class NormMesh : EditorWindow
{
    Mesh source;

    Vector3 v_T = Vector3.zero;
    Vector3 v_S = Vector3.one;

    void OnEnable()
    {
        GUIContent title = titleContent;

        title.text = "Edit Mesh";
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        source = EditorGUILayout.ObjectField(source, typeof( Mesh ), true) as Mesh;

        v_T = EditorGUILayout.Vector3Field("Translate Mesh" , v_T );
        v_S = EditorGUILayout.Vector3Field("Scale Mesh"     , v_S );

        EditorGUILayout.EndVertical();

        if ( GUILayout.Button( "Edit Mesh" ) )
        {
            if (source != null)
            {
                if( v_T != Vector3.zero ) MeshTools.TranslateMesh( source , v_T );
                
                if( v_S != Vector3.one  )
                {
                 Vector3 pre  = MeshTools.GetCoM( source );
                 MeshTools.ScaleMesh    ( source , v_S );
                 Vector3 post = MeshTools.GetCoM( source );
                 MeshTools.TranslateMesh( source , pre - post );
                }
            }
            else
            {
                ShowNotification( new GUIContent( "No mesh selected." ) );
            }
        }
    }
}
}