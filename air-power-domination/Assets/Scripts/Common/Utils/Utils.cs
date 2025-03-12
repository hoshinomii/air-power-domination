using System.Collections.Generic;
using RDP.Building;
using RDP.Grid_System;
using RDP.Multiplayer;
using RDP.Networking.Shared.Game.Data;
using RDP.Networking.Shared.ScriptableObjects;
using RDP.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace RDP.Common.Utils {
	// Haoting's Personal Utils Helper FUnctions that i have collected over the years of using unity

	public static class Utils {
		public const int SortingOrderDefault = 5000;
		public static readonly string SuccessColorHex = "54FF00";
		public static readonly string FailureColorHex = "FF4954";
		public static Color32 SuccessColor => HexToColor32(SuccessColorHex);
		public static Color32 FailureColor => HexToColor32(FailureColorHex);

		public static Vector3 ApplyRotationToVector2D(Vector3 vec, float angle) {
			return Quaternion.Euler(0, 0, angle) * vec;
		}

		public static Vector3 ApplyRotationToVector3D(Vector3 vec, float angle) {
			return Quaternion.Euler(0, angle, 0) * vec;
		}
		
		//Generate a fresh set of positions based on a circle
		public static List<Vector3> GetPositionalVectors(Vector3 startPosition, float dist, int posCount) {
			List<Vector3> positions = new List<Vector3>();
			for (int i = 0; i < posCount; i++) {
				float angle = i * (360 / posCount);
				Vector3 dir = ApplyRotationToVector3D(new Vector3(0, 1, 1), angle);
				Vector3 position = startPosition + dir * dist;
				Vector3 finalPos = new Vector3(position.x, 0, position.z);
				positions.Add(finalPos);
			}

			return positions;
		}

		public static List<T> ShuffleList<T>(List<T> arr) {
			for (int i = 0; i < arr.Count; i++) {
				T temp = arr[i];
				int randomIndex = Random.Range(0, arr.Count);
				arr[i] = arr[randomIndex];
				arr[randomIndex] = temp;
			}

			return arr;
		}

		public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default,
		                                       int fontSize = 40, Color? color = null,
		                                       TextAnchor textAnchor = TextAnchor.UpperLeft,
		                                       TextAlignment textAlignment = TextAlignment.Left,
		                                       int sortingOrder = SortingOrderDefault) {
			if (color == null) color = Color.white;
			return CreateWorldText(parent, text, localPosition, fontSize, (Color) color, textAnchor, textAlignment,
				sortingOrder);
		}

		// Create Text in the World
		public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize,
		                                       Color color, TextAnchor textAnchor, TextAlignment textAlignment,
		                                       int sortingOrder) {
			GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
			Transform transform = gameObject.transform;
			transform.SetParent(parent, false);
			transform.localPosition = localPosition;
			TextMesh textMesh = gameObject.GetComponent<TextMesh>();
			textMesh.anchor = textAnchor;
			textMesh.alignment = textAlignment;
			textMesh.text = text;
			textMesh.fontSize = fontSize;
			textMesh.color = color;
			textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
			return textMesh;
		}

		public static void CreateWorldTextPopup(string text, Vector3 localPosition) {
			CreateWorldTextPopup(null, text, localPosition, 50, Color.white, localPosition + new Vector3(0, 10), 1f);
		}

		// Create a Text Popup in the World
		public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize,
		                                        Color color, Vector3 finalPopupPosition, float popupTime) {
			TextMesh textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft,
				TextAlignment.Left, SortingOrderDefault);
			Transform transform = textMesh.transform;
			Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
			FunctionUpdater.Create(delegate {
				transform.position += moveAmount * Time.deltaTime;
				popupTime -= Time.deltaTime;
				if (popupTime <= 0f) {
					Object.Destroy(transform.gameObject);
					return true;
				}

				return false;
			}, "WorldTextPopup");
		}

		public static Task CreateBuildingTask(TaskData data, BuildingManager manager, string buildingName) {
			return CreateTask(
				$"{data.name} {buildingName}",
				buildingName,
				data.processingTime,
				manager,
				data.playerRoleRequired,
				data.unitRoleRequired,
				data.isRecursive);
		}

		public static Task CreateTask(TaskData data, BuildingManager manager, string buildingName) {
			return CreateTask(
				data.name,
				buildingName,
				data.processingTime,
				manager,
				data.playerRoleRequired,
				data.unitRoleRequired,
				data.isRecursive);
		}

		private static Task CreateTask(string name, string buildingName, float processingRate, BuildingManager manager,
		                               Vocation vocation, UnitRole unitRole, bool recursive = false) {
			return new Task(
				name,
				buildingName,
				processingRate,
				manager,
				vocation,
				unitRole,
				recursive);
		}

		// Return a black or whtie value based on brightness
		public static Color32 GetOppositeColor(Color32 color) {
			// takes in a color32 value and based on the brightness return black or white
			float brightness = color.r * 0.3f + color.g * 0.59f + color.b * 0.11f;
			return brightness > 0.5f ? Color.black : Color.white;
		}

		public static string ColorToHex(Color32 color) {
			string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
			return hex;
		}

		public static Color32 HexToColor32(string hex) {
			hex = hex.Replace("0x", ""); //in case the string is formatted 0xFFFFFF
			hex = hex.Replace("#", ""); //in case the string is formatted #FFFFFF
			byte a = 255; //assume fully visible unless specified in hex
			byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
			//Only use alpha if the string has enough characters
			if (hex.Length == 8) a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
			return new Color32(r, g, b, a);
		}

		// Call this to get the Grid's World Position based on X and Y and also the Team.
		public static Vector3 GetPositionalDataFromGrid(Team team, int x, int y) {
			GridSystem gridSystem = team.GridSystem; // TODO this needs to be updated to get the other team's grid 
			Grid<Node> grid = gridSystem.Grid;
			Vector3 position =
				grid.GetWorldPosition(x > grid.Width ? grid.Width : x, y > grid.Height ? grid.Height : y);
			float cellSizeOffset = grid.GetCellSize() / 2;
			return position + new Vector3(cellSizeOffset, 1f, cellSizeOffset);
		}
	}
}