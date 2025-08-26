import { defineConfig } from "vite";

export default defineConfig({
    build: {
        rollupOptions: {
            input: "index.html"
        }
    },
    base: './', // genera href/src relativi per comodità col sito in Jekyll
});