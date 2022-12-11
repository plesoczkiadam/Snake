using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;

namespace snake
{
    class Program
    {

        struct TSnake
        {
            public char elem;
            public int xpoz;
            public int ypoz;
            public ConsoleColor color;
        }
        static void Main(string[] args)
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 10;
            aTimer.Enabled = true;
            aTimer.Start();

            char fej = '@'; // Kígyó feje karakter
            char test = 'm';// Kígyó teste karakter
            int irany = 0; //0 = fel; 1 = lobb 2 = le; 3 = bal
            int xmax = Console.WindowWidth - 1;
            int ymax = Console.WindowHeight - 2;
           
            TSnake tmps = new TSnake();
            TSnake[] snake = new TSnake[150];
            int hossz = initSnake(snake);
            tmps = snake[0];
            TSnake fruit = new TSnake();
            //int lastXPoz = 0;
            //int lastYPoz = 0;
            Console.SetCursorPosition(fruit.xpoz, fruit.ypoz);
            Console.ForegroundColor = fruit.color;
            
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            Console.CursorVisible = false;
            bool FruitOk = false;
            // Fő ciklus
            do
              {
                
                if (!FruitOk)
                {
                    initFruit(ref fruit, snake, hossz);
                    FruitOk = true;
                   
                }
                else
                {
                    if (snake[0].xpoz == fruit.xpoz && snake[0].ypoz == fruit.ypoz)
                        {
                        if (fruit.color == ConsoleColor.Red)
                        {
                            hossz++;
                          
                        }
                        else if (fruit.color == ConsoleColor.DarkYellow)
                        {
                            hossz--;
                            
                        }
                        else
                            try {
                                throw new Exception();
                            }
                            catch (Exception) { }

                        FruitOk = false;
                            }                   
                }
                fruitWrite(fruit);

                if (Console.KeyAvailable)
                {
                    cki = Console.ReadKey(true);
                    irany = keychar(snake, cki,  irany);
                }
                //Történt billentyű lenyomás, irányt váltunk
                switch (irany)
                {
                    case 0:
                        tmps.ypoz--;
                        break;
                    case 1:
                        tmps.xpoz++;
                        break;
                    case 2:
                        tmps.ypoz++;
                        break;
                    case 3:
                        tmps.xpoz--;
                        break;
                }
                countSnake(snake, ref tmps, hossz);
                //kiszámoljuk a változásokat
                
                //A kígyó kirajzolása
                if (!SnakeWrite(snake, ref hossz, ref tmps))
                    break;

                //Console.SetCursorPosition(lastXPoz, lastYPoz);
                // Console.Write(' ');
                
                //Thread.Sleep(150);
                Console.Clear();
            } while (cki.Key != ConsoleKey.Escape);

        }

        private static void fruitWrite(TSnake fruit)
        {
            Console.SetCursorPosition(fruit.xpoz, fruit.ypoz);
            Console.ForegroundColor = fruit.color;
            Console.Write(fruit.elem);
        }

        private static void countSnake(TSnake[] snake, ref TSnake tmps, int hossz)
        {
            for (int i = hossz - 1; i > 0; i--)
            {
                snake[i].xpoz = snake[i - 1].xpoz;
                snake[i].ypoz = snake[i - 1].ypoz;
                if (i % 2 == 0)
                    snake[i].color = ConsoleColor.Yellow;
                else snake[i].color = ConsoleColor.Green;
                snake[i].elem = 'm';
            }
            snake[0] = tmps;
        }

