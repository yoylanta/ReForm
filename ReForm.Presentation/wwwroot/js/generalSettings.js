document.addEventListener("DOMContentLoaded", () => {
    const checkbox = document.getElementById("IsPublic");
    if (checkbox) {
        checkbox.addEventListener("change", () => {
            checkbox.closest('.form-group').style.backgroundColor = checkbox.checked ? "#e8f5e9" : "transparent";
        });
    }

    const topicInput = document.getElementById("Topic");
    const form = document.querySelector('form');

    // Get suggestions from DB when the user starts typing
    topicInput.addEventListener('input', async () => {
        const searchTerm = topicInput.value;
        if (searchTerm.length > 2) { // Trigger suggestions after 3 characters
            try {
                const response = await fetch(`/api/template/topic/get-all?searchTerm=${searchTerm}`);
                const topics = await response.json();

                // Clear previous suggestions
                const suggestionBox = document.getElementById("topic-suggestions");
                suggestionBox.innerHTML = '';

                // Show new suggestions
                topics.forEach(topic => {
                    const suggestionItem = document.createElement("li");
                    suggestionItem.textContent = topic.name;
                    suggestionItem.classList.add('suggestion-item');
                    suggestionItem.addEventListener('click', () => {
                        topicInput.value = topic.name;
                        suggestionBox.innerHTML = ''; // Clear suggestions
                    });
                    suggestionBox.appendChild(suggestionItem);
                });
            } catch (error) {
                console.error("Error fetching suggestions:", error);
            }
        }
    });

    form.addEventListener('submit', async (event) => {
        event.preventDefault();

        const topicName = document.getElementById("Topic").value.trim();

        if (!topicName) {
            alert("Topic name cannot be empty.");
            return;
        }

        // Check if topic exists in the database
        try {
            const response = await fetch(`/api/template/topic/get-all?searchTerm=${topicName}`);
            const existingTopics = await response.json();
            const topicExists = existingTopics.some(topic => topic.name.toLowerCase() === topicName.toLowerCase());

            if (topicExists) {
                alert("Topic name already exists. Please choose a different one.");
                return;
            }

            const formData = {
                Title: document.getElementById("Title").value,
                Description: document.getElementById("Description").value,
                TopicName: topicName,
                IsPublic: document.getElementById("IsPublic").checked,
                ImageUrl: document.getElementById("ImageUrl").value
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
                alert("Template updated successfully.");
            } else {
                alert(`Error: ${data.message || "Unable to update template."}`);
            }
        } catch (error) {
            console.error("Error:", error);
            alert("There was an error updating the template.");
        }
    });
});
