# Pattern: Audio Unlock con Howler.js nei browser moderni

## Perché il browser blocca l'AudioContext

I browser moderni (Chrome, Firefox, Safari) implementano la **Autoplay Policy**: qualsiasi
`AudioContext` viene creato nello stato `suspended` e non può produrre audio finché
non avviene una **user gesture** (click, tap, keydown).

Howler.js crea internamente un `AudioContext` al primo utilizzo. Se `playSound()` viene
chiamato prima di una gesture, l'audio parte silenziosamente senza errori — ma non si sente.

---

## Come rilevare lo stato

```typescript
import { Howler } from 'howler';

// L'AudioContext esiste solo dopo il primo utilizzo di Howler
const state = Howler.ctx?.state; // 'suspended' | 'running' | 'closed' | undefined
```

- `undefined` → Howler non ha ancora creato l'AudioContext
- `'suspended'` → creato ma bloccato (nessuna gesture ancora)
- `'running'` → sbloccato, audio funzionante

---

## Il pattern `wireAudioUnlockOnce()` + bottone esplicito

### Approccio a due livelli

**Livello 1 — sblocco automatico al primo evento globale:**

```typescript
// AudioUnlocker.ts
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
};

export const unlockHowler = async () => {
    const audioContext = Howler.ctx;

    if (audioContext && audioContext.state !== 'running' && typeof audioContext.resume === 'function') {
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
};

export const isAudioUnlocked = (): boolean =>
    Howler.ctx?.state === 'running';
```

Chiamare `wireAudioUnlockOnce()` in `main.ts`, prima di qualsiasi render:

```typescript
// main.ts
wireAudioUnlockOnce();
router.resolve();
```

**Livello 2 — bottone esplicito nel menu:**

Necessario perché la musica di menu viene avviata al render (prima di qualsiasi gesture).
Il bottone permette all'utente di sbloccare esplicitamente e far ripartire la musica.

---

## Come aggiornare il bottone in base allo stato

```typescript
import { unlockHowler, isAudioUnlocked } from '../services/AudioUnlocker';

// Dopo il render HTML
const audioButton = container.querySelector<HTMLAnchorElement>('#audio-button');
if (audioButton) {
    if (isAudioUnlocked()) {
        // AudioContext già running (utente è tornato da altra pagina)
        audioButton.textContent = 'Audio attivo ✓';
        audioButton.classList.add('disabled');
    } else {
        // AudioContext suspended: bottone attivo
        audioButton.addEventListener('click', async () => {
            await unlockHowler();
            SoundManager.playMenuSoundTrack(); // rilancia la musica che era fallita
            audioButton.textContent = 'Audio attivo ✓';
            audioButton.classList.add('disabled');
        });
    }
}
```

### Tabella stati UX

| Stato AudioContext | Testo bottone   | Classe CSS        | Click         |
|--------------------|-----------------|-------------------|---------------|
| `suspended`        | "Attiva audio"  | `button`          | sblocca + play + aggiorna |
| `running`          | "Audio attivo ✓"| `button disabled` | nulla         |

---

## Snippet CSS per il bottone disabilitato

```css
.button.disabled {
    opacity: 0.5;
    pointer-events: none;
    cursor: default;
}
```

---

## Note per Vite + PixiJS

- L'import di Howler in TypeScript richiede `@types/howler`
- `Howler.ctx` è tipizzato come `AudioContext` — usare `?.` per accesso sicuro
- Non fare `await Howler.ctx.resume()` dentro un `setTimeout`: deve essere chiamato
  direttamente dal gestore dell'evento utente (la call stack deve includere la gesture)
- Safari richiede che `audioContext.resume()` sia chiamato **sincrono** rispetto alla gesture
  (non dentro una Promise lunga) — il pattern qui sopra è compatibile
