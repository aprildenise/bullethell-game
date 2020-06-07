# Untitled Bullet Hell Game
a bullet hell game for the #100daysofgamedev challenge
## Changelog
03/15 - Day 11
* Made ActivateOnProximity update the editor scene based on the given timeToActivate, instead of the given distance.
* Added VFX assets to the project for testing and reference.
* Added shadergraph to the project.

03/29 - Day 12, 2 hours
* Added FollowCurve to the Enemy.
* Added functionality to the FollowCurve and Enemy so that the Enemy can follow a curve and activate its Shooter once it has finished following a curve.
* Began documentation with FollowCurve.

04/02 - Day 13, 1 hour
* Fixing logic error where Enemy shoots all bullets once it finishes the end of its Follow Curve.

04/07 - Day 14, 2 hours
* Have to fixed above logic yet.
* Cleaning up Enemy and Shooter by creating a more robust hierarchy, which starts with the Obstacle and Shooter abstract classes.
* Implemented Obstacle, Enemy, Shooting Enemy. Testing needed.
* Began implementing Shooter abstract class.

04/19 - Day 15, 3 hours
* Fixed the above bug.
* Continued documentation.
* Continued the hiearchy further. Now completed!

05/18 - Day 18, 20 hours
* Created a new multiplicative mechanic, working on prototype.
* Created a new branch type.
* Implementing new player UI.

05/26,27 - Day 19 + 20, 26 hours
* Added new graphics to the UI.
* Implemented the weapon change.

05/31 - Day 22, 30 hours
* Implemented more weapons. Still mising sword.
* Added post processing to the stack.

06/03 - Day 23, 33 hours
* Finally implemented the Sword.
    - ManualSword and Sword.
* Expanded the Shooter hierarchy into a Weapon hierachy.
* Debounced input when opening and closing the weapon wheel.

06/04 - Day 24, 36 hours
* Added bounce effect onto Bullets. Still WIP.

06/07 - Day 25, 38 hours
* Finished bounce effect onto Bullets (for now)
* Fixed laser into a LineRenderer and a Laser/ManualLaser class.
