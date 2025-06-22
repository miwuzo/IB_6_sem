using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
public class Entropy
{
    public static string ReadFromFile(string filePath)
    {
        if (!File.Exists(filePath)) { return ""; }
        return File.ReadAllText(filePath, Encoding.UTF8).ToLower();
    }

    public static Dictionary<char, int> GetSymbolFrequencies(string text)
    {
        Dictionary<char, int> frequencies = new Dictionary<char, int>();
        foreach (char c in text)
        {
            if (char.IsLetterOrDigit(c)) 
            {  if (frequencies.ContainsKey(c))  frequencies[c]++;
                else frequencies[c] = 1;
            }
        }
        return frequencies;
    }

    public static Dictionary<char, double> GetSymbolProbabilities(string text)
    {
        var frequencies = GetSymbolFrequencies(text);
        double totalCount = text.Length;
        Dictionary<char, double> probabilities = frequencies.ToDictionary(
            pair => pair.Key, 
            pair => pair.Value / totalCount);
        return probabilities;
    }

    public static double GetShannonEntropy(string text)
    {
        var probabilities = GetSymbolProbabilities(text);
        double entropy = 0;
        foreach (var prob in probabilities.Values)
        { if (prob > 0) entropy -= prob * Math.Log2(prob); // Энтропия = -Σ p(x) * log2(p(x))
        }
        return Math.Round(entropy, 3); 
    }

    public static double GetInformationAmount(string text)
    {
        double entropy = GetShannonEntropy(text);
        return entropy * text.Length;
    }

    public static double GetInformationAmountWithErrors(string text, double errorProbability)
    {
        double entropy = GetShannonEntropy(text);
        if (errorProbability == 0 || errorProbability == 1)
        { return entropy * text.Length;}
        
        double effectiveError = -(errorProbability * Math.Log2(errorProbability) + (1 - errorProbability) * Math.Log2(1 - errorProbability));

        // колич инф = длина текста * (энтропия - эффективная ошибка)
        double informationAmount = text.Length * (entropy - effectiveError);

        return Math.Round(informationAmount, 3); 
    }
        
    public static void PrintSymbolFrequencies(string text)
    {
        var frequencies = GetSymbolFrequencies(text);
        Console.WriteLine("Частоты символов:");
        foreach (var pair in frequencies)
        {
            Console.WriteLine($"Символ: {pair.Key}, Частота: {(double)pair.Value / text.Length:F4}");
        }
    }
}