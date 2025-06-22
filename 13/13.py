import random

def encode_message(container_file, message_file, stego_file):
    """
    Кодирует сообщение в текстовый контейнер, используя метод переменной длины слов и модификации апроша.

    Args:
        container_file (str): Путь к файлу с текстом-контейнером.
        message_file (str): Путь к файлу с сообщением, которое нужно скрыть.
        stego_file (str): Путь к файлу, куда будет сохранен стеготекст.
    """

    try:
        with open(container_file, 'r', encoding='utf-8') as f:
            container_text = f.read()

        with open(message_file, 'r', encoding='utf-8') as f:
            message = f.read()

    except FileNotFoundError as e:
        print(f"Ошибка: Файл не найден: {e}")
        return
    except Exception as e:
        print(f"Ошибка при чтении файлов: {e}")
        return

    # 1. Подготовка сообщения: Перевод в биты
    binary_message = ''.join(format(ord(char), '08b') for char in message)

    # 2. Разбиение контейнера на слова
    words = container_text.split()
    num_words = len(words)
    binary_message_len = len(binary_message)

    # Проверка на достаточность контейнера
    if num_words < binary_message_len:
        print("Ошибка: Текст-контейнер недостаточно велик для скрытия сообщения.")
        return

    # 3. Кодирование: Модификация апроша (межсловного пробела)
    stego_words = []
    bit_index = 0
    for word in words:
        stego_words.append(word)
        if bit_index < binary_message_len:
            bit = binary_message[bit_index]
            if bit == '1':
                # Добавляем два пробела (большой апрош)
                stego_words.append("  ")
            else:
                # Добавляем один пробел (нормальный апрош)
                stego_words.append(" ")
            bit_index += 1
        else:
            # Добавляем обычный пробел после оставшихся слов
            stego_words.append(" ")

    # 4. Формирование стеготекста
    stego_text = "".join(stego_words)

    # 5. Запись стеготекста в файл
    try:
        with open(stego_file, 'w', encoding='utf-8') as f:
            f.write(stego_text)
        print(f"Сообщение успешно закодировано и сохранено в {stego_file}")

    except Exception as e:
        print(f"Ошибка при записи в файл: {e}")
        return



def decode_message(stego_file):
    """
    Извлекает скрытое сообщение из стеготекста.

    Args:
        stego_file (str): Путь к файлу со стеготекстом.

    Returns:
        str: Извлеченное сообщение, или None в случае ошибки.
    """
    try:
        with open(stego_file, 'r', encoding='utf-8') as f:
            stego_text = f.read()

    except FileNotFoundError as e:
        print(f"Ошибка: Файл не найден: {e}")
        return None
    except Exception as e:
        print(f"Ошибка при чтении файла: {e}")
        return None

    # 1. Анализ пробелов
    binary_message = ""
    words = stego_text.split()
    for i in range(len(words) -1):
        if words[i+1] == "":
           binary_message += '1'
           words = words[:i+1] + words[i+2:]
        elif words[i+1] == " ":
           binary_message += '0'
           words = words[:i+1] + words[i+2:]

    # 2. Преобразование битов в символы
    decoded_message = ""
    for i in range(0, len(binary_message), 8):
        byte = binary_message[i:i + 8]
        if len(byte) == 8:  # Обрабатываем только полные байты
            try:
                char_code = int(byte, 2)
                decoded_message += chr(char_code)
            except ValueError:
                print(f"Ошибка: Некорректная битовая последовательность: {byte}")
                return None

    return decoded_message

# Пример использования:
if __name__ == "__main__":
    container_file = "container.txt"  # Файл с текстом-контейнером
    message_file = "message.txt"  # Файл с сообщением, которое нужно скрыть
    stego_file = "stego.txt"  # Файл, куда будет сохранен стеготекст

    # Создадим файлы (для примера)
    with open(container_file, 'w', encoding='utf-8') as f:
        f.write("Это пример текста, который будет использован в качестве контейнера.  Нужно достаточно слов для успешного скрытия сообщения.  Этот текст должен быть достаточно длинным.")

    with open(message_file, 'w', encoding='utf-8') as f:
        f.write("Секретное сообщение!")


    encode_message(container_file, message_file, stego_file)

    # Декодирование
    decoded_message = decode_message(stego_file)

    if decoded_message:
        print("Извлеченное сообщение:", decoded_message)
    else:
        print("Не удалось извлечь сообщение.")