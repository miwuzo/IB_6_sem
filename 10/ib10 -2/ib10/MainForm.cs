
using System;
using System.Text;
using System.Security.Cryptography;
using System.Windows.Forms;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.Engines;

namespace CryptoApp
{
	public class MainForm : Form
	{
		private RSAParameters rsaPrivateKey;
		private RSAParameters rsaPublicKey;
		private ElGamalParameters elgamalParams;
		private ElGamalPrivateKeyParameters elgamalPrivateKey;
		private ElGamalPublicKeyParameters elgamalPublicKey;

		private string plainText = "Чёрная Яна Руслановна";

		public MainForm()
		{
			InitUI();
		}

		private void InitUI()
		{
			Text = "Криптографическое приложение";
			Width = 800;
			Height = 600;

			var tabControl = new TabControl { Dock = DockStyle.Fill };
			var rsaTab = new TabPage("RSA");
			var elgamalTab = new TabPage("Эль-Гамаль");
			var compareTab = new TabPage("Сравнение");

			tabControl.TabPages.Add(rsaTab);
			tabControl.TabPages.Add(elgamalTab);
			tabControl.TabPages.Add(compareTab);
			Controls.Add(tabControl);

			InitRsaTab(rsaTab);
			InitElGamalTab(elgamalTab);
			InitCompareTab(compareTab);
		}

		private void InitRsaTab(TabPage tab)
		{
			var generateButton = new Button { Text = "Сгенерировать ключи RSA", Top = 10, Left = 10 };
			var publicKeyBox = new TextBox { Multiline = true, Width = 700, Height = 100, Top = 40, Left = 10 };
			var privateKeyBox = new TextBox { Multiline = true, Width = 700, Height = 100, Top = 150, Left = 10 };
			var inputTextBox = new TextBox { Text = plainText, Width = 700, Top = 260, Left = 10 };
			var encryptButton = new Button { Text = "Зашифровать", Top = 290, Left = 10 };
			var encryptedTextBox = new TextBox { Multiline = true, Width = 700, Height = 60, Top = 320, Left = 10 };
			var decryptButton = new Button { Text = "Расшифровать", Top = 390, Left = 10 };
			var decryptedTextBox = new TextBox { Width = 700, Top = 420, Left = 10 };
			var encodingLabel = new Label { Text = "Кодировка:", Top = 10, Left = 200 };
			var encodingBox = new ComboBox { Top = 10, Left = 280, Width = 100 };
			encodingBox.Items.AddRange(new[] { "      Base64", "      ASCII" });
			encodingBox.SelectedIndex = 0;

			generateButton.Click += (_, _) =>
			{
				using var rsa = new RSACryptoServiceProvider(2048);
				rsaPrivateKey = rsa.ExportParameters(true);
				rsaPublicKey = rsa.ExportParameters(false);
				publicKeyBox.Text = Convert.ToBase64String(rsa.ExportRSAPublicKey());
				privateKeyBox.Text = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
			};

			encryptButton.Click += (_, _) =>
			{
				using var rsa = new RSACryptoServiceProvider();
				rsa.ImportParameters(rsaPublicKey);
				var data = Encoding.UTF8.GetBytes(inputTextBox.Text);
				var encrypted = rsa.Encrypt(data, false);
				encryptedTextBox.Text = Convert.ToBase64String(encrypted);
			};

			decryptButton.Click += (_, _) =>
			{
				using var rsa = new RSACryptoServiceProvider();
				rsa.ImportParameters(rsaPrivateKey);
				var encrypted = Convert.FromBase64String(encryptedTextBox.Text);
				var decrypted = rsa.Decrypt(encrypted, false);
				decryptedTextBox.Text = Encoding.UTF8.GetString(decrypted);
			};

			tab.Controls.AddRange(new Control[]
			{
				generateButton, publicKeyBox, privateKeyBox, inputTextBox,
				encryptButton, encryptedTextBox, decryptButton, decryptedTextBox,
				encodingLabel, encodingBox
			});
		}

