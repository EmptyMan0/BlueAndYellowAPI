let isAuthenticated = false;

document.addEventListener('DOMContentLoaded', () => {
    updateUIState(false);
});

function updateUIState(authenticated) {
    isAuthenticated = authenticated;

    const inputs = [
        'messagePhone', 'messageText', 'sendMsgBtn',
        'fileUrlPhone', 'fileUrl', 'fileName', 'fileCaption', 'fileQuotedMessageId', 'sendFileUrlBtn'
    ];

    inputs.forEach(id => {
        const el = document.getElementById(id);
        if (el) {
            el.disabled = !authenticated;
        }
    });
}

function showResponse(data) {
    const responseEl = document.getElementById('response');
    if (responseEl) {
        responseEl.textContent = JSON.stringify(data, null, 4);
    }
}

function showError(message) {
    const errorEl = document.getElementById('error-container');
    if (errorEl) {
        errorEl.textContent = message;
        errorEl.style.display = 'block';
        setTimeout(() => {
            errorEl.style.display = 'none';
        }, 5000);
    }
}

function clearErrors() {
    const errorEl = document.getElementById('error-container');
    if (errorEl) {
        errorEl.style.display = 'none';
    }
}

function getCredentials() {
    return {
        idInstance: document.getElementById('idInstance')?.value.trim() || '',
        apiTokenInstance: document.getElementById('apiTokenInstance')?.value.trim() || ''
    };
}

function getAuthHeaders() {
    const creds = getCredentials();
    return {
        'Content-Type': 'application/json',
        'idInstance': creds.idInstance,
        'apiTokenInstance': creds.apiTokenInstance
    };
}

async function getSettings() {
    clearErrors();
    const creds = getCredentials();

    if (!creds.idInstance || !creds.apiTokenInstance) {
        showError('⚠️ Введите idInstance и apiTokenInstance');
        return;
    }

    try {
        const response = await fetch(`/getSettings?idInstance=${creds.idInstance}&apiTokenInstance=${creds.apiTokenInstance}`, {
            method: 'GET'
        });

        const data = await response.json();

        if (response.ok) {
            updateUIState(true);
            showResponse(data);
        } else {
            updateUIState(false);
            showError(data.message || 'Ошибка авторизации');
            showResponse(data);
        }
    } catch (error) {
        updateUIState(false);
        showError('Ошибка соединения: ' + error.message);
        showResponse({ error: error.message });
    }
}

async function getStateInstance() {
    clearErrors();
    const creds = getCredentials();

    if (!creds.idInstance || !creds.apiTokenInstance) {
        showError('⚠️ Введите idInstance и apiTokenInstance');
        return;
    }

    try {
        const response = await fetch(`/getStateInstance?idInstance=${creds.idInstance}&apiTokenInstance=${creds.apiTokenInstance}`, {
            method: 'GET'
        });

        const data = await response.json();
        showResponse(data);

        if (response.ok && !isAuthenticated) {
            updateUIState(true);
        }
    } catch (error) {
        showError('Ошибка: ' + error.message);
        showResponse({ error: error.message });
    }
}

async function sendMessage() {
    clearErrors();

    const creds = getCredentials();
    if (!creds.idInstance || !creds.apiTokenInstance) {
        showError('⚠️ Введите idInstance и apiTokenInstance');
        return;
    }

    const phone = document.getElementById('messagePhone')?.value.trim() || '';
    const message = document.getElementById('messageText')?.value.trim() || '';
    const quotedMessageId = document.getElementById('messageQuotedMessageId')?.value.trim() || '';

    if (!phone || !message) {
        showError('⚠️ Введите номер телефона и сообщение');
        return;
    }

    try {
        const body = {
            chatId: `${phone}@c.us`,
            message: message
        };

        if (quotedMessageId) {
            body.quotedMessageId = quotedMessageId;
        }

        const response = await fetch('/sendMessage', {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(body)
        });

        const data = await response.json();
        showResponse(data);

        if (!response.ok) {
            showError(data.message || 'Ошибка отправки');
        }
    } catch (error) {
        showError('Ошибка: ' + error.message);
        showResponse({ error: error.message });
    }
}

async function sendFileByUrl() {
    clearErrors();

    const creds = getCredentials();
    if (!creds.idInstance || !creds.apiTokenInstance) {
        showError('⚠️ Введите idInstance и apiTokenInstance');
        return;
    }

    const phone = document.getElementById('fileUrlPhone')?.value.trim() || '';
    const fileUrl = document.getElementById('fileUrl')?.value.trim() || '';
    const fileName = document.getElementById('fileName')?.value.trim() || '';
    const caption = document.getElementById('fileCaption')?.value.trim() || '';
    const quotedMessageId = document.getElementById('fileQuotedMessageId')?.value.trim() || '';

    if (!phone || !fileUrl || !fileName) {
        showError('⚠️ Заполните все обязательные поля');
        return;
    }

    try {
        const body = {
            chatId: `${phone}@c.us`,
            urlFile: fileUrl,
            fileName: fileName
        };

        if (caption) {
            body.caption = caption;
        }

        if (quotedMessageId) {
            body.quotedMessageId = quotedMessageId;
        }

        const response = await fetch('/sendFileByUrl', {
            method: 'POST',
            headers: getAuthHeaders(),
            body: JSON.stringify(body)
        });

        const data = await response.json();
        showResponse(data);

        if (!response.ok) {
            showError(data.message || 'Ошибка отправки файла');
        }
    } catch (error) {
        showError('Ошибка: ' + error.message);
        showResponse({ error: error.message });
    }
}