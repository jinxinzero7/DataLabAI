import pandas as pd
import json
import sys

def main():
    try:
        # Получаем аргументы командной строки
        file_path = sys.argv[1]
        delimiter = sys.argv[2] if len(sys.argv) > 2 else ',' # Обработка опционального разделителя

        try:
            data = pd.read_csv(file_path, delimiter=delimiter)
        except Exception as e:
            raise ValueError(f"Ошибка при чтении файла CSV: {e}")

        statistics = {}
        for column in data.columns:
            try:
                column_data = data[column]
                stats = {
                    "count": column_data.count(),
                    "mean": column_data.mean(),
                    "median": column_data.median(),
                    "std": column_data.std(),
                    "min": column_data.min(),
                    "max": column_data.max(),
                    "nunique": column_data.nunique(),
                    "isnull_sum": column_data.isnull().sum(),
                    "dtype": str(column_data.dtype)  # Преобразуем тип данных в строку для JSON
                }
                statistics[column] = stats
            except Exception as e:
                statistics[column] = {"error": str(e)} # Записываем ошибку, если не удалось вычислить статистику

        with open("results.json", "w", encoding='utf-8') as f: # Указываем кодировку для поддержки кириллицы
            json.dump(statistics, f, indent=4, ensure_ascii=False) # ensure_ascii=False для поддержки кириллицы

    except Exception as e:
        error_message = {"error": str(e)}
        with open("results.json", "w", encoding='utf-8') as f:  # Указываем кодировку
            json.dump(error_message, f, indent=4, ensure_ascii=False) # Поддержка кириллицы
        sys.exit(1)

if __name__ == "__main__":
    main()