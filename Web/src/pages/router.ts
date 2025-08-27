import Navigo from 'navigo';
import { renderMenuPage } from './menu';
import { renderScorePage } from './score';
import { initGame } from './gamebootstrap';
import {renderGameOverPage} from "./gameover.ts";

const router = new Navigo('/', {
    hash: true, // Fondamentale per il funziona su Jeckyll
});

export function initializeRouter() {
    const appElement = document.getElementById('app');

    if (!appElement) {
        console.error('Elemento #app non trovato!');
        return router;
    }

    // Se non c’è hash, imposta #/ (necessario su Jekyll/Pages)
    if (!location.hash || location.hash === '#') {
        location.replace(location.pathname + location.search + '#/');
    }

    router
        .on('',        () => renderMenuPage(appElement!))
        .on('/',       () => renderMenuPage(appElement!))
        .on('game',    () => initGame(appElement!))
        .on('/game',   () => initGame(appElement!))
        .on('scores',  () => renderScorePage(appElement!))
        .on('/scores', () => renderScorePage(appElement!))
        .on('gameover',   () => renderGameOverPage(appElement!))
        .on('/gameover',  () => renderGameOverPage(appElement!))
        .notFound(() => renderMenuPage(appElement!))
        .resolve();

    router.updatePageLinks();

    return router;
}

export default router;