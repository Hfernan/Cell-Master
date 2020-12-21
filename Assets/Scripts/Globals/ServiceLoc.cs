using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ServiceLoc
{
  public  static ServiceLoc Instance => _instance ?? ( _instance = new ServiceLoc() );

  private static ServiceLoc _instance;

  private readonly Dictionary< Type , object > services;

  private ServiceLoc ()
  {
    services = new Dictionary<Type , object>();
  }

  public void RegisterService<T> ( T service )
  {
    Type type = typeof( T );
    
    Assert.IsFalse( services.ContainsKey( type ) , $"Service {type} already registered" );

    services.Add( type , service );
  }

  public T GetService<T> ()
  {
    Type type = typeof(T);

    object service = null;

    if ( !services.TryGetValue( type , out service ) )
    {
      throw new Exception( $"Service {type} not found" );
    }

    Assert.IsNotNull( service );

    return ( T ) service;
  }
}