using System;

// 1. Пакет данных для события угадывания
public class GameEventArgs : EventArgs
{
    public int Attempts { get; set; }
    public int SecretNumber { get; set; }
    public GameEventArgs(int num, int att) { SecretNumber = num; Attempts = att; }
}

public class GuessGame
{
    // События игры
    public event Action? GameStarted;
    public event EventHandler<GameEventArgs>? NumberGuessed;
    public event Action? GameOver;

    public void Start(int secret)
    {
        GameStarted?.Invoke();
        
        Random rand = new Random();
        int attempts = 0;
        int guess = -1;

        // Компьютер пытается угадать случайным образом
        while (guess != secret)
        {
            attempts++;
            guess = rand.Next(1, 100); // Диапазон от 1 до 100
        }

        // Число угадано! Генерируем событие
        NumberGuessed?.Invoke(this, new GameEventArgs(secret, attempts));
        GameOver?.Invoke();
    }
}

// Классы-подписчики
public class AnalysistLog
{
    public void OnStart() => Console.WriteLine("  [Логгер]: Игра началась. ИИ подбирает алгоритмы...");
    public void OnGuess(object? sender, GameEventArgs e) => 
        Console.WriteLine($"  [Логгер]: Число {e.SecretNumber} успешно взломано за {e.Attempts} попыток.");
    public void OnEnd() => Console.WriteLine("  [Логгер]: Игра завершена, ресурсы очищены.");
}

public class Task4_Game
{
    public static void Run()
    {
        Console.WriteLine("=== ДОП 2. Игра 'Угадай число' на событиях ===\n");

        GuessGame game = new GuessGame();
        AnalysistLog logger = new AnalysistLog();

        // Подписываем логику на события игры
        game.GameStarted += logger.OnStart;
        game.NumberGuessed += logger.OnGuess;
        game.GameOver += logger.OnEnd;

        // Загадываем число 42 и запускаем игру
        Console.WriteLine("Загадываем число 42...");
        game.Start(42);
    }
}
