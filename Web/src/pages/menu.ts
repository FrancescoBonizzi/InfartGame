import router from './router';

export function renderMenuPage(container: HTMLElement) {
    container.innerHTML = `
<main>
    <section class="menu">
        
        <div class="menu-content">
            <img src="/public/assets/images/menu/infart-game-title.png" alt="Game menu title" />
            
            <nav class="actions">
                <a href="/game" class="btn primary" data-navigo>Gioca</a>
                <a href="/scores" class="btn" data-navigo>Punteggi</a>
            </nav>
        </div>
        
    </section>
</main>
`;

    router.updatePageLinks();
}