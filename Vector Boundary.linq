<Query Kind="Program">
  <Namespace>LINQPad.Controls</Namespace>
  <Namespace>System.Drawing</Namespace>
</Query>

Bitmap bitmap;
int width;
int height;

void Main()
{
	bitmap = new Bitmap(@"C:\dev\unity\Retrocade\Assets\Resources\Test-Image.png");
	width = bitmap.Width;
	height = bitmap.Height;
	DrawBitmapAsSvg();
	
	Tile[,] tiles = new Tile[height, width];

	DetectEdges(tiles);

	StringBuilder sb = new();
	for (int y = 0; y < 20; y++)
	{
		for (int x = 0; x < 32; x++)
		{
			sb.AppendLine(tiles[y, x].Draw(x * 40, y * 40, 40));
		}
	}
	//sb.ToString().Dump();
	Svg svg = new Svg(sb.ToString(), 1600, 900);
	svg.Dump();
}

void DrawBitmapAsSvg()
{
	StringBuilder sb = new();
	for (int y = 0; y < 20; y++)
	{
		for (int x = 0; x < 32; x++)
		{
			string color = "white";
			if (bitmap.GetPixel(x, y).ToArgb() == -1) color = "black";
			sb.AppendLine(Circle(x * 20 + 10, y * 20 + 10, 5, color));
		}
	}

	Svg svg = new Svg(sb.ToString(), (width + 1) * 20, (height + 1) * 20);
	svg.Dump();
}

void DetectEdges(Tile[,] tiles)
{
	for (int x = 0; x < width; x++)
	{
		for (int y = 0; y < height; y++)
		{
			Tile tile = new();
			
			if (bitmap.GetPixel(x, y).ToArgb() != -1)
			{
				tile.Black = false;
				if (!Check(x, y - 1)) tile.T = true;
				if (!Check(x, y + 1)) tile.B = true;
				if (!Check(x - 1, y)) tile.L = true;
				if (!Check(x + 1, y)) tile.R = true;
			}

			tiles[y, x] = tile;
		}
	}
}

bool Check(int x, int y)
{
	if (x < 0) return false;
	if (y < 0) return false;
	
	if (x >= width) return false;
	if (y >= height) return false;

	return bitmap.GetPixel(x, y).ToArgb() != -1;
}

class Tile
{
	public bool L { get; set; }
	public bool R { get; set; }
	public bool T { get; set; }
	public bool B { get; set; }
	public bool Black { get; set; } = true;

	public string Draw(int x, int y, int scale)
	{
		StringBuilder sb = new();
		int radius = 4;

		int cx = x + scale / 2;
		int cy = y + scale / 2;
		
		string off = Black ? "black" : "gray";

		if (T)
		{
			sb.AppendLine(DoubleLine(x, y, x + scale, y, 5));
		}
		if (B) sb.AppendLine(DoubleLine(x, y + scale, x + scale, y + scale, -5));
		if (L) sb.AppendLine(DoubleLine(x, y, x, y + scale, 5));
		if (R) sb.AppendLine(DoubleLine(x + scale, y, x + scale, y + scale, -5));

		return sb.ToString();
	}
}

static string DoubleLine(int x1, int y1, int x2, int y2, int offset, string color = "green")
{
	int absOffset = Math.Abs(offset);
	StringBuilder sb = new();
	sb.AppendLine($"<line x1='{x1}' y1='{y1}' x2='{x2}' y2='{y2}' style='stroke:{color};stroke-width:2' />");
	if (x1 == x2)
	{
		sb.AppendLine($"<line x1='{x1 + offset}' y1='{y1 + absOffset}' x2='{x1 + offset}' y2='{y2 - absOffset}' style='stroke:blue;stroke-width:2' />");
	}
	else
	{
		sb.AppendLine($"<line x1='{x1 + absOffset}' y1='{y1 + offset}' x2='{x2 - absOffset}' y2='{y2 + offset}' style='stroke:blue;stroke-width:2' />");
	}
	
	return sb.ToString();
}

static string Circle(int x, int y, int r, string color) => $"<circle cx='{x}' cy='{y}' r='{r}' fill='{color}' />";
static string Line(int x1, int y1, int x2, int y2, string color = "green") => $"<line x1='{x1}' y1='{y1}' x2='{x2}' y2='{y2}' style='stroke:{color};stroke-width:2' />";
