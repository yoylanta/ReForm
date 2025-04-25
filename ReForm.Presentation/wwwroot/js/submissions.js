document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".submission-row").forEach(row => {
        row.addEventListener("click", () => {
            const formId = row.dataset.formId;
            window.location.href = `/Submissions/View?id=${formId}`;
        });
    });
});
