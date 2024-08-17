import Interval from "../primitives/Interval.ts";

class DynamicGameParameters {

    private _larghezzaBuchi: Interval = {min: 190, max: 250};

    get larghezzaBuchi(): Interval {
        return this._larghezzaBuchi;
    }

    set larghezzaBuchi(value: Interval) {
        this._larghezzaBuchi = value;
    }


}

export default DynamicGameParameters;