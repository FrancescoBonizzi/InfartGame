const keyMap = {
    Space: 'space'
};

interface KeyState {
    pressed: boolean;
}

class Controller
{
    Keys: {
        space: KeyState;
    };

    constructor()
    {
        this.Keys = {
            space: { pressed: false },
        };

        window.addEventListener('keydown', (event) => this.keydownHandler(event));
        window.addEventListener('keyup', (event) => this.keyupHandler(event));
    }

    keydownHandler(event: KeyboardEvent)
    {
        const key = keyMap[event.code];

        if (!key)
            return;

        // Toggle on the key pressed state.
        this.Keys[key].pressed = true;
    }

    keyupHandler(event : KeyboardEvent)
    {
        const key = keyMap[event.code];

        if (!key)
            return;

        // Reset the key pressed state.
        this.Keys[key].pressed = false;
    }
}

export default Controller;