
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06
{
	public class EnigmaMachine
	{
		private static readonly string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		private static readonly string rotor1 = "AJDKSIRUXBLHWTMCQGZNPYFVOE";
		private static readonly string rotor2 = "LEYJVCNIXWPBQMDRTAKZGFUHOS";
		private static readonly string rotor3 = "FKQHTLXOCBJSPDZRAMEWNIUYGV";

		private static readonly string[] reflectorB = { "AF", "BV", "CP", "DJ", "EI", "GO", "HY", "KR", "LZ", "MX", "NW", "TQ", "SU" };
		public string Encrypt(string text, int posL, int posM, int posR)
		{
			var rotorL = new EnigmaRotor(rotor3, posL);
			var rotorM = new EnigmaRotor(rotor2, posM);
			var rotorR = new EnigmaRotor(rotor1, posR);

			StringBuilder result = new StringBuilder(text.Length);

			foreach (var ch in text)
			{
				char symbol = rotorR[alphabet.IndexOf(ch)];
				symbol = rotorM[alphabet.IndexOf(symbol)];
				symbol = rotorL[alphabet.IndexOf(symbol)];
				symbol = reflectorB.First(x => x.Contains(symbol)).First(x => !x.Equals(symbol));
				symbol = rotorL[alphabet.IndexOf(symbol)];
				symbol = rotorM[alphabet.IndexOf(symbol)];
				symbol = rotorR[alphabet.IndexOf(symbol)];

				result.Append(symbol);
			}
			return result.ToString();
		}

		public string Decrypt(string text, int posL, int posM, int posR)
		{
			var rotorL = new EnigmaRotor(rotor3, posL);
			var rotorM = new EnigmaRotor(rotor2, posM);
			var rotorR = new EnigmaRotor(rotor1, posR);

			StringBuilder result = new StringBuilder(text.Length);

			foreach (var charSymbol in text)
			{
				char symbol = alphabet[rotorR.IndexOf(charSymbol)];
				symbol = alphabet[rotorM.IndexOf(symbol)];
				symbol = alphabet[rotorL.IndexOf(symbol)];
				symbol = reflectorB.First(x => x.Contains(symbol)).First(x => !x.Equals(symbol));
				symbol = alphabet[rotorL.IndexOf(symbol)];
				symbol = alphabet[rotorM.IndexOf(symbol)];
				symbol = alphabet[rotorR.IndexOf(symbol)];

				result.Append(symbol);
			}
			return result.ToString();
		}
	}
}