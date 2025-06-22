import random

def is_prime(n, k=5):
    if n <= 3:
        return n == 2 or n == 3
    elif n % 2 == 0:
        return False

    r, s = 0, n - 1
    while s % 2 == 0:
        r += 1
        s //= 2

    for _ in range(k):
        a = random.randrange(2, n - 1)
        x = pow(a, s, n)
        if x == 1 or x == n - 1:
            continue
        for _ in range(r - 1):
            x = pow(x, 2, n)
            if x == n - 1:
                break
        else:
            return False

    return True


def generate_prime(bit_length):
    while True:
        p = random.getrandbits(bit_length)
        if is_prime(p):
            return p

def generate_rsa_prs(t):
    seq = ''

    # Генерация двух 256-разрядных простых чисел
    p = generate_prime(256)
    q = generate_prime(256)

    print("\np: ", p)
    print("q: ", q)

    # Вычисление произведения p и q
    n = p * q
    print("n: ", n)

    # Выбор открытого ключа e
    phi = (p - 1) * (q - 1)
    e = generate_coprime(phi)
    print("e: ", e)

    x0 = random.randint(0, n - 1)
    print("x0: ", x0)

    x_t = pow(x0, e, n)

    for i in range(t):
        x_t = pow(x_t, e, n)
        seq += str(x_t % 2)
    return seq

def extended_gcd(a, b):
    if b == 0:
        return a, 1, 0
    gcd, x1, y1 = extended_gcd(b, a % b)
    x = y1
    y = x1 - (a // b) * y1
    return gcd, x, y

def generate_coprime(phi):
    while True:
        e = random.randrange(2, phi)
        gcd, x, y = extended_gcd(e, phi)
        if gcd == 1:
            return e