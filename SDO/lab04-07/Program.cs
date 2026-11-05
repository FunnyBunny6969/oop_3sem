using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Лабораторная 07: интерфейсы ===");
        Console.WriteLine("--- Основное задание (вариант 3) ---");
        MainTask.Run();

        Section("Дополнительное задание 1: система плагинов");
        Dop1.Run();

        Section("Дополнительное задание 2: несколько интерфейсов");
        Dop2.Run();

        Section("Дополнительное задание 3: IComparable и IComparer");
        Dop3.Run();
    }

    static void Section(string title)
    {
        Console.WriteLine();
        Console.WriteLine(new string('=', 80));
        Console.WriteLine("=== " + title + " ===");
        Console.WriteLine(new string('=', 80));
    }
}

public static class MainTask
{
    public interface IPlayable
    {
        void Play();
        void Pause();
    }

    public class MusicPlayer : IPlayable
    {
        public string CurrentTrack { get; set; } = "Без названия";
        private bool _isPlaying;

        void IPlayable.Play()
        {
            _isPlaying = true;
            Console.WriteLine($"MusicPlayer: воспроизводится трек \"{CurrentTrack}\".");
        }

        void IPlayable.Pause()
        {
            _isPlaying = false;
            Console.WriteLine("MusicPlayer: пауза.");
        }

        public void Stop()
        {
            _isPlaying = false;
            Console.WriteLine("MusicPlayer: остановлен.");
        }
    }

    public class VideoPlayer : IPlayable
    {
        public string CurrentVideo { get; set; } = "Без названия";
        private bool _isPlaying;

        void IPlayable.Play()
        {
            _isPlaying = true;
            Console.WriteLine($"VideoPlayer: воспроизводится видео \"{CurrentVideo}\".");
        }

        void IPlayable.Pause()
        {
            _isPlaying = false;
            Console.WriteLine("VideoPlayer: пауза.");
        }

        public void Stop()
        {
            _isPlaying = false;
            Console.WriteLine("VideoPlayer: остановлен.");
        }
    }

    public static void Run()
    {
        IPlayable music = new MusicPlayer { CurrentTrack = "Imagine" };
        IPlayable video = new VideoPlayer { CurrentVideo = "Урок C#" };

        music.Play();
        music.Pause();
        video.Play();
        video.Pause();

        Console.WriteLine("Метод Stop() не входит в интерфейс IPlayable и недоступен через него.");

        MusicPlayer musicPlayer = (MusicPlayer)music;
        musicPlayer.Stop();
    }
}
