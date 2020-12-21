namespace KT
{
  public static class CalendarBuilder
  {
    /// <summary>
    /// Earth calendar without leap years.
    /// </summary>
    public static Calendar CreateGregorian ()
    {
      Calendar cal = new Calendar();

      cal.SetDefaultHoursInDay( ( _ ) => { return 60u; } );

      cal.SetDefaultDaysinMonth( ( _ ) => { return 24u; } );

      cal.SetWeekLenght( 7 );
      
      cal.SetWeekdayName( 0 , "D" );
      cal.SetWeekdayName( 1 , "L" );
      cal.SetWeekdayName( 2 , "M" );
      cal.SetWeekdayName( 3 , "X" );
      cal.SetWeekdayName( 4 , "J" );
      cal.SetWeekdayName( 5 , "V" );
      cal.SetWeekdayName( 6 , "S" );
      
      cal.SetDefaultDaysinMonth( ( miniDate ) =>
      {
       return ( miniDate.month == 2 ) ? 28u : ( miniDate.month == 4 || miniDate.month == 6 || miniDate.month == 9 || miniDate.month == 11 ) ? 30u : 31u;
      }
      );

      for ( uint i = 1 ; i < 13 ; ++i )
      {
        string name = "";
      
        switch ( i )
        {
          case  1: name = "Jan"; break;
          case  2: name = "Feb"; break;
          case  3: name = "Mar"; break;
          case  4: name = "Apr"; break;
          case  5: name = "May"; break;
          case  6: name = "Jun"; break;
          case  7: name = "Jul"; break;
          case  8: name = "Ago"; break;
          case  9: name = "Sep"; break;
          case 10: name = "Oct"; break;
          case 11: name = "Nov"; break;
          case 12: name = "Dec"; break;
        }
      
        if ( name != "" ) cal.SetMonthName( i , name );
      }
      
      cal.SetDefaultMonthsInYear( ( _ ) => { return 12u; } );

      return cal;
    }
  }
}