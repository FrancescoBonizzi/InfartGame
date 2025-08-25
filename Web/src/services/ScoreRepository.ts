import stringHelper from "./StringHelper.ts";

const METERS_KEY = 'infart-meters';
const FARTS_KEY = 'infart-farts';
const VEGETABLES_EATEN_KEY = 'infart-vegetables';

const getMeters = () => toNumberOr0(localStorage.getItem(METERS_KEY));
const getFarts = () => toNumberOr0(localStorage.getItem(FARTS_KEY));
const getVegetablesEaten = () => toNumberOr0(localStorage.getItem(VEGETABLES_EATEN_KEY));

export default {
    setMeters: (meters: number) => {
        const storedMeters = getMeters();
        if (meters > storedMeters) {
            localStorage.setItem(METERS_KEY, meters.toString());
        }
    },

    getMeters: getMeters,

    setFarts: (farts: number) => {
        const storedFarts = getFarts();
        if (farts > storedFarts) {
            localStorage.setItem(FARTS_KEY, farts.toString());
        }
    },

    getFarts: getFarts,

    setVegetablesEaten: (vegetablesEaten: number) => {
        const storedVegetablesEaten = getVegetablesEaten();
        if (vegetablesEaten > storedVegetablesEaten) {
            localStorage.setItem(VEGETABLES_EATEN_KEY, vegetablesEaten.toString());
        }
    },

    getVegetablesEaten: getVegetablesEaten
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
