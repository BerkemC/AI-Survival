using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Spawner : MonoBehaviour {
	#region Variables
	[Header("Basic Variables")]
	[Tooltip("When checked, the spawner starts to spawn the given objects with Start method, at the start of its lifetime.")]
	[SerializeField]
	private bool isAtStart;
	[Tooltip("When checked, the spawner starts spawning after the entered delay time.")]
	[SerializeField]
	private bool isDelayed;
	[Tooltip("Delay time for the spawner.")]
	[SerializeField]
	private float delay;
	[Tooltip("The parent object that the spawner will set for the spawned objects.")]
	[SerializeField]
	private GameObject parent;
	[Header("Spawn Location")]
	[Tooltip("When checked, the spawner will spawn the objects at the parent's position.")]
	[SerializeField]
	private bool spawnAtParentLocation;
	[Tooltip("When given an object, the object's location will be used as a spawn position.")]
	[SerializeField]
	private Transform spawnAtObjectLocation;
	[Tooltip("A vector 3 position for object(s) to be spawned at.")]
	[SerializeField]
	private Vector3 spawnLocationVector;
	[Header("Position Randomization")]
	[Tooltip("When checked, a random range for the spawn location's x,y and z axes is created and spawnings are done according to this range.")]
	[SerializeField]
	private bool isRandomized;
	[Tooltip("Creates a range between specified location +  min xOffset and max xOffset, added to the specified x position of the location.")]
	[SerializeField]
	private Vector2 xOffset;
	[Tooltip("Creates a range between specified location + min yOffset and max yOffset, added to the specified y position of the location.")]
	[SerializeField]
	private Vector2 yOffset;
	[Tooltip("Creates a range between specified location + min zOffset and max zOffset, added to the specified z position of the location.")]
	[SerializeField]
	private Vector2 zOffset;
	[Header("Spawn Type")]
	[Tooltip("When checked, spawner only spawns clones of one object.")]
	[SerializeField]
	private bool isSingleObject;
	[Tooltip("When checked, spawner spawns clones of a list of objects.")]
	[SerializeField]
	private bool isMultipleObject;
	[Header("Object References")]
	[Tooltip("Object that will be used to spawn for single object spawning.")]
	[SerializeField]
	private GameObject spawnObject;
	[Tooltip("Objects that will be used to spawn one by one by spawner.")]
	[SerializeField]
	private List<GameObject> spawnObjectList;
	[Header("Repetition and Frequency")]
	[Tooltip("Determines the number of times that the single object spawning will occur.")]
	[SerializeField]
	private int repetitionForSingleObject;
	[Tooltip("Determines the number of times that the multiple object spawning will occur.")]
	[SerializeField]
	private int repetitionForMultiObject;
	[Tooltip("Determines the frequency between single object spawning repetitions.")]
	[Range(0f,1f)]
	[SerializeField]
	private float singleObjectFrequency;
	[Tooltip("Determines the frequency between multiple object spawning repetitions.")]
	[Range(0f,1f)]
	[SerializeField]
	private float multiObjectFrequency;
	[Tooltip("Determines the frequency between spawning of each object in a multiple object spawning.")]
	[Range(0f,1f)]
	[SerializeField]
	private float betweenMultiObjectsFrequency;
	[Header("After Spawning Complete")]
	[Tooltip("When checked, the spawner object will be destroyed after all the spawnings are done.")]
	[SerializeField]
	private bool isDeleteAfterCompletion;
	//------------------------------------//
	private int multiObjectCount = 0;
	#endregion
	#region Start Calculations
	// Use this for initialization
	void Start () {
		//If spawning is requested to start with Start method
		if(isAtStart){
			//If single object spawning is selected
			if(isSingleObject){
				
				SpawnSingleObject ();
			}
			//If multiple object spawning is selected
			else if(isMultipleObject){

				SpawnMultiObject ();


			}

		}
		//If there is a time delay
		else if(isDelayed){

			//If single object spawning is selected
			if (isSingleObject) {
				
				Invoke ("SpawnSingleObject", delay);
			}

			//If multiple object spawning is selected
			else if (isMultipleObject) {

				Invoke ("SpawnMultiObject", delay);

			}
		}
	}
	#endregion
	#region Update Calculations
	// Update is called once per frame
	void Update () {

		//If single object spawning is selected
		if(isSingleObject){

			//Checking the frequency
			if(Random.value < singleObjectFrequency * Time.deltaTime){
				SpawnSingleObject ();
			}

		}

		//If multiple object spawning is selected
		else if(isMultipleObject){

			//Checking the frequency
			if(Random.value < multiObjectFrequency * Time.deltaTime){
				
				SpawnMultiObject ();
			}
		}
		
	}
	#endregion
	#region Setters and Getters

	public bool IsAtStart {
		get {
			return isAtStart;
		}
		set {
			isAtStart = value;
		}
	}

	public bool IsDelayed {
		get {
			return isDelayed;
		}
		set {
			isDelayed = value;
		}
	}

	public float Delay {
		get {
			return delay;
		}
		set {
			delay = value;
		}
	}

	public GameObject Parent {
		get {
			return parent;
		}
		set {
			parent = value;
		}
	}

	public bool SpawnAtParentLocation {
		get {
			return spawnAtParentLocation;
		}
		set {
			spawnAtParentLocation = value;
		}
	}

	public Transform SpawnAtObjectLocation {
		get {
			return spawnAtObjectLocation;
		}
		set {
			spawnAtObjectLocation = value;
		}
	}

	public Vector3 SpawnLocationVector {
		get {
			return spawnLocationVector;
		}
		set {
			spawnLocationVector = value;
		}
	}

	public bool IsRandomized {
		get {
			return isRandomized;
		}
		set {
			isRandomized = value;
		}
	}

	public Vector2 XOffset {
		get {
			return xOffset;
		}
		set {
			xOffset = value;
		}
	}

	public Vector2 YOffset {
		get {
			return yOffset;
		}
		set {
			yOffset = value;
		}
	}

	public Vector2 ZOffset {
		get {
			return zOffset;
		}
		set {
			zOffset = value;
		}
	}

	public bool IsSingleObject {
		get {
			return isSingleObject;
		}
		set {
			isSingleObject = value;
		}
	}

	public bool IsMultipleObject {
		get {
			return isMultipleObject;
		}
		set {
			isMultipleObject = value;
		}
	}

	public GameObject SpawnObject {
		get {
			return spawnObject;
		}
		set {
			spawnObject = value;
		}
	}

	public List<GameObject> SpawnObjectList {
		get {
			return spawnObjectList;
		}
		set {
			spawnObjectList = value;
		}
	}

	public int RepetitionForSingleObject {
		get {
			return repetitionForSingleObject;
		}
		set {
			repetitionForSingleObject = value;
		}
	}

	public int RepetitionForMultiObject {
		get {
			return repetitionForMultiObject;
		}
		set {
			repetitionForMultiObject = value;
		}
	}

	public float SingleObjectFrequency {
		get {
			return singleObjectFrequency;
		}
		set {
			singleObjectFrequency = value;
		}
	}

	public float MultiObjectFrequency {
		get {
			return multiObjectFrequency;
		}
		set {
			multiObjectFrequency = value;
		}
	}

	public bool IsDeleteAfterCompletion {
		get {
			return isDeleteAfterCompletion;
		}
		set {
			isDeleteAfterCompletion = value;
		}
	}
	#endregion


	#region Methods
	/// <summary>
	/// Adds the offset to the given vector, and generates a random vector withing offset range.
	/// </summary>
	/// <returns>The offset to vector.</returns>
	/// <param name="currentVector">Current vector.</param>
	private Vector3 AddOffsetToVectorAndRandomize(Vector3 currentVector){

		return new Vector3 (Random.Range (currentVector.x + xOffset.x, currentVector.x + xOffset.y),
			Random.Range (currentVector.y + yOffset.x, currentVector.x + yOffset.y),
			Random.Range (currentVector.z + zOffset.x, currentVector.x + zOffset.y)); 
	}
	/// <summary>
	/// Sets to object location for the spawned object, considering the randomization.
	/// The new position for the object is returned.
	/// </summary>
	/// <returns>The to object location.</returns>
	private Vector3 SetToObjectLocation(){

		if(isRandomized){
			return AddOffsetToVectorAndRandomize (spawnAtObjectLocation.transform.position);
		}

		return spawnAtObjectLocation.transform.position;

	}
	/// <summary>
	/// Sets to vector location for the spawned object, considering the randomization.
	/// The new position for the object is returned.
	/// </summary>
	/// <returns>The to vector location.</returns>
	private Vector3 SetToVectorLocation(){

		if(isRandomized){
			return AddOffsetToVectorAndRandomize (spawnLocationVector);
		}

		return spawnLocationVector;
	}
	/// <summary>
	/// Sets to parent location for the spawned object, considering the randomization.
	/// The new position for the object is returned
	/// </summary>
	/// <returns>The to parent location.</returns>
	private Vector3 SetToParentLocation(){

		if(isRandomized){

			return AddOffsetToVectorAndRandomize (Vector3.zero);
		}

		return Vector3.zero;
	}

	/// <summary>
	/// Spawns the single object,decrementing the repetition count.
	/// </summary>
	public void SpawnSingleObject(){
		//If repetition is needed
		if(repetitionForSingleObject > 0){

			//If parent is given
			if (parent != null) {

				//Generate without any parent and just with offset
				GameObject clone = Instantiate (spawnObject, parent.transform) as GameObject;
				//Set position for clone to 0
				clone.transform.position = Vector3.zero;

				//If parent is set for location
				if (spawnAtParentLocation) {
					
					clone.transform.position = SetToParentLocation ();
				}
				//If there is an object reference for location
				else if (spawnAtObjectLocation != null) {

					clone.transform.position = SetToObjectLocation ();

				}
				//If there is a vector for location
				else {

					clone.transform.position = SetToVectorLocation ();
				}
			}
			//If parent is not given
			else {
					
				//If there is an object reference for location
				if(spawnAtObjectLocation != null){

					Instantiate (spawnObject,SetToObjectLocation (),Quaternion.identity);

				}
				//If there is a vector for location
				else{

					Instantiate (spawnObject,SetToVectorLocation (),Quaternion.identity);

				}

			}
			//Decrement repetition number
			repetitionForSingleObject--;
		}
	
	}

	/// <summary>
	/// Spawns the single object,decrementing the repetition count.
	/// </summary>
	public void SpawnSingleObjectForMultiple(){
		//Set the current object from the list
		GameObject objectToSpawn = spawnObjectList [multiObjectCount];
			//If parent is given
			if (parent != null) {

				//Generate without any parent and just with offset
				GameObject clone = Instantiate (objectToSpawn, parent.transform) as GameObject;
				//Set position for clone to 0
				clone.transform.position = Vector3.zero;

				//If parent is set for location
				if (spawnAtParentLocation) {

					clone.transform.position = SetToParentLocation ();
				}
				//If there is an object reference for location
				else if (spawnAtObjectLocation != null) {

					clone.transform.position = SetToObjectLocation ();

				}
				//If there is a vector for location
				else {

					clone.transform.position = SetToVectorLocation ();
				}
			}
			//If parent is not given
			else {

				//If there is an object reference for location
				if(spawnAtObjectLocation != null){

					Instantiate (objectToSpawn,SetToObjectLocation (),Quaternion.identity);

				}
				//If there is a vector for location
				else{

					Instantiate (objectToSpawn,SetToVectorLocation (),Quaternion.identity);

				}

			}

		multiObjectCount++;
		//Reset if more than or equal to list size
		if(multiObjectCount >= spawnObjectList.Count){
			
			multiObjectCount = 0;
		}
	}

	void SpawnMultiObject ()
	{
		if (repetitionForMultiObject > 0) {
			
			spawnObjectList.ForEach (delegate (GameObject obj) {
				
				Invoke ("SpawnSingleObjectForMultiple", Random.Range (0f, 1f / betweenMultiObjectsFrequency));

			});

			repetitionForMultiObject--;
		}
	}
	#endregion
}
