// Visual Pinball Engine
// Copyright (C) 2023 freezy and VPE Team
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <https://www.gnu.org/licenses/>.

// ReSharper disable InconsistentNaming

using NLog;
using UnityEngine;
using Logger = NLog.Logger;

namespace VisualPinball.Unity
{
	/// <summary>
	/// The base class for all components on the playfield.<p/>
	/// </summary>
	public abstract class ItemComponent : MonoBehaviour
	{
		public abstract string ItemName { get; }

		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		protected void RegisterPhysics(PhysicsEngine physicsEngine)
		{
			if (physicsEngine) {
				physicsEngine.Register(this);
			} else {
				Logger.Error("No physics engine found in parent hierarchy.");
			}
		}

		protected void RegisterPhysics() => RegisterPhysics(GetComponentInParent<PhysicsEngine>());

		protected static void DrawArrow(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.025f, float arrowHeadAngle = 20.0f)
		{
			Debug.DrawRay(pos, direction);

			var right = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180+arrowHeadAngle,0) * new Vector3(0,0,1);
			var left = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180-arrowHeadAngle,0) * new Vector3(0,0,1);
			Debug.DrawRay(pos + direction, right * arrowHeadLength);
			Debug.DrawRay(pos + direction, left * arrowHeadLength);
		}

		protected void SetEnabled<T>(bool value) where T : Object
		{
			var comp = GetComponentsInChildren<T>();
			foreach (var c in comp) {
				switch (c) {
					case Behaviour behaviourComp:
						behaviourComp.enabled = value;
						break;
					case Renderer rendererComp:
						rendererComp.enabled = value;
						break;
				}
			}
		}

		protected bool GetEnabled<T>() where T : Object
		{
			var comp = GetComponent<T>();
			if (!comp) {
				return false;
			}
			switch (comp) {
				case Behaviour behaviourComp:
					return behaviourComp.enabled;
				case Renderer rendererComp:
					return rendererComp.enabled;
			}

			return false;
		}
	}
}
