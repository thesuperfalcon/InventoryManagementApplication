document.addEventListener("DOMContentLoaded", function () {
    // Select all links that start with #
    const links = document.querySelectorAll('a[href^="#"]');

    links.forEach(link => {
        link.addEventListener('click', function (e) {
            const targetId = this.getAttribute('href');

            if (targetId === '#') {
                return;
            }
            // Check if the targetId starts with '#'
            if (targetId.startsWith('#') && targetId.length > 1) {
                e.preventDefault(); // Prevent default anchor click behavior
                const targetElement = document.querySelector(targetId);

                if (targetElement) {
                    targetElement.scrollIntoView({
                        behavior: 'smooth' // Enable smooth scrolling
                    });
                }
            }
        });
    });
});

let debounceTimer;

function fetchSuggestions() {
    clearTimeout(debounceTimer);  // Clear previous timer if it exists
    debounceTimer = setTimeout(function () {  // Set a new timer for debouncing
        let query = document.getElementById('searchInputNav').value;

        if (query.length > 0) {
            $.ajax({
                url: '/SearchBar?handler=SearchSuggestions',
                type: 'GET',
                data: { query: query },
                success: function (data) {
                    console.log('Data received:', data);  // Log the received data

                    // Assuming the data is returned as { $id, $values: [...] }
                    let suggestions = $('#suggestionsList');  // Make sure suggestionsList is the correct ID
                    suggestions.empty();  // Clear previous suggestions

                    // Access the $values array inside the returned object
                    if (data.$values && data.$values.length > 0) {
                        suggestions.show();  // Show the dropdown
                        data.$values.forEach(function (item) {

                            suggestions.append(`<li onclick="selectSuggestion('${item.name}')" class="list-group-item">${item.secondValue}</li>`);

                        });
                    } else {
                        suggestions.hide();  // Hide the dropdown if no results
                    }
                },
                error: function (error) {
                    console.error('Error fetching suggestions:', error);
                    $('#suggestionsList').hide();  // Hide dropdown on error
                }
            });
        } else {
            $('#suggestionsList').hide();  // Hide the dropdown if input is empty
        }
    }, 300);  // Delay of 300ms for debouncing
}

function selectSuggestion(value) {
    document.getElementById('searchInputNav').value = value;
    $('#suggestionsList').hide(); // Hide suggestions after selection
}