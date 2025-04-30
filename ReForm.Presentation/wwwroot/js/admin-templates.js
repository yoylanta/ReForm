let editMode = false;

document.addEventListener("DOMContentLoaded", () => {
    MicroModal.init();
    const titleInput = document.getElementById('template-title');
    if (titleInput) {
        titleInput.setAttribute('autocomplete', 'off');
    }
    document.querySelectorAll(".template-card").forEach(card => {
        card.addEventListener("click", (e) => {
            if (editMode) return;
            if (e.target.type === "checkbox") return;

            const templateId = card.dataset.templateId;
            window.location.href = `/TemplateSetup/GeneralSettings?id=${templateId}`;
        });
    });
});

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
    const selectedCheckboxes = document.querySelectorAll(".template-checkbox:checked");
    const selectedIds = Array.from(selectedCheckboxes).map(cb => cb.closest(".template-card").dataset.templateId);

    if (selectedIds.length === 0) {
        Swal.fire({
            icon: 'info',
            title: 'Notice',
            text: 'No templates selected.'
        });
        return;
    }

    const confirmation = await Swal.fire({
        icon: 'warning',
        title: 'Are you sure?',
        text: `Are you sure you want to delete ${selectedIds.length} forms?`,
        showCancelButton: true,
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'No, cancel!',
    });

    if (!confirmation.isConfirmed) return;

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
                text: 'Successfully deleted template forms!',
                timer: 3000,
                timerProgressBar: true
            }).then(() => location.reload());
        } else {
            const errorText = await response.text();
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: `Failed to delete templates: ${errorText}`
            });
        }
    } catch (err) {
        console.error(err);
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'An error occurred while deleting templates.'
        });
    }
}
