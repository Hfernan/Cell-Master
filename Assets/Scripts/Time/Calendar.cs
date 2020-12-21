using System;
using System.Collections.Generic;
using UnityEngine;

namespace KT
{
  public class Calendar
  {
    public struct MiniDate
    {
      // What the mini date really represents.
      public enum Cutoff
      {
        // Order matters.
        minute,
        hour,
        day,
        month,
        year,
      }

      public Cutoff cutoff;

      public uint minute;
      public uint hour;
      public uint day;
      public uint month;
      public uint year;

      public static MiniDate FromDate ( Date date , Cutoff cut )
      {
        MiniDate mini;

        mini.cutoff = cut;

        mini.minute = ( cut < Cutoff.hour  ) ? date.minute : 0u;
        mini.hour   = ( cut < Cutoff.day   ) ? date.hour   : 0u;
        mini.day    = ( cut < Cutoff.month ) ? date.day    : 0u;
        mini.month  = ( cut < Cutoff.year  ) ? date.month  : 0u;

        mini.year   = date.year;

        return mini;
      }

      public static MiniDate Hour ( uint hour, uint day, uint month, uint year )
      {
        MiniDate mini;

        mini.cutoff = Cutoff.hour;

        mini.minute = 0u;
        mini.hour   = hour ;
        mini.day    = day  ;
        mini.month  = month;

        mini.year   = year;

        return mini;
      }

      public static MiniDate Day ( uint day, uint month, uint year )
      {
        MiniDate mini;

        mini.cutoff = Cutoff.day;

        mini.minute = 0u   ;
        mini.hour   = 0u   ;
        mini.day    = day  ;
        mini.month  = month;

        mini.year   = year;

        return mini;
      }

      public static MiniDate Month ( uint month, uint year )
      {
        MiniDate mini;

        mini.cutoff = Cutoff.month;

        mini.minute = 0u   ;
        mini.hour   = 0u   ;
        mini.day    = 0u   ;
        mini.month  = month;

        mini.year   = year;

        return mini;
      }

      public static MiniDate Year ( uint year )
      {
        MiniDate mini;

        mini.cutoff = Cutoff.year;

        mini.minute = 0u;
        mini.hour   = 0u;
        mini.day    = 0u;
        mini.month  = 0u;

        mini.year   = year;

        return mini;
      }

      public static void AddToCutoff( ref MiniDate minidate ,uint qty )
      { 
             if( minidate.cutoff == Cutoff.minute ) minidate.minute += qty;
        else if( minidate.cutoff == Cutoff.hour   ) minidate.hour   += qty;
        else if( minidate.cutoff == Cutoff.day    ) minidate.day    += qty;
        else if( minidate.cutoff == Cutoff.month  ) minidate.month  += qty;
        else if( minidate.cutoff == Cutoff.year   ) minidate.year   += qty;
      }
    }

    [Serializable]
    public struct Date
    {
      public uint unixTime;

      public uint minute       ;
      public uint hour         ;
      public uint day          ;
      public uint week         ;
      public uint weekday      ;
      public string weekdayName;
      public uint month        ;
      public string monthName  ;
      public uint year         ;

      public float dayFrac;

      public Date ( Date date )
      {
        unixTime = date.unixTime;

        minute      = date.minute     ;
        hour        = date.hour       ;
        day         = date.day        ;
        week        = date.week       ;
        weekday     = date.weekday    ;
        weekdayName = date.weekdayName;
        month       = date.month      ;
        monthName   = date.monthName  ;
        year        = date.year       ;

        dayFrac = date.dayFrac;
      }
    }

    // Default values.
    // This could be a Func.
    Func< MiniDate, uint > defMinsInHour   = ( ( mini ) => { return 60u; } );
    Func< MiniDate, uint > defHoursInDay   = ( ( mini ) => { return 24u; } );
    Func< MiniDate, uint > defDaysInMonth  = ( ( mini ) => { return 30u; } );
    Func< MiniDate, uint > defMonthsInYear = ( ( mini ) => { return 12u; } );

