using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Genetic_Algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            GABinary gA = new GABinary(6, 7, 7500);
            gA.Looper(100);
        }
    }

    class GABinary
    {
        public int Genomenum;
        public int MutationChance;
        public int Number;
        public int NumforRank = 8;
        public GABinary(int gnum,int num, int percent)
        {
            Genomenum = gnum;
            Number = num;
            MutationChance = percent;
        }

        public static bool[,] Initializer(int gnum, int num)
        {
            bool[,] ReturnArray = new bool[gnum, num];
            Random random = new Random();
            for (int i = 0; i < gnum; i++)
            {
                for(int k = 0; k < num; k++)
                {
                    if (random.Next(2) == 1)
                    {
                        ReturnArray[i,k] = true;
                    }
                    else
                    {
                        ReturnArray[i,k] = false;
                    }
                }
            }
            return ReturnArray;
        }

        public static bool[,] Selector_Ranked(bool[,] operend,int nfr)
        {
            bool[,] ReturnArray = new bool[operend.GetLength(0), operend.GetLength(1)];
            Dictionary<int, bool[]> list = new Dictionary<int, bool[]>();
            for (int i = 0; i < operend.GetLength(0); i++) {
                if(list.ContainsKey(Convertbinarytoint(Seperator(operend, i)))==false)
                    list.Add(Convertbinarytoint(Seperator(operend, i)), Seperator(operend, i));
                }
            var result = list.OrderByDescending(n=>n.Key);
            var resul = result.Take(nfr);
            int a;
            if (nfr > result.Count()) { a = result.Count(); }
            else { a = nfr; }
            for(int i = 0; i < a; i++)
            {
                for(int k = 0; k < operend.GetLength(1); k++)
                {
//                    Console.WriteLine(i+" "+k);
                    ReturnArray[i, k] = (resul.ElementAt(i).Value)[k];
                }
            }
            return ReturnArray;
        }

        public static int Convertbinarytoint(bool[] array)
        {
            int re=0;
            for(int i = 0; i < array.Length; i++)
            {
                if (array[i] == false) continue;
                else
                {
                    re += (int)Math.Pow(2, i);
                }
            }
            return re;
        }

        public static bool[] Seperator(bool[,] array,int rank)
        {
            bool[] re = new bool[array.GetLength(1)];
            for(int i = 0; i < re.Length; i++)
            {
                re[i] = array[rank, i];
            }

            return re;
        }

        public static bool[,] Crossover(bool[,] operend,int gnum)
        {
            bool[,] retu = new bool[gnum,operend.GetLength(1)];
            Random random = new Random();
            for(int i = 0; i < gnum; i++)
            {
                int g1 = random.Next(operend.GetLength(0)), g2 = random.Next(operend.GetLength(0));
                for(int k = 0; k < operend.GetLength(1); k++)
                {
                    if (random.Next(2) == 1)
                    {
                        retu[i, k] = operend[g1, k];
                    }
                    else
                    {
                        retu[i, k] = operend[g2, k];
                    }
                }
            }
            return retu;
        }
        
        public static bool[,] Mutator(bool[,] operend,int mchance)
        {
            bool[,] retu = new bool[operend.GetLength(0), operend.GetLength(1)];
            Random random = new Random();
            for(int i = 0; i < retu.GetLength(0); i++)
            {
                for(int k = 0; k < retu.GetLength(1); k++)
                {
                    if (random.Next(10000) > mchance - 1)
                    {
                        if (random.Next(2) == 1) retu[i, k] = true;
                        else retu[i, k] = false;
                    }
                    else retu[i, k] = operend[i, k];
                }
            }

            return retu;
        }

        public bool[] Looper(int looptime)
        {
            bool[,] Result = Initializer(Genomenum, Number);
            Result = Selector_Ranked(Result, NumforRank);
            Console.Write("Generation :0 , Max :");
            Writer(Result);
             
            for (int i = 1; i < looptime; i++)
            {
                bool[,] Temp = Arraycopy(Result);
                Result = Crossover(Result, Genomenum);
                Result = Mutator(Result, MutationChance);
                Result = Selector_Ranked(Result, NumforRank);
                for (; ; ) {
                    if (Convertbinarytoint(Seperator(Result, 0)) >= Convertbinarytoint(Seperator(Temp, 0)))
                        break;
                    else
                    {
                        Result = Crossover(Temp, Genomenum);
                        Result = Mutator(Temp, MutationChance);
                        Result = Selector_Ranked(Temp, NumforRank);
                    }
                }
                Console.Write("Generation :"+i+" , Max :");
                Writer(Result);
            }
            return Seperator(Result, 0);
        }

        public static void Writer(bool[,] operend)
        {
            for (int i = 0; i < operend.GetLength(1); i++)
            {
                Console.Write(Convert.ToInt32(operend[0, i]));
                Console.Write(',');
            }
            Console.Write("\n");
        }

        public static bool[,] Arraycopy(bool[,] operend)
        {
            bool[,] newcopy = new bool[operend.GetLength(0), operend.GetLength(1)];
            for(int i = 0; i < newcopy.GetLength(0); i++)
            {
                for(int k = 0; k < newcopy.GetLength(1); k++)
                {
                    newcopy[i, k] = operend[i, k];
                }
            }
            return newcopy;
        }
    }
}
