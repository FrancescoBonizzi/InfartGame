class Controller {
    private _pressed = false;
    private _justPressed = false;

    constructor(target: EventTarget = window) {
        target.addEventListener("keydown", this.downHandler);
        target.addEventListener("keyup", this.upHandler);

        target.addEventListener("pointerdown", this.downHandler);
        target.addEventListener("pointerup", this.upHandler);

        window.addEventListener("blur", this.reset);
        document.addEventListener("visibilitychange", this.reset);
    }

    // NB: tipo ampio "Event"
    private downHandler = (_e: Event) => {
        if (!this._pressed) {
            this._pressed = true;
            this._justPressed = true;
        }
    };

    private upHandler = (_e: Event) => {
        this._pressed = false;
        this._justPressed = false;
    };

    private reset = (_e?: Event) => {
        this._pressed = false;
        this._justPressed = false;
    };

    public consumePress(): boolean {
        if (this._justPressed) {
            this._justPressed = false;
            return true;
        }
        return false;
    }

    public get isPressed(): boolean {
        return this._pressed;
    }
}

export default Controller;