    // Cache for internal Use.

    Dictionary< MiniDate , uint> minsInDay   = new Dictionary< MiniDate , uint>();
    Dictionary< MiniDate , uint> minsInMonth = new Dictionary< MiniDate , uint>();
    Dictionary< MiniDate , uint> minsInYear  = new Dictionary< MiniDate , uint>();
    Dictionary< MiniDate , uint> daysInYear  = new Dictionary< MiniDate , uint>();

    Dictionary< MiniDate , uint > monthStartingWeekday = new Dictionary<MiniDate, uint>();

    Dictionary< MiniDate , uint > yearWeekDiff         = new Dictionary<MiniDate, uint>();

    uint GetMinsInDay ( MiniDate miniDay )
    {
      Debug.Assert( miniDay.cutoff == MiniDate.Cutoff.day );
      uint mins = 0;

      if ( !minsInDay.TryGetValue( miniDay , out mins ) )
      {
        // Calculate

        for ( uint i = 1, n = GetHoursInDay( miniDay ) ; ( i <= n ) ; ++i )
        {
          MiniDate miniHour = miniDay;

          miniHour.hour = i;

          miniHour.cutoff = MiniDate.Cutoff.hour;

          mins += GetMinsInHour( miniHour );
        }

        // Then store
        minsInDay[miniDay] = mins;
      }
      return mins;
    }

    uint GetMinsInMonth ( MiniDate miniMonth )
    {
      Debug.Assert( miniMonth.cutoff == MiniDate.Cutoff.month );
      uint mins = 0;

      if ( !minsInMonth.TryGetValue( miniMonth , out mins ) )
      {
        // Calculate

        for ( uint i = 1, n = GetDaysInMonth( miniMonth ) ; ( i <= n ) ; ++i )
        {
          MiniDate miniDay = miniMonth;

          miniDay.day = i;
          miniDay.cutoff = MiniDate.Cutoff.day;

          mins += GetMinsInDay( miniDay );
        }

        // Then store

        minsInMonth[miniMonth] = mins;
      }
      return mins;
    }

    uint GetDaysInYear ( MiniDate miniYear )
    {
      Debug.Assert( miniYear.cutoff == MiniDate.Cutoff.year );
      uint days = 0;

      if ( !daysInYear.TryGetValue( miniYear , out days ) )
      {
        // Calculate

        for ( uint i = 1, n = GetMonthsInYear( miniYear ) ; ( i <= n ) ; ++i )
        {
          MiniDate miniMonth = miniYear;

          miniMonth.month = i;
          miniMonth.cutoff = MiniDate.Cutoff.month;

          days += GetDaysInMonth( miniMonth );
        }

        // Then store

        daysInYear[ miniYear ] = days;
      }

      return days;
    }

    uint GetMinsInYear ( MiniDate miniYear )
    {
      Debug.Assert( miniYear.cutoff == MiniDate.Cutoff.year );
      uint mins = 0;

      if ( !minsInYear.TryGetValue( miniYear , out mins ) )
      {
        // Calculate

        for ( uint i = 1, n = GetMonthsInYear( miniYear ) ; ( i <= n ) ; ++i )
        {
          MiniDate miniMonth = miniYear;

          miniMonth.month = i;
          miniMonth.cutoff = MiniDate.Cutoff.month;

          mins += GetMinsInMonth( miniMonth );
        }

        // Then store

        minsInYear[miniYear] = mins;
      }
      return mins;
    }

    /// <summary>
    /// Calculates the weekday of the first day of a month relative to the first day of the given year weekday.
    /// </summary>
    /// <returns>Weekday difference between "1st-Jan" and 1st of the given month.</returns>
    uint GetMonthStartingWeekday ( MiniDate miniMonth )
    {
      Debug.Assert( miniMonth.cutoff == MiniDate.Cutoff.month );
      uint weekday = 0;

      for ( uint i = 1 ; i < miniMonth.month ; ++i )
      {
        weekday += GetDaysInMonth( MiniDate.Month( i , miniMonth.year ) );
      }

      weekday %= weekLenght;

      return weekday;
    }

