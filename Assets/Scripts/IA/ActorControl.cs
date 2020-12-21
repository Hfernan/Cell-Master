using UnityEngine;
using UnityEngine.EventSystems;

namespace KT
{
  // Common class inherited by all "living" creatures.
  [RequireComponent( typeof( ActorPath ) )]
  public class ActorControl : MonoBehaviour, IPointerClickHandler
  {
    ActorPath path;

    protected IActorState state;

    // Height of the pivot point of the mesh.
    protected float meshHeight;

    protected virtual void Awake ()
    {
      gameObject.GetCompo( ref path );

      meshHeight = 0.0f;

      state = new ActorEmptyState();
    }

    // Start is called before the first frame update
    protected virtual void Start ()
    {
      path.height = meshHeight;
    }

    // Update is called once per frame
    protected virtual void Update ()
    {
      IActorState nextState = state.OnUpdate( this );

      if ( nextState != null ) StateChange( nextState );
    }

    private void StateChange ( IActorState newState )
    {
      state = newState;

      state.OnStart( this );

      path.EndPath();
    }

    /// <summary>
    /// Moves the actor to the desired location.
    /// </summary>
    /// <param name="tgt">Point on the surface to move to.</param>
    public void MoveTo ( Vector3 tgt , bool over = false )
    {
      if ( !path.IsCalculating() || over )
      {
        path.GoTo( tgt );
      }
    }

    /// <summary>
    /// Teleports the actor to the desired location.
    /// </summary>
    /// <param name="tgt">Target point, height included.</param>
    public virtual void Teleport ( Vector3 tgt )
    {
      path.Teleport( tgt );
    }

    public virtual float GetHeight ()
    {
      return meshHeight;
    }

    public virtual void SetHeight ( float h )
    {
      meshHeight = h;

      if ( path != null ) path.height = h;
    }

    protected virtual void OnPointerClick ( PointerEventData eventData ) { }

    void IPointerClickHandler.OnPointerClick ( PointerEventData eventData )
    {
      OnPointerClick( eventData );
    }
  }
}