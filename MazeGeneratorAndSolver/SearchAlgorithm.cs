using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MazeGeneratorAndSolver
{
    public class SearchAlgorithm
    {
        Random r = new Random();
        private Maze _maze;
        public SearchAlgorithm(Maze maze)
        {
            _maze = maze;
        }

        #region Build Maze with Depth-First Search
        public void DepthFirstSearch()
        {
            // implement Depth-First search here
            var stack = new Stack<Cell>();
            stack.Push(_maze.Begin);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                if (!current.IsVisited)
                {
                    current.IsVisited = true;
                    var neighbors = PointsToCells(GetCurrentCellNeighbours(current));
                    while (neighbors.Count > 0)
                    {
                        var next = GetRandom(neighbors);
                        RemoveWall(current, next);
                        neighbors.Remove(next);
                        stack.Push(current);
                        stack.Push(next);
                    }
                }
            }
            MakeMazeBeginEnd();
        }

        private void MakeMazeBeginEnd()
        {
            Point temp = new Point();
            Random random = new Random();
            temp.Y = random.Next(_maze.Height);
            temp.X = 0;
            _maze.MazeArray[temp.X, temp.Y].CellWalls[0] = false;
            _maze.Begin = _maze.MazeArray[temp.X, temp.Y];

            temp.Y = random.Next(_maze.Height);
            temp.X = _maze.Width - 1;
            _maze.MazeArray[temp.X, temp.Y].CellWalls[2] = false;
            _maze.End = _maze.MazeArray[temp.X, temp.Y];
        }

        private void RemoveWall(Cell current, Cell next)
        {
            // Next is down 
            if (current.Position.X == next.Position.X && current.Position.Y > next.Position.Y)
            {
                _maze.MazeArray[current.Position.X, current.Position.Y].CellWalls[1] = false;
                _maze.MazeArray[next.Position.X, next.Position.Y].CellWalls[3] = false;
            }
            // the next is up
            else if (current.Position.X == next.Position.X)
            {
                _maze.MazeArray[current.Position.X, current.Position.Y].CellWalls[3] = false;
                _maze.MazeArray[next.Position.X, next.Position.Y].CellWalls[1] = false;
            }
            // the next is right
            else if (current.Position.X > next.Position.X)
            {
                _maze.MazeArray[current.Position.X, current.Position.Y].CellWalls[0] = false;
                _maze.MazeArray[next.Position.X, next.Position.Y].CellWalls[2] = false;
            }
            // the next is left
            else
            {
                _maze.MazeArray[current.Position.X, current.Position.Y].CellWalls[2] = false;
                _maze.MazeArray[next.Position.X, next.Position.Y].CellWalls[0] = false;
            }
        }

        private List<Point> GetCurrentCellNeighbours(Cell current)
        {
            List<Point> neighbours = new List<Point>();

            Point tempPos = current.Position;
            // Check right neigbour cell 
            tempPos.X = current.Position.X - 1;
            if (tempPos.X >= 0 && AllWallsIntact(_maze.MazeArray[tempPos.X, tempPos.Y]))
            {
                neighbours.Add(tempPos);
            }

            // Check left neigbour cell 
            tempPos.X = current.Position.X + 1;
            if (tempPos.X < _maze.Width && AllWallsIntact(_maze.MazeArray[tempPos.X, tempPos.Y]))
            {
                neighbours.Add(tempPos);
            }

            // Check Upper neigbour cell 
            tempPos.X = current.Position.X;
            tempPos.Y = current.Position.Y - 1;
            if (tempPos.Y >= 0 && AllWallsIntact(_maze.MazeArray[tempPos.X, tempPos.Y]))
            {
                neighbours.Add(tempPos);
            }

            // Check Lower neigbour cell 
            tempPos.Y = current.Position.Y + 1;
            if (tempPos.Y < _maze.Height && AllWallsIntact(_maze.MazeArray[tempPos.X, tempPos.Y]))
            {
                neighbours.Add(tempPos);
            }

            return neighbours;
        }

        private bool AllWallsIntact(Cell cell)
        {
            for (int i = 0; i < 4; i++)
            {
                if (!_maze.MazeArray[cell.Position.X, cell.Position.Y].CellWalls[i])
                {
                    return false;
                }
            }
            return true;
        }

        List<Cell> PointsToCells(List<Point> points)
        {
            return points.Select(p => _maze.MazeArray[p.X, p.Y]).ToList();
        }

        Cell GetRandom(List<Cell> cells)
        {
            return cells[r.Next(cells.Count)];
        }


        #endregion Depth-First Search

        #region Solve maze with Breadth-First Search

        public bool BreadthFirstSearch()
        {
            // Implement Breadth First Search here

            return true;

        }

        private void ShowFoundPath(Cell cell)
        {
            var path = new List<Cell>();
            while (cell != null)
            {
                path.Add(cell);
                cell = cell.PreviousCell;
            }
            _maze.FoundPath = path;
        }

        #endregion Breadth-First Search

    }
}
