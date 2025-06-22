using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06
{
	public class EnigmaRotor
	{
		private readonly char[] rotorChar;
		private int currentIndex;

		public EnigmaRotor(string rotorString, int startIndex)
		{
			rotorChar = rotorString.ToCharArray();
			currentIndex = startIndex >= rotorString.Length ? 0 : startIndex;
		}

		public char this[int index]
		{
			get
			{
				return rotorChar[(index + currentIndex) % rotorChar.Length];
			}
		}

		public int IndexOf(char symbol)
		{
			int index = rotorChar.ToList().IndexOf(symbol);
			int rightOffset = rotorChar.Length - currentIndex;
			int offsetRotorIndex = (index + rightOffset) % rotorChar.Length;

			return offsetRotorIndex;
		}

		public bool MoveRotor(int offset)
		{
			currentIndex = currentIndex + offset;
			if (currentIndex >= rotorChar.Length)
			{

				currentIndex = currentIndex % rotorChar.Length;
				return true;
			}
			return false;
		}

		public char CurrentRotor()
		{
			return rotorChar[currentIndex];
		}

		public void Reset()
		{
			currentIndex = 0;
		}
	}
}