﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gweny.Control {
	public class ItemSelectedEventArgs : EventArgs {
		public Base SelectedItem { get; private set; }

		internal ItemSelectedEventArgs(Base selecteditem) {
			this.SelectedItem = selecteditem;
		}
	}
}
