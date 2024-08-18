import {Ticker} from "pixi.js";

class PixiJsTimer {

    private readonly _intervalMilliseconds: number;
    private readonly _callback: (() => void) | undefined;
    private _timePassedMilliseconds: number = 0;

    constructor(
        intervalMilliseconds: number,
        callback?: () => void) {

        this._intervalMilliseconds = intervalMilliseconds;
        this._callback = callback;
    }

    private _isEnabled: boolean = false;

    get isEnabled(): boolean {
        return this._isEnabled;
    }

    set isEnabled(isEnabled) {
        this._isEnabled = isEnabled;
    }

    update(time: Ticker) {

        if (!this._isEnabled)
            return;

        this._timePassedMilliseconds += time.deltaMS;

        // Se sono in modalità callback, eseguo la callback ogni intervallo di tempo,
        // altrimenti verrà eseguito il tutto manualmente da fuori
        if (this._callback) {
            if (this.isTimePassed()) {
                this._callback();
                this._timePassedMilliseconds = 0;
            }
        }
    }

    private isTimePassed() {
        return this._timePassedMilliseconds >= this._intervalMilliseconds;
    }

    canRunManualCallback() {
        if (this._callback)
            throw new Error("Cannot run manual callback if a callback is set");

        if (this.isTimePassed()) {
            this._timePassedMilliseconds = 0;
            return true;
        }

        return false;
    }

}

export default PixiJsTimer;

// TODO: farlo uguale a come ho fatto per i particle system e ficcarlo lì
// TODO: usarlo per il generate buco
// TODO: Guarda ogni quanto allarga nei dynamic game parameters