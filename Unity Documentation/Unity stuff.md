#### Game Objects and Prefabs
Game Objects are  instances of a prefab, they have their own position and rotation, can override others, but the initial parameters are set in the prefab. 
The prefabs are the model for the game objects, you can initialize instances of the prefab through scripts. (Object.Instantiate())

#### C# Scripts
__Monobehaviour__
- Base class that many scripts derive from
- They always exist as  a component of a GameObject
	- Instantiated with GameObject.AddComponent()
	- If object  needs to exist independently of GameObject → Derive from ScriptableObject instead
*Methods of Monobehaviour*
- Start() → Called before the first frame update
- Update() → Called once per frame
- Awake() → Unity calls when object is active

__Serialization__
Process of turning an object/data structure into a format that can be easily stored/transmitted. In C# the std is JSON serialization.
*SerializeField*
Atribute of the UnityEngine Core Module that serializes private fields. This is done through an internal Unity serialization system, not .NET std.
- Range(x, y) → Gives a slider to modify the variables from x to y (var = n ⇒ default)
#### Vectors
__Rotation of gameobjects__
- done through assigning a quaternion to the localRotation property of a Transform
__Position of gameobjects__
- done through assigning a Vector3 to the localPosition property
__Scale of gameobjects__
- done through assigning a Vector3 to the localScale property

(2.3)