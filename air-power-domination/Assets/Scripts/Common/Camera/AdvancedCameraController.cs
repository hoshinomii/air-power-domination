using UnityEngine;
using UnityEngine.Serialization;

namespace RDP.Common.Camera {
	public class AdvancedCameraController : MonoBehaviour {
		private UnityEngine.Camera camera => GetComponentInChildren<UnityEngine.Camera>();

		[FormerlySerializedAs("FollowTransform")] [Header("Settings")]
		public Transform followTransform;

		public Transform cameraTransform;


		public float normalSpeed;
		public float fastSpeed;

		public float moveSpeed;
		public float moveTime;


		// public float rotationAmount;
		public Vector3 zoomAmount;
		[FormerlySerializedAs("FollowOffset")] public Vector3 followOffset;


		[Header("Camera Range Limits")] public float minY = 50f;
		public float maxY = 125f;

		public float minZ = -200f;
		public float maxZ = 200f;

		public float minX = -200f;
		public float maxX = 200f;


		[FormerlySerializedAs("CameraOriginalPos")] [HideInInspector]
		public Vector3 cameraOriginalPos;

		[HideInInspector] public Vector3 newPosition;

		// [HideInInspector]
		// public Quaternion newRotation;
		// [HideInInspector]
		public Vector3 newZoom;
		[HideInInspector] public Vector3 dragCurrentPosition;
		[HideInInspector] public Vector3 dragStartPosition;

		[HideInInspector]
		// public Vector3 rotateStartPosition;
		// [HideInInspector]
		// public Vector3 rotateCurrentPosition;
		private Vector3 _pos;

		private Vector3 _camPos;
		private Vector3 _camPosOri;

		public bool isEnabled = true;

		// Start is called before the first frame update
		private void Start() {
			newPosition = transform.position;
			// newRotation = transform.rotation;
			newZoom = cameraTransform.localPosition;
			cameraOriginalPos = cameraTransform.localPosition;

			_pos = transform.position;
			_camPos = cameraTransform.localPosition;
			_camPosOri = cameraOriginalPos;
		}

		// Update is called once per frame
		private void Update() {
			if (!isEnabled) return;


			if (followTransform != null) {
				transform.position = followTransform.position + followOffset;
			} else {
				HandleMouseInput();
				HandleMovementInput();
			}

			if (Input.GetKeyDown(KeyCode.Escape)) followTransform = null;


			//Clamp Controls
			_pos = transform.position;
			_camPos = cameraTransform.localPosition;


			_pos.y = Mathf.Clamp(_pos.y, minY, maxY);

			_pos.x = Mathf.Clamp(_pos.x, minX, maxX);
			_pos.z = Mathf.Clamp(_pos.z, minZ, maxZ);

			_camPos.y = Mathf.Clamp(_camPos.y, minY, maxY);
			_camPos.x = Mathf.Clamp(_camPos.x, _camPosOri.x, _camPosOri.x);
			_camPos.z = Mathf.Clamp(_camPos.z, _camPosOri.z, _camPosOri.z);

			transform.position = _pos;
			cameraTransform.localPosition = _camPos;
		}

		public void SetFollowTarget(Transform target) {
			followTransform = target;
		}

		public void ClearFollowTarget() {
			followTransform = null;
		}

		private void FixedUpdate() {
			if (!isEnabled) return;
		}

		private void HandleMouseInput() {
			if (Input.mouseScrollDelta.y != 0) newZoom += Input.mouseScrollDelta.y * zoomAmount;

			// if(Input.GetMouseButtonDown(2)) {
			//     rotateStartPosition = Input.mousePosition;
			// }
			//
			// if(Input.GetMouseButton(2)) {
			//     rotateCurrentPosition = Input.mousePosition;
			//
			//     Vector3 difference = rotateStartPosition - rotateCurrentPosition;
			//     rotateStartPosition = rotateCurrentPosition;
			//
			//     newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5));
			// }


			if (Input.GetMouseButtonDown(2)) {
				Plane plane = new Plane(Vector3.up, Vector3.zero);
				Ray ray = camera.ScreenPointToRay(Input.mousePosition);

				float entry;
				if (plane.Raycast(ray, out entry)) dragStartPosition = ray.GetPoint(entry);
			}

			if (Input.GetMouseButton(2)) {
				Plane plane = new Plane(Vector3.up, Vector3.zero);
				Ray ray = camera.ScreenPointToRay(Input.mousePosition);

				float entry;
				if (plane.Raycast(ray, out entry)) {
					dragCurrentPosition = ray.GetPoint(entry);

					newPosition = transform.position + dragStartPosition - dragCurrentPosition;
				}
			}
		}

		private void HandleMovementInput() {
			if (Input.GetKey(KeyCode.LeftShift))
				moveSpeed = fastSpeed;
			else
				moveSpeed = normalSpeed;

			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) newPosition += transform.forward * moveSpeed;

			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				newPosition += transform.forward * -moveSpeed;

			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) newPosition += transform.right * moveSpeed;
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) newPosition += transform.right * -moveSpeed;

			// if(Input.GetKey(KeyCode.Q)) {
			//     newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
			// }
			//
			// if(Input.GetKey(KeyCode.E)) {
			//     newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
			// }


			newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
			newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
			newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ);

			newZoom.y = Mathf.Clamp(newZoom.y, minY, maxY);
			newZoom.x = Mathf.Clamp(newZoom.x, _camPosOri.x, _camPosOri.x);
			newZoom.z = Mathf.Clamp(newZoom.z, _camPosOri.z, _camPosOri.z);


			transform.position = Vector3.Lerp(transform.position, newPosition, Time.unscaledDeltaTime * moveTime);
			// transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.unscaledDeltaTime * moveTime);
			cameraTransform.localPosition =
				Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.unscaledDeltaTime * moveTime);
		}
	}
}