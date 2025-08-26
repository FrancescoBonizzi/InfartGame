import stringHelper from "./StringHelper.ts";

const ScorePrefix = 'infart-score';
export type ScoreType = 'meters' | 'farts' | 'vegetables';
export type ScoreSource = 'gameover' | 'record';

const makeKey = (type: ScoreType, source: ScoreSource) => `${ScorePrefix}-${source}-${type}`;

export default {
    getScore: (type: ScoreType, source: ScoreSource) => {
        const key = makeKey(type, source);
        return toNumberOr0(localStorage.getItem(key));
    },

    setScore: (type: ScoreType, source: ScoreSource, value: number) => {
        const key = makeKey(type, source);
        localStorage.setItem(key, value.toString());
    }
}

const toNumberOr0 = (value: string | null | undefined): number => {
    if (stringHelper.isNullOrWhitespace(value)) {
        return 0;
    }

    const number = Number(value);
    if (isNaN(number)) {
        return 0;
    }

    return number;
}
