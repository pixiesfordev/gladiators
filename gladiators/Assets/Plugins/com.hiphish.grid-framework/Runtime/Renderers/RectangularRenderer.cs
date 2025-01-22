using UnityEngine;
using GridFramework.Grids;
using GridFramework.Vectors;

namespace GridFramework.Renderers.Rectangular {
	/// <summary>
	///   Base class for all rectangular grid renderers.
	/// </summary>
	[RequireComponent(typeof(RectGrid))]
	public abstract class RectangularRenderer : GridRenderer {
		[SerializeField]
		protected RectGrid _grid;

		private Vector3 oldSpacing;
		private Vector6 oldShearing;

		/// <summary>
		///   Reference to the grid.
		/// </summary>
		protected RectGrid Grid {
			get {
				if (!_grid) {
					_grid = GetComponent<RectGrid>();
				}
				return _grid;
			}
		}

		protected override bool GridHasChanged() {
			var deltaSpacing = Grid.Spacing - oldSpacing;
			for (var i = 0; i < 3; ++i) {
				if (Mathf.Abs(deltaSpacing[i]) > Mathf.Epsilon) {
					goto different;
				}
			}

			var deltaShearing = Grid.Shearing - oldShearing;
			for (var i = 0; i < 3; ++i) {
				if (Mathf.Abs(deltaShearing[i]) > Mathf.Epsilon) {
					goto different;
				}
			}

			return false;

different:
			oldSpacing = Grid.Spacing;
			oldShearing = Grid.Shearing;
			return true;
		}
	}
}
