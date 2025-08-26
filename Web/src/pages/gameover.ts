import router from './router';
import ScoreRepository from "../services/ScoreRepository.ts";

export function renderGameOverPage(container: HTMLElement) {

    container.innerHTML = `
    <main>
      <section class="gameover">
      
        <div class="score-content">
        
            <h1 class="title">GAME OVER</h1>
            
            <div class="score-second-row">
                <div class="score-table">
                     <div class="score-row">
                        <div class="score-label">Scoregge scoreggiate:</div>
                        <div class="score-value" id="score-farts">${ScoreRepository.getScore('farts', 'gameover')}</div>
                    </div>
                    
                      <div class="score-row">
                        <div class="score-label">Verdure digerite:</div>
                        <div class="score-value" id="score-vegetables">${ScoreRepository.getScore('vegetables', 'gameover')}</div>
                    </div>
                    
                    <div class="score-row">
                        <div class="score-label">Metri percorsi:</div>
                        <div class="score-value" id="score-meters">${ScoreRepository.getScore('meters', 'gameover')}</div>
                    </div>
                </div>
                
                <nav class="menu-actions">
                    <a href="/menu" class="button primary" data-navigo>MENU</a>
                </nav>
            
            </div>
      
    </main>
  `;

    router.updatePageLinks();
}
