document.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".submission-row").forEach(row => {
        row.addEventListener("click", (e) => {
            if (e.target.closest("a")) return; // Ignore click on the link
            const formId = row.dataset.formId;
            window.location.href = `/Submissions/View?id=${formId}`;
        });
    });
});
