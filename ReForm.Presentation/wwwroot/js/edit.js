document.addEventListener('DOMContentLoaded', function () {
    const addBtn = document.getElementById('add-btn');
    const formWrapper = document.getElementById('new-question-form-wrapper');
    const cancelBtn = document.getElementById('cancel-btn');
    const saveBtn = document.getElementById('save-btn');

    // Проверка наличия кнопок
    if (!addBtn || !formWrapper) {
        console.error('Кнопка "Add" или форма не найдены!');
        return;
    }

    // 1. Скрываем форму по умолчанию
    formWrapper.classList.add('d-none');

    // 2. Обработчик клика на кнопку "Add Question"
    addBtn.addEventListener('click', function () {
        console.log('Кнопка "Add Question" нажата');
        startNewQuestion();
    });

    // 3. Обработчик на кнопку "Cancel"
    cancelBtn?.addEventListener('click', cancelNewQuestion);

    // 4. Обработчик на кнопку "Save"
    saveBtn?.addEventListener('click', function () {
        const templateId = saveBtn.dataset.templateId;
        console.log('Сохранение вопроса с templateId', templateId);
        if (!templateId) {
            alert('Отсутствует ID шаблона!');
            return;
        }
        saveNewQuestion(templateId);
    });

    // Функция для начала добавления нового вопроса
    function startNewQuestion() {
        console.log('Показ формы для нового вопроса');
        formWrapper.classList.remove('d-none');  // Показываем форму
        addBtn.classList.add('d-none');  // Скрываем кнопку "Add"
        clearFields();
    }

    // Функция отмены создания нового вопроса
    function cancelNewQuestion() {
        console.log('Отмена создания нового вопроса');
        formWrapper.classList.add('d-none');  // Скрываем форму
        addBtn.classList.remove('d-none');  // Показываем кнопку "Add"
        clearFields();
    }

    // Очистка всех полей формы
    function clearFields() {
        console.log('Очистка полей формы');
        document.getElementById('new-text').value = '';
        document.getElementById('new-type').value = '0'; // Значение по умолчанию для SingleChoice
        document.getElementById('new-mandatory').checked = false;
        document.getElementById('options-list').innerHTML = '';
        document.getElementById('rating-min').value = '';
        document.getElementById('rating-max').value = '';
        toggleNewOptions();
    }

    // Функция для отображения опций на основе типа вопроса
    function toggleNewOptions() {
        const typeValue = document.getElementById('new-type').value;
        const optionsContainer = document.getElementById('options-container');
        const ratingContainer = document.getElementById('rating-range-container');

        const needsOptions = typeValue === '0' || typeValue === '1'; // SingleChoice или MultipleChoice
        const needsRating = typeValue === '3'; // Rating

        optionsContainer.style.display = needsOptions ? 'block' : 'none';
        ratingContainer.style.display = needsRating ? 'block' : 'none';

        if (needsOptions && !document.querySelector('#options-list .option-input-group')) {
            addOption(); // Добавляем первую опцию, если необходимо
        }
    }

    // Функция добавления новой опции
    function addOption(value = '') {
        const optionsList = document.getElementById('options-list');
        const group = document.createElement('div');
        group.className = 'option-input-group';

        const icon = document.createElement('span');
        icon.className = 'option-icon';
        icon.innerHTML = '&#9633;'; // Стандартный символ для опции

        const input = document.createElement('input');
        input.type = 'text';
        input.className = 'form-control';
        input.value = value;

        const removeBtn = document.createElement('button');
        removeBtn.type = 'button';
        removeBtn.innerHTML = '&times;';
        removeBtn.onclick = () => group.remove();

        group.append(icon, input, removeBtn);
        optionsList.appendChild(group);
    }
});
