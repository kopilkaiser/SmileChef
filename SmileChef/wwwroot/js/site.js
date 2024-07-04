// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(function () {
    $('#homeNavButton a').on('click', function (event) {

        event.preventDefault(); // Prevent the default action
        $('#loadingSpinnerContainer').show(); // Show the loading spinner


        // Get the URL from the anchor tag
        const url = $(this).attr('href');

        // Redirect to the new page
        window.location.href = url;
    });

    window.addEventListener('load', function () {
        $('#loadingSpinnerContainer').hide(); // Hide the loading spinner when the page is fully loaded
    });
})