    /// <summary>
    /// Calculates the weekday for 1st Jan of the given year.
    /// </summary>
    uint GetYearStartingWeekday ( MiniDate miniYear )
    {
      Debug.Assert( miniYear.cutoff == MiniDate.Cutoff.year );
      uint diff = 0;

      if ( !yearWeekDiff.TryGetValue( miniYear , out diff ) )
      {
        //Calc
      
        diff = ( miniYear.year != 1 ) ? GetYearStartingWeekday( MiniDate.Year( miniYear.year - 1 ) ) + GetDaysInYear( MiniDate.Year( miniYear.year - 1 ) ) : 0;
      
        diff %= weekLenght;
      
        yearWeekDiff[miniYear] = diff;
      }

      return diff;
    }

    // Proper calendar values.

    Dictionary< MiniDate , uint> hourToMins   = new Dictionary< MiniDate , uint>();
    Dictionary< MiniDate , uint> dayToHours   = new Dictionary< MiniDate , uint>();
    Dictionary< MiniDate , uint> monthToDays  = new Dictionary< MiniDate , uint>();
    Dictionary< MiniDate , uint> yearToMonths = new Dictionary< MiniDate , uint>();

    // Week

    uint weekLenght = 7;

    public uint GetWeekLenght () { return weekLenght; }

    public void SetWeekLenght ( uint qty ) { weekLenght = qty; }

    // Names.

    string defWeekDayName = "Def";
    string defMonthName = "Default";

    Dictionary< uint, string > weekdayNames = new Dictionary<uint, string>();
    Dictionary< uint , string > monthNames  = new Dictionary<uint , string>();

    // Get - Set functions.

    public uint GetMinsInHour   ( MiniDate minidate ) { uint mins  ; if ( !hourToMins  .TryGetValue( minidate , out mins   ) ) { mins   = defMinsInHour  ( minidate ); } return mins; }
    public uint GetHoursInDay   ( MiniDate minidate ) { uint hours ; if ( !dayToHours  .TryGetValue( minidate , out hours  ) ) { hours  = defHoursInDay  ( minidate ); } return hours; }
    public uint GetDaysInMonth  ( MiniDate minidate ) { uint days  ; if ( !monthToDays .TryGetValue( minidate , out days   ) ) { days   = defDaysInMonth ( minidate ); } return days; }
    public uint GetMonthsInYear ( MiniDate minidate ) { uint months; if ( !yearToMonths.TryGetValue( minidate , out months ) ) { months = defMonthsInYear( minidate ); } return months; }

    public string GetWeekdayName ( uint weekday ) { string name; if ( !weekdayNames.TryGetValue( weekday , out name ) ) { name = defWeekDayName; } return name; }
    public string GetMonthName   ( uint month   ) { string name; if ( !  monthNames.TryGetValue( month   , out name ) ) { name = defMonthName;   } return name; }

    public uint GetDefaultMinsInHour   ( MiniDate minidate ) { return defMinsInHour  ( minidate ); }
    public uint GetDefaultHoursInDay   ( MiniDate minidate ) { return defHoursInDay  ( minidate ); }
    public uint GetDefaultDaysinMonth  ( MiniDate minidate ) { return defDaysInMonth ( minidate ); }
    public uint GetDefaultMonthsInYear ( MiniDate minidate ) { return defMonthsInYear( minidate ); }

    public void SetMinsInHour   ( MiniDate minidate , uint mins   ) {  hourToMins  [ minidate ] = mins;   }
    public void SetHoursInDay   ( MiniDate minidate , uint hours  ) {   dayToHours [ minidate ] = hours;  }
    public void SetDaysInMonth  ( MiniDate minidate , uint days   ) { monthToDays  [ minidate ] = days;   }
    public void SetMonthsInYear ( MiniDate minidate , uint months ) {  yearToMonths[ minidate ] = months; }

