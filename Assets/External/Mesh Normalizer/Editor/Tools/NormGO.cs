using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KT
{

public class NormGO : EditorWindow
{
    public GameObject source;

    bool do_T = true;
    bool do_S = true;

    void OnEnable()
    {
        GUIContent title = titleContent;

        title.text = "Normalize GameObject";
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        source = EditorGUILayout.ObjectField(source, typeof(GameObject), true) as GameObject;

        do_T = EditorGUILayout.Toggle( "Move Pivot to Center" , do_T );
        do_S = EditorGUILayout.Toggle( "Normalize Scale"      , do_S );

        EditorGUILayout.EndVertical();

        if (GUILayout.Button("Normalize Mesh"))
        {
            if (source != null)
            {
                MeshFilter mf = source.GetComponent< MeshFilter >();

                if( mf != null )
                {
                    if( do_T ) MeshTools.DoT( source.transform, mf.sharedMesh );
                    if( do_S ) MeshTools.DoS( source.transform, mf.sharedMesh );
                }
                else
                {
                    ShowNotification( new GUIContent( "Couldn't find a mesh." ) );    
                }
            }
            else
            {
                ShowNotification( new GUIContent( "No object selected." ) );
            }
        }
    }
}
}