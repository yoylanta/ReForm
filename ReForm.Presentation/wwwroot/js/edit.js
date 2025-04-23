// edit.js
const questionType = {
    SingleChoice: 0,
    MultipleChoice: 1,
    Text: 2,
    Rating: 3,
    Date: 4
};

document.addEventListener('DOMContentLoaded', () => {
    const addBtn = document.getElementById('add-btn');
    const formWrapper = document.getElementById('new-question-form-wrapper');
    const cancelBtn = document.getElementById('cancel-btn');
    const saveBtn = document.getElementById('save-btn');
    const hiddenId = document.getElementById('question-id');
    let currentEditingCard = null;

    // ——— Helpers ———
    function clearFields() {
        document.getElementById('new-text').value = '';
        document.getElementById('new-type').value = questionType.SingleChoice;
        document.getElementById('new-mandatory').checked = false;
        document.getElementById('options-list').innerHTML = '';
        document.getElementById('rating-min').value = '';
        document.getElementById('rating-max').value = '';
        hiddenId.value = '';
        toggleNewOptions();
    }

    function addOption(value = '') {
        const list = document.getElementById('options-list');
        const group = document.createElement('div');
        group.className = 'option-input-group';

        const icon = document.createElement('span');
        icon.className = 'option-icon';
        const t = String(document.getElementById('new-type').value);
        icon.innerHTML = (t === '0' ? '&#9675;' : '&#9633;');

        const input = document.createElement('input');
        input.type = 'text';
        input.className = 'form-control';
        input.value = value;

        const btn = document.createElement('button');
        btn.type = 'button';
        btn.innerHTML = '×';
        btn.onclick = () => group.remove();

        group.append(icon, input, btn);
        list.appendChild(group);
    }

    function toggleNewOptions() {
        const t = String(document.getElementById('new-type').value);
        const needsOpts = (t === '0' || t === '1');
        const needsRate = (t === '3');
        document.getElementById('options-container').style.display = needsOpts ? 'block' : 'none';
        document.getElementById('rating-range-container').style.display = needsRate ? 'block' : 'none';

        if (needsOpts && !document.querySelector('#options-list .option-input-group')) {
            addOption();
        }
        // update each icon
        document.querySelectorAll('#options-list .option-icon').forEach(el => {
            el.innerHTML = (t === '0' ? '&#9675;' : '&#9633;');
        });
    }

    async function saveQuestion() {
        const text = document.getElementById('new-text').value.trim();
        if (!text) {
            await Swal.fire({ icon: 'warning', title: 'Missing text', text: 'Please enter a question.' });
            return;
        }

        const type = +document.getElementById('new-type').value;
        const isMand = document.getElementById('new-mandatory').checked;
        let opts = [];

        if (type === questionType.SingleChoice || type === questionType.MultipleChoice) {
            opts = Array.from(document.querySelectorAll('#options-list input'))
                .map(i => i.value.trim()).filter(v => v);
            if (!opts.length) {
                await Swal.fire({ icon: 'warning', title: 'No options', text: 'Add at least one option.' });
                return;
            }
        }

        if (type === questionType.Rating) {
            const min = parseInt(document.getElementById('rating-min').value, 10);
            const max = parseInt(document.getElementById('rating-max').value, 10);
            if (isNaN(min) || isNaN(max) || min >= max || min == 0) {
                await Swal.fire({
                    icon: 'warning',
                    title: 'Invalid range',
                    text: 'Please provide a valid rating range (e.g., 1–5 or 1–10).'
                });
                return;
            }
            opts = [`${min}-${max}`];
        }

        const payload = {
            id: hiddenId.value ? +hiddenId.value : undefined,
            text,
            type,
            isMandatory: isMand,
            options: opts.join(','),
            templateFormId: +saveBtn.dataset.templateId
        };

        const url = payload.id
            ? '/api/template/question/edit'
            : '/api/template/question/add';

        const res = await fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(payload)
        });

        if (res.ok) {
            return location.reload();
        }
        const err = await res.text();
        await Swal.fire({ icon: 'error', title: 'Save failed', text: err });
    }

    function startNewQuestion() {
        formWrapper.classList.remove('d-none');
        addBtn.classList.add('d-none');
        clearFields();
    }

    // helper to reposition the single form under any element
    function moveFormAfter(element) {
        element.parentNode.insertBefore(formWrapper, element.nextElementSibling);
        }

    // ——— Wire UI ———
    formWrapper.classList.add('d-none');

    // — Add‑question button ——
    addBtn.addEventListener('click', () => {
        moveFormAfter(addBtn.closest('.col-12'));
        startNewQuestion();
    });
    cancelBtn?.addEventListener('click', () => {
        formWrapper.classList.add('d-none');
        addBtn.classList.remove('d-none');
        clearFields();
        if (currentEditingCard) {
            currentEditingCard.classList.remove('d-none');
            currentEditingCard = null;
        }
    });
    saveBtn.addEventListener('click', saveQuestion);
    document.getElementById('new-type')
        .addEventListener('change', toggleNewOptions);

    // ——— Delete ———
    document.querySelectorAll('.delete-btn').forEach(btn => {
        btn.addEventListener('click', async () => {
            const id = btn.dataset.id;
            const c = await Swal.fire({
                icon: 'warning',
                title: 'Delete?',
                showCancelButton: true,
                confirmButtonText: 'Yes'
            });
            if (!c.isConfirmed) return;
            const r = await fetch(`/api/template/question/delete/${encodeURIComponent(id)}`, { method: 'DELETE' });
            if (r.ok) location.reload();
            else Swal.fire('Error', 'Could not delete', 'error');
        });
    });

    // ——— Edit ———
    document.querySelectorAll('.edit-btn').forEach(btn => {
        btn.addEventListener('click', async () => {
            const id = btn.dataset.id;
            try {
                const cardContainer = btn.closest('.col-12');
                currentEditingCard = cardContainer;
                cardContainer.classList.add('d-none');
                const r = await fetch(`/api/template/question/${id}`, { credentials: 'same-origin' });
                const txt = await r.text();
                console.log('RAW!', r.status, txt);
                if (!r.ok) throw new Error(`HTTP ${r.status}`);
                const data = JSON.parse(txt);

                // exactly the same show flow as Add
                // move form right under this specific question card
                moveFormAfter(cardContainer);
                startNewQuestion();

                // fill form
                document.getElementById('new-text').value = data.text;
                document.getElementById('new-type').value = data.type;
                document.getElementById('new-mandatory').checked = data.isMandatory;
                hiddenId.value = data.id;

                toggleNewOptions();

                // repopulate opts or rating
                const list = document.getElementById('options-list');
                list.innerHTML = '';
                if (data.type === questionType.SingleChoice || data.type === questionType.MultipleChoice) {
                    (data.options || '').split(',').forEach(o => addOption(o.trim()));
                }
                if (data.type === questionType.Rating && data.options?.includes('-')) {
                    const [min, max] = data.options.split('-').map(n => n.trim());
                    document.getElementById('rating-min').value = min;
                    document.getElementById('rating-max').value = max;
                }
            }
            catch (e) {
                console.error('Load question failed', e);
                await Swal.fire('Error', 'Could not load question', 'error');
            }
        });
    });
    window.addOption = addOption;
});
