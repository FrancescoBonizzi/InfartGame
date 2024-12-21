import Gemma from "./Gemma.ts";
import Camera from "../world/Camera.ts";
import {Point, Texture} from "pixi.js";

class Hamburger extends Gemma {

 constructor(
     world: Camera,
     texture: Texture,
     position: Point) {
     super(world, texture, position);

 }



}

export default Hamburger;