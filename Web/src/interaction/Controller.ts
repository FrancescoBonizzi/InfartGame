const keyMap: Record<string, string> = {
    Space: 'space',
    ArrowUp: 'up',
    ArrowDown: 'down'
};

interface KeyState {
    pressed: boolean;
}

interface Keys {
    space: KeyState;
    up: KeyState;
    down: KeyState;
}

class Controller {

    private readonly _keys: Keys;

    constructor() {
        this._keys = {
            space: { pressed: false },
            up: { pressed: false },
            down: { pressed: false }
        };

        window.addEventListener('keydown', this.keydownHandler.bind(this));
        window.addEventListener('keyup', this.keyupHandler.bind(this));
    }

    private keydownHandler(event: KeyboardEvent): void {
        const key = keyMap[event.code];

        if (!key)
            return;

        if (key === 'space') {
            this._keys.space.pressed = true;
        } else if (key === 'up') {
            this._keys.up.pressed = true;
        } else if (key === 'down') {
            this._keys.down.pressed = true;
        }
    }

    private keyupHandler(event: KeyboardEvent): void {
        const key = keyMap[event.code];

        if (!key)
            return;

        if (key === 'space') {
            this._keys.space.pressed = false;
        } else if (key === 'up') {
            this._keys.up.pressed = false;
        } else if (key === 'down') {
            this._keys.down.pressed = false;
        }
    }

    get Keys(): Keys {
        return this._keys;
    }

}

export default Controller;
