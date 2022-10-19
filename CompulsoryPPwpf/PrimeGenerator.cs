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
        private static bool IsPrime(long number)

        {

            if (number < 2) return false;

            if (number == 2 || number == 3) return true;

            if (number % 2 == 0 || number % 3 == 0) return false;

            for (int i = 5; i * i <= number; i += 6)

            {

                if (number % i == 0 || number % (i + 2) == 0)

                    return false;

            }

            return true;

        }

        public List<long> GetPrimeNumbersSequential(long first, long last)
        {
            long num;
            List<long> primes = new List<long>();

            for (num = first; num <= last; num++)
            {
                
                if(IsPrime(num)) { 
                    primes.Add(num);
                }

            }
            return primes;
        }

        public List<long> GetPrimeNumbersParallel(long first, long last)
        {
            List<long> primes = new List<long>();
            Parallel.ForEach(Partitioner.Create(first, last), range =>
            {
                //Debug.WriteLine("Chunk from "+ range.Item1 + " to "+ range.Item2);
                var generatedPrimes = GetPrimeNumbersSequential(range.Item1, range.Item2 - (range.Item2 == last ? 0 : 1));
                lock (lockMe)
                {
                    primes.AddRange(generatedPrimes);
                }
                
            });
            primes.Sort();
            
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
    }
}