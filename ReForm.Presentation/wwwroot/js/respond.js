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

            case 3:
                container.innerHTML = `
          <input type="number" class="form-control rating-input" name="q${card.dataset.id}"
                 min="1" max="5" value="3" />
        `;
                break;

            case 4:
                container.innerHTML = `<input type="date" class="form-control" name="q${card.dataset.id}" />`;
                break;
        }
    });

    submitBtn.addEventListener('click', async () => {
        const payload = {
            templateFormId: templateId,
            userId: userId,
            questions: []
        };

        questionCards.forEach(card => {
            const qid = Number(card.dataset.id);
            const type = parseInt(card.dataset.type, 10);
            let answers = [];
            let options = [];

            switch (type) {
                case 0:
                    const radioSelected = card.querySelector(`input[name="q${qid}"]:checked`);
                    if (radioSelected) answers = [radioSelected.value];

                    options = Array.from(card.querySelectorAll(`input[name="q${qid}"]`))
                        .map(opt => opt.value);
                    break;

                case 1:
                    const checkboxes = card.querySelectorAll(`input[name="q${qid}"]:checked`);
                    answers = Array.from(checkboxes).map(i => i.value);

                    options = Array.from(card.querySelectorAll(`input[name="q${qid}"]`))
                        .map(opt => opt.value);
                    break;

                case 2:
                case 4:
                case 3:
                    const input = card.querySelector(`input[name="q${qid}"]`);
                    if (input && input.value) answers = [input.value];
                    options = [];
                    break;
            }

            const isMandatory = card.querySelector('h5 span.text-danger') !== null;
            if (isMandatory && answers.length === 0) {
                alert('Please answer the required question: ' + card.querySelector('h5').innerText);
                throw 'validation';
            }

            payload.questions.push({
                templateQuestionId: qid,
                text: card.querySelector('h5')?.innerText || '',
                options: (type === 0 || type === 1) ? options.join(';') : '',
                answers: answers.map(v => ({
                    response: v,
                    userId: userId
                }))
            });

        });

        try {
            const finalPayload = {
                dto: payload
            };
                const res = await fetch('/api/submission/submit', {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(finalPayload)
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
