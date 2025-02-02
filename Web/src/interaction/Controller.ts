interface KeyState {
    pressed: boolean;
    justPressed: boolean;
}

interface Keys {
    space: KeyState;
    KeyP: KeyState;
}

const keyMap: Record<string, keyof Keys> = {
    Space: 'space',
    KeyP: 'KeyP',
};

class Controller {
    private readonly _keys: Keys;

    constructor() {
        this._keys = {
            space: { pressed: false, justPressed: false },
            KeyP: { pressed: false, justPressed: false },
        };

        window.addEventListener('keydown', this.keydownHandler.bind(this));
        window.addEventListener('keyup', this.keyupHandler.bind(this));
    }

    private keydownHandler(event: KeyboardEvent): void {
        const key = keyMap[event.code];

        if (!key) return;

        if (!this._keys[key].pressed) {
            this._keys[key].pressed = true;
            this._keys[key].justPressed = true; // Viene impostato solo una volta
        }
    }

    private keyupHandler(event: KeyboardEvent): void {
        const key = keyMap[event.code];

        if (!key) return;

        this._keys[key].pressed = false;
        this._keys[key].justPressed = false; // Reset totale
    }

    public consumeKeyPress(key: keyof Keys): boolean {
        if (this._keys[key].justPressed) {
            this._keys[key].justPressed = false; // Consuma il valore
            return true;
        }
        return false;
    }

    get Keys(): Keys {
        return this._keys;
    }
}

export default Controller;