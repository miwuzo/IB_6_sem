/*using System;
using System.Collections.Generic;
using System.IO;

class Program
{
	static void Main(string[] args)
	{
		string filePath = "input.txt"; // Путь к вашему текстовому файлу
		string plaintext = File.ReadAllText(filePath);

		string keyword = "black"; // Замените на вашу фамилию

		// Шифрование с использованием шифра Виженера
		long encryptTimeVigenere = MeasureExecutionTime(() =>
		{
			string encryptedVigenere = VigenereEncrypt(plaintext, keyword);
			var histogramEncrypted = GenerateFrequencyHistogram(encryptedVigenere);
			Console.WriteLine("Зашифрованный текст (Виженера): " + encryptedVigenere);
			PrintHistogram(histogramEncrypted, "Гистограмма (Виженера)");
		});

		// Шифрование с использованием шифра Порты
		long encryptTimePort = MeasureExecutionTime(() =>
		{
			string encryptedPort = PortEncrypt(plaintext);
			var histogramPort = GenerateFrequencyHistogram(encryptedPort);
			Console.WriteLine("Зашифрованный текст (Порты): " + encryptedPort);
			PrintHistogram(histogramPort, "Гистограмма (Порты)");
		});

		Console.WriteLine($"Время шифрования Виженера: {encryptTimeVigenere} мс");
		Console.WriteLine($"Время шифрования Порты: {encryptTimePort} мс");
	}

	public static string VigenereEncrypt(string plaintext, string keyword)
	{
		string result = "";
		int keywordIndex = 0;

		foreach (char c in plaintext)
		{
			if (char.IsLetter(c))
			{
				char offset = char.IsUpper(c) ? 'A' : 'a';
				char keyChar = char.ToLower(keyword[keywordIndex % keyword.Length]);
				int shift = keyChar - 'a'; // Сдвиг на основе позиции в алфавите

				// Зашифровка символа
				char encryptedChar = (char)((c + shift - offset) % 26 + offset);
				result += encryptedChar;

				keywordIndex++; // Увеличиваем индекс ключа только для букв
			}
			else
			{
				result += c; // Не изменяем символы, которые не являются буквами
			}
		}

		return result;
	}

	public static string PortEncrypt(string plaintext)
	{
		int shift = 3; // Фиксированный сдвиг
		string result = "";

		foreach (char symbol in plaintext)
		{
			if (char.IsLetter(symbol))
			{
				char offset = char.IsUpper(symbol) ? 'A' : 'a';
				char encryptedChar = (char)((symbol + shift - offset) % 26 + offset);
				result += encryptedChar;
			}
			else
			{
				result += symbol; // Оставляем символ без изменений, если он не буква
			}
		}

		return result;
	}

	public static Dictionary<char, int> GenerateFrequencyHistogram(string text)
	{
		var histogram = new Dictionary<char, int>();

		foreach (char c in text)
		{
			if (char.IsLetter(c))
			{
				if (histogram.ContainsKey(c))
					histogram[c]++;
				else
					histogram[c] = 1;
			}
		}

		return histogram;
	}

	public static long MeasureExecutionTime(Action action)
	{
		var watch = System.Diagnostics.Stopwatch.StartNew();
		action();
		watch.Stop();
		return watch.ElapsedMilliseconds;
	}

	public static void PrintHistogram(Dictionary<char, int> histogram, string title)
	{
		Console.WriteLine(title);
		foreach (var entry in histogram)
		{
			Console.WriteLine($"{entry.Key}: {entry.Value}");
		}
		Console.WriteLine();
	}
}*/



using System;
using System.Collections.Generic;
using System.IO;

