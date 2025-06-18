<script>
    // JavaScript to validate password match before submitting the form
    const form = document.querySelector('form');
    form.addEventListener('submit', function(event) {
        const password = document.getElementById('password').value;
    const confirmPassword = document.getElementById('confirmPassword').value;

    if (password !== confirmPassword) {
        alert("Passwords do not match.");
    event.preventDefault(); // Prevent form submission if passwords don't match
        }
    });
</script>