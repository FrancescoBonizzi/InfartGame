import router from './router';

export function renderScorePage(container: HTMLElement) {
    container.innerHTML = `
    <main class="main">
      <section class="card">
        <h1 class="title">Punteggi</h1>
        <p id="last">Ultimo: —</p>
        <div class="actions">
          <a href="/game" class="btn primary" data-navigo>Gioca</a>
          <a href="/" class="btn" data-navigo>Menu</a>
        </div>
      </section>
    </main>
  `;

    // Carica l'ultimo punteggio dal localStorage se esiste
    const lastScore = localStorage.getItem('lastScore');
    if (lastScore) {
        document.getElementById('last')!.textContent = `Ultimo: ${lastScore}`;
    }

    router.updatePageLinks();
}