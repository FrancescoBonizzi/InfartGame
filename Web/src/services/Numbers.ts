export default {

    randomBetween: (min: number, max: number) => {
        return min + Math.random() * (max - min);
    },

    headOrTail: () => {
        return Math.random() > 0.5;
    }
}