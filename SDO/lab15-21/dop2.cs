using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class Dop2
{
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    private class PrioritizedTask
    {
        public string Name { get; init; } = string.Empty;
        public TaskPriority Priority { get; init; }
        public Func<Task> Work { get; init; } = () => Task.CompletedTask;
    }

    public class AsyncTaskProcessor
    {
        private readonly List<PrioritizedTask> _tasks = new List<PrioritizedTask>();

        public void Add(string name, TaskPriority priority, Func<Task> work)
        {
            _tasks.Add(new PrioritizedTask { Name = name, Priority = priority, Work = work });
        }

        public async Task RunAsync()
        {
            foreach (PrioritizedTask task in _tasks.OrderByDescending(t => t.Priority))
            {
                Console.WriteLine($"  Старт задачи \"{task.Name}\" (приоритет {task.Priority})");
                await task.Work();
                Console.WriteLine($"  Завершена задача \"{task.Name}\"");
            }
        }
    }

    private static async Task SimulateAsync(string name, int delay)
    {
        await Task.Delay(delay);
        Console.WriteLine($"    {name}: работа выполнена");
    }

    public static async Task RunAsync()
    {
        AsyncTaskProcessor processor = new AsyncTaskProcessor();
        processor.Add("Низкий приоритет 1", TaskPriority.Low, () => SimulateAsync("Низкий 1", 200));
        processor.Add("Высокий приоритет 1", TaskPriority.High, () => SimulateAsync("Высокий 1", 100));
        processor.Add("Средний приоритет 1", TaskPriority.Medium, () => SimulateAsync("Средний 1", 150));
        processor.Add("Высокий приоритет 2", TaskPriority.High, () => SimulateAsync("Высокий 2", 100));
        processor.Add("Низкий приоритет 2", TaskPriority.Low, () => SimulateAsync("Низкий 2", 200));

        await processor.RunAsync();
    }
}
