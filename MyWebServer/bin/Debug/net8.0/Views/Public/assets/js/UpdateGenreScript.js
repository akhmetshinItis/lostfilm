document.getElementById('update-genre-form').addEventListener('submit', async function (event) {
    event.preventDefault();
    const form = event.target;
    const formData = new FormData(form);
    const data = Object.fromEntries(formData.entries());

    data.Id = parseInt(data.Id);
    data.UsageCount = parseInt(data.UsageCount);

    const response = await fetch(form.action, {
        method: form.method,
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
    });

    if (response.ok) {
        window.location.href = "/admin";
    } else {
        console.error("Failed to update genre:", await response.text());
    }
});