        private static bool SnakeWrite(TSnake[] snake, ref int hossz, ref TSnake tmps)
        {
            ConsoleKeyInfo cki = new ConsoleKeyInfo();
            int xmax = Console.WindowWidth - 1;
            int ymax = Console.WindowHeight - 2;
            for (int i = hossz - 1; i >= 0; i--)
            {
                try
                {
                    //megvizsgáljuk a kígyó fej pozicióját
                    Console.SetCursorPosition(snake[i].xpoz, snake[i].ypoz);
                    if (snake[i].ypoz == Console.WindowHeight - 1)
                        throw new Exception  ();
                }
                catch (Exception)
                {
                    //Ha területen kívül kerül befejezzük a jáccmát
                    string t1 = "meddöglött a kígyo! Még egy játszma?";
                    string t2 = "<< bármely ==  igen; esc == nem";
                    Console.SetCursorPosition((xmax / 2) -
                                                (t1.Length / 2), ymax / 2);
                    Console.WriteLine(t1);
                    Console.SetCursorPosition((xmax / 2) -
                                                (t2.Length / 2), ymax / 2 + 1);
                    Console.WriteLine(t2);
                    cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Escape) break;
                    // Mi van, ha nem Esc? 
                    else
                    {
                        hossz = initSnake(snake);
                        tmps = snake[0];
                        return false;
                    }
                }
                int j = 0;
               // snake[j].elem = '\0';
                while (snake[j].elem != '\0' )
                {
                    // ha nem, kirajzoljuk az adott elemet
                    Console.SetCursorPosition(snake[j].xpoz, snake[j].ypoz);
                    Console.ForegroundColor = snake[j].color;
                    Console.Write(snake[j].elem);
                    j++;

                }

            }
            return true;
        }

        
        private static int keychar(TSnake[] snake, ConsoleKeyInfo cki, int irany )
        { 
            int xmax = Console.WindowWidth - 1;
            int ymax = Console.WindowHeight - 2;
            switch (cki.Key)
            {
                case ConsoleKey.UpArrow: // fel nyil
                    if (snake[0].ypoz > 0 && irany != 2)
                        irany = 0;
                    break;
                case ConsoleKey.DownArrow: //le nyil
                    if (snake[0].ypoz < ymax && irany != 0)
                        irany = 2;
                    //snake[0].ypoz++;
                    break;
                case ConsoleKey.LeftArrow: // bal nyil
                    if (snake[0].xpoz > 0 && irany != 1)
                        irany = 3;
                    //snake[0].xpoz--;
                    break;
                case ConsoleKey.RightArrow: // jobb nyil
                    if (snake[0].xpoz < xmax && irany != 3)
                        irany = 1;
                    //snake[0].xpoz++;
                    break;
                    // }
            }
            return irany;
        }

        private static int initSnake(TSnake[] snake)
        {
            Console.Clear();
            char fej = '@'; // Kígyó feje karakter
            char test = 'm';// Kígyó teste karakter
            int xmax = Console.WindowWidth - 1;
            int ymax = Console.WindowHeight - 2;
            /** Inicializáljuk a kígyót */
            snake[0].elem = fej;
            snake[0].color = ConsoleColor.Red;
            snake[0].xpoz = xmax / 2;
            snake[0].ypoz = ymax / 2;



            snake[1].elem = test;
            snake[1].color = ConsoleColor.Yellow;
            snake[1].xpoz = xmax / 2;
            snake[1].ypoz = ymax / 2 + 1;

            snake[2].elem = test;
            snake[2].color = ConsoleColor.Green;
            snake[2].xpoz = xmax / 2;
            snake[2].ypoz = ymax / 2 + 2;

            snake[3].elem = test;
            snake[3].color = ConsoleColor.Yellow;
            snake[3].xpoz = xmax / 2;
            snake[3].ypoz = ymax / 2 + 3;

            snake[4].elem = test;
            snake[4].color = ConsoleColor.Green;
            snake[4].xpoz = xmax / 2;
            snake[4].ypoz = ymax / 2 + 4;
            return 5;
        }

        private static void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            ;
        }

        private static void initFruit(ref TSnake fruit, TSnake[] snake, int hossz)
        { // 8 lila = döglés, 6 = rövidülés Ó növekedés
            {
                int xmax = Console.WindowWidth - 1;
                int ymax = Console.WindowHeight - 2;
                bool van = false;
                Random rnd = new Random();
                do
                {
                    int i = 0;
                    fruit.xpoz = rnd.Next(xmax);
                    fruit.ypoz = rnd.Next(ymax);
                    while (i < hossz - 1 && !van)
                    {
                        if (snake[i].xpoz == fruit.xpoz && snake[i].ypoz == fruit.xpoz)
                            van = true;
                        i++;
                    }
                } while (van);
                int szin = rnd.Next(6);
                switch (szin)
                {
                    case 1:
                        fruit.elem = '8';
                        fruit.color = ConsoleColor.DarkMagenta;
                        break;
                    case 2:
                        fruit.elem = '6';
                        fruit.color = ConsoleColor.DarkYellow;
                        break;
                    default:
                            fruit.elem = 'Ó';
                            fruit.color = ConsoleColor.Red;                        
                        break;
                }
                Console.SetCursorPosition(fruit.xpoz, fruit.ypoz);
                Console.ForegroundColor = fruit.color;
                Console.Write(fruit.elem);
            }
        }
    }
}
