using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dijkstra
{
    internal class Program
    {

        class Kupac<T>
        {
            class Lista<R>
            {
                List<R> list = new List<R>();

                public void Add(R elem) => list.Add(elem);
                public void RemoveLast() => list.RemoveAt(list.Count - 1);
                public R this[int i]
                {
                    get => list[i - 1];
                    set => list[i - 1] = value;
                }
                public override string ToString()
                {
                    string sum = "[ ";
                    foreach (R item in list)
                    {
                        sum += item + " ";
                    }
                    return sum + "]";
                }
                public int Count { get => list.Count; }
            }

            Lista<T> lista = new Lista<T>();
            Func<T, T, int> relacio;

            public Kupac(Func<T, T, int> r)
            {
                this.relacio = r;
            }


            private int Szülő(int i) => i / 2;

            private void Csere(int i, int j) => (lista[i], lista[j]) = (lista[j], lista[i]);

            private bool Gyökér(int i) => i == 1;

            private void Bugyborékolj(int gyerek)
            {
                while (!Gyökér(gyerek) && relacio(lista[gyerek], lista[Szülő(gyerek)]) == 1)
                {
                    Csere(gyerek, Szülő(gyerek));
                    gyerek = Szülő(gyerek);
                }
            }

            private int Gyerekek(int szülő, out int gyerek1, out int gyerek2)
            {
                // emlékeztető
                /*
				int b;
				bool lehete = int.TryParse("5", out b);
				*/

                gyerek1 = 0;
                gyerek2 = 0;
                int gyerekszám = 0;
                if (2 * szülő <= lista.Count)
                {
                    gyerek1 = 2 * szülő;
                    gyerekszám = 1;
                    if (2 * szülő + 1 <= lista.Count)
                    {
                        gyerek2 = 2 * szülő + 1;
                        gyerekszám = 2;
                    }
                }
                return gyerekszám;
            }

            private bool Idősebbgyerek(int szülő, out int idősebb_gyerek)
            {
                int gyerek1;
                int gyerek2;
                int gyerekszám = Gyerekek(szülő, out gyerek1, out gyerek2);

                switch (gyerekszám)
                {
                    case 0:
                        idősebb_gyerek = 0;
                        return false;
                    case 1:
                        idősebb_gyerek = gyerek1;
                        return true;
                    case 2:
                        idősebb_gyerek = relacio(lista[gyerek1], lista[gyerek2]) == -1 ? gyerek2 : gyerek1;
                        return true;
                    default: // ide sose fog befutni a program
                        idősebb_gyerek = 0;
                        return true;
                }
            }

            private void Süllyesztés()
            {
                int szülő = 1;
                int idősebb_gyerek;
                bool vanegyerek = Idősebbgyerek(szülő, out idősebb_gyerek);
                while (vanegyerek && relacio(lista[szülő], lista[idősebb_gyerek]) == -1)
                {
                    Csere(szülő, idősebb_gyerek);
                    szülő = idősebb_gyerek;
                    vanegyerek = Idősebbgyerek(szülő, out idősebb_gyerek);
                }
            }

            public void Push(T elem)
            {
                lista.Add(elem);
                Bugyborékolj(lista.Count);
            }

            public T Pop()
            {
                Csere(1, lista.Count);
                T result = lista[lista.Count];
                lista.RemoveLast();
                Süllyesztés();
                return result;
            }
            public int Count => lista.Count;



            public bool Empty() => lista.Count == 0;
            public override string ToString() => lista.ToString();

        }

        class Graf
        {
            int[][] m;
            int N;
            int M;
            public Graf()
            {
                string sor = Console.ReadLine();
                string[] t = sor.Split(' ');

                (N, M) = (int.Parse(t[0]), int.Parse(t[1]));

                m = new int[N + 1][];
                // [0]: 
                // [1]: 
                // [2]: 
                // [3]: 
                // [4]: 

                for (int i = 0; i < N + 1; i++)
                {
                    m[i] = new int[N + 1];
                }

                // [0]: [0, 0, 0, 0, 0]
                // [1]: [0, 0, 0, 0, 0]
                // [2]: [0, 0, 0, 0, 0]
                // [3]: [0, 0, 0, 0, 0]
                // [4]: [0, 0, 0, 0, 0]

                for (int i = 0; i < N + 1; i++)
                {
                    for (int j = 0; j < N + 1; j++)
                    {
                        m[i][j] = -1;

                    }
                }

                // [0]: [-1, -1, -1, -1, -1]
                // [1]: [-1, -1, -1, -1, -1]
                // [2]: [-1, -1, -1, -1, -1]
                // [3]: [-1, -1, -1, -1, -1]
                // [4]: [-1, -1, -1, -1, -1]


                for (int i = 0; i < M; i++)
                {
                    sor = Console.ReadLine();
                    t = sor.Split(' ');
                    (int honnan, int hova) = (int.Parse(t[0]), int.Parse(t[1]));
                    int suly = 0;

               

                    if (t[2] == "F")
                    {
                        suly = 1;
                    }
                    m[honnan][hova] = suly;
                    m[hova][honnan] = suly;
                }
            }

            public void Diagnosztika()
            {
                Console.WriteLine("Így néz ki a szomszédsági mátrix:");
                for (int i = 1; i < N + 1; i++)
                {
                    for (int j = 1; j < N + 1; j++)
                    {
                        if (m[i][j] != -1)
                            Console.Write($" {m[i][j]} ");
                        else
                            Console.Write($"{m[i][j]} ");
                    }
                    Console.WriteLine();

                }
            }

            public List<int> Szomszédai(int n)
            {
                List<int> result = new List<int>();
                for (int i = 0; i < N+1; i++)
                    if (m[n][i] != -1)
                        result.Add(i);

                return result;
            }

            public (int[], int[]) Dijkstra(int start)
            {
                // inicializáltuk a tav-ot
                int[] tav = new int[N+1];
                for (int i = 0; i < N+1; i++)
                    tav[i] = int.MaxValue;

                tav[start] = 0;

                // inicializáltuk a honnan-t
                int[] honnan = new int[N+1];
                for (int i = 0; i < N+1; i++)
                    honnan[i] = -1;


                // Mindent beledobálunk egy kupacba! videóból az unvisited
                Kupac<int> kupac = new Kupac<int>((x, y) => tav[x] > tav[y] ? -1 : (tav[x] == tav[y] ? 0 : 1));
                for (int i = 0; i < N+1; i++)
                    kupac.Push(i);

                // videóból a visited
                bool[] voltamitt = new bool[N+1];


                // kiszedünk egy elemet
                // vesszük a szomszédait
                // apdételjük az eléréseiket
                // kidobjuk az elemet


                while (kupac.Count > 0)
                {
                    int aktCsomopont = kupac.Pop();
                    voltamitt[aktCsomopont] = true;

                    List<int> szomszedok = Szomszédai(aktCsomopont);

                    foreach (int szomszed in szomszedok)
                    {
                        if (!voltamitt[szomszed])
                        {
                            int ujTav = tav[aktCsomopont] + m[aktCsomopont][szomszed];

                            if (ujTav < tav[szomszed])
                            {
                                tav[szomszed] = ujTav;
                                honnan[szomszed] = aktCsomopont;

                                kupac.Push(szomszed);
                            }
                        }
                    }
                }


                return (tav, honnan);
            }

            public List<int> LegrovidebbUt(int start, int cel, int[] honnan)
            {
                List<int> ut = new List<int>();

                int varos = cel;
                    
                ut.Add(cel);

                while (varos != start)
                {
                    ut.Add(honnan[varos]);
                    varos = honnan[varos];

                }
                ut.Reverse();

                return ut;
            }
        }
        static void Main(string[] args)
        {
            Graf g = new Graf();
            string sor = Console.ReadLine();
            string[] t = sor.Split(' ');
            (int Start, int Cel) = (int.Parse(t[0]), int.Parse(t[1]));
            (int[] tav, int[] honnan) = g.Dijkstra(Start);


            Console.WriteLine("-----------------------------------------------------");

            g.Diagnosztika();



            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine("Távolságok a kezdéstől:");
            for (int i = 0; i < tav.Length; i++)
            {
                Console.WriteLine($" {i}: {tav[i]}");
            }

            Console.WriteLine("-----------------------------------------------------");

            Console.WriteLine("Leggyorsabb útvonalak:");
            for (int i = 0; i < honnan.Length; i++)
            {
                Console.WriteLine($"{i} --> {honnan[i]}");
            }

            List<int> legkisebbUt = g.LegrovidebbUt(Start, Cel, honnan);
            Console.WriteLine("-----------------------------------------------------");
            Console.WriteLine(tav[Cel]);
            Console.WriteLine(string.Join(" ", legkisebbUt));

            Console.WriteLine("-----------------------------------------------------");

        }
    }
}
/*
7 11
1 2 F
1 5 I
2 5 I
2 3 I
5 3 F
5 6 F
3 6 I
3 4 F
6 4 F
6 7 I
4 7 F
1 4



7 11
1 2 F
1 5 I
2 5 I
2 3 I
5 3 F
5 6 F
3 6 I
3 4 F
6 4 F
6 7 I
4 7 I
1 4

*/