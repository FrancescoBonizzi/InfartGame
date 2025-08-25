import Navigo from 'navigo';
import { renderMenuPage } from './menu';
import { renderScorePage } from './score';
import { initGame } from './gamebootstrap';

const router = new Navigo('/');

export function initializeRouter() {
    const appElement = document.getElementById('app');

    if (!appElement) {
        console.error('Elemento #app non trovato!');
        return router;
    }

    router
        .on('/', () => renderMenuPage(appElement!))
        .on('/game', () => initGame(appElement!))
        .on('/scores', () => renderScorePage(appElement!))
        .notFound(() => renderMenuPage(appElement!))
        .resolve();

    return router;
}

export default router;