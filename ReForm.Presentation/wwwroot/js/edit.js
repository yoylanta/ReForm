const questionType = {
    SingleChoice: 0,
    MultipleChoice: 1,
    Text: 2,
    Rating: 3,
    Date: 4
};

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

    window.toggleNewOptions = toggleNewOptions;

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

    window.addOption = addOption;

    async function saveNewQuestion(templateId) {
        const text = document.getElementById('new-text').value.trim();
        const type = parseInt(document.getElementById('new-type').value);
        const isMandatory = document.getElementById('new-mandatory').checked;

        if (!text) {
            alert('Question text is required');
            return;
        }

        let options = [];
        if (type === questionType.SingleChoice || type === questionType.MultipleChoice) {
            const inputs = document.querySelectorAll('#options-list input');
            options = Array.from(inputs)
                .map(input => input.value.trim())
                .filter(value => value);
            if (options.length === 0) {
                alert('Please add at least one option.');
                return;
            }
        }

        const payload = {
            text: text,
            type: type,
            isMandatory: isMandatory,
            options: options.join(','),
            templateFormId: templateId
        };

        try {
            const res = await fetch('/api/template/question/add', {
                method: 'POST',
                headers: {'Content-Type': 'application/json'},
                body: JSON.stringify(payload)
            });

            if (res.ok) {
                location.reload();
            } else {
                const err = await res.text();
                alert(`Could not add question:\n${err}`);
            }
        } catch (error) {
            alert('An error occurred while adding the question.');
        }
    }
});

document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.delete-btn');

    deleteButtons.forEach(button => {
        button.addEventListener('click', async () => {
            const questionId = button.getAttribute('data-id');

            const confirmed = await Swal.fire({
                icon: 'warning',
                title: 'Are you sure?',
                text: `Are you sure you want to delete this question?`,
                showCancelButton: true,
                confirmButtonText: 'Yes, delete it!',
                cancelButtonText: 'No, cancel!',
            });

            if (confirmed.isConfirmed) {
                try {
                    const response = await fetch(`/api/template/question/delete/${encodeURIComponent(questionId)}`, {
                        method: 'DELETE',
                        headers: {
                            'Content-Type': 'application/json',
                        }
                    });

                    if (response.ok) {
                        Swal.fire({
                            icon: 'success',
                            title: 'Deleted!',
                            text: `Successfully deleted question with ID: ${questionId}`,
                        }).then(() => location.reload());
                    } else {
                        Swal.fire({
                            icon: 'error',
                            title: 'Failed to delete',
                            text: 'An error occurred while deleting the question.',
                        });
                    }
                } catch (error) {
                    console.error(error);
                    Swal.fire({
                        icon: 'error',
                        title: 'An error occurred',
                        text: 'Failed to delete the question due to an error.',
                    });
                }
            }
        });
    });
});