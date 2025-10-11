(function () {
    const gallery = document.getElementById('assistantGallery');
    const conversation = document.getElementById('assistantConversation');
    const resetButton = document.getElementById('resetAssistant');

    if (!gallery || !conversation) {
        return;
    }

    const createMessage = (type, content, title) => {
        const wrapper = document.createElement('div');
        wrapper.classList.add('assistant-message');
        wrapper.classList.add(type === 'bot' ? 'assistant-message-bot' : 'assistant-message-user');

        const icon = document.createElement('div');
        icon.classList.add('assistant-message-icon');
        icon.textContent = type === 'bot' ? '🤖' : '🧑';

        const body = document.createElement('div');
        if (title) {
            const heading = document.createElement('div');
            heading.classList.add('assistant-message-title');
            heading.textContent = title;
            body.appendChild(heading);
        }

        const paragraph = document.createElement('p');
        paragraph.classList.add('assistant-message-text');
        paragraph.textContent = content;
        body.appendChild(paragraph);

        wrapper.appendChild(icon);
        wrapper.appendChild(body);
        return wrapper;
    };

    const scrollConversationToBottom = () => {
        conversation.scrollTop = conversation.scrollHeight;
    };

    const clearSelection = () => {
        const cards = gallery.querySelectorAll('.assistant-image-card');
        cards.forEach(card => card.classList.remove('selected'));
    };

    gallery.addEventListener('click', (event) => {
        const card = event.target.closest('.assistant-image-card');
        if (!card) {
            return;
        }

        clearSelection();
        card.classList.add('selected');

        const title = card.dataset.title ?? '';
        const description = card.dataset.description ?? '';
        const summary = card.dataset.summary ?? '';
        const tags = card.dataset.tags ?? '';

        const userMessage = createMessage('user', `Quisiera más información sobre "${title}".`);
        conversation.appendChild(userMessage);

        const botSegments = [description];
        if (tags) {
            botSegments.push(`Etiquetas sugeridas: ${tags}.`);
        }
        if (summary && summary !== description) {
            botSegments.push(`Resumen rápido: ${summary}`);
        }

        botSegments.forEach((text, index) => {
            const delay = 200 * (index + 1);
            setTimeout(() => {
                const botMessage = createMessage('bot', text, 'MerAI');
                conversation.appendChild(botMessage);
                scrollConversationToBottom();
            }, delay);
        });

        scrollConversationToBottom();
    });

    if (resetButton) {
        resetButton.addEventListener('click', () => {
            const defaultMessage = conversation.querySelector('.assistant-message-bot');
            conversation.innerHTML = '';
            if (defaultMessage) {
                conversation.appendChild(defaultMessage.cloneNode(true));
            }
            clearSelection();
            scrollConversationToBottom();
        });
    }
})();
