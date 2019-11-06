# CapstoneProject_VR_Workspace
I am the co-author with 5 other contributors on this project. This project was a capstone project for CSE students at the University of Texas at Arlington that was 2 semesters long and ended in the spring of 2019.

1 PRODUCT CONCEPT

  This section describes the purpose, use and intended user audience for VR Work Space developed by
  the On Point development team.
  
  
1.1 PURPOSE AND USE

  VR Work Space is a virtual environment, meant to simulate the office space of your dreams. With a
  fully customizable background environment and the ability to have each individual desktop program
  run as its own virtual screen, VR Work Space is expected to increase productivity and ease of use while
  immersed in the application.
  
1.2 INTENDED AUDIENCE


  VRWork Space is intended to be geared towards developers to work on coding projects in a VR space. In
  order for this application to be a viable option for developers, it needs to be as easy to use as a traditional
  computer with a mouse and keyboard. VR Work Space is designed with developers in mind but that by
  no means implies that the regular user can not use this application as a work station for other types of
  projects or even just regular computer usage (such as browsing the web, writing in word, etc.).
  
2 PRODUCT DESCRIPTION

  This section provides the reader with an overview of the VRWork Space project and it’s general concept.
  The general features, functions, and interface to be implemented in the system are described below.
  
2.1 FEATURES & FUNCTIONS

  The implementation will include a specific work-space/environment for each individual project. Meaning,
  based on the specific project that is loaded, the developer would have all the tools, screens, programs,
  frameworks, etc that they need specifically for the project they are working on with the ability
  to pick up exactly where they left off previously. The virtual environment will include a static desk in
  version 1 of the system, with a virtual keyboard positioned as it is in the real world. The keyboard will
  be tracked liked the hand controllers are. The user will have the ability to add screens and components
  to the environment to assist in development. As seen in Figure 1, the screens are floating and will give
  the user the ability to move the screens around as preferred. The user will be able to add components to
  the environment to further assist, such as the whiteboard and pens shown in Figure 1 that the user can
  interact with. This will all be set in a fixed space, and in Figure 1 you can see that the grey area ends
  and where the terrain begins the user will be able to load in custom terrains beyond this fixed space.
  The system will allow the user to control their desktop limitation free. This desktop will initially be on
  a screen in version 1 of the system. The user will have the ability to create a screen with each active
  window on their desktop. There will be a subsection of this fixed space to be used as a testing unit. This
  testing unit will display real time output of whatever project they are working on. This feature will be
  scale-able to allow the user to enter a VR demo when developing VR projects. Small scale the testing
  unit will display a console with the output window of the active development environment.
  
2.2 EXTERNAL INPUTS & OUTPUTS

  The external elements that will be accessible to the user is any programs on the host machine. The
  external elements that the system will use will be OS hooks that supply the system the necessary I/O
  actions from the keyboard, as well as the feeds from each window being ported to the system for display.
  Outputs will include sending the keyboard and input data from the user to the correct window that is currently
  active in the virtual space as will as translating and sending the os the proper clicks/interactions
  with the window elements. Project files will be generated and saved on the host system.
  
2.3 PRODUCT INTERFACES

  As shown in Figure 1 concept, all the active screens and components that the user has brought into
  the virtual environment will be visible until the user chooses otherwise. They will be able to navigate
  around the fixed space freely but restricted to this space. The actual method of navigating the fixed
  space in relation to their actual space will depend on the hardware they have activated on Steam VR
  and actual space around them, which will be mapped as visible boundaries in the configuration phase
  of the Steam VR plugin. The mouse will be usable to the user to navigate their desktop window (not
  tracked in version 1). However, pointer use on all other windows will be done via an integration of a
  single Vive controller combined with hand gestures via an optional hand controller. Not shown in Figure
  1, a control unit will be accessible to the user in the virtual space in order for them to add components,
  open programs, etc. Version 1 will include a version of this that the user will interact with via their hand
  controller, and the unit itself will be visible as part of the desk. Without a hand controller, this unit will
  be controlled with the Vive controller.