    public void SetWeekdayName ( uint weekday , string name ) { weekdayNames[weekday] = name; }
    public void SetMonthName ( uint month , string name ) { monthNames[month] = name; }

    public void SetDefaultMinsInHour   ( Func< MiniDate, uint > qty ) { defMinsInHour   = qty; }
    public void SetDefaultHoursInDay   ( Func< MiniDate, uint > qty ) { defHoursInDay   = qty; }
    public void SetDefaultDaysinMonth  ( Func< MiniDate, uint > qty ) { defDaysInMonth  = qty; }
    public void SetDefaultMonthsInYear ( Func< MiniDate, uint > qty ) { defMonthsInYear = qty; }

    /// <summary>
    /// From Unix Time to current calendar.
    /// </summary>
    /// <param name="unix">Minutes since the start of the calendar</param>
    /// <returns></returns>
    public Date DateFromUnix ( uint unix )
    {
      Date date = new Date();

      date.unixTime = unix;

      uint timeLeft = unix;

      // Determine the year/month/day.
      // Order matters.
      MiniDate miniDate = MiniDate.Day( 1 , 1 , 1 );

      miniDate.cutoff = MiniDate.Cutoff.year;
      
      ComputeTime( ref timeLeft , GetMinsInYear  , ref miniDate ); miniDate.cutoff = MiniDate.Cutoff.month ;
      ComputeTime( ref timeLeft , GetMinsInMonth , ref miniDate ); miniDate.cutoff = MiniDate.Cutoff.day   ;
      ComputeTime( ref timeLeft , GetMinsInDay   , ref miniDate ); miniDate.cutoff = MiniDate.Cutoff.hour  ;
      ComputeTime( ref timeLeft , GetMinsInHour  , ref miniDate ); miniDate.cutoff = MiniDate.Cutoff.minute;

      date.year  = miniDate.year ;
      date.month = miniDate.month;
      date.day   = miniDate.day  ;
      date.hour  = miniDate.hour ;

      date.minute = timeLeft;

      date.weekday = CalcWeekday( date.year , date.month , date.day );

      date.weekdayName = GetWeekdayName( date.weekday );

      date.monthName = GetMonthName( date.month );

      date.dayFrac = ( float ) ( date.hour + ( ( float ) date.minute / GetMinsInHour( MiniDate.Hour( date.hour , date.day , date.month , date.year ) ) ) ) 
                               / ( GetHoursInDay( MiniDate.Day( date.day , date.month , date.year ) ) );

      return date;
    }


    /// <summary>
    /// Removes, from timeLeft, an amount of time sequentially given by Func starting at offset.
    /// </summary>
    /// <param name="timeLeft">Starting time.</param>
    /// <param name="Func">Func against the time is checked. Starts checking Func(offset) then Func(offset+1) until timeLeft is lesser.</param>
    /// <param name="offset">Index at which Func starts.</param>
    /// <returns></returns>
    void ComputeTime ( ref uint timeLeft , Func< MiniDate , uint> Func , ref MiniDate offset )
    {
      // Division is the potential Year/Month/Day/Hour.
      // This assumes the timeLest is shorter than the division's span of time in minutes.
      //MiniDate division = offset;

      bool divFound = false;

      while ( !divFound )
      {
        uint m = Func( offset );

        if ( m <= timeLeft )
        {
          timeLeft -= m;
          MiniDate.AddToCutoff( ref offset , 1 );
        }
        else
        {
          divFound = true;
        }
      }
    }

    uint CalcWeekday ( uint year , uint month , uint day )
    {
      uint weekday = 0;

      weekday = GetYearStartingWeekday( MiniDate.Year( year ) ) + GetMonthStartingWeekday( MiniDate.Month( month , year ) ) + ( day - 1 );

      weekday %= weekLenght;

      return weekday;
    }
  }
}