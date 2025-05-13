document.addEventListener('DOMContentLoaded', () => {
    const sfFormCollapse = document.getElementById('sfForm');
    sfFormCollapse.addEventListener('shown.bs.collapse', () => {
        const firstInput = sfFormCollapse.querySelector('input, textarea');
        if (firstInput) {
            firstInput.focus();
        }
    });
});
