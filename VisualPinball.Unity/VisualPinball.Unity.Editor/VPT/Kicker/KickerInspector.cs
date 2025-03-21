﻿// Visual Pinball Engine
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

// ReSharper disable AssignmentInConditionalExpression

using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using VisualPinball.Engine.VPT.Kicker;

namespace VisualPinball.Unity.Editor
{
	[CustomEditor(typeof(KickerComponent)), CanEditMultipleObjects]
	public class KickerInspector : MainInspector<KickerData, KickerComponent>
	{
		private SerializedProperty _orientationProperty;
		private SerializedProperty _coilsProperty;

		protected override void OnEnable()
		{
			base.OnEnable();

			_orientationProperty = serializedObject.FindProperty(nameof(KickerComponent.Orientation));
			_coilsProperty = serializedObject.FindProperty(nameof(KickerComponent.Coils));
		}

		public override void OnInspectorGUI()
		{
			if (HasErrors()) {
				return;
			}

			BeginEditing();

			OnPreInspectorGUI();

			// position
			EditorGUI.BeginChangeCheck();
			var newPos = EditorGUILayout.Vector3Field(new GUIContent("Position", "Position of the kicker on the playfield, relative to its parent."), MainComponent.Position);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(MainComponent.transform, "Change Kicker Position");
				MainComponent.Position = newPos;
			}

			// radius
			EditorGUI.BeginChangeCheck();
			var newRadius = EditorGUILayout.FloatField(new GUIContent("Radius", "Kicker radius. Scales the mesh accordingly."), MainComponent.Radius);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(MainComponent.transform, "Change Kicker Radius");
				MainComponent.Radius = newRadius;
			}

			PropertyField(_orientationProperty, updateTransforms: true);
			PropertyField(_coilsProperty);

			base.OnInspectorGUI();

			EndEditing();
		}

		private void OnSceneGUI()
		{
			if (Event.current.type != EventType.Repaint) {
				return;
			}

			var playfield = MainComponent.GetComponentInParent<PlayfieldComponent>();
			var playfieldToWorld = playfield ? playfield.transform.localToWorldMatrix : Matrix4x4.identity;
			var transform = MainComponent.transform;
			var localPos = MainComponent.Position;
			var worldPos = transform.parent == null ? localPos : localPos.TranslateToWorld();

			Handles.color = Color.cyan;
			Handles.matrix = playfieldToWorld;
			foreach (var coil in MainComponent.Coils) {
				var from = MainComponent.GetBallCreationPosition().ToUnityVector3();
				var l = coil.Speed == 0 ? 1f : 20f * coil.Speed;
				var dir = new Vector3(
					l * math.sin(math.radians(coil.Angle)),
					l * math.sin(math.radians(coil.Inclination)),
					l * math.cos(math.radians(coil.Angle))
				);
				var to = from + dir;
				var worldDir = transform.TransformDirection(math.normalize( to - from));
				var length = coil.Speed == 0 ? 0.1f : coil.Speed / 10f;

				Handles.ArrowHandleCap(-1, worldPos, Quaternion.LookRotation(worldDir), length, EventType.Repaint);
			}
		}
	}
}
