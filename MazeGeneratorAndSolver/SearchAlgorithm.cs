using System;
using System.Collections;
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

                var neighbors = PointsToCells(GetCurrentCellNeighbours(current));
                if (neighbors.Count > 0)
                {
                    var next = GetRandom(neighbors);
                    RemoveWall(current, next);
                    stack.Push(current);
                    stack.Push(next);
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
            var queue = new Queue<Cell>();
            queue.Enqueue(_maze.Begin);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                current.IsVisited = true;

                Thread.Sleep(30);

                var adjencents = PointsToCells(GetAdjacentPoints(current));
                if (adjencents.Count > 0)
                {
                    foreach (var cell in adjencents)
                    {
                        if (!cell.IsVisited)
                        {
                            cell.PreviousCell = current;
                            queue.Enqueue(cell);
                            if (_maze.End.Position == cell.Position)
                            {
                                ShowFoundPath(cell);
                                queue.Clear();
                                break;
                            }
                        }
                    }
                }
            }
            return true;

        }

        List<Point> GetAdjacentPoints(Cell cell)
        {
            var list = new List<Point>();
            var tempPos = cell.Position;
            tempPos.X = cell.Position.X - 1;
            if (tempPos.X >= 0 && !cell.CellWalls[0])
            {
                list.Add(tempPos);
            }
            tempPos.X = cell.Position.X + 1;
            if (tempPos.X < _maze.Width && !cell.CellWalls[2])
            {
                list.Add(tempPos);
            }
            tempPos = cell.Position;
            tempPos.Y = cell.Position.Y - 1;
            if (tempPos.Y >= 0 && !cell.CellWalls[1])
            {
                list.Add(tempPos);
            }
            tempPos.Y = cell.Position.Y + 1;
            if (tempPos.Y < _maze.Height && !cell.CellWalls[3])
            {
                list.Add(tempPos);
            }

            return list;
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


        #region A*

        private void ShowFoundPath(Node node)
        {
            var path = new List<Node>();
            while (node != null)
            {
                path.Add(node);
                node = node.Parent;
            }
            _maze.FoundPath = path.Select(p => _maze.MazeArray[p.X, p.Y]).ToList();
        }

        public bool AStarSearch()
        {
            var goal = new Node(_maze.End);
            var start = new Node(_maze.Begin);
            var openList = new SortedDictionary<Node, float>();
            openList.Add(start, 0);
            var closeList = new Dictionary<Node, float>();

            while (openList.Count > 0)
            {
                var q = openList.First().Key;
                openList.Remove(q);

                _maze.MazeArray[q.X, q.Y].IsVisited = true;

                Thread.Sleep(20);

                var successors = GetAdjacentNodes(q);
                foreach (var s in successors)
                {
                    s.Parent = q;
                    if (s.Equals(goal))
                    {
                        ShowFoundPath(s);
                        return true;
                    }
                    s.G = q.G + Node.Distance(q, s);
                    s.H = Node.Distance(goal, s);
                    s.F = s.G + s.H;

                    if (openList.ContainsKey(s) && openList[s] < s.F)
                        continue;
                    if (closeList.ContainsKey(s) && closeList[s] < s.F)
                        continue;
                    openList[s] = s.F;
                }
                closeList[q] = q.F;
            }
            return false;
        }

        private List<Node> GetAdjacentNodes(Node node)
        {
            var points = GetAdjacentPoints(_maze.MazeArray[node.X, node.Y]);
            return points.Select(p => new Node(p)).ToList();
        }

        private List<Node> GetEightAdjacents(Node node)
        {
            var list = new List<Node>();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    var tempPoint = new Point(node.X + x, node.Y + y);
                    if (tempPoint.X >= 0 && tempPoint.X < _maze.Width
                        && tempPoint.Y >= 0 && tempPoint.Y < _maze.Height)
                    {
                        list.Add(new Node(tempPoint));
                    }
                }
            }
            return list;
        }

        #endregion A*
    }

    public class Node : IComparable<Node>
    {
        public Node Parent { get; set; } = null;
        public Point Position { get; set; }
        public int X => Position.X;
        public int Y => Position.Y;
        public float F { get; set; }
        public float G { get; set; }
        public float H { get; set; }

        public Node()
        {

        }

        public Node(int x, int y) : this(new Point(x, y))
        {

        }

        public Node(Point point)
        {
            Position = point;
        }

        public Node(Cell cell) : this(cell.Position)
        {
        }

        public static float Distance(Node a, Node b)
        {
            return (float)Math.Sqrt(Math.Pow(a.Position.X - b.Position.X, 2)
                + Math.Pow(a.Position.Y - b.Position.Y, 2));
        }

        public static int CompareF(Node x, Node y)
        {
            return y.F.CompareTo(x.F);
        }

        public override bool Equals(object obj)
        {
            if (obj is Node)
            {
                return this.Position == ((Node)obj).Position;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }

        public int CompareTo(Node other)
        {
            return CompareF(this, other);
        }
    }
}
