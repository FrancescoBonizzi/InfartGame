import Interval from "../primitives/Interval.ts";

const randomBetween = (min: number, max: number) => {
    return min + Math.random() * (max - min);
};

export default {

    randomBetween: randomBetween,

    randomBetweenInterval: (interval: Interval) => {
        return randomBetween(interval.min, interval.max);
    },

    headOrTail: () => {
        return Math.random() > 0.5;
    },

    toRadians: (degrees: number) => {
        return degrees * Math.PI / 180;
    }
}