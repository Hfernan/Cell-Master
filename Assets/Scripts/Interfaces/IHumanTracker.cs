namespace KT
{
// For classes that link to humans upon creation.
  public interface IHumanTracker
  {
    void OnHumanClick ( HumanControl h );
    void OnHumanDataChange ( HumanControl h );
    void OnHumanDeath ( HumanControl h );
  }
}