// document.getElementById('add-movie-form').addEventListener('submit', async function (event) {
//     event.preventDefault(); // Предотвращаем стандартную отправку формы
//
//     const form = event.target;
//
//     // Формируем данные из формы
//     const formData = new FormData(form);
//     const data = Object.fromEntries(formData.entries());
//
//     try {
//         // Выполняем AJAX-запрос
//         const response = await fetch(form.action, {
//             method: form.method,
//             headers: {
//                 'Content-Type': 'application/json',
//             },
//             body: JSON.stringify(data),
//         });
//         console.log(111)
//         if (!response.ok) {
//             throw new Error('Ошибка при отправке данных');
//         }
//
//         const result = await response.json();
//         if (result == false){
//             alert('Такой фильм уже есть');
//             throw new Error('Такой фильм уже есть');
//         }
//         console.log(result)
//         // Создаем новую строку таблицы
//         const newRow = document.createElement('tr');
//         newRow.innerHTML = `
// <td>${result.MovieId}</td>
//     <td>${result.TitleRu}</td>
//     <td>${result.TitleEng}</td>
//     <td>${result.ReleaseDateWorld}</td>
//     <td>${result.ReleaseDateRu}</td>
// `;
//
//         // Находим таблицу и добавляем новую строку
//         const table = document.getElementById('movies-table');
//         table.appendChild(newRow);
//
//     } catch (error) {
//         console.error('Ошибка при добавлении фильма:', error);
//     }
// });