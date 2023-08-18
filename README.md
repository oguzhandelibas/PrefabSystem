# PrefabSystem w-PrototypePattern
## Unity Prefab Clone and Geometric Shape Generator

ProceduralCloner is a Unity project that provides functionalities to clone prefabs and generate different geometric shapes procedurally.

## Features

- **Prefab Cloning:** Clone predefined geometric shapes (Cube and Sphere) at specified positions.
- **Dynamic Shape Generation:** Generate 3D shapes like cubes and spheres using procedural methods.
- **Object Pooling:** Improve performance by using object pooling techniques for instantiated objects.
- **Easy to Use:** The provided functions make it simple to create clones and procedural shapes in your Unity projects.

## Installation

1. Clone this repository to your local machine.
2. Open the project in Unity.
3. Explore the `ProceduralCloner` script for functions and usage examples.

## Usage

You can use the `ProceduralCloner` script to clone prefabs or create procedural shapes:

```csharp
// Clone a cube at a specific position
ProceduralCloner.Clone(new Vector3(0f, 0f, 0f), "Cube");

// Create a procedural sphere at a specific position
ProceduralCloner.CreateSphere(new Vector3(2f, 0f, 0f));
