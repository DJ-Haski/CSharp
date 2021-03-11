using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Xml.Serialization;
namespace ConsoleSnake
{

    public abstract class GameObject
    {
        public char symb;
        public List<Point> part;
        public ConsoleColor color;

        public GameObject(char symb, ConsoleColor color)
        {
            this.symb = symb;
            this.color = color;
            this.part = new List<Point>();
        }
        public void draw()
        {
            Console.ForegroundColor = color;
            for (int i = 0; i < part.Count; i++)
            {
                Console.SetCursorPosition(part[i].X, part[i].Y);
                Console.Write(symb);
            }
        }
        public void clear()
        {
            for (int i = 0; i < part.Count; i++)
            {
                Console.SetCursorPosition(part[i].X, part[i].Y);
                Console.Write(' ');
            }
        }
    }

    //Координаты

    public class Point
    {
        int x;
        int y;
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                if (value < 0)
                {
                    x = Program.Width - 1;
                }
                else if (value > Program.Width + 1)
                {
                    x = 0;
                }
                else
                {
                    x = value;
                }
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                if (value < 0)
                {
                    y = Program.Height - 1;
                }
                else if (value > Program.Height + 1)
                {
                    y = 0;
                }
                else
                {
                    y = value;
                }
            }
        }
    }
    //Змея
    class Snake : GameObject
    {
        public int dx = 0;
        public int dy = 0;
        public Snake(int snakeheadX, int snakeheadY, char symb, ConsoleColor color) : base(symb, color)
        {
            Point snakehead = new Point { X = snakeheadX, Y = snakeheadY };
            part.Add(snakehead);
            draw();
        }
        //Движение
        public void move(apple apple)
        {
            for (int i = part.Count - 1; i > 0; --i)
            {
                part[i].X = part[i - 1].X;
                part[i].Y = part[i - 1].Y;
            }
            part[0].X += dx;
            part[0].Y += dy;
            apple.isCollision(part[0]);
        }
        public void changeDirection(int dx, int dy)
        {
            this.dx = dx;
            this.dy = dy;
        }
        public void growth(Point point)
        {
            part.Add(new Point { X = point.X, Y = point.Y });
        }
    }
    class apple : GameObject
    {
        Random rnd = new Random();
        public apple(int x, int y, char symb, ConsoleColor color) : base(symb, color)
        {
            Point position = new Point { X = x, Y = y };

            part.Add(position);
            draw();
        }
        public bool isCollision(Point snakehead)
        {
            return snakehead.X == part[0].X && snakehead.Y == part[0].Y;
        }
        //Генерация в свободном месте
        public void generate(Snake snake, Wall wall)
        {
            while (true)
            {
                bool ok = true;
                int randomX = rnd.Next(2, Program.Width - 1);
                int randomY = rnd.Next(2, Program.Height - 1);

                for (int i = 0; i < snake.part.Count; i++)
                {
                    if (snake.part[i].X == randomX && snake.part[i].Y == randomY)
                    {
                        ok = false;
                    }
                }
                for (int i = 0; i < wall.part.Count; i++)
                {
                    if (wall.part[i].X == randomX && wall.part[i].Y == randomY)
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    this.part.Add(new Point { X = randomX, Y = randomY });
                    break;
                }
            }
        }

        //Отрисовка
        public void draw()
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(part[part.Count - 1].X, part[part.Count - 1].Y);
            Console.Write(symb);
        }
    }

    // Стена
    class Wall : GameObject
    {
        public Wall(char symb, ConsoleColor color, string path) : base(symb, color)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(fs))
                {
                    int rowNubmer = 0;
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        for (int columnNumber = 0; columnNumber < line.Length; columnNumber++)
                        {
                            if (line[columnNumber] == '#')
                            {
                                part.Add(new Point { X = columnNumber, Y = rowNubmer });
                            }
                        }
                        rowNubmer++;
                    }
                }
            }
            draw();
        }
    }
    public class Program
    {

        //Настройки
        public static int spd = 100;
        public static int num = 0;
        public static TimerCallback tm = new TimerCallback(moving);
        public static Timer snakeTimer = new Timer(tm, num, 0, spd);
        public static TimerCallback tm2 = new TimerCallback(spawnNewapple);
        public static Timer spawnNewappleTimer = new Timer(tm2, num, 2000, 2000);
        public static bool running = true;
        public static Random rnd = new Random();

        //Размер поля
        public static int Width = 40;
        public static int Height = 40;

        //Символы
        static Snake snake = new Snake(5, 4, 'O', ConsoleColor.Green);
        static apple apple = new apple(5, 5, '♥', ConsoleColor.Red);
        static Wall wall = new Wall('▀', ConsoleColor.White, @"Lvl1.txt");
        //static int level = 1;
        static public bool nextLevel = true;




        // Движение и чекеры
        static void moving(object obj)
        {
            snake.clear();
            snake.move(apple);
            snake.draw();
            for (int i = 0; i < apple.part.Count; i++)
            {
                if (snake.part[0].X == apple.part[i].X && snake.part[0].Y == apple.part[i].Y)
                {
                    snake.growth(snake.part[0]);
                }
            }
            for (int i = 0; i < wall.part.Count; i++)
            {
                if (snake.part[0].X == wall.part[i].X && snake.part[0].Y == wall.part[i].Y)
                {

                    Console.Clear();
                    Console.WriteLine("Game Over!");
                    Program.running = false;
                    Console.Write("Press Esc to leave");
                    snakeTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    spawnNewappleTimer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }

            if (snake.part.Count > 3 && Program.nextLevel)
            {
                Program.nextLevel = false;
                Console.Clear();
                wall = new Wall('▀', ConsoleColor.White, @"Lvl2.txt");
                for (int i = 1; i < snake.part.Count; i++)
                {
                    snake.part.RemoveAt(i);
                }

            }
            Console.SetCursorPosition(Program.Width + 5, 0);
            Console.Write("Score: " + snake.part.Count + " " + DateTime.Now /*- starttime*/);
        }
        //Еда
        static void spawnNewapple(object obj)
        {

            apple.generate(snake, wall);
            apple.draw();
        }
        //Мейн

        public static void Main(string[] args)
        {
            Console.SetWindowSize(Console.WindowWidth, 80);
            Console.SetWindowSize(Console.WindowHeight, 50);
            Console.WriteLine("______________###___________###____________________");
            Console.WriteLine("____________##.O.##_______##.O.##__________________");
            Console.WriteLine("__________########################_________________");
            Console.WriteLine("______.##########.o.###.o.##########_______________");
            Console.WriteLine("_____################################______________");
            Console.WriteLine("________#####”__V____V__”#####_____________________");
            Console.WriteLine("___________#######____#######______________________");
            Console.WriteLine("__________________######___________________________");
            Console.WriteLine("____________________####___________________________");
            Console.WriteLine("____________________####___________####____________");
            Console.WriteLine("____________________####_________#######___________");
            Console.WriteLine("____________________####________###___###__________");
            Console.WriteLine("______|_____________####_______###_____###_________");
            Console.WriteLine("____##|##___________.####______###______###________");
            Console.WriteLine("___#######___________.####____####______.###_______");
            Console.WriteLine("___#######_____________.####__####________.###___.#_");
            Console.WriteLine("____#####______________.########__________###...##_");
            Console.WriteLine("_____###_______________######_____________####___");
            Console.WriteLine("Welcome to Snake Game by DJHaski!");
            Console.WriteLine("WASD to Move");
            Console.WriteLine("P to pause");
            Console.WriteLine("S to save(wip)");
            Console.WriteLine("L to load(wip)");
            Console.WriteLine("Wall will damage you");
            Console.WriteLine("Apples is good");
            Console.WriteLine("Eat as much as possible!");
            Console.WriteLine("To Start Press Any Key");
            Console.ReadLine();
            Console.Clear();
            MainF();
        }
        public static void MainF()
        {
            //int starttime = Convert.ToInt32(DateTime.Now) ;
            Console.SetWindowSize(Console.WindowWidth, 80);
            Console.SetWindowSize(Console.WindowHeight, 50);
            bool pause = false;
            Random rnd = new Random();
            Console.CursorVisible = false;
            Console.SetCursorPosition(20, 20);
            while (running)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey();
                switch (pressedKey.Key)
                {
                    case ConsoleKey.W:
                        snake.changeDirection(0, -1);
                        break;
                    case ConsoleKey.S:
                        snake.changeDirection(0, 1);
                        break;
                    case ConsoleKey.A:
                        snake.changeDirection(-1, 0);
                        break;
                    case ConsoleKey.D:
                        snake.changeDirection(1, 0);
                        break;
                    case ConsoleKey.P:
                        pause = !pause;
                        if (!pause)
                        {
                            snakeTimer.Change(0, spd);
                        }
                        else
                        {
                            snakeTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        }
                        break;
                    case ConsoleKey.Escape:
                        running = false;
                        break;
                }
            }
        }
    }
}