import tkinter as tk
from tkinter import ttk
import datetime
from hashlib import sha256

class App:
    def __init__(self, master):
        self.master = master
        master.title("SHA-256 Хеширование")
        master.geometry("600x350")
        master.configure(bg="#f0f0f0")

        style = ttk.Style()
        style.configure("TButton", font=("Arial", 10))
        style.configure("TLabel", font=("Arial", 10), background="#f0f0f0")

        # Ввод сообщения
        ttk.Label(master, text="Введите сообщение:").grid(row=0, column=0, padx=10, pady=5, sticky="w")
        self.textbox = tk.Text(master, height=4, width=60)
        self.textbox.grid(row=1, column=0, columnspan=2, padx=10)

        # Кнопка хеширования
        ttk.Button(master, text="Хешировать", command=self.hash_message).grid(row=2, column=0, padx=10, pady=10, sticky="w")

        # Отображение хеша
        self.result = ttk.Label(master, text="Хеш: ")
        self.result.grid(row=3, column=0, columnspan=2, padx=10, pady=5, sticky="w")

        # Отображение времени
        self.time_label = ttk.Label(master, text="")
        self.time_label.grid(row=4, column=0, columnspan=2, padx=10, sticky="w")

        
       
    def hash_message(self):
        message = self.textbox.get("1.0", "end-1c")
        start_time = datetime.datetime.now()
        hashed = sha256(message.encode()).hexdigest()
        end_time = datetime.datetime.now()
        self.result.config(text=f"Хеш: {hashed}")
        self.time_label.config(text="Время выполнения: {:.6f} мс".format((end_time - start_time).total_seconds() * 1000))

   
root = tk.Tk()
app = App(root)
root.mainloop()
