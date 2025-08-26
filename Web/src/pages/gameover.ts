import router from './router';
import stringHelper from "../services/StringHelper.ts";

export function renderGameOverPage(container: HTMLElement) {

    const score = getScore();
    if (score === null) {
        router.navigate('/menu');
        return;
    }

    container.innerHTML = `
    <main>
      <section class="gameover">
      
        <div class="score-content">
        
            <h1 class="title">GAME OVER</h1>
            
            <div class="score-second-row">
                <div class="score-table">
                    <div class="score-row">
                        <div class="score-label">Scoregge scoreggiate:</div>
                        <div class="score-value" id="score-farts">${score.farts}</div>
                    </div>
                    
                      <div class="score-row">
                        <div class="score-label">Verdure digerite:</div>
                        <div class="score-value" id="score-vegetables">${score.vegetables}</div>
                    </div>
                    
                    <div class="score-row">
                        <div class="score-label">Metri percorsi:</div>
                        <div class="score-value" id="score-meters">${score.meters}</div>
                    </div>
                </div>
                
                <nav class="menu-actions">
                    <a href="/menu" class="button primary" data-navigo>INDIETRO</a>
                </nav>
            
            </div>
      
    </main>
  `;

    router.updatePageLinks();
}

interface GameOverParams {
    farts: number;
    vegetables: number;
    meters: number;
}

const getScore = (): GameOverParams | null => {
    const query = new URLSearchParams(window.location.search);
    const farts = parseNumberOrNull(query.get("farts"))
    const vegetables = parseNumberOrNull(query.get("vegetables"));
    const meters = parseNumberOrNull(query.get("meters"));

    if (farts === null || vegetables === null || meters === null)
        return null;

    return {
        farts,
        vegetables,
        meters
    };
}

const parseNumberOrNull = (value: string | null) => {
    if (stringHelper.isNullOrWhitespace(value))
        return null;

    const number = parseInt(value!);
    if (isNaN(number))
        return null;

    return number;
}