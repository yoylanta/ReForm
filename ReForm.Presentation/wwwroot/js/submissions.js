document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".submission-row").forEach(row => {
        row.addEventListener("click", () => {
            const filledFormId = row.dataset.formId;
            window.location.href = `/TemplateSetup/SubmissionView?id=${filledFormId}`;
        });
    });
});