using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KT {

public class GeomNormGUI
{

// Normalize Mesh found in a GO.

[MenuItem("Tools/Mesh Normalizer/Normalize from GameObject..." , false , 2 )]
static void InitGO()
{
    // Get existing open window or if none, make a new one:
    NormGO window = (NormGO)EditorWindow.GetWindow(typeof(NormGO));
    window.Show();
}

// Directly edit a Mesh. May be dragged from Assets.

[MenuItem("Tools/Mesh Normalizer/Translate or Scale Mesh..." , false , 3 )]
static void InitMesh()
{
    // Get existing open window or if none, make a new one:
    NormMesh window = (NormMesh)EditorWindow.GetWindow(typeof(NormMesh));
    window.Show();
}

// Context menu in MeshFilter Components.

[MenuItem("CONTEXT/MeshFilter/Normalize Mesh")]
static void MeshFilterCtx()
{
    Transform sel = Selection.activeTransform;

    MeshFilter mf = sel.GetComponent< MeshFilter >();

    if( mf != null )
    {
        Mesh mesh = mf.sharedMesh;

        MeshTools.NormalizeMesh( mesh, sel, /* do T */ true , /* doS */ true );            
    }
    else
    {
        Debug.LogWarning("Mesh not found in " + Selection.activeTransform.name);
    }
}

// Quick Normalization with default params.

[MenuItem("Tools/Mesh Normalizer/Quick Normalize Scale" , true )]
static bool ValidateQuick()
{
    // Return false if no transform is selected.
    return Selection.activeTransform != null;
}

[MenuItem("Tools/Mesh Normalizer/Quick Normalize Scale" , false , 0 )]
static void NormalizeSelection()
{
    // Moves the pivot to the center of the geometry.
    // Changes the mesh so that the local scale can be Vector3.one withough moving the vertex.

    foreach (Transform sel in Selection.transforms)
    {
        MeshFilter mf = sel.GetComponent< MeshFilter >();

        if( mf != null )
        {
            Mesh mesh = mf.sharedMesh;

            MeshTools.NormalizeMesh( mesh, sel , /* do T */ false , /* do S */ true );            
        }
        else
        {
            Debug.LogWarning("Mesh not found in " + Selection.activeTransform.name);
        }
    }
}

[MenuItem("Tools/Mesh Normalizer/Move Pivot to Center" , true )]
static bool ValidatePivot()
{
    // Return false if no transform is selected.
    return Selection.activeTransform != null;
}

[MenuItem("Tools/Mesh Normalizer/Pivot to Center" , false , 1 )]
static void CenterPivotSelection()
{
    // Moves the pivot to the center of the geometry.
    // Changes the mesh so that the local scale can be Vector3.one withough moving the vertex.

    foreach (Transform sel in Selection.transforms)
    {
        MeshFilter mf = sel.GetComponent< MeshFilter >();

        if( mf != null )
        {
            Mesh mesh = mf.sharedMesh;

            MeshTools.NormalizeMesh( mesh, sel , /* do T */ true , /* do S */ false );
        }
        else
        {
            Debug.LogWarning("Mesh not found in " + Selection.activeTransform.name);
        }
    }
}
}

}