using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TridionCommunity.NotificationFramework;

namespace NotificationService
{
    public class Notifiers
    {
        private static bool _loaded;
        private static readonly List<INotifier> _notifiers = new List<INotifier>();

        protected static void Load()
        {
            var path = GetNotifiersFolder();
            
            if (!Directory.Exists(path))
            {
                throw new NotificationFailedException(string.Format(Resources.FailedToLoadNotifiers, path));
            }

            var notifierType = typeof(INotifier);
            var assemblies = Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFile);
            var types = assemblies.SelectMany(s => s.GetTypes()).Where(notifierType.IsAssignableFrom);

            foreach(var type in types)
            {
                _notifiers.Add((INotifier)Activator.CreateInstance(type));
            }
            
            _loaded = true;
        }

        private static string GetNotifiersFolder()
        {
            return ConfigurationManager.AppSettings["notifiersFolder"] ??
                     Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Notifiers");
        }

        public static List<INotifier> GetAll(bool force)
        {
            _loaded = false;
            return GetAll();
        }

        public static List<INotifier> GetAll()
        {
            if (!_loaded) Load();
            return _notifiers;
        }
    }
}
