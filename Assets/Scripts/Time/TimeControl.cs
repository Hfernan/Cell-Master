using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace KT
{
  public class TimeControl : MonoBehaviour
  {
    [HideInInspector] public class MinChangedEvent   : UnityEvent<Calendar.Date> { }
    [HideInInspector] public class HourChangedEvent  : UnityEvent<Calendar.Date> { }
    [HideInInspector] public class DayChangedEvent   : UnityEvent<Calendar.Date> { }
    [HideInInspector] public class MonthChangedEvent : UnityEvent<Calendar.Date> { }
    [HideInInspector] public class YearChangedEvent  : UnityEvent<Calendar.Date> { }

    public MinChangedEvent   onMinChanged   = new MinChangedEvent  ();
    public HourChangedEvent  onHourChanged  = new HourChangedEvent ();
    public DayChangedEvent   onDayChanged   = new DayChangedEvent  ();
    public MonthChangedEvent onMonthChanged = new MonthChangedEvent();
    public YearChangedEvent  onYearChanged  = new YearChangedEvent ();

    Calendar calendar;

    Calendar.Date curDate;

    uint curUnixTime = 0; // Current time.

    uint minPerRealStep = 1; // Minutes per step.

    float sPerStep = 0.05f; // Game seconds per step.

    WaitForSeconds timeWait;

    private void Awake ()
    {
      calendar = CalendarBuilder.CreateGregorian();

      curDate = calendar.DateFromUnix( curUnixTime );

      timeWait = new WaitForSeconds( sPerStep );
    }

    // Start is called before the first frame update
    void Start ()
    {
      StartCoroutine( CO_TimeArrow() );
    }

    void OnDestroy ()
    {
      onMinChanged  .RemoveAllListeners();
      onHourChanged .RemoveAllListeners();
      onDayChanged  .RemoveAllListeners();
      onMonthChanged.RemoveAllListeners();
      onYearChanged .RemoveAllListeners();
    }

    public uint GetCurrentUnixTime () { return curUnixTime; }

    public Calendar.Date GetCurrentDate () { return calendar.DateFromUnix( curUnixTime ); }

    public uint GetDefMinInHour ( Calendar.MiniDate miniHour ) { return calendar.GetDefaultMinsInHour( miniHour ); }

    IEnumerator CO_TimeArrow ()
    {
      while ( true )
      {
        yield return timeWait;
        curUnixTime += minPerRealStep;

        Calendar.Date newDate = calendar.DateFromUnix(curUnixTime);

        // Check what changed and trigger

        if ( curDate.year != newDate.year )
        {
          onYearChanged.Invoke( newDate );
          onMonthChanged.Invoke( newDate );
          onDayChanged.Invoke( newDate );
          onHourChanged.Invoke( newDate );
          onMinChanged.Invoke( newDate );
        }
        else if ( curDate.month != newDate.month )
        {
          onMonthChanged.Invoke( newDate );
          onDayChanged.Invoke( newDate );
          onHourChanged.Invoke( newDate );
          onMinChanged.Invoke( newDate );
        }
        else if ( curDate.day != newDate.day )
        {
          onDayChanged.Invoke( newDate );
          onHourChanged.Invoke( newDate );
          onMinChanged.Invoke( newDate );
        }
        else if ( curDate.hour != newDate.hour )
        {
          onHourChanged.Invoke( newDate );
          onMinChanged.Invoke( newDate );
        }
        else if ( curDate.minute != newDate.minute )
        {
          onMinChanged.Invoke( newDate );
        }

        // Update curDate.
        curDate = newDate;
      }
    }

    /// <summary>
    /// Date to string format. PHP notation: https://www.php.net/manual/es/function.date.php
    /// </summary>
    /// <param name="date"></param>
    /// <param name="fmt"></param>
    /// <returns></returns>
    public string TimeString ( in Calendar.Date date , string fmt = "r" )
    {
      string txt = fmt;

      //      Día	---	---
      //d	Día del mes, 2 dígitos con ceros iniciales	01 a 31
      txt = txt.Replace( "d" , date.day.ToString( "00" ) );
      //D	Una representación textual de un día, tres letras	Mon hasta Sun
      txt = txt.Replace( "D" , ( date.weekdayName.Length > 3 ) ? date.weekdayName.Substring( 0 , 3 ) : date.weekdayName );
      //j	Día del mes sin ceros iniciales	1 a 31
      txt = txt.Replace( "j" , date.day.ToString() );
      //l ('L' minúscula)	Una representación textual completa del día de la semana	Sunday hasta Saturday
      txt = txt.Replace( "l" , date.weekdayName );
      //N	Representación numérica ISO-8601 del día de la semana (añadido en PHP 5.1.0)	1 (para lunes) hasta 7 (para domingo)
      // Not following ISO bc weeks may not have 7 days. 0 first day - N-1 last day.
      txt = txt.Replace( "N" , date.weekday.ToString() );
      //S	Sufijo ordinal inglés para el día del mes, 2 caracteres	st, nd, rd o th. Funciona bien con j
      // --
      //w	Representación numérica del día de la semana	0 (para domingo) hasta 6 (para sábado)
      txt = txt.Replace( "w" , date.weekday.ToString() );
      //z	El día del año (comenzando por 0)	0 hasta 365
      // --
      //W	Número de la semana del año ISO-8601, las semanas comienzan en lunes	Ejemplo: 42 (la 42ª semana del año)
      // --
      //F	Una representación textual completa de un mes, como January o March	January hasta December
      txt = txt.Replace( "F" , date.monthName );
      //m	Representación numérica de un mes, con ceros iniciales	01 hasta 12
      txt = txt.Replace( "m" , date.month.ToString( "00" ) );
      //M	Una representación textual corta de un mes, tres letras	Jan hasta Dec
      txt = txt.Replace( "M" , date.monthName.Substring( 0 , 3 ) );
      //n	Representación numérica de un mes, sin ceros iniciales	1 hasta 12
      txt = txt.Replace( "n" , date.month.ToString() );
      //t	Número de días del mes dado	28 hasta 31
      txt = txt.Replace( "t" , calendar.GetDaysInMonth( Calendar.MiniDate.Month( date.month , date.year ) ).ToString() );
      //Año	---	---
      //L	Si es un año bisiesto	1 si es bisiesto, 0 si no.
      // --
      //o	Año según el número de la semana ISO-8601. Esto tiene el mismo valor que Y, excepto que si el número de la semana ISO (W) pertenece al año anterior o siguiente, se usa ese año en su lugar. (añadido en PHP 5.1.0)	Ejemplos: 1999 o 2003
      // --
      //Y	Una representación numérica completa de un año, 4 dígitos	Ejemplos: 1999 o 2003
      txt = txt.Replace( "Y" , date.year.ToString( "0000" ) );
      //y	Una representación de dos dígitos de un año	Ejemplos: 99 o 03
      txt = txt.Replace( "y" , date.year.ToString( "00" ) );
      //Hora	---	---
      //a	Ante meridiem y Post meridiem en minúsculas	am o pm
      // --
      //A	Ante meridiem y Post meridiem en mayúsculas	AM o PM
      // --
      //B	Hora Internet	000 hasta 999
      // --
      //g	Formato de 12 horas de una hora sin ceros iniciales	1 hasta 12
      txt = txt.Replace( "g" , date.hour.ToString() );
      //G	Formato de 24 horas de una hora sin ceros iniciales	0 hasta 23
      txt = txt.Replace( "G" , date.hour.ToString() );
      //h	Formato de 12 horas de una hora con ceros iniciales	01 hasta 12
      txt = txt.Replace( "h" , date.hour.ToString( "00" ) );
      //H	Formato de 24 horas de una hora con ceros iniciales	00 hasta 23
      txt = txt.Replace( "H" , date.hour.ToString( "00" ) );
      //i	Minutos con ceros iniciales	00 hasta 59
      txt = txt.Replace( "i" , date.minute.ToString( "00" ) );
      //s	Segundos con ceros iniciales	00 hasta 59
      //--
      //u	Microsegundos (añadido en PHP 5.2.2). Observe que date() siempre generará 000000 ya que toma un parámetro de tipo integer, mientras que DateTime::format() admite microsegundos si DateTime fue creado con microsegundos.	Ejemplo: 654321
      // --
      //v	Milisegundos (añadido en PHP 7.0.0). La misma observación se aplica para u.	Example: 654
      // --
      //Zona Horaria	---	---
      //e	Identificador de zona horaria (añadido en PHP 5.1.0)	Ejemplos: UTC, GMT, Atlantic/Azores
      // --
      //I (i mayúscula)	Si la fecha está en horario de verano o no	1 si está en horario de verano, 0 si no.
      // --
      //O	Diferencia de la hora de Greenwich (GMT) sin colon entre horas y minutos	Ejemplo: +0200
      // --
      //P	Diferencia con la hora de Greenwich (GMT) con dos puntos entre horas y minutos (añadido en PHP 5.1.3)	Ejemplo: +02:00
      // --
      //T	Abreviatura de la zona horaria	Ejemplos: EST, MDT ...
      // --
      //Z	Índice de la zona horaria en segundos. El índice para zonas horarias al oeste de UTC siempre es negativo, y para aquellas al este de UTC es siempre positivo.	-43200 hasta 50400
      // --
      //Fecha/Hora Completa	---	---
      //c	Fecha ISO 8601 (añadido en PHP 5)	2004-02-12T15:19:21+00:00
      txt = txt.Replace( "c" , date.year.ToString( "0000" ) + "-" + date.month.ToString( "00" ) + "-" + date.day.ToString( "00" ) + " " + date.hour.ToString( "00" ) + ":" + date.minute.ToString( "00" ) );
      //r	Fecha con formato » RFC 2822	Ejemplo: Thu, 21 Dec 2000 16:01:07 +0200
      txt = txt.Replace( "r" , date.weekdayName + " " + date.day.ToString( "00" ) + " " + date.monthName + " " + date.year.ToString( "0000" ) + " " + date.hour.ToString( "00" ) + ":" + date.minute.ToString( "00" ) );
      //U	Segundos desde la Época Unix (1 de Enero del 1970 00:00:00 GMT)	Vea también time()
      txt = txt.Replace( "U" , date.unixTime.ToString() );

      return txt;
    }

    public string TimeString ( uint time , string fmt = "r" )
    {
      return TimeString( calendar.DateFromUnix( time ) , fmt );
    }
  }
}