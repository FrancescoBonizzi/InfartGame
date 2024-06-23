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
    },

    /**
     * Linear Interpolation between two values.
     * @param start The start value.
     * @param end The end value.
     * @param t The interpolation factor, typically between 0 and 1.
     * @returns The interpolated value.
     */
    lerp: (start: number, end: number, t: number): number => {
        return start + t * (end - start);
    }
}