
import { initializeRouter } from './pages/router';
import {SoundManagerInstance} from "./services/SoundInstance.ts";

const router = initializeRouter();

router.hooks({
    before: (done, match) => {

        // Se navigo via dalla pagina del gioco, distruggo le risorse
        if (match && match.url !== 'game') {
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

SoundManagerInstance.wireAudioUnlockOnce();