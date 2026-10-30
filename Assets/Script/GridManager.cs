using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int Width { get; private set; }
    public int Height { get; private set; }
    public GameMode Mode { get; private set; }
    public Cell[,] Grid { get; private set; }

    public bool RecreateGrid(int width, int height, GameMode mode)
    {
        if (width < 1 || height < 1)
        {
            return false;
        }

        DestroyCurrentGrid();

        Mode = mode;
        Width = mode == GameMode.Infinite ? width + 2 : width;
        Height = mode == GameMode.Infinite ? height + 2 : height;
        Grid = new Cell[Width, Height];

        InitializeCells();
        return true;
    }

    private void DestroyCurrentGrid()
    {
        if (Grid == null)
        {
            return;
        }

        foreach (Cell cell in Grid)
        {
            if (cell != null)
            {
                Destroy(cell.gameObject);
            }
        }
    }

    private void InitializeCells()
    {
        Cell cellPrefab = Resources.Load<Cell>("Cell");
        if (cellPrefab == null)
        {
            Debug.LogError("Le prefab Cell est introuvable dans Resources.");
            return;
        }

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                Cell cell = Instantiate(cellPrefab, new Vector3(x, y, 0), Quaternion.identity);
                Grid[x, y] = cell;

                bool isHiddenBorder =
                    Mode == GameMode.Infinite &&
                    (x == 0 || y == 0 || x == Width - 1 || y == Height - 1);

                if (isHiddenBorder)
                {
                    cell.GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }
}
