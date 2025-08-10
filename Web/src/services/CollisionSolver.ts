import IHasCollisionRectangle from "../IHasCollisionRectangle.ts";
import {Rectangle} from "pixi.js";

export default {

    checkCollisions: (
        who: Rectangle,
        withRectangles: Rectangle[]) : boolean =>
    {
        for (const element of withRectangles) {
            if (who.intersects(element)) {
                return true;
            }
        }

        return false;
    },

    checkCollisionsReturnCollidingRectangle: (
        who: Rectangle,
        withRectangles: Rectangle[]) : Rectangle | null =>
    {
        for (const element of withRectangles) {
            if (who.intersects(element))
                return element;
        }

        return null;
    },

    checkCollisionsReturnCollidingObjectSpecific: <TWho extends IHasCollisionRectangle, TWith extends IHasCollisionRectangle>(
        who: TWho,
        withRectangles: TWith[]
    ): TWith | null => {
        for (const element of withRectangles) {
            if (who.collisionRectangle.intersects(element.collisionRectangle)) {
                return element;
            }
        }
        return null;
    }

}