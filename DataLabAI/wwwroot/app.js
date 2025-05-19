// app.js
const form = document.getElementById('uploadForm');
const resultsDiv = document.getElementById('results');

form.addEventListener('submit', async (e) => {
    e.preventDefault();
    resultsDiv.innerHTML = 'Загрузка...'; // Показывать сообщение о загрузке
    const fileInput = document.getElementById('fileInput');
    const file = fileInput.files[0];
    const delimiterInput = document.getElementById('delimiter');
    const delimiter = delimiterInput.value;

    const formData = new FormData();
    formData.append('file', file);

    try {
        const url = `https://localhost:7285/File/upload?delimiter=${delimiter}`; // Добавляем разделитель в URL
        const response = await fetch(url, {
            method: 'POST',
            body: formData
        });

        if (response.ok) {
            const data = await response.json();
            displayResults(data); // Функция для отображения результатов
        } else {
            resultsDiv.innerHTML = `Ошибка: ${response.status}`;
            console.error('Upload failed:', response.status);
        }
    } catch (error) {
        resultsDiv.innerHTML = `Ошибка: ${error}`;
        console.error('Error uploading:', error);
    }
});

function displayResults(data) {
    resultsDiv.innerHTML = ''; // Очищаем предыдущие результаты

    if (data.hasOwnProperty('error')) {
        resultsDiv.innerHTML = `<p>Ошибка: ${data.error}</p>`;
        return;
    }

    const table = document.createElement('table');
    table.border = '1'; // Для простого отображения

    // Создаем заголовок таблицы
    const thead = document.createElement('thead');
    const headerRow = document.createElement('tr');
    const columnHeader = document.createElement('th');
    columnHeader.textContent = 'Столбец';
    headerRow.appendChild(columnHeader);

    const statsHeader = document.createElement('th');
    statsHeader.textContent = 'Статистика';
    headerRow.appendChild(statsHeader);

    thead.appendChild(headerRow);
    table.appendChild(thead);

    const tbody = document.createElement('tbody');

    for (const column in data) {
        if (data.hasOwnProperty(column)) {
            const columnStats = data[column];
            const row = document.createElement('tr');

            // Ячейка для названия столбца
            const columnNameCell = document.createElement('td');
            columnNameCell.textContent = column;
            row.appendChild(columnNameCell);

            // Ячейка для статистик
            const statsCell = document.createElement('td');
            let statsHTML = '';
            for (const stat in columnStats) {
                if (columnStats.hasOwnProperty(stat)) {
                    statsHTML += `<b>${stat}:</b> ${columnStats[stat]}<br>`;
                }
            }
            statsCell.innerHTML = statsHTML;
            row.appendChild(statsCell);

            tbody.appendChild(row);
        }
    }
    table.appendChild(tbody);

    resultsDiv.appendChild(table);
}