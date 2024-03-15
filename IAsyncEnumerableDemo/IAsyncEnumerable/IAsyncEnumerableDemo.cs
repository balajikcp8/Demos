using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAsyncEnumerable;

public class IAsyncEnumerableDemo
{
    public static async IAsyncEnumerable<int> GetEvenNumbersAsync(IEnumerable<int> integers)
    {
        foreach (var n in integers)
        {
            if (n % 2 == 0)
            {
                await Task.Delay(2000);
                yield return n;
            }
        }
    }

    public static async IAsyncEnumerable<int> GetEvenNumbers(IEnumerable<int> integers)
    {
        List<Task> items = new List<Task>();
        List<int> evenNumbers = new List<int>();
        foreach (var n in integers)
        {
            if (n % 2 == 0)
            {
                items.Add(Task.Delay(2000));
                evenNumbers.Add(n);
            }
        }
        int i = 0;
        while (items.Count != 0)
        {
            var completed = await Task.WhenAny(items).ConfigureAwait(false);
            items.Remove(completed);
            yield return evenNumbers[i++];
        }
    }

    public static async Task Test()
    {
        int[] integers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: Start");
        await foreach (var item in GetEvenNumbers(integers))
        {
            Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: {item}");
        }
        Console.WriteLine($"{DateTime.Now.ToLongTimeString()}: End");
    }
}

