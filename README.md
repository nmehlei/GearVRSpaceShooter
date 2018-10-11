# GearVRSpaceShooter

## Description

GearVRSpaceShooter is a mobile-oriented 3D VR-based Science Fiction Space Shooter that uses a gyroscope-based controller to emulate a joystick as input medium. It was implemented with Unity as well as the Oculus SDK.

The project idea had it's origin in the dire need to have the most immersive flight input method in a world where there are no (or very few) joystick-like input methods available for mobile devices. It was developed in the context of an academic exercise to showcase a component implementation of realtime interactive input handling.

Though there is, as of now, no further development planned, I am open to anyone who wants to contribute in some way.

## Screenshot

![Screenshot](https://raw.githubusercontent.com/nmehlei/GearVRSpaceShooter/master/Documents/Screenshot.jpg =250x250 "Screenshot")

## Usage of demo app

### Requirements

To properly run the game, one needs:
* Samsung Gear VR
* Samsung Gear VR Controller
* Compatible Smartphone (test device was a Samsung Galaxy S7)

### Setup / Install

1. Install Oculus on your mobile device
2. Activate ADB on your mobile device

   If you need to upload the game via WiFi, use *adb tcpip 5555* and *adb connect [your-ip]:5555* to connect to your mobile device while it is plugged in your pc, then disconnect it afterwards.

3. Clone the project to your development computer
4. Open the project in Unity
5. Click *Build & Run*
6. Enjoy
7. Send me your high score

### Controls

* __3-axis movement__: Rotate ship
* __Touch touchpad__: Set speed, based on y-axis position of touch
* __Press touchpad__: Enable/Disable 3-axis movement
* __Back key__: Reset level
* __Trigger__: Shoot projectiles
* __Hold home key__: Recalibrate controller (via Oculus SDK)

The cockpit was designed to help you visualize your current input. Should help to understand quickly how all of it works.

### Goal

Fly through all 14 rings in the lowest possible time. Don't fly too fast or you'll overshoot and very likely have a veeery bad score.

## Component information

The main component inside the source code is the *MovementControlComponent*. It uses the Gear VR Controller input data to extract rotational data as well as touchpad touch and click data to control a player object (like a space ship).

It can be configured to multiple scenarios and preferences. For example, it contains two variants for deadzone handling (strict and flexible, or axis and radial), since different projects could require different restrictions. Additionally, it contains a prefab to quickstart a cockpit-implementation with UI elements for visualization of input data.

It can easily be used for other projects, as long as the Oculus SDK is included as well. During development, there were curious things that happened with Oculus' calibration, so it is adviced to use the newest available SDK version.

### Component usage

Same requirements as mentioned above apply for the component. 

1. Import all Script files from *Assets/Scripts/MovementControl* to your unity project.
2. Add script *MovementControlComponent* to your player object
3. Configure script properties to suit your needs
4. If you like, you can use the prefab from *Assets/Prefabs/CockpitControlsTemplate* to have all the necessary objects for a basic cockpit
5. Enjoy

### Dependencies

* Unity (Version 2017.3.0f3 was used during development)
* Oculus Integration Package from Unity Asset Store (Version 1.18 was used during development)

Newer versions than this should most likely work as well, though earlier versions of the Oculus Integration Package did seem to have problems during calibration of the Gear VR controller.

## Additional files

There are three additional files inside the directory "Documents":

* __VRSpaceShooter-PitchDocument.pdf__: The pitch document for this project (in german)
* __VR-Presentation.pptx__: The presentation slides (in german)
* __Demo-Video.mp4__: A demo video to show the game in action, as a Virtual Reality game is hard to show in pictures

## Tests

The source code includes several unit test classes that allow testing of all important calculation and utility methods. These can be found under the project *GearVRSpaceShooter.Editor* inside the folder *Assets/Editor/*

## License

[MIT License](/LICENSE)
