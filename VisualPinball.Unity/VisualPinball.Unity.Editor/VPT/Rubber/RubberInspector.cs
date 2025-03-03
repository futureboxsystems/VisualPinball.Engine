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

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VisualPinball.Engine.Math;
using VisualPinball.Engine.VPT.Rubber;

namespace VisualPinball.Unity.Editor
{
	[CustomEditor(typeof(RubberComponent)), CanEditMultipleObjects]
	public class RubberInspector : MainInspector<RubberData, RubberComponent>, IDragPointsInspector
	{

		public Transform Transform => MainComponent.transform;

		private SerializedProperty _thicknessProperty;

		protected override void OnEnable()
		{
			base.OnEnable();

			DragPointsHelper = new DragPointsInspectorHelper(MainComponent, this);
			DragPointsHelper.OnEnable();

			_thicknessProperty = serializedObject.FindProperty(nameof(RubberComponent._thickness));
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			DragPointsHelper.OnDisable();
		}

		public override void OnInspectorGUI()
		{
			if (HasErrors()) {
				return;
			}

			BeginEditing();

			OnPreInspectorGUI();

			// height
			EditorGUI.BeginChangeCheck();
			var newHeight = EditorGUILayout.FloatField(new GUIContent("Height", "Height of the rubber (in VPX units."), MainComponent.Height);
			if (EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(MainComponent.transform, "Change Rubber Height");
				MainComponent.Height = newHeight;
			}
			PropertyField(_thicknessProperty, rebuildMesh: true);

			DragPointsHelper.OnInspectorGUI(this);

			base.OnInspectorGUI();

			EndEditing();
		}

		private void OnSceneGUI()
		{
			DragPointsHelper.OnSceneGUI(this);
		}

		#region Dragpoint Tooling

		public bool DragPointsActive => true;
		public DragPointData[] DragPoints { get => MainComponent.DragPoints; set => MainComponent.DragPoints = value; }
		public bool PointsAreLooping => true;
		public IEnumerable<DragPointExposure> DragPointExposition => new[] { DragPointExposure.Smooth };
		public DragPointTransformType HandleType => DragPointTransformType.TwoD;
		public DragPointsInspectorHelper DragPointsHelper { get; private set; }
		public float ZOffset => 0;
		public float[] TopBottomZ => null;
		public void SetDragPointPosition(DragPointData dragPoint, Vertex3D value, int numSelectedDragPoints,
			float[] topBottomZ) => dragPoint.Center = value;

		#endregion
	}
}
