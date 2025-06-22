using System;

namespace Lab06
{
	public class Program
	{
		public static void Main()
		{
			EnigmaMachine enigmaMachine = new EnigmaMachine();

			string encodeMessage = enigmaMachine.Encrypt("YANACHERN", 0, 2, 2);
			Console.WriteLine("Начальный текст: YANACHERN ");

			Console.WriteLine("Зашифрованный текст: " + encodeMessage);

			Console.WriteLine("Расшифрованный текст: " + enigmaMachine.Decrypt(encodeMessage, 0, 2, 2));
			
			string encodeMessage1 = enigmaMachine.Encrypt("YANCHERN", 2, 1, 2);

			Console.WriteLine("212: " + encodeMessage1);
			string encodeMessage2 = enigmaMachine.Encrypt("YANCHERN", 1, 2, 2);

			Console.WriteLine("122: " + encodeMessage2);
			string encodeMessage3 = enigmaMachine.Encrypt("YANCHERN", 1, 2, 1);

			Console.WriteLine("121: " + encodeMessage3);
			string encodeMessage4 = enigmaMachine.Encrypt("YANCHERN", 2, 0, 2);

			Console.WriteLine("321: " + encodeMessage4);
		}
	}
}