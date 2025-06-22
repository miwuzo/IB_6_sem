# Генерация ПСП Вариант 2 RSA p, q, e – 256-разрядные числа
from prs import *
from rc4 import *
from datetime import datetime

print('--------------- ПСП ---------------')
print('Введите длину последовательности t:')
t = int(input())
start_time = datetime.now()
print(start_time)
prs = generate_rsa_prs(t)
print('\nПСП:', prs)
print(f'Время генерации ПСП RSA: {datetime.now() - start_time}')

print()
