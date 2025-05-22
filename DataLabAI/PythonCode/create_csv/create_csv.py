import csv
import random
import time
from datetime import datetime, timedelta

# Настройки генератора
RECORDS = 100  # Количество записей
FILE_NAME = f'D:/.NET/projects/DataLabAI/DataLabAI/DataLabAI/PythonCode/create_csv/sales_data_{time.strftime("%Y%m%d-%H%M%S")}.csv' # создание файла с временной меткой

# Списки для случайной генерации данных
products = ['Ноутбук', 'Смартфон', 'Планшет', 'Наушники', 'Флешка', 'Клавиатура', 'Монитор']
categories = ['Электроника', 'Гаджеты', 'Комплектующие', 'Аксессуары']
regions = ['Москва', 'СПб', 'Казань', 'Новосибирск', 'Екатеринбург']

def random_date(start_date, end_date):
    """Генерация случайной даты между двумя датами"""
    time_between = end_date - start_date
    random_days = random.randrange(time_between.days)
    return start_date + timedelta(days=random_days)

# Генерация данных
data = []
start_date = datetime(2023, 1, 1)
end_date = datetime(2023, 12, 31)

for i in range(1, RECORDS + 1):
    product = random.choice(products)
    category = random.choice(categories)
    region = random.choice(regions)
    
    record = {
        'sale_id': i,
        'date': random_date(start_date, end_date).strftime('%Y-%m-%d'),
        'product': product,
        'category': category,
        'price': round(random.uniform(1000, 100000), 2),
        'quantity': random.randint(1, 5),
        'region': region
    }
    data.append(record)

# Запись в CSV
with open(FILE_NAME, 'w', newline='', encoding='utf-8') as csvfile:
    fieldnames = ['sale_id', 'date', 'product', 'category', 'price', 'quantity', 'region']
    writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
    
    writer.writeheader()
    writer.writerows(data)

print(f'Файл {FILE_NAME} успешно создан с {RECORDS} записями')