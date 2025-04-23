document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.template-card').forEach(card => {
        card.addEventListener('click', () => {
            const tid = card.dataset.templateId;
            window.location.href = `/Respond/Respond?id=${tid}`;
        });
    });
});
