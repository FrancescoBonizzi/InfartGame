import { initializeRouter } from './pages/router';
import {wireAudioUnlockOnce} from "./services/AudioUnlocker.ts";

initializeRouter();
wireAudioUnlockOnce();