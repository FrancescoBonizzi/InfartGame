import { initializeRouter } from './pages/router';
import {wireAudioUnlockOnce} from "./services/AudioUnlocker.ts";

const router = initializeRouter();

router.hooks({
    before: (done, match) => {

        // Se navigo via dalla pagina del gioco, distruggo le risorse
        if (match && match.url !== 'gamebootstrap') {
            console.log(match.url);
            import('./pages/gamebootstrap').then(module => {
                if (module.destroyGame) {
                    module.destroyGame();
                }
                done();
            });
        } else {
            done();
        }
    }
});

wireAudioUnlockOnce();