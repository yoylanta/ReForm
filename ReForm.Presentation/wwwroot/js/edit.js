const questionType = {
    SingleChoice: 0,
    MultipleChoice: 1,
    Text: 2,
    Rating: 3,
    Date: 4
};

function startNewQuestion() {
    document.getElementById("add-question-card").classList.add("d-none");
    document.getElementById("new-question-form").classList.remove("d-none");

    clearFields();
}

function cancelNewQuestion() {
    document.getElementById("add-question-card").classList.remove("d-none");
    document.getElementById("new-question-form").classList.add("d-none");

    clearFields();
}

function clearFields() {
    document.getElementById('new-text').value = '';
    document.getElementById('new-type').value = questionType.SingleChoice;
    document.getElementById('new-mandatory').checked = false;
    document.getElementById('options-list').innerHTML = '';
    toggleNewOptions();
}

function toggleNewOptions() {
    const typeValue = parseInt(document.getElementById("new-type").value);
    const optionsContainer = document.getElementById("options-container");
    const needsOptions = typeValue === questionType.SingleChoice || typeValue === questionType.MultipleChoice;

    optionsContainer.style.display = needsOptions ? 'block' : 'none';

    if (needsOptions && document.getElementById('options-list').children.length === 0) {
        addOption();
    }
}

function addOption(value = '') {
    const typeValue = parseInt(document.getElementById("new-type").value);
    const optionsList = document.getElementById('options-list');

    const optionGroup = document.createElement('div');
    optionGroup.className = 'option-input-group';

    const icon = document.createElement('span');
    icon.className = 'option-icon';
    icon.innerHTML = typeValue === questionType.SingleChoice ? '&#9675;' : '&#9633;';

    const input = document.createElement('input');
    input.type = 'text';
    input.className = 'form-control';
    input.value = value;

    const removeBtn = document.createElement('button');
    removeBtn.type = 'button';
    removeBtn.innerHTML = '&times;';
    removeBtn.onclick = () => optionGroup.remove();

    optionGroup.appendChild(icon);
    optionGroup.appendChild(input);
    optionGroup.appendChild(removeBtn);

    optionsList.appendChild(optionGroup);
}

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
