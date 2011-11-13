using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pundit.Core.Model;
using Pundit.Core.Utils;

namespace Pundit.Core.Application.Console.Commands
{
   class RepoConsoleCommand : BaseConsoleCommand
   {
      public RepoConsoleCommand(IConsoleOutput console, string currentDirectory, string[] args) : base(console, currentDirectory, args)
      {
      }

      public override void Execute()
      {
         if(NamelessParameters.Length == 0)
            throw new ArgumentException("no action");

         string action = NamelessParameters[0];

         if(action == "list")
            ListRepositories();
         else if(action == "delete")
            DeleteRepository();
         else if(action == "add")
            AddRepository();
         else if(action == "caps")
            UpdateCaps();
         else if(action == "update")
            Update();
         else if(action == "purgelocal")
            PurgeLocalRepository();
         else throw new ArgumentException("unknown action " + action);
      }

      private void PurgeLocalRepository()
      {
         console.WriteLine("purging local repository data...");
         LocalConfiguration.RepositoryManager.ZapCache();
         console.WriteLine(ConsoleColor.Green, "complete");
      }

      private void ListRepositories()
      {
         LocalStats stats = LocalConfiguration.RepositoryManager.Stats;

         console.WriteLine("local repository");
         console.Write("env:".PadRight(20));
         console.Write(Environment.GetEnvironmentVariable(LocalConfiguration.LocalRepositoryRootVar) ?? "<not set>");
         console.Write(" (");
         console.Write(LocalConfiguration.LocalRepositoryRootVar);
         console.WriteLine(")");
         console.Write("location:".PadRight(20));
         console.WriteLine(LocalConfiguration.DbLocation);
         console.Write("occupied space:".PadRight(20));
         console.WriteLine(PathUtils.FileSizeToString(stats.OccupiedSpaceTotal));
         console.Write("binary cache:".PadRight(20));
         console.WriteLine(PathUtils.FileSizeToString(stats.OccupiedSpaceBinaries));
         console.Write("manifests:".PadRight(20));
         console.WriteLine(stats.TotalManifestsCount.ToString());

         console.WriteLine("");
         console.Write("configured repositories ({0}):", LocalConfiguration.RepositoryManager.AllRepositories.Count());
         foreach (Repo rr in LocalConfiguration.RepositoryManager.AllRepositories)
         {
            console.WriteLine("");

            console.Write("tag:".PadRight(20));
            console.WriteLine(rr.Tag);

            console.Write("enabled:".PadRight(20));
            console.WriteLine(rr.IsEnabled ? ConsoleColor.Green : ConsoleColor.Red, rr.IsEnabled ? "yes" : "no");

            console.Write("publish:".PadRight(20));
            console.WriteLine(rr.UseForPublishing ? ConsoleColor.Green : ConsoleColor.Yellow,
               rr.UseForPublishing ? "yes" : "no");

            console.Write("url:".PadRight(20));
            console.WriteLine(rr.Uri);

            console.Write("refresh:".PadRight(20));
            console.Write("every ");
            console.Write(rr.RefreshIntervalInHours.ToString());
            console.WriteLine(" hour(s)");

            console.Write("last refreshed:".PadRight(20));
            console.WriteLine(rr.LastRefreshed == DateTime.MinValue ? "never" : rr.LastRefreshed.ToString());

            console.Write("delta:".PadRight(20));
            console.WriteLine(rr.LastChangeId ?? "<null>");
         }

      }

      private void DeleteRepository()
      {
         string tag = GetParameter("tag:", 1);
         if (string.IsNullOrEmpty(tag)) throw new ApplicationException("repository tag required");

         console.Write("deleting repository...");

         try
         {
            Repo r = LocalConfiguration.RepositoryManager.GetRepositoryByTag(tag);
            if (r == null) throw new ApplicationException("repository not found");

            LocalConfiguration.RepositoryManager.Unregister(r.Id);
            console.Write(true);
         }
         catch
         {
            console.Write(false);
            throw;
         }
      }

      private void AddRepository()
      {
         string tag = GetParameter("tag:", 1);
         string uri = GetParameter("uri:", 2);
         int hours = GetIntParameter("refresh:", 3);

         if(string.IsNullOrEmpty(tag)) throw new ApplicationException("repository tag required");
         if(string.IsNullOrEmpty(uri)) throw new ApplicationException("repository uri required");
         if(hours < 1) throw new ApplicationException("positive number of hours required");
         if(LocalConfiguration.RepositoryManager.AllRepositories.Any(r => r.Tag == tag))
            throw new ApplicationException("repository '" + tag + "' already registered");

         console.WriteLine("adding repository '{0}' from {1}, refresh interval: {2} hour(s)...",
            tag, uri, hours);

         IRemoteRepository repository = RemoteRepositoryFactory.Create(uri);

         console.Write("fetching first snapshot...");
         string nextChangeId;
         var snapshot = repository.GetSnapshot(null, out nextChangeId);

         if(snapshot != null && snapshot.Length > 0)
         {
            console.Write(true);
            long repoId = 0;

            try
            {
               Repo newRepo;
               try
               {
                  console.Write("registering repository...");
                  Repo newRepo1 = new Repo(tag, uri);
                  newRepo1.RefreshIntervalInHours = hours;
                  newRepo1.LastRefreshed = DateTime.Now;
                  newRepo1.LastChangeId = nextChangeId;
                  newRepo1.IsEnabled = true;
                  newRepo1.UseForPublishing = false;
                  newRepo = LocalConfiguration.RepositoryManager.Register(newRepo1);
                  repoId = newRepo.Id;
                  console.Write(true);
               }
               catch
               {
                  console.Write(false);
                  throw;
               }

               console.Write("persisting {0} snapshot entries...", snapshot.Length);
               LocalConfiguration.RepositoryManager.PlaySnapshot(newRepo, snapshot);
               console.Write(true);
               console.WriteLine(ConsoleColor.Green, "repository added");
            }
            catch(Exception ex)
            {
               console.WriteLine(ConsoleColor.Red, "failed to register repository: " + ex.ToString());

               if(repoId != 0)
                  LocalConfiguration.RepositoryManager.Unregister(repoId);
            }
         }
         else
         {
            console.Write(false);
            console.WriteLine(ConsoleColor.Red, "cannot add empty repository");
         }
      }

      private void UpdateCaps()
      {
         string tag = GetParameter("tag:", 1);
         if (string.IsNullOrEmpty(tag)) throw new ApplicationException("repository tag required");

         Repo r = LocalConfiguration.RepositoryManager.GetRepositoryByTag(tag);
         if (r == null) throw new ApplicationException("repository not found");

         string enabled = GetParameter("enabled:");
         int hours = GetIntParameter("refresh:");
         string publish = GetParameter("publish:");

         bool isEnabled;
         if (bool.TryParse(enabled, out isEnabled)) r.IsEnabled = isEnabled;
         if (hours > 0) r.RefreshIntervalInHours = hours;
         bool useForPublishing;
         if (bool.TryParse(publish, out useForPublishing)) r.UseForPublishing = useForPublishing;

         console.Write("updating repository...");
         LocalConfiguration.RepositoryManager.Update(r);
         console.Write(true);
      }

      private void Update()
      {
      }
   }
}
