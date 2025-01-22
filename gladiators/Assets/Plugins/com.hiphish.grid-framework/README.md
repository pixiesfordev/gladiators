# Overview {#overview}


## Preface

Thank you for choosing Grid Framework for Unity3D. My goal was to create an
easy to use setup where the users only needs to say what kind of grid they want
and let the computer do all the math.

This package provides you with new  grid components (subclasses of `BaseGrid`)
which can be added to any `GameObject` in the scene, `GridRenderer` components
for displaying grids, lots of functions to use with the grids and an editor
panel for aligning and scaling objects in your scene. Every grid is
three-dimensional and infinite in size.  You can draw your grids in the editor
using gizmos, render them at runtime and even use Grid Framework along with
[Vectrosity] and [Playmaker] if you have a license for them.

By buying this package you support further development and you are entitled to
all future upgrades for free.

Alejandro "HiPhish" Sanchez


### Reading this documentation

The full user manual and scripting reference is available in HTML format from
within Unity3D's "Help" menu under "Grid Framework Documentation". The
documentation consists of two parts: the user manual and the API reference. Use
the navigation panel in the left-hand side to browse the manual topics and use
the search bar in to top right-hand corner to quickly find the reference for
classes and methods you want to use.


## Setup

Nothing special is needed if you get Grid Framework from the Unity Asset Store.
All features of Grid Framework, including the documentation, can be accessed
through the menu bar, see the @ref starting page for more information, there is
no reason to attach scripts manually.


## What is included

The package contains one main directory with further sub-directories and files:

- `Editor`: editor extensions
- `Runtime`: The core of Grid Framework
- `Documentation`: This file and the complete user manual
- `CHANGELOG.md`: Changelog, also accessible from the manual
- `README.md`: Readme file, also accessible from the manual

The directories contain the source files of Grid Framework and are required for
working. Please do not modify them directly or your changes will be overwritten
when updating. It is better to write your own grids, renderers, rendering
backends and extension methods instead, that way your custom code remains
compatible with all future updates.

Playable samples are provided as a separate download, see below. Why not
include them? Unity would treat the scenes, scripts, materials and textures as
part of your project's assets. I was to avoid spamming your project, but
unfortunately the only solution to the problem is to ship the samples
separately.


### Extra content

There are two additional packages available as as public Git repositories:

