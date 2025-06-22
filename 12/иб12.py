import ipywidgets as widgets
from IPython.display import display
import time
import random
import hashlib
from sympy import randprime, isprime
import math

class DigitalSignatureApp:
    def __init__(self):
        self.public_key = None
        self.private_key = None
        self.signature = None

        self.algorithm_var = widgets.RadioButtons(
            options=["RSA", "ElGamal", "Schnorr"],
            description='Алгоритм:',
            disabled=False
        )

        self.key_size_var = widgets.Dropdown(
            options=["128", "256", "512", "1024", "2048", "3072", "4096"],
            value="1024",
            description='Битовая длина ключа:'
        )

        self.message_text = widgets.Textarea(
            description='Сообщение:',
            layout=widgets.Layout(width='100%', height='100px')
        )

        self.results_text = widgets.Output()
        self.status_var = widgets.Label(value="")

        self.generate_button = widgets.Button(description='Сгенерировать ключи')
        self.sign_button = widgets.Button(description='Подписать')
        self.verify_button = widgets.Button(description='Проверить подпись')
        self.clear_button = widgets.Button(description='Очистить')

        self.generate_button.on_click(self.generate_keys)
        self.sign_button.on_click(self.sign_message)
        self.verify_button.on_click(self.verify_signature)
        self.clear_button.on_click(self.clear_all)

        self.setup_ui()

    def setup_ui(self):
        display(self.algorithm_var)
        display(self.key_size_var)
        display(self.message_text)
        display(self.generate_button, self.sign_button, self.verify_button, self.clear_button)
        display(self.results_text)
        display(self.status_var)

    def log(self, message):
        with self.results_text:
            print(message)

    def clear_all(self, b):
        self.message_text.value = ""
        self.results_text.clear_output()
        self.public_key = None
        self.private_key = None
        self.signature = None
        self.status_var.value = ""

    def generate_keys(self, b):
        algorithm = self.algorithm_var.value
        key_size = int(self.key_size_var.value)

        try:
            start_time = time.time()
            if algorithm == "RSA":
                self.private_key, self.public_key = self.generate_rsa_keys(key_size)
            elif algorithm == "ElGamal":
                self.private_key, self.public_key = self.generate_elgamal_keys(key_size)
            elif algorithm == "Schnorr":
                self.private_key, self.public_key = self.generate_schnorr_keys(key_size)

            elapsed_time = time.time() - start_time
            self.log(f"\n[{algorithm}] Ключи сгенерированы (время: {elapsed_time:.4f} сек)")
            self.log(f"Открытый ключ: {self.public_key}")
            self.log(f"Закрытый ключ: {self.private_key}")
            self.status_var.value = f"Ключи {algorithm} сгенерированы за {elapsed_time:.4f} секунд"

        except Exception as e:
            self.log(f"Ошибка при генерации ключей: {str(e)}")

    def sign_message(self, b):
        if not self.private_key:
            self.log("Сначала сгенерируйте ключи!")
            return

        message = self.message_text.value.strip()
        if not message:
            self.log("Введите сообщение для подписи!")
            return

        algorithm = self.algorithm_var.value

        try:
            start_time = time.time()
            if algorithm == "RSA":
                self.signature = self.rsa_sign(message, self.private_key)
            elif algorithm == "ElGamal":
                self.signature = self.elgamal_sign(message, self.private_key)
            elif algorithm == "Schnorr":
                self.signature = self.schnorr_sign(message, self.private_key)

            elapsed_time = time.time() - start_time
            self.log(f"\n[{algorithm}] Сообщение подписано (время: {elapsed_time:.4f} сек)")
            self.log(f"Подпись: {self.signature}")
            self.status_var.value = f"Сообщение подписано ({algorithm}) за {elapsed_time:.4f} секунд"

        except Exception as e:
            self.log(f"Ошибка при подписании сообщения: {str(e)}")

    def verify_signature(self, b):
        if not self.public_key or not self.signature:
            self.log("Сначала сгенерируйте ключи и подпишите сообщение!")
            return

        message = self.message_text.value.strip()
        if not message:
            self.log("Введите сообщение для проверки!")
            return

        algorithm = self.algorithm_var.value

        try:
            start_time = time.time()
            if algorithm == "RSA":
                is_valid = self.rsa_verify(message, self.signature, self.public_key)
            elif algorithm == "ElGamal":
                is_valid = self.elgamal_verify(message, self.signature, self.public_key)
            elif algorithm == "Schnorr":
                is_valid = self.schnorr_verify(message, self.signature, self.public_key)

            elapsed_time = time.time() - start_time
            if is_valid:
                self.log(f"\n[{algorithm}] Подпись ВЕРНА (время проверки: {elapsed_time:.4f} сек)")
                self.status_var.value = f"Подпись верна ({algorithm}), проверка заняла {elapsed_time:.4f} секунд"
            else:
                self.log(f"\n[{algorithm}] Подпись НЕВЕРНА (время проверки: {elapsed_time:.4f} сек)")
                self.status_var.value = f"Подпись неверна ({algorithm}), проверка заняла {elapsed_time:.4f} секунд"

        except Exception as e:
            self.log(f"Ошибка при проверке подписи: {str(e)}")

    # Факторизация числа
    def factor(self, n):
        factors = set()
        while n % 2 == 0:
            factors.add(2)
            n //= 2
        for i in range(3, int(math.sqrt(n)) + 1, 2):
            while n % i == 0:
                factors.add(i)
                n //= i
        if n > 2:
            factors.add(n)
        return factors

    # RSA методы
    def generate_rsa_keys(self, key_size):
        p = randprime(2**(key_size//2 - 1), 2**(key_size//2))
        q = randprime(2**(key_size//2 - 1), 2**(key_size//2))

        n = p * q
        phi = (p - 1) * (q - 1)

        e = 65537  # Открытая экспонента
        d = pow(e, -1, phi)  # Закрытая экспонента

        public_key = (n, e)
        private_key = (n, d)

        return private_key, public_key

    def rsa_sign(self, message, private_key):
        n, d = private_key
        msg_hash = int.from_bytes(hashlib.sha256(message.encode()).digest(), byteorder='big')
        signature = pow(msg_hash, d, n)
        return signature

    def rsa_verify(self, message, signature, public_key):
        n, e = public_key
        msg_hash = int.from_bytes(hashlib.sha256(message.encode()).digest(), byteorder='big')
        decrypted_hash = pow(signature, e, n)
        return msg_hash == decrypted_hash

    # Эль-Гамаль методы
    def generate_elgamal_keys(self, key_size):
      p = randprime(2**(key_size - 1), 2**key_size)
      x = random.randint(1, p-2)  # Private key
      g = 2  # Commonly used base
      y = pow(g, x, p)  # Public key component
      public_key = (p, g, y)
      private_key = (p, g, x)
      return private_key, public_key

    def elgamal_sign(self, message, private_key):
        p, g, x = private_key
        msg_hash = int.from_bytes(hashlib.sha256(message.encode()).digest(), byteorder='big') % p

        while True:
            k = random.randint(1, p-2)
            if math.gcd(k, p-1) == 1:
                break

        r = pow(g, k, p)
        k_inv = pow(k, -1, p-1)
        s = (msg_hash - x * r) * k_inv % (p-1)

        return (r, s)

    def elgamal_verify(self, message, signature, public_key):
        p, g, y = public_key
        r, s = signature
        msg_hash = int.from_bytes(hashlib.sha256(message.encode()).digest(), byteorder='big') % p
        left = (pow(y, r, p) * pow(r, s, p)) % p
        right = pow(g, msg_hash, p)
        return left == right

    # Шнорр методы
    def generate_schnorr_keys(self, key_size):
        while True:
            q = randprime(2**(key_size//2 - 1), 2**(key_size//2))
            k = random.randint(2**(key_size - key_size//2 - 1), 2**(key_size - key_size//2))
            p = k * q + 1
            if isprime(p):
                break

        h = random.randint(2, p-2)
        g = pow(h, (p-1)//q, p)

        x = random.randint(1, q-1)
        y = pow(g, x, p)

        public_key = (p, q, g, y)
        private_key = (p, q, g, x)

        return private_key, public_key

    def schnorr_sign(self, message, private_key):
        p, q, g, x = private_key
        k = random.randint(1, q-1)
        r = pow(g, k, p)
        e = int.from_bytes(hashlib.sha256((message + str(r)).encode()).digest(), byteorder='big') % q
        s = (k + x * e) % q
        return (e, s)

    def schnorr_verify(self, message, signature, public_key):
        p, q, g, y = public_key
        e, s = signature
        g_s = pow(g, s, p)
        y_inv_e = pow(y, -e, p)
        r_prime = (g_s * y_inv_e) % p
        e_prime = int.from_bytes(hashlib.sha256((message + str(r_prime)).encode()).digest(), byteorder='big') % q
        return e == e_prime

# Создание приложения
app = DigitalSignatureApp()