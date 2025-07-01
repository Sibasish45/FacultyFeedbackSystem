
// Site-wide JavaScript functionality
$(document).ready(function () {
    // Initialize tooltips
    $('[data-toggle="tooltip"]').tooltip();

    // Auto-hide alerts after 5 seconds
    setTimeout(function () {
        $('.alert').fadeOut('slow');
    }, 5000);
});

// Function to load chart data via API
function loadChartData(subjectId) {
    fetch(`/api/FeedbackApi/charts/${subjectId}`)
        .then(response => response.json())
        .then(data => {
            // Update chart with API data
            console.log('Chart data loaded:', data);
        })
        .catch(error => {
            console.error('Error loading chart data:', error);
        });
}
