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

// ReSharper disable MemberCanBePrivate.Global

namespace VisualPinball.Unity
{
	public struct DropTargetPackable
	{
		public static byte[] Pack(DropTargetComponent _) => PackageApi.Packer.Pack(new DropTargetPackable());

		public static void Unpack(byte[] bytes, DropTargetComponent comp)
		{
			// no data
		}
	}

	public struct DropTargetColliderPackable
	{
		public bool IsMovable;
		public bool IsLegacy;
		public float Threshold;
		public bool UseHitEvent;

		public static byte[] Pack(DropTargetColliderComponent comp)
		{
			return PackageApi.Packer.Pack(new DropTargetColliderPackable {
				IsMovable = comp._isKinematic,
				IsLegacy = comp.IsLegacy,
				Threshold = comp.Threshold,
				UseHitEvent = comp.UseHitEvent,
			});
		}

		public static void Unpack(byte[] bytes, DropTargetColliderComponent comp)
		{
			var data = PackageApi.Packer.Unpack<DropTargetColliderPackable>(bytes);
			comp._isKinematic = data.IsMovable;
			comp.IsLegacy = data.IsLegacy;
			comp.Threshold = data.Threshold;
			comp.UseHitEvent = data.UseHitEvent;
		}
	}

	public struct DropTargetAnimationPackable
	{
		public float Speed;
		public int RaiseDelay;
		public bool IsDropped;

		public static byte[] Pack(DropTargetAnimationComponent comp)
		{
			return PackageApi.Packer.Pack(new DropTargetAnimationPackable {
				Speed = comp.Speed,
				RaiseDelay = comp.RaiseDelay,
				IsDropped = comp.IsDropped,
			});
		}

		public static void Unpack(byte[] bytes, DropTargetAnimationComponent comp)
		{
			var data = PackageApi.Packer.Unpack<DropTargetAnimationPackable>(bytes);
			comp.Speed = data.Speed;
			comp.RaiseDelay = data.RaiseDelay;
			comp.IsDropped = data.IsDropped;
		}
	}
}
