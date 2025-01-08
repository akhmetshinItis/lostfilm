document.getElementById('add-movie-form').addEventListener('submit', async function (event) {
    event.preventDefault(); // Предотвращаем стандартную отправку формы

    const form = event.target;

    // Формируем данные из формы
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());

    try {
        // Выполняем AJAX-запрос
        const response = await fetch(form.action, {
            method: form.method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });
        console.log(111)
        if (!response.ok) {
            throw new Error('Ошибка при отправке данных');
        }

        const result = await response.json();
        if (result == false){
            alert('Такой фильм уже есть');
            throw new Error('Такой фильм уже есть');
        }
        console.log(result)
        // Создаем новую строку таблицы
        const newRow = document.createElement('tr');
        newRow.innerHTML += `
            <tr>
                <td>${result.Id}</td>
                <td>${result.TitleRu}</td>
                <td>${result.TitleEng}</td>
                <td>${result.ReleaseDateWorld}</td>
                <td>${result.ReleaseDateRu}</td>
            </tr>
        `;
        // Находим таблицу и добавляем новую строку
        const table = document.getElementById('movies-table');
        table.appendChild(newRow);
        form.reset();

    } catch (error) {
        console.error('Ошибка при добавлении фильма:', error);
    }
});


document.getElementById('add-movie-detail-form').addEventListener('submit', async function (event) {
    event.preventDefault(); // Предотвращаем стандартную отправку формы

    const form = event.target;

    // Формируем данные из формы
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());
    data.MovieId = parseInt(data.MovieId); // Преобразуем MovieId в число
    data.RatingIMDb = parseFloat(data.RatingIMDb); // Преобразуем RatingIMDb в число с плавающей точкой
    data.Duration = parseInt(data.Duration); // Преобразуем Duration в число
    try {
        // Выполняем AJAX-запрос
        const response = await fetch(form.action, {
            method: form.method,
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Ошибка при отправке данных');
        }

        const result = await response.json();
        if (result == false) {
            alert('Такие детали уже существуют');
            throw new Error('Такие детали уже существуют');
        }

        // Создаем новую строку таблицы
        const newRow = document.createElement('tr');
        newRow.innerHTML += `
            <tr>
                <td>${result.Id}</td>
                <td>${result.MovieId}</td>
                <td>${result.RatingIMDb}</td>
                <td>${result.Duration}</td>
                <td>${result.Type}</td>
                <td>${result.Description}</td>
                <td>${result.ImageUrl}</td>
            </tr>
        `;

        // Находим таблицу и добавляем новую строку
        const table = document.getElementById('movie-details-table');
        table.appendChild(newRow);
        form.reset();

    } catch (error) {
        console.error('Ошибка при добавлении деталей фильма:', error);
    }
});


document.getElementById('add-genre-form').addEventListener('submit', async function (event) {
    event.preventDefault(); // Предотвращаем стандартную отправку формы

    const form = event.target;

    // Формируем данные из формы
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());
    data.UsageCount = parseInt(data.UsageCount); // Преобразуем Usage Count в число
    console.log(data)

    try {
        // Выполняем AJAX-запрос
        const response = await fetch('/admin/genres/add', { // Укажите правильный URL для обработки запроса
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Ошибка при отправке данных');
        }

        const result = await response.json();
        if (result == false) {
            alert('Такой жанр уже существует');
            throw new Error('Такой жанр уже существует');
        }

        // Создаем новую строку таблицы
        const newRow = document.createElement('tr');
        newRow.innerHTML += `
            <tr>
                <td>${result.Id}</td>
                <td>${result.Name}</td>
                <td>${result.Description}</td>
                <td>${result.UsageCount}</td>
            </tr>
        `;

        // Находим таблицу и добавляем новую строку
        const table = document.getElementById('genres-table'); // Убедитесь, что таблица с таким ID существует
        table.appendChild(newRow);

        // Очищаем форму после успешной отправки
        form.reset();

    } catch (error) {
        console.error('Ошибка при добавлении жанра:', error);
    }
});

document.getElementById('add-movie-genre-form').addEventListener('submit', async function (event) {
    event.preventDefault(); // Предотвращаем стандартную отправку формы

    const form = event.target;

    // Формируем данные из формы
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());
    data.MovieId = parseInt(data.MovieId);
    data.GenreId = parseInt(data.GenreId);
    console.log(data)

    try {
        // Выполняем AJAX-запрос
        const response = await fetch('/admin/moviesgenres/add', { // Укажите правильный URL для обработки запроса
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Ошибка при отправке данных');
        }

        const result = await response.json();
        if (result == false) {
            alert('Такая связь уже существует');
            throw new Error('Такая связь уже существует');
        }

        // Создаем новую строку таблицы
        const newRow = document.createElement('tr');
        newRow.innerHTML += `
            <tr>
                <td>${result.Id}</td>
                <td>${result.MovieId}</td>
                <td>${result.GenreId}</td>
                <td>${result.AddedDate}</td>
            </tr>
        `;

        // Находим таблицу и добавляем новую строку
        const table = document.getElementById('movie-genres-table'); // Убедитесь, что таблица с таким ID существует
        table.appendChild(newRow);

        // Очищаем форму после успешной отправки
        form.reset();

    } catch (error) {
        console.error('Ошибка при добавлении связи:', error);
    }
});


document.getElementById('add-user-form').addEventListener('submit', async function (event) {
    event.preventDefault(); // Предотвращаем стандартную отправку формы

    const form = event.target;

    // Формируем данные из формы
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());
    console.log(data)

    try {
        // Выполняем AJAX-запрос
        const response = await fetch('admin/users/add', { // Укажите правильный URL для обработки запроса
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            throw new Error('Ошибка при отправке данных');
        }

        const result = await response.json();
        if (result == false) {
            alert('Такой пользователь уже существует');
            throw new Error('Такой пользователь уже существует');
        }

        // Создаем новую строку таблицы
        const newRow = document.createElement('tr');
        newRow.innerHTML += `
            <tr>
                <td>${result.Id}</td>
                <td>${result.Login}</td>
                <td>${result.Username}</td>
                <td>${result.Password}</td>
            </tr>
        `;

        // Находим таблицу и добавляем новую строку
        const table = document.getElementById('users-table'); // Убедитесь, что таблица с таким ID существует
        table.appendChild(newRow);

        // Очищаем форму после успешной отправки
        form.reset();

    } catch (error) {
        console.error('Ошибка при добавлении связи:', error);
    }
});
