using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day_01_G03
{
    class CaseInsensitiveComparer : IComparer<string>
    {
        public int Compare(string x, string y) =>
            string.Compare(x, y, StringComparison.OrdinalIgnoreCase);
    }

    class Program
    {
        static void Show<T>(string title, IEnumerable<T> items)
        {
            Console.WriteLine($"\n--- {title} ---");
            foreach (var i in items) Console.WriteLine(i);
        }

        static void Main()
        {
            var products = ListGenerator.ProductsList;
            var customers = ListGenerator.CustomersList;
            var cmp = new CaseInsensitiveComparer();

            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            int[] arr = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };
            string[] fruits = { "aPPLE", "AbAcUs", "bRaNcH", "BlUeBeRrY", "ClOvEr", "cHeRry" };

            // ===== Restriction =====
            Show("R1 out of stock", products.Where(p => p.UnitsInStock == 0));
            Show("R2 in stock and price > 3", products.Where(p => p.UnitsInStock > 0 && p.UnitPrice > 3.00M));
            Show("R3 digits name shorter than value", digits.Where((d, i) => d.Length < i));

            // ===== Element =====
            Console.WriteLine("\n--- E1 ---");
            Console.WriteLine(products.First(p => p.UnitsInStock == 0));
            Console.WriteLine("\n--- E2 ---");
            Console.WriteLine(products.FirstOrDefault(p => p.UnitPrice > 1000)?.ToString() ?? "null");
            Console.WriteLine("\n--- E3 ---");
            Console.WriteLine(arr.Where(n => n > 5).ElementAt(1));

            // ===== Aggregate =====
            Console.WriteLine("\n--- A1 odd count ---");
            Console.WriteLine(arr.Count(n => n % 2 != 0));
            Show("A2 customers and orders count",
                customers.Select(c => new { c.CustomerID, OrdersCount = c.Orders.Count() }));
            Show("A3 categories and products count",
                products.GroupBy(p => p.Category).Select(g => new { Category = g.Key, ProductCount = g.Count() }));
            Console.WriteLine("\n--- A4 sum ---");
            Console.WriteLine(arr.Sum());

            string[] words = File.ReadAllLines("dictionary_english.txt");
            Console.WriteLine("\n--- A5 total chars ---");
            Console.WriteLine(words.Sum(w => w.Length));
            Console.WriteLine("--- A6 shortest ---");
            Console.WriteLine(words.Min(w => w.Length));
            Console.WriteLine("--- A7 longest ---");
            Console.WriteLine(words.Max(w => w.Length));
            Console.WriteLine("--- A8 average ---");
            Console.WriteLine(words.Average(w => w.Length));

            // ===== Ordering =====
            Show("O1 products by name", products.OrderBy(p => p.ProductName));
            Show("O2 case-insensitive", fruits.OrderBy(w => w, cmp));
            Show("O3 by stock desc", products.OrderByDescending(p => p.UnitsInStock));
            Show("O4 digits by length then name", digits.OrderBy(d => d.Length).ThenBy(d => d));
            Show("O5 length then case-insensitive", fruits.OrderBy(w => w.Length).ThenBy(w => w, cmp));
            Show("O6 category then price desc", products.OrderBy(p => p.Category).ThenByDescending(p => p.UnitPrice));
            Show("O7 length then case-insensitive desc", fruits.OrderBy(w => w.Length).ThenByDescending(w => w, cmp));
            Show("O8 second letter i, reversed", digits.Where(d => d[1] == 'i').Reverse());

            // ===== Transformation =====
            Show("T1 product names", products.Select(p => p.ProductName));

            string[] w2 = { "aPPLE", "BlUeBeRrY", "cHeRry" };
            Show("T2 upper and lower", w2.Select(w => new { Upper = w.ToUpper(), Lower = w.ToLower() }));

            Show("T3 renamed price", products.Select(p => new { p.ProductName, p.Category, Price = p.UnitPrice }));

            Console.WriteLine("\n--- T4 ---");
            Console.WriteLine("Number: In-place?");
            foreach (var x in arr.Select((n, i) => new { Number = n, InPlace = (n == i) }))
                Console.WriteLine($"{x.Number}: {x.InPlace}");

            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };
            Console.WriteLine("\n--- T5 ---");
            Console.WriteLine("Pairs where a < b:");
            var t5 = from a in numbersA
                     from b in numbersB
                     where a < b
                     select new { a, b };
            foreach (var p in t5) Console.WriteLine($"{p.a} is less than {p.b}");

            Show("T6 orders total < 500",
                customers.SelectMany(c => c.Orders).Where(o => o.Total < 500.00M));
            Show("T7 orders in 1998 or later",
                customers.SelectMany(c => c.Orders).Where(o => o.OrderDate.Year >= 1998));
        }
    }
}