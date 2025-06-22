using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Lab9
{
    public class Knapsack
    {
        public static Random random = new Random();

        public static List<int> GenerateSuperIncreasingSequence(int length)
        {
            List<int> sequence = new List<int>();
            int total = 0;
            for (int i = 0; i < length; i++)
            {
                int nextValue = total + random.Next(1, 10);
                sequence.Add(nextValue);
                total += nextValue;
            }
            return sequence;
        }

        public static int GenerateQ(int sum)
        {
            return sum + random.Next(1, 10);
        }

        public static int GenerateR(int q)
        {
            int r;
            do
            {
                r = random.Next(1, q);
            } while (Gcd(r, q) != 1);
            return r;
        }

        public static int Gcd(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static List<int> GenerateNormalSequence(List<int> superIncreasingSequence, int q, int r)
        {
            return superIncreasingSequence.Select(x => (x * r) % q).ToList();
        }

        public static List<int> Encrypt(string message, List<int> publicKey)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(message);
            List<int> encrypted = new List<int>();

            foreach (byte b in bytes)
            {
                int sum = 0;
                for (int i = 0; i < 8; i++)
                {
                    if ((b & (1 << i)) != 0)
                    {
                        sum += publicKey[i];
                    }
                }
                encrypted.Add(sum);
            }

            return encrypted;
        }

        public static string Decrypt(List<int> encryptedMessage, List<int> privateKey, int q, int r)
        {
            int rInverse = ModInverse(r, q);
            List<byte> decryptedBytes = new List<byte>();

            foreach (int encryptedValue in encryptedMessage)
            {
                int cPrime = (encryptedValue * rInverse) % q;
                byte b = 0;

                for (int i = privateKey.Count - 1; i >= 0; i--)
                {
                    if (cPrime >= privateKey[i])
                    {
                        cPrime -= privateKey[i];
                        b |= (byte)(1 << i);
                    }
                }

                decryptedBytes.Add(b);
            }

            return Encoding.ASCII.GetString(decryptedBytes.ToArray());
        }

        public static int ModInverse(int a, int m)
        {
            int m0 = m, t, q;
            int x0 = 0, x1 = 1;

            if (m == 1)
                return 0;

            while (a > 1)
            {
                q = a / m;
                t = m;

                m = a % m;
                a = t;
                t = x0;

                x0 = x1 - q * x0;
                x1 = t;
            }

            if (x1 < 0)
                x1 += m0;

            return x1;
        }
    }
}
