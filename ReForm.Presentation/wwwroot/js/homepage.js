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
            window.location.href = `/TemplateSetup/GeneralSettings?id=${newTemplate.id}`;
        } else {
            const error = await response.text();
            Swal.fire('Error', "Failed to save template: " + error, 'error');
        }
    } catch (err) {
        console.error(err);
        Swal.fire('Failed to save template', err.message || 'Unknown error', 'error');
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
    const selectedIds = Array.from(document.querySelectorAll(".template-checkbox:checked"))
        .map(cb => cb.closest(".template-card").dataset.templateId);

    if (selectedIds.length === 0) {
        Swal.fire('Notice', 'No templates selected.', 'info');
        return;
    }

    const confirmed = await Swal.fire({
        icon: 'warning',
        title: 'Are you sure?',
        text: `Are you sure you want to delete ${selectedIds.length} template(s)?`,
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
    });

    if (!confirmed.isConfirmed) return;

    try {
        const response = await fetch('/api/template/delete', {
            method: 'POST',
            headers: {'Content-Type': 'application/json'},
            body: JSON.stringify(selectedIds)
        });

        if (response.ok) {
            Swal.fire({
                icon: 'success',
                title: 'Deleted!',
                text: 'Selected templates have been deleted.',
                timer: 3000,
                timerProgressBar: true
            }).then(() => location.reload());
        } else {
            const error = await response.text();
            Swal.fire('Error', "Failed to delete templates: " + error, 'error');
        }
    } catch (err) {
        console.error(err);
        Swal.fire('Error', 'Error occurred while deleting templates.', 'error');
    }

}