- [Playable samples](https://gitlab.com/HiPhish/grid-framework-samples/)
- [Grid align panel](https://gitlab.com/hiphish/grid-framework-align-panel/)
- [Vectrosity support](https://gitlab.com/HiPhish/grid-framework-vectrosity)
- [Playmaker actions](https://gitlab.com/HiPhish/grid-framework-playmaker)

These upgrades are free of charge; they are not included in the main
distribution as to not bloat the packages size for users who do not need them,
and to prevent compilation errors for users who do not have the extra
dependencies.

Please refer to the official
[Unity manual](https://docs.unity3d.com/Manual/upm-git.html) for information on
how to add Git dependencies to your project. Every release has its own Git tag,
so I recommend targeting a fixed release of each package. The initial tag is
always `v3.0.0`, subsequent tags follow
[semantic versioning](https://semver.org/).


## Hello Grid Framework

Let us create our first grid and perform some basic scripting on it.


### Setting up and displaying a grid

Import Grid Framework and create a new scene. From Unity's menu select
*GameObject* → *3D Object* → *Grid* → *Rectangular*. A new object will appear
in our scene with two components attached to it. If you do not see the grid
lines in scene view you will have to turn on gizmos display.

- `RectGrid` is the class of the grid, in this case a rectangular grid
- `Parallelepiped` is the class of the renderer which is what lets us see the
	grid

There are different types of grids, and the grid class (subclass of
`GridFramework.Grids.BaseGrid`) determines which type it is. In our case we
have a rectangular grid which has spacing (how far apart the lines are) and
shearing (the angle between lines). Try tweaking these properties, you will see
the grid getting updated live.

There is a problem however: grids are infinitely large, but we only want to
show a finite slice. This is where the grid renderers (subclasses of
`GridFramework.Renderers.GridRenderer`) come in: The renderer lets us choose
how much of the grid we want to show. Rectangular grids come with only one
renderer (`GridFramework.Renderers.Rectangular.Parallelepiped`), but we can
write our own renderers if we want to. We can attach multiple renderers to the
same grid if we wish to render multiple slices of it.

Position the grid at the origin of the world if it is not yet, and enter play
mode. You will not be able to see the grid in the game unless you have gizmos
turned on. In order to actually see the grid in the finished game we have to
render it using Unity's rendering facilities. There are a number of rendering
backends available, but for now we will keep it simple. Select the grid object,
and then from Unity's menu choose *Component* → *Grid Framework* → *Rendering*
→ *Local mesh rendering backend*. This will attach three new components to the
object:

- `MeshFilter` (from Unity)
- `MeshRenderer` (from Unity)
- `LocalMeshBackend` (the backend)

When you enter play mode now you should see the grid in your game. Since the
grid is being rendered using a regular mesh (with `Lines` topology) you can
affect its appearance by assigning a material to the mesh filter. Your Unity
installation should come with some basic default materials for you to try out.


### Scripting a grid

Now that we can place a grid in the scene, adjust its properties and display
it, it is time to actually write some code with it. Create a new script, let's
call it `HellGrid.cs`. The script will be simple: it takes the player's mouse
cursor position and snaps it to the nearest vertex of the grid, then draws a
little sphere at that position. Make sure you have gizmos turned on in play
mode and that your main camera is set to orthographic.


```cs
using UnityEngine;
using UnityEditor;
using GridFramework.Grids;  // All grids reside in this namespace

[RequireComponent(typeof(RectGrid))]
public class HelloGrid : MonoBehaviour {
    private RectGrid grid;  // The grid for our logic

    private void Start() {
        grid = GetComponent<RectGrid>();  // Aquire grid reference
    }

    private void OnDrawGizmos() {
        if (!EditorApplication.isPlaying)
            return;
        var input = GetWorldInput();
        var snappedPoint = SnapToGrid(input);
        Gizmos.DrawSphere(snappedPoint, 0.3f);
    }

    private Vector3 SnapToGrid(Vector3 point) {
        // Using Grid Framework to perform the calculation
        var gridPoint = grid.WorldToGrid(point);
        for (var i = 0; i < 3; ++i) {
            gridPoint[i] = Mathf.Round(gridPoint[i]);
        }

        return grid.GridToWorld(gridPoint);
    }

    private Vector3 GetWorldInput() {
        // Regular Unity code
        var result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        result.z = 0;
        return result;
    }
}
```

The `GetWorldInput` method simply converts from screen coordinates to grid
coordinates, it only works if the camera is set to orthographic. The
`SnapToGrid` method shows a common pattern:

- Take some point in world coordinates
- Convert it to grid coordinates
- Perform your logic in grid space (in this case we round all coordinates to
	the nearest integer)
- Convert back to world coordinates so Unity can understand the result

You might have noticed that the sphere can also go beyond the visible area of
the grid because grids are infinitely large. Let's use the renderer to limit
the sphere to the visible slice of the grid.

```cs
using UnityEngine;
using UnityEditor;
using GridFramework.Grids;
using GridFramework.Renderers.Rectangular;  // NEW

[RequireComponent(typeof(RectGrid))]  // NEW
[RequireComponent(typeof(Parallelepiped))]
public class HelloGrid : MonoBehaviour {
    private RectGrid grid;
    private Parallelepiped parallelepiped;  // NEW

    private void Start() {
        grid = GetComponent<RectGrid>();
        parallelepiped = GetComponent<Parallelepiped>();  // NEW
    }

    private void OnDrawGizmos() {
        if (!EditorApplication.isPlaying)
            return;
        var input = GetWorldInput();
        var snappedPoint = SnapToGrid(input);
        Gizmos.DrawSphere(snappedPoint, 0.3f);
    }

    private Vector3 GetWorldInput() {
        var result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        result.z = 0;
        return result;
    }

    private Vector3 SnapToGrid(Vector3 point) {
        var gridPoint = grid.WorldToGrid(point);
        for (var i = 0; i < 3; ++i) {
            // ALTERED
            var min = parallelepiped.From[i];
            var max = parallelepiped.To[i];
            gridPoint[i] = Mathf.Clamp(Mathf.Round(gridPoint[i]), min, max);
        }

        return grid.GridToWorld(gridPoint);
    }
}
```

If we don't count the boilerplate code we have effectively only changed one
line of code in order to limit the value of the result. This is another common
pattern: Use the grid to write your logic, and use the renderer to apply
constraints to your logic. Using the renderer has the advantage that your logic
will always match the grid you see in your game.


[Vectrosity]: http://starscenesoftware.com/vectrosity.html
[Playmaker]: https://hutonggames.com/
