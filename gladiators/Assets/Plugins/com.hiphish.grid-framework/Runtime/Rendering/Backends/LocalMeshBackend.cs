using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using GridFramework.Renderers;

namespace GridFramework.Rendering {
	/// <summary>
	///   Grid rendering backend which renders one grid as a mesh with lines topology.
	/// </summary>
	/// <remarks>
	///   <para>
	///     This rendering backend needs to be attach to a <c>GameObject</c> which also has a
	///     <c>GridRenderer</c> component, it will only render the grids of its <c>GameObject</c>.
	///     At runtime a <c>Mesh</c> is generated which will be passed assigned to the
	///     <c>MeshFilter</c>. In order to change the appearance of the grids you need to assign
	///     <c>Material</c> to the <c>MeshRenderer</c> of the object.
	///   </para>
	/// </remarks>
	/// <seealso cref="GridFramework.Rendering.GridMeshBackend" />
	[RequireComponent(typeof(MeshFilter))]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(GridRenderer))]
	[AddComponentMenu("Grid Framework/Rendering/Local Mesh Backend")]
	public class LocalMeshBackend : MonoBehaviour {
		// This has a lot of duplication with the global grid mesh backend. The only real difference
		// is in which renderers we regard and whether the vertices are in global or local
		// coordinates.
		private const MeshTopology topology = MeshTopology.Lines;
		private readonly Func<Vector3, int, Vector3> first = (v, n) => v;
		private readonly Func<int, int, int> sum = (i, n) => i + n;

		private ICollection<GridRenderer> renderers;
		private readonly IDictionary<GridRenderer, Mesh> meshes = new Dictionary<GridRenderer, Mesh>();
		private Mesh mesh;

    	void Awake() {
    		mesh = new Mesh();
    		renderers = new List<GridRenderer>();
    		GetComponent<MeshFilter>().mesh = mesh;

    		foreach (var renderer in GetComponents<GridRenderer>()) {
    			RegisterRenderer(renderer);
    		}

			System.RendererRegistered   += OnRendererRegister;
			System.RendererUnregistered += OnRendererUnregister;
    	}

    	void OnDestroy() {
			System.RendererRegistered   -= OnRendererRegister;
			System.RendererUnregistered -= OnRendererUnregister;
    	}

    	void OnEnable() {
			System.RendererRefreshed += OnRendererRefresh;
    	}

    	void OnDisable() {
			System.RendererRefreshed -= OnRendererRefresh;
    	}

    	void FixedUpdate() {
			System.PollRenderers(renderers);
    	}

    	private void OnRendererRegister(GridRenderer renderer) {
    		if (!GetComponents<GridRenderer>().Contains(renderer) || renderers.Contains(renderer)) {
    			return;  // Avoid registering a renderer twice
    		}
			RegisterRenderer(renderer);
    	}

    	private void OnRendererUnregister(GridRenderer renderer) {
    		if (!renderers.Contains(renderer)) {
    			return;
    		}
    		meshes.Remove(renderer);
    		renderers.Remove(renderer);
    		CombineMeshes(meshes.Values, mesh);
    	}

    	private void OnRendererRefresh(GridRenderer renderer, System.RendererRefreshEventArgs args) {
    		if (!renderers.Contains(renderer)) {
    			return;
    		}

			UpdateMesh(args.Segments, meshes[renderer]);
			CombineMeshes(meshes.Values, mesh);
    	}

    	private void RegisterRenderer(GridRenderer renderer) {
			meshes.Add(renderer, new Mesh());
    		renderers.Add(renderer);
    	}

    	private void UpdateMesh(RendererLineSegments segments, Mesh mesh) {
			var vertices = new Vector3[2 * segments.Length];
			var indices  = new int[2 * segments.Length];

			var i = 0;
			foreach (var segmentSet in segments) {
				foreach (var segment in segmentSet) {
					for (var j = 0; j < 2; ++j) {
						var vertex = transform.worldToLocalMatrix * (segment[j] - transform.position);
						vertices[i+j] = vertex;
						indices[i+j] = i+j;
					}
					i += 2;
				}
			}

			mesh.Clear(true);
			mesh.SetVertices(vertices);
			mesh.SetIndices(indices, topology, 0);
			mesh.RecalculateBounds();
    	}

		/// <summary>
		///   Combine all the individual meshes for each renderer into one common mesh.
		/// </summary>
		/// <param name="meshes">
		///   Collection of meshes to combine
		/// </param>
		/// <param name="result">
		///   Mesh object to assign the new values to. Will be mutated.
		/// </param>
		private void CombineMeshes(ICollection<Mesh> meshes, Mesh result) {
			var vertices = Flatten(from mesh in meshes select mesh.vertices, first);
			var indices = Flatten(from mesh in meshes select mesh.GetIndices(0), sum);

			result.Clear(true);
			result.SetVertices(vertices);
			result.SetIndices(indices, topology, 0);
			result.RecalculateBounds();
		}

		/// <summary>
		///   Flatten a collection of arrays of data points into one array of data points according
		///   to a provided rule.
		/// </summary>
		/// <param name="how">
		///   A function which projects the current item and the total length of data points from
		///   the previous groups onto the final value.
		/// </param>
		/// <seealso cref="System.Linq.SelectMany"/>
		private T[] Flatten<T> (IEnumerable<T[]> source, Func<T, int, T> how) {
			var length = source.Select(dataPoints => dataPoints.Length).Sum();
			var result = new T[length];

			var total = 0;
			foreach (var dataPoints in source) {
				var n = dataPoints.Length;
				for (var i = 0; i < n; ++i) {
					result[total + i] = how(dataPoints[i], total);
				}
				total += n;
			}

			return result;
		}
	}
}
