document.addEventListener("DOMContentLoaded", async () => {
    const formEl = document.querySelector("form");
    const isPublicCb = document.getElementById("IsPublic");
    const privateDiv = document.getElementById("private-settings");
    const debounce = (fn, ms) => {
        let tid;
        return (...args) => {
            clearTimeout(tid);
            tid = setTimeout(() => fn.apply(this, args), ms);
        };
    };

    // ── 1) Public toggle show/hide private settings ──
    privateDiv.style.display = isPublicCb.checked ? "none" : "block";
    isPublicCb.addEventListener("change", () => {
        privateDiv.style.display = isPublicCb.checked ? "none" : "block";
        // optional: clear any selected users when switching to Public
    });

    // ── 2) Allowed‐Users Tagify ──
    let allUsers = [];

    await fetch(`/api/user/getAllUsers`)
        .then(r => r.json())
        .then(list => {
            allUsers = list.map(u => ({
                value: u.id,
                name: u.name,
                email: u.email,
            }));
        })
        .catch(err => console.error("User initialization error:", err));

    function suggestionItemTemplate(tagData) {
        console.log('tagData', tagData);
        return `
        <div 
            value="${tagData.value}" 
            name="${tagData.name}" 
            email="${tagData.email}" 
            mappedvalue="${tagData.mappedValue}" 
            class="tagify__dropdown__item tagifySuggestionItem" 
            tabindex="0" 
            role="option" 
            aria-selected="true">
            <strong>${tagData.name}</strong><br>
            <small>${tagData.email}</small>
        </div>
    `;
    }

    const allowedInput = document.getElementById("allowed-users-input");
    const allowedHidden = document.getElementById("AllowedUsersCsv");
    const tagifyUsers = new Tagify(allowedInput, {
            tagTextProp: 'name',
            enforceWhitelist: true,
            keepInvalidTags: false,
            valueField: "value",
            //skipInvalid: true,
            dropdown: {
                enabled: 1,
                position: "text",
                highlightFirst: true,
                searchKeys: ['name', 'email']
            },
            whitelist: allUsers,
            templates:
                {
                    dropdownItem: suggestionItemTemplate
                }
        })
    ;

    // preload existing allowed‐users
    if (allowedHidden.value) {

        const initialIds = allowedHidden.value.split(",").map(id => id.trim()).filter(id => id);
        const initialUsers = allUsers.filter(u => initialIds.includes(String(u.value)));
        tagifyUsers.addTags(initialUsers);
    }

    // ── 3) Tags Tagify ──
    const tagInput = document.getElementById("tags-input");
    const tagifyTags = new Tagify(tagInput, {
        keepInvalidTags: true,
        enforceWhitelist: false,
        whitelist: [],
        dropdown: {
            enabled: 1,
            position: "text",
            highlightFirst: true
        }
    });

    tagifyTags.on("input", debounce(e => {
        const v = e.detail.value;
        if (!v) {
            tagifyTags.dropdown.hide();
            return;
        }
        fetch(`/api/template/tags?query=${encodeURIComponent(v)}`)
            .then(r => r.json())
            .then(list => {
                tagifyTags.settings.whitelist = list;
                console.log('tagifyTags list', list);
                tagifyTags.dropdown.show(v);
            })
            .catch(err => console.error("Tag search error:", err));
    }, 300));

    // ── 4) Topic autocomplete (unchanged) ──
    const topicInput = document.getElementById("Topic");
    const topicBox = document.getElementById("topic-suggestions");
    topicInput.addEventListener("input", async () => {
        const term = topicInput.value.trim();
        if (!term) {
            topicBox.innerHTML = "";
            return;
        }
        try {
            const res = await fetch(
                `/api/template/topic/get-all?searchTerm=${encodeURIComponent(term)}`
            );
            let topics = await res.json();
            topics = topics
                .filter(t => t.name.toLowerCase().includes(term.toLowerCase()))
                .sort((a, b) => a.name.localeCompare(b.name));
            topicBox.innerHTML = "";
            topics.forEach(t => {
                const li = document.createElement("li");
                li.textContent = t.name;
                li.classList.add("suggestion-item");
                li.addEventListener("click", () => {
                    topicInput.value = t.name;
                    topicBox.innerHTML = "";
                });
                topicBox.appendChild(li);
            });
        } catch (err) {
            console.error("Topic search error:", err);
        }
    });

    // ── 5) Form submit ──
    formEl.addEventListener("submit", async event => {
        event.preventDefault();

        // gather all fields
        const payload = {
            Id: +document.getElementById("TemplateId").value,
            Title: document.getElementById("Title").value,
            Description: descriptionArea.value(),
            TopicName: document.getElementById("Topic").value.trim(),
            IsPublic: isPublicCb.checked,
            ImageUrl: document.getElementById("ImageUrl").value,
            Tags: tagifyTags.value.map(t => t.value),
            AllowedUserIds: tagifyUsers.value.map(u => +u.value)
        };

        try {
            const res = await fetch("/api/template/edit", {
                method: "POST",
                headers: {"Content-Type": "application/json"},
                body: JSON.stringify(payload)
            });
            const data = await res.json();
            if (res.ok) {
                Swal.fire("Success", "Template updated successfully.", "success");
            } else {
                Swal.fire("Error", data.message || "Unable to update template.", "error");
            }
        } catch (err) {
            console.error("Submit error:", err);
            Swal.fire("Error", "Network error while updating.", "error");
        }
    });

    // disable browser autocomplete
    formEl.setAttribute("autocomplete", "off");
    formEl.querySelectorAll("input").forEach(i => i.setAttribute("autocomplete", "off"));

    // ── 6) Initialize EasyMDE for Markdown editor ──
    const descriptionArea = new EasyMDE({
        element: document.getElementById("markdown-description"),
        spellChecker: false,
        autosave: {
            enabled: true,
            uniqueId: "markdown-description",
            delay: 1000
        },
        initialValue: document.getElementById("markdown-description").value,
        toolbar: ["bold", "italic", "heading", "|", "quote", "unordered-list", "ordered-list", "|", "preview", "guide"]
    });
});
