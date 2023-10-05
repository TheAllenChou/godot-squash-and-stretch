## Squash & Stretch - Plugin for Godot
by **Ming-Lun "Allen" Chou** / [AllenChou.net](http://AllenChou.net) / [@TheAllenChou](http://twitter.com/TheAllenChou) / [Patreon](https://www.patreon.com/TheAllenChou)

![squash-and-stretch-2d](/img/squash-and-stretch-2d.gif) ![squash-and-stretch-3d](/img/squash-and-stretch-3d.gif)

Applies squash & stretch effects based on velocity.  
This is my first Godot project. I hope you like it.

### Usage

To enable the plugin, the C# scripts need to be built first:
1. Create/Update the C# solution: Project > Tools > C# > Create C# Solution
2. Press the Build button at the top-right corner of the editor.
3. Enable the plugin: Project > Plugins > Enable the Squash & Stretch plugin

Then, just add a squash & stretch node to the desired node as a child.

![nodes](/img/nodes.png)

## Overview

Welcome to the Squash & Stretch plugin project for the Godot game engine. This project provides a set of tools and utilities to add dynamic squash and stretch effects to your 2D and 3D nodes in Godot. The plugin includes custom types for Squash & Stretch 2D and 3D nodes, utility functions for working with vectors and quaternions, and spring trackers for creating animations with squash and stretch effects. The project also includes test scenes and scripts for 2D and 3D objects with squash and stretch effects, as well as mouse manipulators for interactive object manipulation.

The Squash & Stretch plugin is designed to be easy to integrate into your existing Godot projects. It provides a flexible and powerful framework for adding squash and stretch effects to your game objects, enhancing the visual appeal and dynamism of your animations. Whether you're creating a fast-paced action game or a slow, atmospheric exploration game, the Squash & Stretch plugin can help bring your game objects to life.

## Technologies and Frameworks

This project is built with and for the Godot game engine, and is written in C#. It uses the .NET framework as specified in the project.godot engine configuration file. The project includes several C# scripts, including `SquashAndStretchPlugin.cs`, `QuaternionUtil.cs`, `SpringTracker.cs`, `VectorUtil.cs`, `MathUtil.cs`, `MouseManipulator3D.cs`, `MouseFollower2D.cs`, `PostRender2D.cs`, `PostRender3D.cs`, `SquashAndStretch3D.cs`, and `SquashAndStretch2D.cs`. These scripts provide the core functionality of the Squash & Stretch plugin.

The project also includes several scene files for testing and demonstration purposes, including `SquashAndStretchTest3D.tscn`, `SquashAndStretchTest2D.tscn`, and `Satellite.tscn`. These scenes showcase the capabilities of the Squash & Stretch plugin in a variety of contexts.

The project is licensed under the MIT License, which allows for free use, modification, and distribution of the software, provided that the copyright notice and permission notice are included.
# Installation

Follow these steps to install and start working with the project:

1. **Install Godot Game Engine**
   The project requires the Godot game engine to be installed. You can download it from the official Godot website.

2. **Clone the Repository**
   Clone the repository to your local machine using the following command in your terminal:
   ```
   git clone https://github.com/TheAllenChou/godot-squash-and-stretch.git
   ```
3. **Install the Squash & Stretch Plugin**
   The project requires the Squash & Stretch plugin. After cloning the repository, place the plugin files in the appropriate directory within the Godot project.

4. **Configure the Project**
   Open the `project.godot` file and ensure the following configurations are set:
   - The "squash-and-stretch" configuration is enabled.
   - The main scene for the application is located at "res://addons/squash-and-stretch/test/SquashAndStretchTest3D.tscn".
   - The application version is set to "4.1".
   - The dotnet project's assembly name is "squash-and-stretch".
   - The editor plugin located at "res://addons/squash-and-stretch/plugin.cfg" is enabled.
   - The rendering method for the application is "gl_compatibility" for both desktop and mobile platforms.

5. **Install the Required Scripts**
   The project requires several scripts to be installed. These include:
   - `SquashAndStretchPlugin.cs`
   - `QuaternionUtil.cs`
   - `SpringTracker.cs`
   - `VectorUtil.cs`
   - `MathUtil.cs`
   - `MouseManipulator3D.cs`
   - `MouseFollower2D.cs`
   - `PostRender2D.cs`
   - `PostRender3D.cs`
   - `SquashAndStretch3D.cs`
   - `SquashAndStretch2D.cs`

6. **Install the Required Scenes**
   The project requires the following scenes to be installed:
   - `SquashAndStretchTest3D.tscn`
   - `SquashAndStretchTest2D.tscn`
   - `Satellite.tscn`

7. **Run the Project**
   After completing the above steps, you should be able to run the project in the Godot game engine.
