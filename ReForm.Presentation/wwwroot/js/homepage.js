document.addEventListener("DOMContentLoaded", () => {
    MicroModal.init();
});

function createNewTemplate() {
    MicroModal.show('create-template-modal');
}

async function saveTemplate() {
    const title = document.getElementById('template-title').value;

    if (!title.trim()) {
        alert("Title cannot be empty");
        return;
    }

    try {
        const response = await fetch('/api/template/create', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({title})
        });

        if (response.ok) {
            const newTemplate = await response.json();
            window.location.href = `/TemplateSetup/Edit?id=${newTemplate.id}`;
        } else {
            const error = await response.text();
            alert("Failed to save template: " + error);
        }
    } catch (err) {
        console.error(err);
        alert("Error occurred while saving template.");
    }
}
