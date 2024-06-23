import {Rectangle} from "pixi.js";

export default {

    checkCollisions: (
        who: Rectangle,
        withRectangles: Rectangle[]) =>
    {
        withRectangles.forEach((element) => {
           if (who.intersects(element))
               return ;
        });

        return false;
    },

    checkCollisionsReturnCollidingObject: (
        who: Rectangle,
        withRectangles: Rectangle[]) =>
    {
        for (const element of withRectangles) {
            if (who.intersects(element))
                return element;
        }

        return null;
    }

}