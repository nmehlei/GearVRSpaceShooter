# GearVRSpaceShooter

## Description

GearVRSpaceShooter is a mobile-oriented VR-based Science Fiction Space Shooter (yes I am awesome at naming stuff) that uses a gyroscope-based controller to emulate a joystick as input medium.

It was developed in the context of an academic exercise to show an implementation of realtime interactive input handling.

Though there is, as of now, no further development planned, I am open to anyone who wants to contribute in some way.

## Usage

To properly run the game, one needs:
* Samsung Gear VR HMD
* Samsung Gear VR Controller
* Compatible Smartphone (test device was a Samsung Galaxy S7)

## Component information

The main component inside the source code is the *MovementControlComponent*. It uses the Gear VR Controller input data to extract rotational data aswell as touchpad touch and click data to control a player object (like a space ship).

## Additional files

There are three additional files inside the directory "Documents":

* __VRSpaceShooter-Projektbeschreibung.pdf__: The pitch document for this project
* __VR-Presentation.pptx__: The presentation slides
* __Demo-Video.mp4__: A demo video to show the game in action, as a Virtual Reality game is hard to show in pictures

## Tests

The source code includes several unit test classes that allow testing of all important calculation and utility methods. These can be found under the project *GearVRSpaceShooter.Editor* inside the folder *Assets/Editor/*

## License

[MIT License](/LICENSE)
 
