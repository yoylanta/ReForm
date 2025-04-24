document.addEventListener('DOMContentLoaded', () => {
    const questionCards = document.querySelectorAll('.question-card');
    const submitBtn = document.getElementById('submit-btn');
    const page = document.getElementById('respond-page');
    const templateId = Number(page.dataset.templateId);
    const userId = Number(page.dataset.userId);

    questionCards.forEach(card => {
        const type = parseInt(card.dataset.type, 10);
        const opts = card.dataset.options ? card.dataset.options.split(',') : [];
        const container = card.querySelector('.response-container');

        switch (type) {
            case 0:
                opts.forEach(opt => {
                    const id = `q${card.dataset.id}_${opt}`;
                    const rb = document.createElement('div');
                    rb.className = 'form-check';
                    rb.innerHTML = `
                        <input class="form-check-input" type="radio" name="q${card.dataset.id}" id="${id}" value="${opt}">
                        <label class="form-check-label" for="${id}">${opt}</label>
                    `;
                    container.appendChild(rb);
                });
                break;

            case 1:
                opts.forEach(opt => {
                    const id = `q${card.dataset.id}_${opt}`;
                    const cb = document.createElement('div');
                    cb.className = 'form-check';
                    cb.innerHTML = `
                        <input class="form-check-input" type="checkbox" name="q${card.dataset.id}" id="${id}" value="${opt}">
                        <label class="form-check-label" for="${id}">${opt}</label>
                    `;
                    container.appendChild(cb);
                });
                break;

            case 2:
                container.innerHTML = `<input type="text" class="form-control" name="q${card.dataset.id}" />`;
                break;

            case 3: {
                let min = 1;
                let max = 5;
                const range = card.dataset.options?.split('-').map(Number);
                if (range.length === 2 && !isNaN(range[0]) && !isNaN(range[1])) {
                    [min, max] = range;
                }

                const starContainer = document.createElement('div');
                starContainer.classList.add('star-rating');
                starContainer.dataset.selected = 0;

                for (let i = min; i <= max; i++) {
                    const star = document.createElement('span');
                    star.classList.add('star');
                    star.innerHTML = '★';
                    star.dataset.value = i;

                    star.addEventListener('click', () => {
                        const value = parseInt(star.dataset.value);
                        starContainer.dataset.selected = value;

                        Array.from(starContainer.children).forEach((s) => {
                            const starVal = parseInt(s.dataset.value);
                            s.classList.toggle('selected', starVal <= value);
                        });
                    });

                    starContainer.appendChild(star);
                }

                container.appendChild(starContainer);
                break;
            }

            case 4: {
                const input = document.createElement('input');
                input.type = 'text';
                input.className = 'form-control flatpickr-input';
                input.name = `q${card.dataset.id}`;
                container.appendChild(input);

                flatpickr(input, {
                    dateFormat: 'Y-m-d',
                    position: 'above',
                    allowInput: true,
                    clickOpens: true,
                    wrap: false
                });

            }
        }
    });

    submitBtn.addEventListener('click', async () => {
        const payload = {
            templateFormId: templateId,
            userId: userId,
            questions: []
        };

        try {
            questionCards.forEach(card => {
                const qid = Number(card.dataset.id);
                const type = parseInt(card.dataset.type, 10);
                let answers = [];
                let options = [];

                switch (type) {
                    case 0: {
                        const radioSelected = card.querySelector(`input[name="q${qid}"]:checked`);
                        if (radioSelected) answers = [radioSelected.value];
                        options = Array.from(card.querySelectorAll(`input[name="q${qid}"]`)).map(opt => opt.value);
                        break;
                    }

                    case 1: {
                        const checkboxes = card.querySelectorAll(`input[name="q${qid}"]:checked`);
                        answers = Array.from(checkboxes).map(i => i.value);
                        options = Array.from(card.querySelectorAll(`input[name="q${qid}"]`)).map(opt => opt.value);
                        break;
                    }

                    case 2:
                    case 4: {
                        const input = card.querySelector(`input[name="q${qid}"]`);
                        if (input && input.value.trim()) answers = [input.value.trim()];
                        break;
                    }

                    case 3: {
                        const starContainer = card.querySelector('.star-rating');
                        if (starContainer) {
                            const selected = parseInt(starContainer.dataset.selected || 0);
                            if (selected > 0) answers = [selected.toString()];

                            const stars = Array.from(starContainer.children).map(star => parseInt(star.dataset.value));
                            if (stars.length > 0) {
                                const min = Math.min(...stars);
                                const max = Math.max(...stars);
                                options = [`${min}-${max}`];
                            }
                        }
                        break;
                    }
                }

                const isMandatory = card.querySelector('h5 span.text-danger') !== null;
                if (isMandatory && answers.length === 0) {
                    throw new Error(`Please answer the required question: ${card.querySelector('h5').innerText}`);
                }

                payload.questions.push({
                    templateQuestionId: qid,
                    text: card.querySelector('h5')?.innerText || '',
                    options: (type === 0 || type === 1 || type === 3) ? options.join(';') : '',
                    answers: answers.map(v => ({
                        response: v,
                        userId: userId
                    }))
                });
            });
        } catch (e) {
            console.error(e);
            Swal.fire('Required field missing', e.message || 'Please complete all required questions.', 'warning');
            return;
        }

        try {
            const res = await fetch('/api/submission/submit', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });
            if (res.ok) {
                Swal.fire('Thanks!', 'Your responses have been saved.', 'success')
                    .then(() => location.reload());
            } else {
                const txt = await res.text();
                Swal.fire('Error', txt, 'error');
            }
        } catch (e) {
            console.error(e);
            Swal.fire('Error', 'Network error while submitting.', 'error');
        }
    });
});
