using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace KT
{

static class MeshTools
{

static public void NormalizeMesh( Mesh mesh , Transform sel , bool doT = true , bool doS = true )
{
    if( doT ) DoT( sel , mesh );

    if( doS && ( sel.localScale != Vector3.one ) )
    {
        DoS( sel, mesh );
    }
}

static public void DoT( Transform sel, Mesh mesh )
{
    Vector3 off = GetCoM( mesh ) ;

    TranslateMesh( mesh , - off );

    sel.Translate( VMul( off , sel.localScale ) );
}

static public void DoS( Transform sel, Mesh mesh )
{
    Vector3 scale = sel.localScale;

    if( !WindingChange( scale ) )
    {// Winding has to be conserved 
        
        ScaleMesh( mesh , scale );

        FlipNormals( mesh , ( scale.x < 0 ) , ( scale.y < 0 ) , ( scale.z < 0 ) );

        sel.localScale = Vector3.one;
    }
    else
    {
        Debug.LogWarning("Preventing Unwinding.");
    }
}

static public void TranslateMesh( Mesh mesh , Vector3 offset )
{
    Vector3[] vertices = mesh.vertices;

    for (int i = 0, n = vertices.Length ; i < n ; ++i )
    {
        vertices[i] += offset;
    }

    mesh.vertices = vertices;
}

static public void ScaleMesh( Mesh mesh , Vector3 scale )
{
    Vector3[] vertices = mesh.vertices;

    for (int i = 0, n = vertices.Length ; i < n ; ++i )
    {
        vertices[i] = VMul( vertices[i] , scale );
    }

    mesh.vertices = vertices;
}

static bool WindingChange( Vector3 scale )
{
    int cnt = 0;

    if( scale.x < 0 ) ++ cnt;
    if( scale.y < 0 ) ++ cnt;
    if( scale.z < 0 ) ++ cnt;

    return ( cnt % 2 == 1 );
}

static void FlipNormals( Mesh mesh , bool do_X , bool do_Y , bool do_Z )
{
    Vector3[] normals = mesh.normals;

    Vector3 flip_v = Vector3.one;
    
    if( do_X ) { flip_v.x *= -1; }
    if( do_Y ) { flip_v.y *= -1; }
    if( do_Z ) { flip_v.z *= -1; }

    if( flip_v != Vector3.one )
    {
        for (int i = 0, n = normals.Length ; i < n ; ++i )
        {
            normals[i] = VMul( normals[i] , flip_v );
        }

        mesh.normals = normals;
    }   
}

// Center of the geometry.
static public Vector3 GetCoM( Mesh mesh )
{
    Vector3 v_sum = Vector3.zero;

    Vector3[] vertices = mesh.vertices;

    for (int i = 0, n = vertices.Length ; i < n ; ++i )
    {
        v_sum += new Vector3( vertices[i].x , vertices[i].y , vertices[i].z );
    }

    return new Vector3( v_sum.x , v_sum.y , v_sum.z ) / vertices.Length;
}

static Vector3 VMul( Vector3 a , Vector3 b )
{
    return new Vector3( a.x * b.x , a.y * b.y , a.z * b.z );
}

}
}