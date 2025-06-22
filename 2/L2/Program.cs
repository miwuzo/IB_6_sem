class Program
{
    static void Main()
    {

        var MongName = Entropy.ReadFromFile("./MongName.txt");
        var RumName = Entropy.ReadFromFile("./RumName.txt");
        var ASCIIName = Entropy.ReadFromFile("./ASCIIName.txt");
        string MongText = Entropy.ReadFromFile("./MongText.txt");
        string RumText = Entropy.ReadFromFile("./RumText.txt");
        string BinText = Entropy.ReadFromFile("./BinText.txt");

      
        Entropy.PrintSymbolFrequencies(MongText);
        Console.WriteLine("\n");
        Entropy.PrintSymbolFrequencies(RumText);
        Console.WriteLine("\n");
        Console.WriteLine("Монгольский текст. Энтропия --- " + Entropy.GetShannonEntropy(MongText));
        Console.WriteLine("Румынский текст. Энтропия --- " + Entropy.GetShannonEntropy(RumText));
        Console.WriteLine("Бинарный текст. Энтропия --- " + Entropy.GetShannonEntropy(BinText));
        Console.WriteLine("\n|| при P = 0 количество информации равно:");
        Console.WriteLine("Монгольский --- " + Entropy.GetInformationAmount(MongName));
        Console.WriteLine("Румынский --- " + Entropy.GetInformationAmount(RumName));
        Console.WriteLine("ASCII ---- " + Entropy.GetInformationAmount(ASCIIName));
        double[] err = { 0.1, 0.5, 1.0 };
        foreach (double p in err)
        {
            Console.WriteLine($"\n|| при P = {p} количество информации равно:");
			Console.WriteLine("ASCII --- " + Entropy.GetInformationAmountWithErrors(ASCIIName, p));
            Console.WriteLine();
        }
    }
}