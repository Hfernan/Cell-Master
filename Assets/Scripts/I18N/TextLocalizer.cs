using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace KT
{
  [RequireComponent( typeof( TextMeshProUGUI ) )]
  public class TextLocalizer : MonoBehaviour
  {
    public enum Id : int
    {
      Empty,

      Title,
      Subtitle,

      MenuStart,
      MenuInstructions,
      MenuCredits,
      MenuQuit,

      CredDesign,
      CredUIIcon,

      Icon1,
      Icon2,
      Icon3,
      Icon4,

      Return,

      InstTitle,
      Inst11,
      Inst21,
      Inst31,
      Inst32,
      Inst33,
      Inst41,
      Inst42,
      Inst43,
      Inst51,
      Inst52,
      Inst53,

      DetName,
      DetAge,
      DetHunger,
      DetColor,

      DetStarving,
      DetHungry,
      DetOK,
      DetFull,

      End,
    }

    public enum Languages : int
    {
      English,
      Spanish,
    }

    public static Languages CurrentLanguage = Languages.English;

    static Dictionary<Languages, Dictionary< Id, string > > Translations = new Dictionary<Languages, Dictionary<Id, string>>()
    {
      [Languages.English] = new Dictionary< Id, string >()
      {
        [Id.Empty] = "",

        [Id.Title   ] = "Cell Master",
        [Id.Subtitle] = "Release your inner biologist",

        [Id.MenuStart       ] = "Start",
        [Id.MenuInstructions] = "How To Play",
        [Id.MenuCredits     ] = "Credits",
        [Id.MenuQuit        ] = "Quit",

        // Credits.

        [Id.CredDesign] = ">>> Design & Programming <<<",
        [Id.CredUIIcon] = ">>> UI Icons <<<",

        [Id.Icon1] = "Poison by Nubaia Karim Barsha",
        [Id.Icon2] = "Food by Rahmat Hidayat",
        [Id.Icon3] = "Teleport by Jems Mayor",
        [Id.Icon4] = "Hunger by Luis Prado",

        // Buttons.

        [Id.Return] = "Return",

        [Id.InstTitle] = ">>> How To Play <<<",

        [Id.Inst11] = "Welcome to the KP Laboratories!\n\n\nYou have been selected for a job interview at our peaceful company!\n\nThe first step is a small skill exam.\n\nTurn the page to find the detailed instructions.",
        [Id.Inst21] = "Main Controls\n\n\n\n\nWASD keys can be used to look around.\n\nThe wheel mouse allows you to zoom in and out.\n\nNumber 1-4 control the cells speed.\n\nAnd 0 pauses the game completely.",
        [Id.Inst31] = "We are providing you a Petri dish with several cells.",
        [Id.Inst32] = "The system puts food in the dish every hour so the cells can eat and grow.",
        [Id.Inst33] = "You are allowed to place and remove food but the tools need to cooldown in order to clean them.",
        [Id.Inst41] = "You can also move and remove cells from the Petri dish.",
        [Id.Inst42] = "The cells will search for food until they have Filled their reserves.",
        [Id.Inst43] = "When they are full, they will attempt to duplicate.",
        [Id.Inst51] = "The new cell may have mutated and show a different color.",
        [Id.Inst52] = "We will evaluate them every hour until they reach a 95% likelihood or better.",
        [Id.Inst53] = "The goal is to get a group of cells close to the target color shown.",

        [Id.DetAge   ] = "Age",
        [Id.DetColor ] = "Color",
        [Id.DetHunger] = "Hunger",
        [Id.DetName  ] = "Name",

        [Id.DetStarving] = "Starving",
        [Id.DetHungry  ] = "Hungry",
        [Id.DetOK      ] = "OK",
        [Id.DetFull    ] = "Full",

        [Id.End] = "End",
      },
      [Languages.Spanish] = new Dictionary<Id , string>()
      {
        [Id.Empty] = "",

        [Id.Title   ] = "Cell Master",
        [Id.Subtitle] = "Descubre tu biologo interior",

        [Id.MenuStart       ] = "Empezar",
        [Id.MenuInstructions] = "Cómo jugar",
        [Id.MenuCredits     ] = "Créditos",
        [Id.MenuQuit        ] = "Salir",

        [Id.CredDesign] = ">>> Diseño y Programación <<<",
        [Id.CredUIIcon] = ">>> Iconos IU <<<",

        [Id.Icon1] = "Veneno por Nubaia Karim Barsha",
        [Id.Icon2] = "Comida por Rahmat Hidayat",
        [Id.Icon3] = "Teletransporte por Jems Mayor",
        [Id.Icon4] = "Hambre por Luis Prado",

        [Id.Return] = "Volver",

        [Id.InstTitle] = ">>> Cómo Gugar <<<",

        [Id.Inst11] = "¡Bienvenido a los Laboratorios KP!\n\n\n¡Has sido elegido para una entrevista de trabajo en nuestra amada compañía!\n\nEl primer paso es una pequeña prueba.\n\nSigue leyendo para encontrar las instrucciones.",
        [Id.Inst21] = "Controles\n\n\n\n\nWASD para moverse alrededor.\n\nLa rueda del ratón para hacer zoom.\n\nNúmeros 1-4 para controlar la velocidad.\n\nY el 0 para pausar el juego.",
        [Id.Inst31] = "Tendrás una placa Petri con varias células.",
        [Id.Inst32] = "El sistema colocará comida cada hora para que las células coman y crezcan.",
        [Id.Inst33] = "Puedes poner y quitar comida pero las herramientas necesitan limpiarse después de cada uso.",
        [Id.Inst41] = "También puedes mover y quitar células de la placa Petri.",
        [Id.Inst42] = "Las células buscarán comida hasta que dejen de estar hambrientas.",
        [Id.Inst43] = "En ese momento, intentarán replicarse.",
        [Id.Inst51] = "La nueva célula puede haber mutado y ser de un color diferente.",
        [Id.Inst52] = "Cada hora evaluaremos tu progreso hasta que alcance el 95% de parecido.",
        [Id.Inst53] = "El objetivo es conseguir un grupo de células con un color cercano al mostrado.",

        [Id.DetAge   ] = "Edad",
        [Id.DetColor ] = "Color",
        [Id.DetHunger] = "Hambre",
        [Id.DetName  ] = "Nombre",

        [Id.DetStarving] = "Muriendo",
        [Id.DetHungry  ] = "Hambriento",
        [Id.DetOK      ] = "Bien",
        [Id.DetFull    ] = "Lleno",

        [Id.End] = "Volver",
      }
    };

    [SerializeField] Id id;

    // Just to shorten code outside this class.
    public static string Get ( Id id )
    {
      string o = "";

      if ( !Translations[CurrentLanguage].TryGetValue( id , out o ) )
      {
        Translations[Languages.English].TryGetValue( id , out o );
      }

      return o;
    }

    void Start ()
    {
      GetComponent<TextMeshProUGUI>().text = Get( id );
    }

    void OnValidate ()
    {
      GetComponent<TextMeshProUGUI>().text = Get( id );
    }
  }
}