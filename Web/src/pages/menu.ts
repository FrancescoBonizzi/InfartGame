import router from './router';

export function renderMenuPage(container: HTMLElement) {
    container.innerHTML = `
<main>
    <section class="menu">
        
        <div class="menu-content">
            
            <h1 class="title">
              <span class="in">IN</span><span class="fart">FART</span>
            </h1>
            
            <nav class="menu-actions">
                <a href="/game" class="button primary" data-navigo>GIOCA</a>
                <a class="button button fart">SCOREGGIA</a>
                <a href="/scores" class="button" data-navigo>PUNTEGGIO</a>
                <a class="button" target="_blank" href="https://imaginesoftware.it/open-source-projects/infart">ABOUT</a>
            </nav>
            
        </div>
        
    </section>
</main>
`;

    router.updatePageLinks();
}