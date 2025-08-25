import router from './router';

export function renderMenuPage(container: HTMLElement) {
    container.innerHTML = `
<main class="main">
    <section class="card">
        <h1 class="title">INFART</h1>
        <div class="actions">
            <a href="/game" class="btn primary" data-navigo>Gioca</a>
            <a href="/scores" class="btn" data-navigo>Punteggi</a>
        </div>
    </section>
</main>
`;

    router.updatePageLinks();
}