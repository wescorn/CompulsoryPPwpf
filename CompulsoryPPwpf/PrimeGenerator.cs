using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace CompulsoryPPwpf
{
    public class PrimeGenerator
    {

        Object lockMe = new Object();
        public PrimeGenerator()
        {

        }  
        

        public List<long> GetPrimeNumbersSequential(long first, long last)
        {
            long num, i, ctr;
            List<long> primes = new List<long>();

            for (num = first; num <= last; num++)
            {
                ctr = 0;

                for (i = 2; i <= num / 2; i++)
                {
                    if (num % i == 0)
                    {
                        ctr++;
                        break;
                    }
                }

                if (ctr == 0 && num != 1)
                {
                    primes.Add(num);
                }

            }
            return primes;
        }

        public Task<List<long>> GetPrimeNumbersParallelAsync(long first, long last)
        {
            return Task.Run(() => GetPrimeNumbersParallel(first, last));
        }

        public Task<List<long>> GetPrimeNumbersSequentialAsync(long first, long last)
        {
            return Task.Run(() => GetPrimeNumbersSequential(first, last));
        }

        public List<long> GetPrimeNumbersParallel(long first, long last)
        {
            List<long> primes = new List<long>();
            Parallel.ForEach(Partitioner.Create(first, last), range =>
            {
                Debug.WriteLine("Chunk from "+ range.Item1 + " to "+ range.Item2);
                lock(lockMe)
                {
                    primes.AddRange(GetPrimeNumbersSequential(range.Item1, range.Item2 - (range.Item2 == last ? 0 : 1)));
                }
                
            });
           // primes.Sort();
            foreach (var item in primes)
            {
                Debug.WriteLine(item);
            }
            
            return primes;


            //for (int iThread = 0; iThread < threads; iThread++)
            //{
            //    int localThread = iThread;
            //    task[localThread] = Task.Run(() =>
            //    {
            //        Parallel.ForEach(Partitioner.Create(0, threadedArray.Length), range =>
            //        {
            //            for (int i = range.Item1; i < range.Item2; i++)
            //            {
            //                MathF.Sqrt(threadedArray[i]);
            //            }
            //        });
            //    });
            //}
        }



        public List<long> GetPrimesTest(long first, long last)
        {
            long num, i, ctr;
            List<long> primes = new List<long>();

            for (num = first; num <= last; num++)
            {
                ctr = 0;

                for (i = 2; i <= num / 2; i++)
                {
                    if (num % i == 0)
                    {
                        ctr++;
                        break;
                    }
                }

                if (ctr == 0 && num != 1)
                {
                   
                    primes.Add(num);
                }
                    
            }
            return primes;
        }
    }
}