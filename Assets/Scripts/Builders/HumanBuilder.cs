using UnityEngine;

namespace KT
{
  public static class HumanBuilder
  {
    // Human observers that receive events.
    public static IHumanTracker UILink;
    public static IHumanTracker CamLink;

    static RNGOD.NameGenerator nameGen = new RNGOD.NameGenerator( RNGOD.NameGenerator.DictSeeds.ENPOKEMON , 3 , 6 );

    public static GameObject Create ( HumanControl.HumanDNA dna = null , bool clone = false )
    {
      GameObject human = Resources.Load<GameObject>( "Actors/Human" );

      HumanControl hControl;

      human.GetCompOrAdd<HumanControl>( out hControl );

      if ( human != null && hControl != null )
      {
        SetColor( human , hControl , dna , clone );

        SetName( hControl );

        SetBirth( hControl );

        SetHeight( hControl );
      }

      return human;
    }

    public static HumanControl Instantiate ( this HumanControl human , Vector3 pos , Vector3 normal )
    {
      // Instantiation clones and return a new GO so we cannot link human and have to fetch the new HumanControl of the clone.
      HumanControl h = GameObject.Instantiate( human.gameObject , pos , Quaternion.FromToRotation( Vector3.up , normal ) ).GetComp<HumanControl>();

      h.AfterInstInit();

      return h;
    }

    /// <summary>
    /// Links a human to the observer classes. Should be done after Instatiation because the listeners can be removed (?)
    /// </summary>
    /// <param name="h"></param>
    private static void AfterInstInit ( this HumanControl h )
    {
      h.onHumanClick.AddListener( UILink.OnHumanClick );
      h.onHumanClick.AddListener( CamLink.OnHumanClick );

      ServiceLoc.Instance?.GetService<HumanityControl>()?.RegisterHuman( h );
    }

    static void SetBirth ( HumanControl h ){ h.SetBirth( ServiceLoc.Instance.GetService<TimeControl>().GetCurrentDate() ); }

    static void SetName  ( HumanControl h ){ h.SetName( nameGen.NextName ); }

    static void SetColor ( GameObject h , HumanControl hControl , HumanControl.HumanDNA dna , bool clone )
    {
      MeshRenderer mr;

      Color col;

      if ( dna != null )
      {
        float _h = ( clone ) ? dna.hue : dna.hue + UnityEngine.Random.Range( -.1f , .1f );

             if ( _h < 0f ) _h += 1f;
        else if ( _h > 1f ) _h -= 1f;

        col = Color.HSVToRGB( _h , 1f , 1f );
      }
      else
      {
        col = ColorCreator.HSVRandom();
      }

      float hue;

      Color.RGBToHSV( col , out hue , out _ , out _ );

      hControl.SetHue( hue );

      // Body
      if ( h.transform.GetChild( 0 ).TryGetComponent( out mr ) )
      {
        Material mat = new Material( mr.sharedMaterial );

        mat.color = col;

        mr.sharedMaterial = mat;
      }
    }

    static void SetHeight ( HumanControl h )
    {
      h.SetHeight( 1f );
    }
  }
}