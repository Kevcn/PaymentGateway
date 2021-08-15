using System;
using BenchmarkDotNet.Running;

namespace Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Payment gateway benchmark");
            
            // dotnet build -c Release -
            // The release version of the application is optimised
            // which gives us a more realistic performance benchmark during run time

            // dotnet Benchmarks.dll
            
            BenchmarkRunner.Run<CardServiceBenchmarkets>();
        }
    }
}