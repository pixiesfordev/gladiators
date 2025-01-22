using UnityEngine;
using GridFramework.Grids;

namespace GridFramework.Renderers.Spherical {
	/// <summary>
	///   Base class for all spherical grid renderers.
	/// </summary>
	[RequireComponent(typeof(SphereGrid))]
	public abstract class SphericalRenderer : GridRenderer {

		[SerializeField]
		protected SphereGrid _grid;

		private float oldRadius;
		private int oldParallels, oldMeridians;

		/// <summary>
		///   Reference to the grid.
		/// </summary>
		protected SphereGrid Grid {
			get {
				if (!_grid) {
					_grid = GetComponent<SphereGrid>();
				}
				return _grid;
			}
		}

		protected override bool GridHasChanged() {
            var different = Mathf.Abs(Grid.Radius - oldRadius) > Mathf.Epsilon
            	|| Grid.Parallels != oldParallels
            	|| Grid.Meridians != oldMeridians;

            if (different) {
            	oldRadius = Grid.Radius;
            	oldParallels = Grid.Parallels;
            	oldMeridians = Grid.Meridians;
            	return true;
            }

            return false;
		}
	}
}
