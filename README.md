# FloatingMesh - fast approximate and exact buoyancy calculation for meshes.

This Unity implementation is part of a paper currently under review. Details will follow later.

![short_demo_640x480](https://github.com/user-attachments/assets/e4dde61f-453d-41b3-a11e-f4590dd3af6d)

## How to use?
### 1. Add .cs files to your project.

<img width="799" alt="01_load_scripts" src="https://github.com/user-attachments/assets/1dc5de56-2868-431a-803d-3d043acbd48f" />

### 2. Create a plane representing the fluid surface. 

Do not forget to disable its collider component (or set to trigger).

<img width="799" alt="02_add_plane" src="https://github.com/user-attachments/assets/50af2430-deb4-48d3-afe2-90448a1e516f" />

### 3. Add a new object whose motion you wish to simulate. 

Transform it as you like.

<img width="799" alt="03_cube_settings" src="https://github.com/user-attachments/assets/e7fcd87c-77a1-4d06-873c-3896f06dc467" />

### 4. Add a FloatingMesh component to the object. 

This automatically adds a Rigidbody component, as well.

<img width="799" alt="04_add_floatingmesh" src="https://github.com/user-attachments/assets/0c462a9e-0a56-4ba5-9112-b2d779a06700" />

### 5. Set the physical parameters.

Focus on following properties.
  - Mass
  - Drag
  - Angular drag
  - Fluid level
  - Fluid density 

<img width="799" alt="05_physics_settings" src="https://github.com/user-attachments/assets/b164266b-0437-453c-bfab-8ff0d628fb8b" />

You should also select the calculation method:
  - Fast: most efficient, works fine with large, dense meshes.
  - Improved: more expensive, but works fine also with coarse meshes.
  - Exact: the most expensive, but the most accurate algorithm.
    
The fluid level should be the y-coordinate of the plane representing the fluid. 

**Important note**

The floating motion is a result of the upward-pointing buoyant force and the downward-pointing gravity force. Floating motion can be experienced when the object density is less than the fluid density. The object density is the quotient of its mass (you can set) and its volume (calculated by SubmergedPartCalculator class). So, if you see your object submerging without perceptible resistance, increase fluid density or decrease the object's mass. Similarly, if your object is "bouncing" on the fluid surface, increase its mass or decrease the fluid density. 

### Custom meshes

Certainly, you can also use this component on your own meshes. In this case, please ensure the Read/Write property is enabled in import settings.

<img width="799" alt="06_custom_mesh" src="https://github.com/user-attachments/assets/ae7c4bc5-6bda-47f9-9d82-5d5fd7a0333b" />
