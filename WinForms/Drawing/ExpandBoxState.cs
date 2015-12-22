﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using AdamsLair.WinForms.Properties;

namespace AdamsLair.WinForms.Drawing
{
	public enum ExpandBoxState
	{
		ExpandDisabled,
		ExpandPressed,
		ExpandHot,
		ExpandNormal,

		CollapseDisabled,
		CollapsePressed,
		CollapseHot,
		CollapseNormal
	}
}
