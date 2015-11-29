using UnityEngine;
using System.Collections;

public class LineDrawer : MonoBehaviour {

	static public void DrawGrid(Texture2D texture, int lines) {
		for (int i = 0; i < lines; i++) {
			int x = i * (texture.width / lines);
			DrawLine(texture, x, 0, x, texture.height, Color.black);
		}
		DrawLine(texture, texture.width - 1, 0, texture.width - 1, texture.height, Color.black);

		for (int j = 0; j < lines; j++) {
			int y = j * (texture.height / lines);
			DrawLine(texture, 0, y, texture.width, y, Color.black);
		}
		DrawLine(texture, 0, texture.height - 1, texture.width, texture.height - 1, Color.black);
	}

	static public void DrawLine(Texture2D texture, int x0, int y0, int x1, int y1, Color color) {
		int dy = (int)(y1-y0);
		int dx = (int)(x1-x0);
		int stepx, stepy;
		
		if (dy < 0) {
			dy = -dy; 
			stepy = -1;
		} else {
			stepy = 1;
		} 
		if (dx < 0) {
			dx = -dx; 
			stepx = -1;
		} else {
			stepx = 1;
		}
		dy <<= 1;
		dx <<= 1;
		
		float fraction = 0;
		
		texture.SetPixel(x0, y0, color);
		if (dx > dy) {
			fraction = dy - (dx >> 1);
			while (Mathf.Abs(x0 - x1) > 1) {
				if (fraction >= 0) {
					y0 += stepy;
					fraction -= dx;
				}
				x0 += stepx;
				fraction += dy;
				texture.SetPixel(x0, y0, color);
			}
		} else {
			fraction = dx - (dy >> 1);
			while (Mathf.Abs(y0 - y1) > 1) {
				if (fraction >= 0) {
					x0 += stepx;
					fraction -= dy;
				}
				y0 += stepy;
				fraction += dx;
				texture.SetPixel(x0, y0, color);
			}
		}
	}

}
