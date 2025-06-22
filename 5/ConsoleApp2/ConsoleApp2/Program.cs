using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CryptoApp
{
	class Program
	{
		// Константы для алфавита и ключевых слов (замените своими значениями)
		private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÜß "; // Немецкий алфавит + пробел
		private const string KeyWordName = "YANA";
		private const string KeyWordSurname = "CHORNAYA";

		static void Main(string[] args)
		{
			// Пример использования
			string filePath = "input.txt"; // Укажите путь к вашему текстовому файлу
			string encryptedFilePath = "encrypted.txt";
			string decryptedFilePath = "decrypted.txt";

			try
			{
				// 1. Чтение текста из файла
				string plainText = File.ReadAllText(filePath).ToUpper(); // Читаем и переводим в верхний регистр

				// ****************** Маршрутная перестановка ******************
				Console.WriteLine("Шифрование маршрутной перестановкой (змейкой)...");
				// *** ВАЖНО: Размеры таблицы, чтобы было легко увидеть результат ***
				int tableRows = 3;   // 2 строки
				int tableCols = 220;  // 10 столбцов (измените, если исходный текст очень длинный!)

				Stopwatch stopwatchRouteEncryption = Stopwatch.StartNew();
				string encryptedTextRoute = RouteCipherEncrypt(plainText, tableRows, tableCols);
				stopwatchRouteEncryption.Stop();
				Console.WriteLine($"Время шифрования маршрутной перестановкой: {stopwatchRouteEncryption.ElapsedMilliseconds} мс");
				File.WriteAllText(encryptedFilePath + ".route", encryptedTextRoute);

				Console.WriteLine("Расшифрование маршрутной перестановкой (змейкой)...");
				Stopwatch stopwatchRouteDecryption = Stopwatch.StartNew();
				string decryptedTextRoute = RouteCipherDecrypt(encryptedTextRoute, tableRows, tableCols);
				stopwatchRouteDecryption.Stop();
				Console.WriteLine($"Время расшифрования маршрутной перестановкой: {stopwatchRouteDecryption.ElapsedMilliseconds} мс");
				File.WriteAllText(decryptedFilePath + ".route", decryptedTextRoute);

				Console.WriteLine("\nГистограммы частот (Маршрутная перестановка):");
				Console.WriteLine("Исходный текст:");
				PrintHistogram(plainText);
				Console.WriteLine("Зашифрованный текст:");
				PrintHistogram(encryptedTextRoute);

				// ****************** Множественная перестановка ******************
				Console.WriteLine("\nШифрование множественной перестановкой...");
				Stopwatch stopwatchMultipleEncryption = Stopwatch.StartNew();
				string encryptedTextMultiple = MultiplePermutationEncrypt(plainText, KeyWordName, KeyWordSurname);
				stopwatchMultipleEncryption.Stop();
				Console.WriteLine($"Время шифрования множественной перестановкой: {stopwatchMultipleEncryption.ElapsedMilliseconds} мс");
				File.WriteAllText(encryptedFilePath + ".multiple", encryptedTextMultiple);

				Console.WriteLine("Расшифрование множественной перестановкой...");
				Stopwatch stopwatchMultipleDecryption = Stopwatch.StartNew();
				string decryptedTextMultiple = MultiplePermutationDecrypt(encryptedTextMultiple, KeyWordName, KeyWordSurname);
				stopwatchMultipleDecryption.Stop();
				Console.WriteLine($"Время расшифрования множественной перестановкой: {stopwatchMultipleDecryption.ElapsedMilliseconds} мс");
				File.WriteAllText(decryptedFilePath + ".multiple", decryptedTextMultiple);

				Console.WriteLine("\nГистограммы частот (Множественная перестановка):");
				Console.WriteLine("Исходный текст:");
				PrintHistogram(plainText);
				Console.WriteLine("Зашифрованный текст:");
				PrintHistogram(encryptedTextMultiple);


				Console.WriteLine("\nОперации завершены.  Результаты сохранены в файлах encrypted.txt и decrypted.txt");

			}
			catch (FileNotFoundException)
			{
				Console.WriteLine($"Ошибка: Файл {filePath} не найден.");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Произошла ошибка: {ex.Message}");
			}

			Console.ReadKey();
		}

		// ****************** Маршрутная перестановка (змейкой) ******************

		static string RouteCipherEncrypt(string plainText, int rows, int cols)
		{
			Console.WriteLine($"Encrypting with rows={rows}, cols={cols}, length={plainText.Length}"); // Отладочное сообщение
																									   // Дополняем текст до кратного размера таблицы
			int paddingLength = rows * cols - plainText.Length;
			if (paddingLength > 0)
			{
				plainText += new string(' ', paddingLength); // Добавляем пробелы для заполнения
			}

			Console.WriteLine($"Padded text: '{plainText}'"); // Отладочное сообщение

			char[,] matrix = new char[rows, cols];
			int textIndex = 0;

			// Заполняем матрицу змейкой
			for (int i = 0; i < rows; i++)
			{
				if (i % 2 == 0)
				{
					for (int j = 0; j < cols; j++)
					{
						matrix[i, j] = plainText[textIndex++];
						Console.WriteLine($"matrix[{i},{j}] = '{matrix[i, j]}'"); // Отладочное сообщение
					}
				}
				else
				{
					for (int j = cols - 1; j >= 0; j--)
					{
						matrix[i, j] = plainText[textIndex++];
						Console.WriteLine($"matrix[{i},{j}] = '{matrix[i, j]}'"); // Отладочное сообщение
					}
				}
			}

			// Читаем матрицу по столбцам для получения зашифрованного текста
			string encryptedText = "";
			for (int j = 0; j < cols; j++)
			{
				for (int i = 0; i < rows; i++)
				{
					encryptedText += matrix[i, j];
				}
			}

			Console.WriteLine($"Encrypted text: '{encryptedText}'"); // Отладочное сообщение
			return encryptedText;
		}

		static string RouteCipherDecrypt(string encryptedText, int rows, int cols)
		{
			Console.WriteLine($"Decrypting with rows={rows}, cols={cols}, length={encryptedText.Length}"); // Отладочное сообщение

			char[,] matrix = new char[rows, cols];
			int textIndex = 0;

			// Заполняем матрицу из зашифрованного текста по столбцам
			for (int j = 0; j < cols; j++)
			{
				for (int i = 0; i < rows; i++)
				{
					matrix[i, j] = encryptedText[textIndex++];
				}
			}

			// Читаем матрицу змейкой для получения исходного текста
			string decryptedText = "";
			for (int i = 0; i < rows; i++)
			{
				if (i % 2 == 0)
				{
					for (int j = 0; j < cols; j++)
					{
						decryptedText += matrix[i, j];
					}
				}
				else
				{
					for (int j = cols - 1; j >= 0; j--)
					{
						decryptedText += matrix[i, j];
					}
				}
			}

			Console.WriteLine($"Decrypted text before trim: '{decryptedText}'"); // Отладочное сообщение

			return decryptedText.TrimEnd();  //Удаляем добавленные пробелы
		}

		// ****************** Множественная перестановка ******************

		static string MultiplePermutationEncrypt(string plainText, string keyWord1, string keyWord2)
		{
			keyWord1 = keyWord1.ToUpper();
			keyWord2 = keyWord2.ToUpper();

			// Создаем две таблицы перестановки на основе ключевых слов
			int[] permutationTable1 = CreatePermutationTable(keyWord1);
			int[] permutationTable2 = CreatePermutationTable(keyWord2);

			// Вычисляем общую длину перестановок
			int length1 = permutationTable1.Length;
			int length2 = permutationTable2.Length;
			int totalLength = length1 * length2;

			// Дополняем текст пробелами до кратного длине обеих таблиц
			while (plainText.Length % totalLength != 0)
			{
				plainText += " ";
			}

			string encryptedText = "";
			for (int i = 0; i < plainText.Length; i += totalLength)
			{
				string block = plainText.Substring(i, totalLength);
				string tempBlock = ApplyPermutation(block, permutationTable1);
				string encryptedBlock = ApplyPermutation(tempBlock, permutationTable2);
				encryptedText += encryptedBlock;
			}

			return encryptedText;
		}

		static string MultiplePermutationDecrypt(string encryptedText, string keyWord1, string keyWord2)
		{
			keyWord1 = keyWord1.ToUpper();
			keyWord2 = keyWord2.ToUpper();

			// Создаем две таблицы перестановки на основе ключевых слов
			int[] permutationTable1 = CreatePermutationTable(keyWord1);
			int[] permutationTable2 = CreatePermutationTable(keyWord2);

			// Создаем обратные таблицы перестановки
			int[] inversePermutationTable1 = CreateInversePermutationTable(permutationTable1);
			int[] inversePermutationTable2 = CreateInversePermutationTable(permutationTable2);

			// Вычисляем общую длину перестановок
			int length1 = permutationTable1.Length;
			int length2 = permutationTable2.Length;
			int totalLength = length1 * length2;

			string decryptedText = "";
			for (int i = 0; i < encryptedText.Length; i += totalLength)
			{
				string block = encryptedText.Substring(i, totalLength);
				string tempBlock = ApplyPermutation(block, inversePermutationTable2);
				string decryptedBlock = ApplyPermutation(tempBlock, inversePermutationTable1);
				decryptedText += decryptedBlock;
			}

			return decryptedText.TrimEnd(); // Remove trailing spaces
		}

		// Создание таблицы перестановки на основе ключевого слова
		static int[] CreatePermutationTable(string keyWord)
		{
			// Удаляем повторяющиеся символы и сортируем
			string uniqueKeyWord = new string(keyWord.Distinct().ToArray());
			char[] sortedKeyWord = uniqueKeyWord.ToCharArray();
			Array.Sort(sortedKeyWord);

			int[] permutationTable = new int[uniqueKeyWord.Length];
			for (int i = 0; i < uniqueKeyWord.Length; i++)
			{
				permutationTable[i] = uniqueKeyWord.IndexOf(sortedKeyWord[i]);
			}

			return permutationTable;
		}

		// Создание обратной таблицы перестановки
		static int[] CreateInversePermutationTable(int[] permutationTable)
		{
			int[] inversePermutationTable = new int[permutationTable.Length];
			for (int i = 0; i < permutationTable.Length; i++)
			{
				inversePermutationTable[permutationTable[i]] = i;
			}
			return inversePermutationTable;
		}

		// Применение перестановки к блоку текста
		static string ApplyPermutation(string block, int[] permutationTable)
		{
			char[] permutedBlock = new char[block.Length];
			for (int i = 0; i < block.Length; i++)
			{
				int permIndex = permutationTable[i % permutationTable.Length];  // Циклически перебираем таблицу перестановки
				permutedBlock[i] = block[permIndex];
				Console.WriteLine($"block[{i}] = '{block[i]}', permIndex = {permIndex}, permutedBlock[{i}] = '{permutedBlock[i]}'"); // Отладочная печать!
			}
			return new string(permutedBlock);
		}

		// ****************** Гистограмма частот ******************

		static void PrintHistogram(string text)
		{
			Dictionary<char, int> frequency = new Dictionary<char, int>();
			foreach (char c in text)
			{
				if (frequency.ContainsKey(c))
				{
					frequency[c]++;
				}
				else
				{
					frequency[c] = 1;
				}
			}

			// Сортируем по убыванию частоты
			var sortedFrequency = frequency.OrderByDescending(x => x.Value);

			foreach (var item in sortedFrequency)
			{
				Console.WriteLine($"{item.Key}: {item.Value}");
			}
		}
	}
}