		private void InitElGamalTab(TabPage tab)
		{
			var generateButton = new Button { Text = "Сгенерировать ключи Эль-Гамаля", Top = 10, Left = 10 };
			var publicKeyBox = new TextBox { Multiline = true, Width = 700, Height = 100, Top = 40, Left = 10 };
			var privateKeyBox = new TextBox { Multiline = true, Width = 700, Height = 50, Top = 150, Left = 10 };
			var inputTextBox = new TextBox { Text = plainText, Width = 700, Top = 210, Left = 10 };
			var encryptButton = new Button { Text = "Зашифровать", Top = 240, Left = 10 };
			var encryptedTextBox = new TextBox { Multiline = true, Width = 700, Height = 60, Top = 270, Left = 10 };
			var decryptButton = new Button { Text = "Расшифровать", Top = 340, Left = 10 };
			var decryptedTextBox = new TextBox { Width = 700, Top = 370, Left = 10 };
			var encodingLabel = new Label { Text = "Кодировка:", Top = 10, Left = 200 };
			var encodingBox = new ComboBox { Top = 10, Left = 280, Width = 100 };
			encodingBox.Items.AddRange(new[] { "      Base64", "      ASCII" });
			encodingBox.SelectedIndex = 0;

			generateButton.Click += (_, _) =>
			{
				var generator = new ElGamalParametersGenerator();
				generator.Init(512, 20, new SecureRandom());
				elgamalParams = generator.GenerateParameters();

				var keyGen = new ElGamalKeyPairGenerator();
				keyGen.Init(new ElGamalKeyGenerationParameters(new SecureRandom(), elgamalParams));
				var keyPair = keyGen.GenerateKeyPair();

				elgamalPublicKey = (ElGamalPublicKeyParameters)keyPair.Public;
				elgamalPrivateKey = (ElGamalPrivateKeyParameters)keyPair.Private;

				publicKeyBox.Text = $"P: {elgamalParams.P}\nG: {elgamalParams.G}\nY: {elgamalPublicKey.Y}";
				privateKeyBox.Text = $"X: {elgamalPrivateKey.X}";
			};

			encryptButton.Click += (_, _) =>
			{
				var data = Encoding.UTF8.GetBytes(inputTextBox.Text);

				// Получаем максимальный допустимый размер блока
				var maxLength = (elgamalParams.P.BitLength - 1) / 8;
				if (data.Length > maxLength)
				{
					MessageBox.Show($"Слишком длинный ввод для ElGamal ({data.Length} байт). Максимум: {maxLength} байт.");
					return;
				}

				var engine = new ElGamalEngine();
				engine.Init(true, elgamalPublicKey);
				var cipher = engine.ProcessBlock(data, 0, data.Length);

				// Кодировка по выбору пользователя
				if (encodingBox.SelectedItem?.ToString()?.Trim() == "ASCII")
				{
					// Преобразуем байты в HEX строку (ASCII-совместимую)
					encryptedTextBox.Text = BitConverter.ToString(cipher).Replace("-", "");
				}
				else // Base64 по умолчанию
				{
					encryptedTextBox.Text = Convert.ToBase64String(cipher);
				}
			};



			decryptButton.Click += (_, _) =>
			{
				byte[] cipher;
				if (encodingBox.SelectedItem?.ToString()?.Trim() == "ASCII")
				{
					// Декодируем HEX строку обратно в байты
					string hex = encryptedTextBox.Text;
					if (hex.Length % 2 != 0)
					{
						MessageBox.Show("Неверная HEX-строка");
						return;
					}
					int len = hex.Length / 2;
					cipher = new byte[len];
					for (int i = 0; i < len; i++)
					{
						cipher[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
					}
				}
				else // Base64
				{
					cipher = Convert.FromBase64String(encryptedTextBox.Text);
				}

				var engine = new ElGamalEngine();
				engine.Init(false, elgamalPrivateKey);
				var decrypted = engine.ProcessBlock(cipher, 0, cipher.Length);
				decryptedTextBox.Text = Encoding.UTF8.GetString(decrypted);
			};



			tab.Controls.AddRange(new Control[]
			{
				generateButton, publicKeyBox, privateKeyBox, inputTextBox,
				encryptButton, encryptedTextBox, decryptButton, decryptedTextBox,
				encodingLabel, encodingBox
			});
		}

		private void InitCompareTab(TabPage tab)
		{
			var compareButton = new Button { Text = "Сравнить производительность", Top = 10, Left = 10 };
			var resultBox = new TextBox { Multiline = true, Width = 700, Height = 400, Top = 40, Left = 10 };

			compareButton.Click += (_, _) =>
			{
				var report = "";
				var plainBytes = Encoding.UTF8.GetBytes(plainText);

				var rsa = new RSACryptoServiceProvider();
				rsa.ImportParameters(rsaPublicKey);
				var rsaEncStart = DateTime.Now;
				var enc = rsa.Encrypt(plainBytes, false);
				var rsaEncTime = (DateTime.Now - rsaEncStart).TotalMilliseconds;

				rsa.ImportParameters(rsaPrivateKey);
				var rsaDecStart = DateTime.Now;
				var dec = rsa.Decrypt(enc, false);
				var rsaDecTime = (DateTime.Now - rsaDecStart).TotalMilliseconds;

				report += $"RSA Шифрование: {rsaEncTime} мс\nRSA Дешифрование: {rsaDecTime} мс\n\n";

				var engine = new ElGamalEngine();
				engine.Init(true, elgamalPublicKey);
				var elEncStart = DateTime.Now;
				var elEnc = engine.ProcessBlock(plainBytes, 0, plainBytes.Length);
				var elEncTime = (DateTime.Now - elEncStart).TotalMilliseconds;

				engine.Init(false, elgamalPrivateKey);
				var elDecStart = DateTime.Now;
				var elDec = engine.ProcessBlock(elEnc, 0, elEnc.Length);
				var elDecTime = (DateTime.Now - elDecStart).TotalMilliseconds;

				report += $"Эль-Гамаль Шифрование: {elEncTime} мс\nЭль-Гамаль Дешифрование: {elDecTime} мс";
				resultBox.Text = report;
			};

			tab.Controls.Add(compareButton);
			tab.Controls.Add(resultBox);
		}
	}
}
