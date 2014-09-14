SC Joystick Mapper V 2.4
(c) Cassini, StandardToaster - 14-September-2014

Contains 9 files:

SCJMapper.exe                The program
SCJMapper.exe.config         Program config                      - MUST be in the same folder as the Exe file
SharpDX.DirectInput.dll      Managed DirectInput Assembly        - MUST be in the same folder as the Exe file
SharpDX.dll                  Managed DirectX Assembly            - MUST be in the same folder as the Exe file
Ionic.Zip.Reduced.dll        Managed Zip Assembly                - MUST be in the same folder as the Exe file
log4net.dll                  Managed Logging Assembly            - MUST be in the same folder as the Exe file
log4net.config.OFF           Config file for logging             - To use it - rename as  log4net.config and run the program
                                                                   then look for  trace.log  in the same folder
SCJMapper_QGuide V2.4.pdf    Quick Guide
ReadMe.txt                   This file

Read the Guide first RTFM ;-)
Put all files into one folder and hit SCJMapper.exe to run it

For Updates and information visit:
https://github.com/SCToolsfactory/SCJMapper-V2/releases

Scanned for viruses before packing... 
cassini@burri-web.org

Changelog:
V 2.4
- improvement - add new actionmaps for AC 0.9 (flycam, spaceship_turret)
- improvement - supports now assignment of js1 .. js8 - SC may not support all though...
- Update of the Guide for V2.4
V 2.3
- new feature - allow reassignment of the jsN group
- improvement - uniquely identified devices with the same name (use GUID)
- improvement - shows jsN assignment in Joystick tab
- improvement - detection of the SC install path extended to one more Registry entry
- fix - blend unmapped works properly now
- fix - manual entry of SC directory works now
- Update of the Guide for V2.3
V 2.2
- new feature - option to ignore actionmaps in Settings
- improvement - add actionmaps for multiplayer, singleplayer, player
- improvement - GUI layout of Joystick tabs for more than 4 devices
- Update of the Guide for V2.2
V 2.1
- program is maintained at "https://github.com/SCToolsfactory/SCJMapper-V2/releases"
- new feature - option to blend unmapped actions
- improvement - ignore buttons in Settings
- improvement - override the built in detection of the SC folder in Settings
- added - trace log for resolving crash and other issues
- Update of the Guide for V2.1
V 2.0
- program is maintained at "https://github.com/bm98/SCJMapper-V2/releases"
- new feature - get defaultProfile.xml from game assets
- new feature - get actionsmaps from game assets
- new feature - reset to defaults
- new feature - load and save own maps to gamefolders (makes backup in My Documents\SCJMapper)
- new feature - filter the action tree
- new feature - drag and drop an mapping file into the XML window
- new feature - make throttle assignment for any axis
- improved joystick detection (jitter avoidance, limit can be set in Program config file)
- improved button detection (blend buttons that are always on)
- improved sizeable window
- improved settings persist between sessions
- Update of the Guide for V2.0
- removed defaultProfile.xml from distribution (REMOVE IT FROM YOUR FOLDER - else it will be taken as action list)
V 1.4PRE
- using a new Managed DirectX assembly and built with .Net4 (Hope this works for Win8.1)
- added Joystick properties and Axis Names from the Joystick driver
V 1.3
- new feature - read the original defaultProfile.xml from SC to derive the actions (must be in the EXE folder)
- added support for up to 8 devices
- added multibinding i.e. bind the same action to multiple buttons, one for kbd, one for xbox etc. if the profile supports it
- added Dump List - a readable list of the commands (can be saved as txt file - using Save as)
- fixed "Find 1st" 
- Update of the Guide 
- removed MappingVars file from distribution (REMOVE IT FROM YOUR FOLDER - else it will be taken as action list)
V 1.2
- added support for rebinding xboxpad and ps3pad
- added Find 1st  for a Control
- fixed Hat direction not maintained as last Control used
- some GUI refinements
- Update of the Guide (incl MappingVar.csv format)
MappingVar file
- added commands that where missing
- changed from keyboard to xboxpad rebinding where possible to leave kbd intact
V 1.1 
- fixed issue with less than 3 joysticks attached
V 1.0 initial 
