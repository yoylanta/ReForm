let editMode = false;

document.addEventListener("DOMContentLoaded", () => {
    MicroModal.init();
    document.getElementById('template-title').setAttribute('autocomplete', 'off');
    document.querySelectorAll(".template-card").forEach(card => {
        card.addEventListener("click", (e) => {
            if (editMode) return; 
            if (e.target.type === "checkbox") return;

            const templateId = card.dataset.templateId;
            window.location.href = `/TemplateSetup/GeneralSettings?id=${templateId}`;
        });
    });
});

function createNewTemplate() {
    MicroModal.show('create-template-modal');
}

async function saveTemplate() {
    const title = document.getElementById('template-title').value;

    if (!title.trim()) {
        Swal.fire('Oops!', 'Title cannot be empty', 'warning');
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
            window.location.href = `/TemplateSetup/GeneralSettings?id=${templateId}`;
        } else {
            const error = await response.text();
            alert("Failed to save template: " + error);
        }
    } catch (err) {
        console.error(err);
        Swal.fire('Failed to save template', error, 'error');
    }
}

function redirectToEditPage(templateId) {
    window.location.href = `/TemplateSetup/Edit?id=${templateId}`;
}

let deleteMode = false;

function toggleEditMode() {
    editMode = !editMode;

    const checkboxes = document.querySelectorAll(".template-checkbox");
    const deleteBtn = document.getElementById("delete-selected");
    const editBtn = document.getElementById("edit-mode-toggle");

    if (editMode) {
        checkboxes.forEach(cb => cb.style.display = "inline-block");
        deleteBtn.style.display = "inline-block";
        editBtn.innerText = "Exit Edit Mode";
    } else {
        checkboxes.forEach(cb => {
            cb.checked = false;
            cb.style.display = "none";
        });
        deleteBtn.style.display = "none";
        editBtn.innerText = "Edit";
    }
}

async function deleteSelectedTemplates() {
    const confirmed = confirm("Are you sure you want to delete the selected templates?");
    if (!confirmed) return;

    const selectedIds = Array.from(document.querySelectorAll(".template-checkbox:checked"))
        .map(cb => cb.closest(".template-card").dataset.templateId);

    if (selectedIds.length === 0) {
        alert("No templates selected.");
        return;
    }

    try {
        const response = await fetch('/api/template/delete', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(selectedIds)
        });

        if (response.ok) {
            location.reload();
        } else {
            const error = await response.text();
            alert("Failed to delete templates: " + error);
        }
    } catch (err) {
        console.error(err);
        alert("Error occurred while deleting templates.");
    }
}
