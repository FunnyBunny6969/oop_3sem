using System;

public static class Dop2
{
    public abstract class Character
    {
        public string Name { get; set; }
        public int Health { get; protected set; }
        public int Damage { get; protected set; }
        public int Defense { get; protected set; }
        public int Level { get; protected set; } = 1;
        public int Experience { get; protected set; }

        protected Character(string name, int health, int damage, int defense)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Defense = defense;
        }

        public abstract void Attack(Character target);
        public abstract void TakeDamage(int damage);

        public void GainExperience(int amount)
        {
            Experience += amount;
            while (Experience >= Level * 100)
            {
                Experience -= Level * 100;
                Level++;
                Health += 10;
                Damage += 2;
                Defense += 1;
                Console.WriteLine($"{Name} повысил уровень до {Level}!");
            }
        }

        public override string ToString() =>
            $"{Name}: HP={Health}, урон={Damage}, защита={Defense}, уровень={Level}";
    }

    public class Warrior : Character
    {
        public Warrior(string name) : base(name, 150, 25, 12) { }

        public override void Attack(Character target)
        {
            int damage = Math.Max(1, Damage - target.Defense);
            Console.WriteLine($"{Name} наносит удар мечом по {target.Name} на {damage} урона.");
            target.TakeDamage(damage);
        }

        public override void TakeDamage(int damage)
        {
            int reduced = Math.Max(1, damage - Defense / 2);
            Health = Math.Max(0, Health - reduced);
            Console.WriteLine($"{Name} получает {reduced} урона. HP: {Health}");
            if (Health == 0) Console.WriteLine($"{Name} повержен.");
        }
    }

    public class Mage : Character
    {
        public Mage(string name) : base(name, 100, 35, 5) { }

        public override void Attack(Character target)
        {
            int damage = Math.Max(1, Damage - target.Defense / 2);
            Console.WriteLine($"{Name} запускает огненный шар в {target.Name} на {damage} урона.");
            target.TakeDamage(damage);
        }

        public override void TakeDamage(int damage)
        {
            Health = Math.Max(0, Health - damage);
            Console.WriteLine($"{Name} получает {damage} урона. HP: {Health}");
            if (Health == 0) Console.WriteLine($"{Name} повержен.");
        }

        public void Heal()
        {
            Health = Math.Min(150, Health + 20);
            Console.WriteLine($"{Name} лечится. HP: {Health}");
        }
    }

    public class Archer : Character
    {
        public Archer(string name) : base(name, 120, 30, 8) { }

        public override void Attack(Character target)
        {
            bool critical = new Random().Next(1, 101) <= 25;
            int damage = Math.Max(1, Damage - target.Defense);
            if (critical) damage *= 2;
            Console.WriteLine($"{Name} стреляет в {target.Name}{(critical ? " (критический удар)" : "")}: {damage} урона.");
            target.TakeDamage(damage);
        }

        public override void TakeDamage(int damage)
        {
            int reduced = Math.Max(1, damage - Defense);
            Health = Math.Max(0, Health - reduced);
            Console.WriteLine($"{Name} получает {reduced} урона. HP: {Health}");
            if (Health == 0) Console.WriteLine($"{Name} повержен.");
        }
    }

    public static void Run()
    {
        Character warrior = new Warrior("Воин");
        Character mage = new Mage("Маг");
        Character archer = new Archer("Лучник");

        Character[] characters = { warrior, mage, archer };

        foreach (Character character in characters)
        {
            Console.WriteLine(character);
        }

        Console.WriteLine();
        warrior.Attack(mage);
        mage.Attack(archer);
        archer.Attack(warrior);

        Console.WriteLine();
        warrior.GainExperience(250);
        mage.GainExperience(120);

        Console.WriteLine();
        Console.WriteLine(warrior);
        Console.WriteLine(mage);
        Console.WriteLine(archer);
    }
}
