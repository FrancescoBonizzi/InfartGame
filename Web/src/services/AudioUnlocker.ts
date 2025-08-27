
// Altrimenti sui Browser non funziona l'audio
export const wireAudioUnlockOnce = () => {
    const unlock = async () => {
        try {
            await unlockHowler();
        } finally {
            window.removeEventListener('pointerdown', unlock as EventListener);
            window.removeEventListener('keydown', unlock as EventListener);
            window.removeEventListener('touchend', unlock as EventListener);
        }
    };

    window.addEventListener('pointerdown', unlock as EventListener, { once: true, passive: true });
    window.addEventListener('keydown',     unlock as EventListener, { once: true });
    window.addEventListener('touchend',    unlock as EventListener, { once: true, passive: true });
}

export const unlockHowler = async () => {
    const audioContext = Howler.ctx;

    if (audioContext && audioContext.state !== "running" && typeof audioContext.resume === "function") {
        await audioContext.resume();
    }

    if (Howler.usingWebAudio && audioContext) {
        const buffer = audioContext.createBuffer(1, 1, audioContext.sampleRate);
        const src = audioContext.createBufferSource();
        src.buffer = buffer;
        src.connect((Howler as any).masterGain || audioContext.destination);
        try { src.start(0); } catch {}
        src.disconnect();
    }

    Howler.mute(false);
}