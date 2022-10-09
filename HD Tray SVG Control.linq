<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
</Query>

StringBuilder svg = new StringBuilder();
int well_gap = 4;
int radius = 24;
int horizontal_offset = 128;
int vertical_offset = 64;

int min_well_x = Int32.MaxValue;
int min_well_y = Int32.MaxValue;
int max_well_x = Int32.MinValue;
int max_well_y = Int32.MinValue;

int cy1 = Int32.MinValue;
int cy2 = Int32.MinValue;

List<string> colors = new List<string>() { "gray", "white", "green", "yellow" };

void Main()
{
	int offset = radius + well_gap;

	for (int x = 0; x < 32; x++)
	{
		int previous_x = -1;
		int previous_y = -1;
		
		for (int y = 0; y < 9; y++)
		{
			// Calculate the position for the first row
			int px1 = x * 2 * (radius + well_gap);
			int px2 = px1 - radius - well_gap;
			
			int py1 = y * 4 * (radius + well_gap);
			int py2 = py1 + 2 * (radius + well_gap);
			
			int tileX = x / 8;
			int tileY = y / 3;
			
			offset_circle(px1, py1, colors[(tileX + tileY) % 2]);
			offset_circle(px2, py2, colors[(tileX + tileY) % 2]);
			
			update_helper_variables(px1, py1, px2, py2);
			
			// Draw the f-plate dividers
			if ((x % 8 == 0) && (x != 0))
			{
				// Vertical lines
				offset_line(px2, py1 - radius, px2, py1 + radius);
				offset_line(px2 - offset, py2 - radius, px2 - offset, py2 + radius);

				// Diagonal line between verticals in the same 'row'
				offset_line(px2, py1 + radius, px2 - offset, py2 - radius);

				// Diagonal line between verticals in adjacent 'rows'
				if ((previous_x != -1) && (y % 3 != 0))
				{
					offset_line(previous_x, previous_y, px2, py1 - radius);
				}
				
				if (y == 2) cy1 = py2 + radius + well_gap;
				if (y == 5) cy2 = py2 + radius + well_gap;
				
				previous_x = px2 - offset;
				previous_y = py2 + radius;
			}			
		}
	}

	// Draw the horizontal cross-bars
	offset_line(min_well_x - radius, cy1, max_well_x + radius, cy1);
	offset_line(min_well_x - radius, cy2, max_well_x + radius, cy2);

	// Draw the vertical boundaries
	offset_line(min_well_x - radius - well_gap, min_well_y - radius, min_well_x - radius - well_gap, max_well_y + radius);
	offset_line(max_well_x + radius + well_gap, min_well_y - radius, max_well_x + radius + well_gap, max_well_y + radius);
	
	// Draw the 'block' text
	int tx = (4 * 2 - 1) * (radius + well_gap);
	int ty = -(radius + 2 * well_gap);
	offset_text(tx, ty, "Blocks 1, 3, 5");

	tx = ((4 + 8) * 2 - 1) * (radius + well_gap);
	offset_text(tx, ty, "Blocks 2, 4, 6");

	tx = ((4 + 8 + 8) * 2 - 1) * (radius + well_gap);
	offset_text(tx, ty, "Blocks 1, 3, 5");

	tx = ((4 + 8 + 8 + 8) * 2 - 1) * (radius + well_gap);
	offset_text(tx, ty, "Blocks 2, 4, 6");
	
	// Draw the 'channel' text
	int left = min_well_x - (radius + well_gap) * 2;
	int right = max_well_x + (radius + well_gap) * 2;
	
	for (int cy = 0; cy < 18; cy++)
	{
		int ypos = cy * (radius + well_gap) * 2 + 6;
		offset_text(left, ypos, calculate_module_text_from_y_value(cy, false));
		offset_text(right, ypos, calculate_module_text_from_y_value(cy, true));
	}

	Svg result = new Svg(svg.ToString(), 2048, 1200);
	
	result.Dump();
	result.Content.Dump();
}

string calculate_module_text_from_y_value(int value, bool isRight)
{
	// First start by converting to a value in the range 0-5
	int first = value % 6;
	
	// Get the channel as 1, 2, 3
	int channel = (first / 2) + 1;
	
	char side = 'A';
	if (value % 2 != 0) side = 'B';
	
	if (isRight) channel += 3;

	return $"{channel}-{side}";
}

void update_helper_variables(int x1, int y1, int x2, int y2)
{
	min_well_x = Math.Min(min_well_x, x1);
	min_well_x = Math.Min(min_well_x, x2);

	max_well_x = Math.Max(max_well_x, x1);
	max_well_x = Math.Max(max_well_x, x2);

	min_well_y = Math.Min(min_well_y, y1);
	min_well_y = Math.Min(min_well_y, y2);

	max_well_y = Math.Max(max_well_y, y1);
	max_well_y = Math.Max(max_well_y, y2);
}

void offset_line(int x1, int y1, int x2, int y2, string color = null)
{
	if (color is null) color = "white";
	svg.AppendLine(SVGHelper.Line(x1 + horizontal_offset, y1 + vertical_offset, x2 + horizontal_offset, y2 + vertical_offset, color));
}

void offset_circle(int x, int y, string color = null)
{
	if (color is null) color = "white";
	svg.AppendLine(SVGHelper.Circle(x + horizontal_offset, y + vertical_offset, radius, color));
}

void offset_text(int x, int y, string text)
{
	svg.AppendLine(SVGHelper.Text(x + horizontal_offset, y + vertical_offset, text, UserQuery.SVGHelper.TextAlign.MIDDLE));
}

public static class SVGHelper
{
	public enum TextAlign { START, MIDDLE, END }

	public static string Circle(int x, int y, int r, string stroke = "white", string fill = "white")
	{
		return $"<circle cx='{x}' cy='{y}' r='{r}' stroke='{stroke}' stroke-width='2' />"; // fill='{fill}'
	}

	public static string Rect(int x, int y, int width, int height, int r = 0, string stroke = "white", string fill = null, int strokeWidth = 1)
	{
		return $"<rect x='{x}' y='{y}' width='{width}' height='{height}' rx='{r}' stroke='{stroke}' fill='{fill}' stroke-width='{strokeWidth}' />";
	}

	public static string Line(int x1, int y1, int x2, int y2, string stroke = "white", int strokeWidth = 1)
	{
		return $"<line x1='{x1}' y1='{y1}' x2='{x2}' y2='{y2}' stroke='{stroke}' stroke-width='{strokeWidth}' />";
	}

	public static string Text(int x, int y, string text, TextAlign align = TextAlign.START, string stroke = "white")
	{
		string anchor = null;
		switch (align)
		{
			case TextAlign.START: anchor = "start"; break;
			case TextAlign.MIDDLE: anchor = "middle"; break;
			case TextAlign.END: anchor = "end"; break;
		}
		return $"<text x='{x}' y='{y}' text-anchor='{anchor}' stroke='{stroke}' fill='{stroke}' font-size='x-large'>{text}</text>";
	}
}