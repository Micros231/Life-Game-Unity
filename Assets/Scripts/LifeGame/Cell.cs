using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LifeGame
{
    /// <summary>
    /// The cell of the game "Life"
    /// </summary>
    public class Cell : MonoBehaviour
    {

        public bool isAlive;

        [SerializeField] private CellVisualizationHelper _visualizationHelper;

        private int _aliveNeighbors;

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            InitCell();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Scan cell neighbors
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="cellMatrix">Universe Cell Matrix</param>
        public void ScanNeighbors(int x, int y, Cell[,] cellMatrix)
        {
            _aliveNeighbors = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (i == x && j == y) 
                    {
                        continue;
                    }  
                    if (i >= 0 && i < cellMatrix.GetLength(0) && 
                        j >= 0 && j < cellMatrix.GetLength(1))
                    {
                        if (cellMatrix[i, j].isAlive) 
                        {
                            _aliveNeighbors++;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Determination of the state of the cell and visualization of this state
        /// </summary>
        public void Determine()
        {
            if (isAlive && (_aliveNeighbors < 2 || _aliveNeighbors > 3))
            {
                isAlive = false;
            }
            else if (isAlive && (_aliveNeighbors == 2 || _aliveNeighbors == 3))
            {
                isAlive = true;
            }
            else if (!isAlive && _aliveNeighbors == 3)
            {
                isAlive = true;
            }
            _visualizationHelper.SetStateVisualization(isAlive);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialization component
        /// </summary>
        private void InitCell()
        {
            _visualizationHelper = new CellVisualizationHelper(GetComponent<SpriteRenderer>(), _visualizationHelper);
        }
        #endregion
    }
}

