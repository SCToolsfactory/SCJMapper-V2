SC Joystick Mapper V 1.4PRE 
(c) Cassini - 14-June-2014

Contains 4 files:

SCJMapper.exe                The program
SharpDX.DirectInput.dll      Managed DirectInput Assembly        - MUST be in the same folder as the Exe file
SharpDX.dll                  Managed DirectX Assembly            - MUST be in the same folder as the Exe file
defaultProfile.xml           The default map from SC Build 12.2  - MUST be in the same folder as the Exe file
SCJMapper_QGuide V1.4.pdf    Quick Guide
ReadMe.txt                   This file

Read the Guide first RTFM ;-)
Put all files into one folder and hit SCJMapper.exe to run it

Scanned for viruses before packing... 
cassini@burri-web.org

Changelog:
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
