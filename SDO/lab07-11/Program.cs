class Program
{
    static void Main()
    {
        ProgramTask1.Run();
        Console.WriteLine("\n" + new string('-', 50) + "\n");
        ProgramTask2.Run();

        Console.WriteLine("\n" + new string('=', 50) + "\n");
        Console.WriteLine("        БЛОК ЗАДАНИЙ ПОВЫШЕННОЙ СЛОЖНОСТИ");
        Console.WriteLine("\n" + new string('=', 50) + "\n");

        // Новые допы без всяких new
        Task3_Calc.Run();
        Console.WriteLine("\n" + new string('-', 50) + "\n");
        Task4_Game.Run();
        Console.WriteLine("\n" + new string('-', 50) + "\n");
        Task5_Chain.Run();
    }
}
