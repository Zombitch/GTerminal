# Introduction

GTerminal aim to simulate a terminal behaviour.
Up to you to implement the commands the user can enter.

# Prerequisites

Install TextMeshPro :
Window -> PAckage Manager -> Select "Unity Registry" -> Text Mesh Pro -> Install

Install TMP Essentials :

Window -> Text Mesh Pro -> Import TMP Essential Ressources -> Select all and import

# How to use

## Display terminal
Drag and drop Terminal prefab into a canvas.
In Terminal inspector, link Canvas attribute to the canvas.
Terminal will expand its own size to fit canvas size.

## Managing commands

Create a new script file "ExampleCommand.cs" add a function with two parameters (first is a reference to GTerminal and second is a String corresponding to the executed command).
Use this script and function in the Terminal Inspector to set an "OnCommandLineEvent".

# Example

Check Scene/SampleScene and Scripts/SampleCommandeLineManager