class Program
{
	static void Main(string[] args)
	{
		string filePath = "input.txt";
		string filePath2 = "v.txt";
		string filePath3 = "p.txt"; 

		string plaintext = File.ReadAllText(filePath);
		string plaintext2 = File.ReadAllText(filePath2);
		string plaintext3 = File.ReadAllText(filePath3);

		string keyword = "black";

		// Генерация и вывод гистограммы исходного текста
		var histogramOriginal = GenerateFrequencyHistogram(plaintext);
		PrintHistogram(histogramOriginal, "Гистограмма (Исходный текст)");

		// Шифрование с использованием шифра Виженера
		long encryptTimeVigenere = MeasureExecutionTime(() =>
		{
			string encryptedVigenere = VigenereEncrypt(plaintext, keyword);
			var histogramEncrypted = GenerateFrequencyHistogram(encryptedVigenere);
			Console.WriteLine("Зашифрованный текст (Виженера): " + encryptedVigenere);
			PrintHistogram(histogramEncrypted, "Гистограмма (Виженера)");

			string decryptedVigenere = VigenereDecrypt(plaintext2, keyword);
			Console.WriteLine("Расшифрованный текст (Виженера): " + decryptedVigenere);
		});

		// Шифрование с использованием шифра Порты
		long encryptTimePort = MeasureExecutionTime(() =>
		{
			string encryptedPort = PortEncrypt(plaintext);
			var histogramPort = GenerateFrequencyHistogram(encryptedPort);
			Console.WriteLine("Зашифрованный текст (Порты): " + encryptedPort);
			PrintHistogram(histogramPort, "Гистограмма (Порты)");

			string decryptedPort = PortDecrypt(plaintext3);
			Console.WriteLine("Расшифрованный текст (Порты): " + decryptedPort);
		});

		// Время расшифровки для Виженера
		long decryptTimeVigenere = MeasureExecutionTime(() =>
		{
			string decryptedVigenere = VigenereDecrypt(plaintext2, keyword);
			Console.WriteLine("Расшифрованный текст (Виженера): " + decryptedVigenere);
		});

		// Время расшифровки для Порты
		long decryptTimePort = MeasureExecutionTime(() =>
		{
			string decryptedPort = PortDecrypt(plaintext3);
			Console.WriteLine("Расшифрованный текст (Порты): " + decryptedPort);
		});

		// Вывод времени расшифровки
		Console.WriteLine($"Время расшифровки Виженера: {decryptTimeVigenere} мс");
		Console.WriteLine($"Время расшифровки Порты: {decryptTimePort} мс");

		Console.WriteLine($"Время шифрования Виженера: {encryptTimeVigenere} мс");
		Console.WriteLine($"Время шифрования Порты: {encryptTimePort} мс");
	}

	public static string VigenereEncrypt(string plaintext, string keyword)
	{
		string result = "";
		int keywordIndex = 0;

		foreach (char c in plaintext)
		{
			if (char.IsLetter(c))
			{
				char offset = char.IsUpper(c) ? 'A' : 'a';
				char keyChar = char.ToLower(keyword[keywordIndex % keyword.Length]);
				int shift = keyChar - 'a'; // Сдвиг на основе позиции в алфавите

				// Зашифровка символа
				char encryptedChar = (char)((c + shift - offset) % 26 + offset);
				result += encryptedChar;

				keywordIndex++; // Увеличиваем индекс ключа только для букв
			}
			else
			{
				result += c; // Не изменяем символы, которые не являются буквами
			}
		}

		return result;
	}

	public static string VigenereDecrypt(string ciphertext, string keyword)
	{
		string result = "";
		int keywordIndex = 0;

		foreach (char c in ciphertext)
		{
			if (char.IsLetter(c))
			{
				char offset = char.IsUpper(c) ? 'A' : 'a';
				char keyChar = char.ToLower(keyword[keywordIndex % keyword.Length]);
				int shift = keyChar - 'a'; // Сдвиг на основе позиции в алфавите

				// Расшифровка символа
				char decryptedChar = (char)((c - shift - offset + 26) % 26 + offset);
				result += decryptedChar;

				keywordIndex++; // Увеличиваем индекс ключа только для букв
			}
			else
			{
				result += c; // Не изменяем символы, которые не являются буквами
			}
		}

		return result;
	}

	public static string PortEncrypt(string plaintext)
	{
		int shift = 3; // Фиксированный сдвиг
		string result = "";

		foreach (char symbol in plaintext)
		{
			if (char.IsLetter(symbol))
			{
				char offset = char.IsUpper(symbol) ? 'A' : 'a';
				char encryptedChar = (char)((symbol + shift - offset) % 26 + offset);
				result += encryptedChar;
			}
			else
			{
				result += symbol; // Оставляем символ без изменений, если он не буква
			}
		}

		return result;
	}

	public static string PortDecrypt(string ciphertext)
	{
		int shift = 3; // Фиксированный сдвиг
		string result = "";

		foreach (char symbol in ciphertext)
		{
			if (char.IsLetter(symbol))
			{
				char offset = char.IsUpper(symbol) ? 'A' : 'a';
				char decryptedChar = (char)((symbol - shift - offset + 26) % 26 + offset);
				result += decryptedChar;
			}
			else
			{
				result += symbol; // Оставляем символ без изменений, если он не буква
			}
		}

		return result;
	}

	public static Dictionary<char, int> GenerateFrequencyHistogram(string text)
	{
		var histogram = new Dictionary<char, int>();

		foreach (char c in text)
		{
			if (char.IsLetter(c))
			{
				if (histogram.ContainsKey(c))
					histogram[c]++;
				else
					histogram[c] = 1;
			}
		}

		return histogram;
	}

	public static long MeasureExecutionTime(Action action)
	{
		var watch = System.Diagnostics.Stopwatch.StartNew();
		action();
		watch.Stop();
		return watch.ElapsedMilliseconds;
	}



	public static void PrintHistogram(Dictionary<char, int> histogram, string title)
	{
		Console.WriteLine(title);
		foreach (var entry in histogram)
		{
			Console.WriteLine($"{entry.Key}: {entry.Value}");
		}
		Console.WriteLine();
	}
}