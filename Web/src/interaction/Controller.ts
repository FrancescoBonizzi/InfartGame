const keyMap: Record<string, string> = {
    Space: 'space',
    KeyP: 'KeyP',
};

interface KeyState {
    pressed: boolean;
}

interface Keys {
    space: KeyState;
    KeyP: KeyState;
}

class Controller {
    private readonly _keys: Keys;

    constructor() {
        this._keys = {
            space: { pressed: false },
            KeyP: { pressed: false },
        };

        window.addEventListener('keydown', this.keydownHandler.bind(this));
        window.addEventListener('keyup', this.keyupHandler.bind(this));
    }

    private keydownHandler(event: KeyboardEvent): void {
        const key = keyMap[event.code];

        if (!key) {
            return;
        }

        if (key === 'space' && !this._keys.space.pressed) {
            this._keys.space.pressed = true;
        }
        else if (key === 'KeyP' && !this._keys.KeyP.pressed) {
            this._keys.KeyP.pressed = true;
        }
    }

    private keyupHandler(event: KeyboardEvent): void {
        const key = keyMap[event.code];

        if (!key) {
            return;
        }

        if (key === 'space') {
            this._keys.space.pressed = false;
        }
        else if (key === 'KeyP') {
            this._keys.KeyP.pressed = false;
        }
    }

    public consumeKeyPress(key: keyof Keys): boolean {
        if (this._keys[key].pressed) {
            this._keys[key].pressed = false;
            return true;
        }
        return false;
    }

    get Keys(): Keys {
        return this._keys;
    }
}

export default Controller;
