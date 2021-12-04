using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace _2021aoc4
{
    class Program
    {
        class Tábla
        {
            private int[,] tartalom;
            private bool[,] állapot;
            private int[] sorállapot;
            private int[] oszlopállapot;
            static private IEnumerable<int> jelölések;
            private static List<Tábla> lista = new List<Tábla>();

            public Tábla(int[,] tartalom)
            {
                this.tartalom = tartalom;
                this.sorállapot = new int[5];
                this.oszlopállapot = new int[5];
                this.állapot = new bool[5, 5];
            }

            private bool Nyert() => sorállapot.Contains(5) || oszlopállapot.Contains(5);

            public static void Input_feldolgozása(string path)
            {
                using (StreamReader f = new StreamReader(path, Encoding.Default))
                {
                    jelölések = f.ReadLine().Split(',').Select(int.Parse);
                    while (!f.EndOfStream)
                    {
                        Tábla.lista.Add(Tábla.Beolvasása(f));
                    }
                }
            }

            public static (Tábla, int) Játék_nyertese()
            {
                foreach (int jelölés in jelölések)
                {
                    foreach (Tábla tábla in Tábla.lista)
                    {
                        tábla.Bejelöl(jelölés);
                        if (tábla.Nyert())
                        {
                            Console.WriteLine($"Ez nyert, kiszáll: "); tábla.Kiir();
                            return (tábla, jelölés);
                        }
                    }
                }
                return (null, -1);
            }

            public static (Tábla, int) Játék_vesztese()
            {
                List<Tábla> nyertesek = new List<Tábla>();
                foreach (int jelölés in jelölések)
                {
                    foreach (Tábla tábla in Tábla.lista)
                    {
                        if (nyertesek.Contains(tábla))
                            continue;

                        tábla.Bejelöl(jelölés);
                        if (tábla.Nyert())
                        {
                            Console.WriteLine($"Ez nyert, kiszáll: "); tábla.Kiir();
                            nyertesek.Add(tábla);
                            if (nyertesek.Count == Tábla.lista.Count)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"Ez a vesztes: "); 
                                Console.ForegroundColor = ConsoleColor.White;
                                tábla.Kiir();
                                return (tábla, jelölés);
                            }
                        }
                    }
                }
                return (null, -1);
            }

            private void Bejelöl(int jelölés)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (tartalom[i,j]==jelölés)
                        {
                            állapot[i, j] = true;
                            sorállapot[j]++;
                            oszlopállapot[i]++;
                        }
                    }
                }
            }

            public void Kiir()
            {
                string f = "{0,3} ";
                Console.WriteLine($"A(z) {1+Tábla.lista.FindIndex(x=> x==this)}. tábla:");
                for (int i = 0; i < 5; i++)
                {
                    for (int j = 0; j < 5; j++)
                    {
                        if (állapot[i,j])
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(string.Format(f, tartalom[i, j]));
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.Write(string.Format(f, tartalom[i, j]));
                        }
                    }
                    Console.WriteLine();
                }
            }
            private static Tábla Beolvasása(StreamReader f)
            {
                int[,] tartalom = new int[5, 5];
                f.ReadLine();
                for (int i = 0; i < 5; i++)
                {
                    string[] sortömb = f.ReadLine().Trim().Replace("  "," ").Split(' ');
                    for (int j = 0; j < 5; j++)
                    {
                        tartalom[i, j] = int.Parse(sortömb[j]);
                    }
                }
                return new Tábla(tartalom);
            }

            public int Pontszáma(int utolsó_jelölés)
            {
                int sum = 0;
                for (int i = 0; i < 5; i++)
                    for (int j = 0; j < 5; j++)
                        if (!állapot[i, j])
                            sum += tartalom[i, j];
                return sum*utolsó_jelölés;
            }
        }
        static void Main(string[] args)
        {
            /** /
            Tábla.Input_feldolgozása("teszt.txt");
            /*/
            Tábla.Input_feldolgozása("input.txt");
            /**/

            /** /
            (Tábla nyertes, int utolsó_jelölés) = Tábla.Játék_nyertese();
            Console.WriteLine($"A nyertes pontszáma: {nyertes.Pontszáma(utolsó_jelölés)}");
            /*/
            (Tábla vesztes, int utolsó_jelölés) = Tábla.Játék_vesztese();            
            Console.WriteLine($"A vesztes pontszáma: {vesztes.Pontszáma(utolsó_jelölés)}");
            /**/

            Console.ReadKey();
        }
    }
}
