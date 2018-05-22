namespace EBerzosa.Pundit.Core.Repository
{
   public class RegisteredRepository
   {
      public string Name { get; set; }

      public string Uri { get; set; }

      public string ApiKey { get; set; }

      public RepositoryType Type { get; set; }

      public bool Disabled { get; set; }

      public bool UseForPublishing { get; set; }

      public override string ToString() => Name;
   }
}