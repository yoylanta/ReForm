document.addEventListener("DOMContentLoaded", () => {
    const checkbox = document.getElementById("IsPublic");
    if (checkbox) {
        checkbox.addEventListener("change", () => {
            checkbox.closest('.form-group').style.backgroundColor = checkbox.checked ? "#e8f5e9" : "transparent";
        });
    }

    const topicInput = document.getElementById("Topic");
    const form = document.querySelector('form');

    function debounce(fn, delay) {
        let id;
        return (...args) => {
            clearTimeout(id);
            id = setTimeout(() => fn.apply(this, args), delay);
        }
    }

    // 3) Init Tagify
    const input = document.getElementById('tags-input');
    const tagify = new Tagify(input, {
        keepInvalidTags: true,       // <-- won't auto-remove unknown tags
        enforceWhitelist: false,     // <-- allow new tags
        whitelist: [],               // <-- we'll fill dynamically
        dropdown: {
            enabled: 1,                // show suggestions after 1 char
            position: 'text',
            highlightFirst: true
        }
    });

    // 4) As user types, fetch suggestions
    tagify.on('input', debounce(e => {
        const value = e.detail.value;
        if (!value) return tagify.dropdown.hide();

        fetch(`/api/template/tags?query=${encodeURIComponent(value)}`)
            .then(res => res.json())
            .then(list => {
                tagify.settings.whitelist = list;
                tagify.dropdown.show(value);
            });
    }, 300));

    topicInput.addEventListener('input', async () => {
        const searchTerm = topicInput.value.trim();
        const suggestionBox = document.getElementById("topic-suggestions");

        if (searchTerm.length === 0) {
            suggestionBox.innerHTML = '';
            return;
        }

        try {
            const response = await fetch(`/api/template/topic/get-all?searchTerm=${encodeURIComponent(searchTerm)}`);
            let topics = await response.json();

            topics.sort((a, b) => a.name.localeCompare(b.name));

            topics = topics.filter(topic => topic.name.toLowerCase().includes(searchTerm.toLowerCase()));

            suggestionBox.innerHTML = '';

            topics.forEach(topic => {
                const suggestionItem = document.createElement("li");
                suggestionItem.textContent = topic.name;
                suggestionItem.classList.add('suggestion-item');
                suggestionItem.addEventListener('click', () => {
                    topicInput.value = topic.name;
                    suggestionBox.innerHTML = '';
                });
                suggestionBox.appendChild(suggestionItem);
            });
        } catch (error) {
            console.error("Error fetching suggestions:", error);
        }
    });

    form.addEventListener('submit', async (event) => {
        event.preventDefault();

        const topicName = document.getElementById("Topic").value.trim();
        if (!topicName) {
            Swal.fire({
                icon: 'warning',
                title: 'Missing Topic',
                text: 'Topic name cannot be empty.'
            });
            return;
        }

        const tags = tagify.value.map(item => item.value);

        try {
            const formData = {
                Id: document.getElementById("TemplateId").value,
                Title: document.getElementById("Title").value,
                Description: document.getElementById("Description").value,
                TopicName: topicName,
                IsPublic: document.getElementById("IsPublic").checked,
                ImageUrl: document.getElementById("ImageUrl").value,
                Tags: tags
            };

            const postResponse = await fetch('/api/template/edit', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData)
            });

            const data = await postResponse.json();
            if (postResponse.ok) {
                Swal.fire({
                    icon: 'success',
                    title: 'Success',
                    text: 'Template updated successfully.'
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Error',
                    text: data.message || "Unable to update template."
                });
            }
        } catch (error) {
            console.error("Error:", error);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'There was an error updating the template.'
            });
        }
    });

    form.setAttribute("autocomplete", "off");
    document.querySelectorAll("input").forEach(input => {
        input.setAttribute("autocomplete", "off");
    });
});
