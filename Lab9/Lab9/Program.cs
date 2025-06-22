using Lab9;
using System.Diagnostics;
using System.Text;

string message = "ChornayaYanaRuslanovna";

byte[] bytesToEncode = Encoding.UTF8.GetBytes(message);

//message = Convert.ToBase64String(bytesToEncode);
int z = 8;

DateTime Start = DateTime.Now; 
DateTime End = DateTime.Now;

List<int> privateKey = Knapsack.GenerateSuperIncreasingSequence(z);

int q = Knapsack.GenerateQ(privateKey.Sum());
int r = Knapsack.GenerateR(q);

List<int> publicKey = Knapsack.GenerateNormalSequence(privateKey, q, r);

Start = DateTime.Now;
List<int> encryptedMessage = Knapsack.Encrypt(message, publicKey);
End = DateTime.Now;

Console.WriteLine($"Зашифрованное сообщение: {string.Join(", ", encryptedMessage)}");
Console.WriteLine($"Время шифрования: {(End - Start).TotalMilliseconds} ms");
Console.WriteLine();

Start = DateTime.Now;
string decryptedMessage = Knapsack.Decrypt(encryptedMessage, privateKey, q, r);
End = DateTime.Now;
Console.WriteLine($"Расшифрованное сообщение: {decryptedMessage}");
Console.WriteLine($"Время расшифрования: {(End - Start).TotalMilliseconds} ms");