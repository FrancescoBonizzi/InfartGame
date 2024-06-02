export default {

    isNullOrWhitespace: (value: string | null | undefined): boolean => {
        if (value === null) {
            return true;
        }

        if (!value) {
            return true;
        }

        // Faccio il .toString perché potrebbe essere un numero (va trovato il punto in cui è numero)
        return value.toString().trim().length <= 0;
    },

};
