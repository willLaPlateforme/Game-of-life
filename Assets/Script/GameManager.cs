using UnityEngine;

public enum GameMode
{
    Infinite,
    Symmetric
}

[RequireComponent(typeof(GridManager))]
public class GameManager : MonoBehaviour
{
    [SerializeField, Min(1)] private int initialWidth = 20;
    [SerializeField, Min(1)] private int initialHeight = 20;
    [SerializeField] private GameMode initialGameMode = GameMode.Infinite;
    [SerializeField] public float speed = 0.1f;

    private  float timer = 0f;
    private GridManager gridManager;


    void Awake()
    {
        gridManager = GetComponent<GridManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridManager.RecreateGrid(initialWidth, initialHeight, initialGameMode);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool RecreateGrid(int width, int height, GameMode mode)
    {
        return gridManager.RecreateGrid(width, height, mode);
    }

    public bool SetSpeed(float newSpeed)
    {
        if (newSpeed <= 0f)
        {
            return false;
        }

        speed = newSpeed;
        timer = 0f;
        return true;
    }

    public void RandomCellAlive()
    {
        for (int x = 0; x < gridManager.Width; x++)
        {
            for (int y = 0; y < gridManager.Height; y++)
            {
                bool isAlive = Random.value > 0.5f;
                gridManager.Grid[x, y].SetAlive(isAlive);
            }
        }
    }

public void CountNeighbors()
{
    int start = gridManager.Mode == GameMode.Infinite ? 1 : 0;
    int endX = gridManager.Mode == GameMode.Infinite ? gridManager.Width - 1 : gridManager.Width;
    int endY = gridManager.Mode == GameMode.Infinite ? gridManager.Height - 1 : gridManager.Height;

    for (int x = start; x < endX; x++)
    {
        for (int y = start; y < endY; y++)
        {
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    int neighborX;
                    int neighborY;

                    if (gridManager.Mode == GameMode.Infinite)
                    {
                        neighborX = x + i;
                        neighborY = y + j;

                        if (neighborX < 0 || neighborX >= gridManager.Width ||
                            neighborY < 0 || neighborY >= gridManager.Height)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        neighborX = (x + i + gridManager.Width) % gridManager.Width;
                        neighborY = (y + j + gridManager.Height) % gridManager.Height;
                    }

                    if (gridManager.Grid[neighborX, neighborY].isAlive)
                    {
                        count++;
                    }
                }
            }

            gridManager.Grid[x, y].NeighborCount = count;
        }
    }
}

public void LifeRule()
{
    int start = gridManager.Mode == GameMode.Infinite ? 1 : 0;
    int endX = gridManager.Mode == GameMode.Infinite ? gridManager.Width - 1 : gridManager.Width;
    int endY = gridManager.Mode == GameMode.Infinite ? gridManager.Height - 1 : gridManager.Height;

    for (int x = start; x < endX; x++)
    {
        for (int y = start; y < endY; y++)
        {
            Cell cell = gridManager.Grid[x, y];
            if (cell.isAlive)
            {
                if (cell.NeighborCount < 2 || cell.NeighborCount > 3)
                {
                    cell.SetAlive(false);
                }
            }
            else
            {
                if (cell.NeighborCount == 3)
                {
                    cell.SetAlive(true);
                }
            }
        }
    }
}

public void UpdateGrid()
{
    if (timer >= speed)
    {
        timer = 0f;
        CountNeighbors();
        LifeRule(); 
        
    }else
    {
        timer += Time.deltaTime;
    }

}



}
