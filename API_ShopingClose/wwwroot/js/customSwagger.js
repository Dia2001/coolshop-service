(function () {
    // Lấy token từ input và thêm vào header
    function addAuthorization() {
        var token = document.getElementById('input_token').value;
        if (token && token.trim() !== '') {
            var authorizationValue = 'Bearer ' + token;
            window.swaggerUi.authActions.authorize('Bearer', authorizationValue);
            console.log('Authorization header has been added: ' + authorizationValue);
        }
    }

    // Xử lý sự kiện khi click nút "Authorize"
    function handleAuthorizeButtonClick() {
        addAuthorization();
        $('#input_token').on('input', function () {
            addAuthorization();
        });
    }

    // Thêm nút "Authorize" và input token
    function addAuthorizeButton() {
        var authContainer = document.createElement('div');
        authContainer.className = 'auth-container';

        var inputGroup = document.createElement('div');
        inputGroup.className = 'input-group';

        var inputToken = document.createElement('input');
        inputToken.id = 'input_token';
        inputToken.type = 'text';
        inputToken.className = 'form-control';
        inputToken.placeholder = 'Enter token';
        inputToken.setAttribute('aria-label', 'Enter token');
        inputToken.setAttribute('aria-describedby', 'authorize-button');

        var authorizeButton = document.createElement('button');
        authorizeButton.className = 'btn btn-primary';
        authorizeButton.type = 'button';
        authorizeButton.id = 'authorize-button';
        authorizeButton.textContent = 'Authorize';

        inputGroup.appendChild(inputToken);
        inputGroup.appendChild(authorizeButton);

        authContainer.appendChild(inputGroup);

        authorizeButton.addEventListener('click', handleAuthorizeButtonClick);

        var swaggerUIContainer = document.getElementsByClassName('swagger-ui')[0];
        swaggerUIContainer.insertBefore(authContainer, swaggerUIContainer.firstChild);
    }

    // Chờ DOM sẵn sàng rồi thêm nút "Authorize"
    document.addEventListener('DOMContentLoaded', function () {
        addAuthorizeButton();
    });
})();