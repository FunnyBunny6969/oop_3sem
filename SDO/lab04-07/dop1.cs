using System;
using System.Collections.Generic;
using System.Linq;

public static class Dop1
{
    public interface IPlugin
    {
        string Name { get; }
        IEnumerable<string> Dependencies { get; }
        void Initialize();
        void Execute();
        void Shutdown();
    }

    public class LoggerPlugin : IPlugin
    {
        public string Name => "Logger";
        public IEnumerable<string> Dependencies => Array.Empty<string>();
        public void Initialize() => Console.WriteLine("LoggerPlugin: инициализация.");
        public void Execute() => Console.WriteLine("LoggerPlugin: запись сообщения в журнал.");
        public void Shutdown() => Console.WriteLine("LoggerPlugin: завершение работы.");
    }

    public class TimerPlugin : IPlugin
    {
        public string Name => "Timer";
        public IEnumerable<string> Dependencies => new[] { "Logger" };
        public void Initialize() => Console.WriteLine("TimerPlugin: инициализация.");
        public void Execute() => Console.WriteLine("TimerPlugin: замер времени выполнения.");
        public void Shutdown() => Console.WriteLine("TimerPlugin: завершение работы.");
    }

    public class ValidatorPlugin : IPlugin
    {
        public string Name => "Validator";
        public IEnumerable<string> Dependencies => new[] { "Logger", "Timer" };
        public void Initialize() => Console.WriteLine("ValidatorPlugin: инициализация.");
        public void Execute() => Console.WriteLine("ValidatorPlugin: проверка входных данных.");
        public void Shutdown() => Console.WriteLine("ValidatorPlugin: завершение работы.");
    }

    public class PluginManager
    {
        private readonly List<IPlugin> _plugins = new List<IPlugin>();

        public void Register(IPlugin plugin) => _plugins.Add(plugin);

        private IEnumerable<IPlugin> OrderByDependencies()
        {
            List<IPlugin> ordered = new List<IPlugin>();
            HashSet<string> loaded = new HashSet<string>();

            bool added;
            do
            {
                added = false;
                foreach (IPlugin plugin in _plugins)
                {
                    if (ordered.Contains(plugin)) continue;
                    if (plugin.Dependencies.All(loaded.Contains))
                    {
                        ordered.Add(plugin);
                        loaded.Add(plugin.Name);
                        added = true;
                    }
                }
            } while (added);

            if (ordered.Count != _plugins.Count)
                throw new InvalidOperationException("Обнаружена циклическая зависимость плагинов.");

            return ordered;
        }

        public void RunAll()
        {
            foreach (IPlugin plugin in OrderByDependencies())
            {
                Console.WriteLine($"--- Плагин {plugin.Name} ---");
                plugin.Initialize();
                plugin.Execute();
                plugin.Shutdown();
            }
        }
    }

    public static void Run()
    {
        PluginManager manager = new PluginManager();
        manager.Register(new LoggerPlugin());
        manager.Register(new TimerPlugin());
        manager.Register(new ValidatorPlugin());
        manager.RunAll();
    }
}
