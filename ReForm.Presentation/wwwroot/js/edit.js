document.addEventListener("DOMContentLoaded", () => {
    MicroModal.init({
        onClose: () => {
            clearModalFields();
        }
    });
});

function clearModalFields() {
    document.getElementById('question-text').value = '';
    document.getElementById('question-type').value = 'SingleChoice';
    document.getElementById('is-mandatory').checked = false;
    document.getElementById('question-options').value = '';
    document.getElementById('options-container').style.display = 'block'; // for default type
    toggleOptionsInput(); // apply visibility rules again
}

function toggleOptionsInput() {
    const type = document.getElementById("question-type").value;
    const optionsContainer = document.getElementById("options-container");

    if (type === "SingleChoice" || type === "MultipleChoice") {
        optionsContainer.style.display = "block";
    } else {
        optionsContainer.style.display = "none";
    }
}

async function saveQuestion(templateId) {
    const text = document.getElementById('question-text').value;
    const type = document.getElementById('question-type').value;
    const isMandatory = document.getElementById('is-mandatory').checked;
    const options = document.getElementById('question-options').value;

    const payload = {
        text,
        type,
        isMandatory,
        options,
        templateFormId: templateId
    };

    const response = await fetch('/api/template/question/add', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(payload)
    });

    if (response.ok) {
        location.reload();
    } else {
        const error = await response.text();
        alert("Failed to add question: " + error);
    }
}
