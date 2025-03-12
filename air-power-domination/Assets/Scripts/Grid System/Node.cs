namespace RDP.Grid_System {
	public class Node {
		private readonly Grid<Node> _node;
		public PlacedObjectDone PlacedObject;
		private readonly int _x;
		private readonly int _y;

		private bool _concealed, _built, _destroyed;

		public Node(Grid<Node> node, int x, int y) {
			_node = node;
			_x = x;
			_y = y;
			PlacedObject = null;
		}

		public override string ToString() {
			return _x + ", " + _y + "\n" + PlacedObject;
		}

		public void SetPlacedObject(PlacedObjectDone placedObject) {
			PlacedObject = placedObject;
			_node.TriggerGridObjectChanged(_x, _y);
		}

		public void ClearPlacedObject() {
			PlacedObject = null;
			_node.TriggerGridObjectChanged(_x, _y);
		}

		public PlacedObjectDone GetPlacedObject() {
			return PlacedObject;
		}

		public bool CanBuild() {
			return PlacedObject == null;
		}

		public bool BuiltState {
			get => _built;
			set => _built = value;
		}

		public bool Concealed {
			get => _concealed;
			set => _concealed = value;
		}

		public bool Destroyed {
			get => _destroyed;
			set => _destroyed = value;
		}

		public int X => _x;

		public int Y => _y;
	}
}