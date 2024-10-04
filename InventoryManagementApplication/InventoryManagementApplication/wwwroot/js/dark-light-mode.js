document.addEventListener('DOMContentLoaded', function () {
    const toggleButton = document.getElementById('toggleTheme');
    const body = document.body;

    // Check local storage for theme preference
    const theme = localStorage.getItem('theme');
    if (theme === 'dark') {
        body.classList.add('dark-mode');
    }

    toggleButton.addEventListener('click', function () {
        body.classList.toggle('dark-mode');
        // Save the user's preference in local storage
        if (body.classList.contains('dark-mode')) {
            localStorage.setItem('theme', 'dark');
            toggleButton.innerHTML = '<i class="bi bi-sun"></i>'; // Change to sun icon
        } else {
            localStorage.setItem('theme', 'light');
            toggleButton.innerHTML = '<i class="bi bi-moon"></i>'; // Change to moon icon
        }
    });
});