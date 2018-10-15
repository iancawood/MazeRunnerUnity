using UnityEngine;
using System.Collections;

/*
 * source of algorithm: http://weblog.jamisbuck.org/2011/1/12/maze-generation-recursive-division-algorithm
 * 
 * source of troll avatar and model: https://www.assetstore.unity3d.com/en/#!content/18219
 * originally written by: PushPlay Games
 * 
 * source of teleporter particle system: https://www.assetstore.unity3d.com/en/#!/content/25645
 * originally written by: M31
 * Note that this asset was been modified in this project.
 * 
 * source of wall textures: https://www.assetstore.unity3d.com/en/#!/content/12567
 * originally written by: A dog's life software
 */

// Generates a random maze and then builds into using vertical and horizontal prefabs
public class MazeBuilder : MonoBehaviour {

    public Transform eastWall;
    public Transform southWall;
    public Transform mazeExit;

    public int cellDimension = 3;

    public const int mazeSize = 10;

    private int[,] maze = new int[mazeSize, mazeSize];

    private enum Orientation {
        vertical,
        horizontal
    };

    int South = 1;
    int East = 2;

    void Start() {
        generateMaze();
        buildMaze();
    }

    // Build the maze using prefabs
    void buildMaze() {
        // build maze interior
        for (int i = 0; i < mazeSize; i++) {
            for (int j = 0; j < mazeSize; j++) {
                switch (maze[i, j]) {
                    case 1: // South
                        drawWall(southWall, new Vector3(j * cellDimension, 0, i * cellDimension), new Vector3(cellDimension, 1, 1));
                        break;

                    case 2: // East
                        drawWall(eastWall, new Vector3(j * cellDimension, 0, i * cellDimension), new Vector3(1, 1, cellDimension));
                        break;

                    case 3: // SouthEast
                        drawWall(southWall, new Vector3(j * cellDimension, 0, i * cellDimension), new Vector3(cellDimension, 1, 1));
                        drawWall(eastWall, new Vector3(j * cellDimension, 0, i * cellDimension), new Vector3(1, 1, cellDimension));
                        break;

                    default:
                        continue;
                }
            }
        }

        // build outer wall
        drawWall(southWall, 
            new Vector3((cellDimension * mazeSize) / 2.0f - (cellDimension / 2.0f), 0, (cellDimension * mazeSize) - cellDimension),
            new Vector3(cellDimension * mazeSize, 1, 1)
            ); // north
        drawWall(southWall,
            new Vector3((cellDimension * mazeSize) / 2.0f - (cellDimension / 2.0f), 0, -cellDimension),
            new Vector3(cellDimension * mazeSize, 1, 1)
            ); // south
        drawWall(eastWall,
            new Vector3((cellDimension * mazeSize) - cellDimension, 0, (cellDimension * mazeSize) / 2.0f - (cellDimension / 2.0f)),
            new Vector3(1, 1, cellDimension * mazeSize)
            ); // east
        drawWall(eastWall,
            new Vector3(-cellDimension, 0, (cellDimension * mazeSize) / 2.0f - (cellDimension / 2.0f)),
            new Vector3(1, 1, cellDimension * mazeSize)
            ); // west

        // build maze exit
        Instantiate(mazeExit, new Vector3((cellDimension * mazeSize) - cellDimension, 0, (cellDimension * mazeSize) - cellDimension), Quaternion.identity);
    }

    // Utility function for instantiating a wall prefab and then scaling it
    void drawWall(Transform transform, Vector3 position, Vector3 scaleFactor) {
        Transform wallTransform = Instantiate(transform, position, Quaternion.identity) as Transform;
        wallTransform.localScale = scaleFactor;
    }

    // Starts the recursive function for building the maze array
    void generateMaze() {
        divide(0, 0, mazeSize, mazeSize, chooseOrientation(mazeSize, mazeSize));
    }
        
    // Recursive function that builds a random maze in the form of a 2D array
    void divide(int x, int y, int width, int height, Orientation orientation) {
        if (width < 2 || height < 2) {
            return;
        }

        bool horizontal = orientation == Orientation.horizontal;

        // where will the wall be drawn from
        int wx = x + (horizontal ? 0 : Random.Range(0, width - 2));
        int wy = y + (horizontal ? Random.Range(0, height - 2) : 0);

        // where will the passage through the wall exist
        int px = wx + (horizontal ? Random.Range(0, width) : 0);
        int py = wy + (horizontal ? 0 : Random.Range(0, height));

        // what direction will the wall be drawn
        int dx = horizontal ? 1 : 0;
        int dy = horizontal ? 0 : 1;

        // how long will the wall be
        int length = horizontal ? width : height;

        // what direction is perpendicular to the wall
        int dir = horizontal ? South : East;
        
        // draw the wall
        for (int i = 0; i < length; i++) {
            if (wx != px || wy != py) {
                maze[wy, wx] += dir;
            }
            wx += dx;
            wy += dy;
        }

        int nx, ny, w, h;

        nx = x;
        ny = y;
        w = horizontal ? width : wx - x + 1;
        h = horizontal ? wy - y + 1 : height;
        divide(nx, ny, w, h, chooseOrientation(w, h));
    
        nx = horizontal ? x : wx + 1;
        ny = horizontal ? wy + 1 : y;
        w = horizontal ? width : x + width - wx - 1;
        h = horizontal ? y + height - wy - 1 : height;
        divide( nx, ny, w, h, chooseOrientation(w, h));

    }

    // Choose an orientation (should the wall be vertically or horizontally built)
    Orientation chooseOrientation(int width, int height) {
        if (width < height) {
            return Orientation.horizontal;
        } else if (height < width) {
            return Orientation.vertical;
        } else {
            return Random.Range(0, 2) == 1 ? Orientation.horizontal : Orientation.vertical;
        }
    }

    // Debug function for printing out the maze
    void printMaze() {
        string mazeString = "";
        for (int i = 0; i < mazeSize; i++) {
            for (int j = 0; j < mazeSize; j++) {
                mazeString += maze[i, j];
                if (j + 1 != mazeSize) {
                    mazeString += "-";
                }
            }
            mazeString += "\n";
        }
        Debug.Log(mazeString);
    }
}
