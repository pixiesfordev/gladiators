using UnityEngine;
using GridFramework.Grids;

namespace GridFramework.Renderers.Polar {
	/// <summary>
	///   Base class for all polar grid renderers.
	/// </summary>
	[RequireComponent(typeof(PolarGrid))]
	public abstract class PolarRenderer : GridRenderer {
		[SerializeField]
		protected PolarGrid _grid;

        private float oldDepth, oldRadius;
        private int oldSectors;

		/// <summary>
		///   Reference to the grid.
		/// </summary>
		protected PolarGrid Grid {
			get {
				if (!_grid) {
					_grid = GetComponent<PolarGrid>();
				}
				return _grid;
			}
		}

		protected override bool GridHasChanged() {
            var different = Mathf.Abs(Grid.Depth - oldDepth) > Mathf.Epsilon
            	|| Mathf.Abs(Grid.Radius - oldRadius) > Mathf.Epsilon
            	|| Grid.Sectors != oldSectors;

            if (different) {
            	oldDepth = Grid.Depth;
            	oldRadius = Grid.Radius;
            	oldSectors = Grid.Sectors;
            	return true;
            }

            return false;
		}
	}
}
